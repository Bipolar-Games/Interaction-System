using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Bipolar.InteractionSystem.Tests
{
    public class InteractionSystemTests
    {
        private readonly List<Object> created = new List<Object>();

        [TearDown]
        public void TearDown()
        {
            foreach (Object obj in created)
                if (obj)
                    Object.DestroyImmediate(obj);

            created.Clear();
        }

        // ---- Building blocks ----

        private RecordingInteraction NewType(string name)
        {
            var type = ScriptableObject.CreateInstance<RecordingInteraction>();
            type.name = name;
            created.Add(type);
            return type;
        }

        private static TWrapper Wrap<TWrapper, TInterface>(TInterface value)
            where TWrapper : SerializedInterface<TInterface>, new()
            where TInterface : class
        {
            var serialized = new Serialized<TInterface, Object>();
            serialized.Value = value;
            var wrapper = new TWrapper();
            typeof(SerializedInterface<TInterface, Object>)
                .GetField("serializedValue", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(wrapper, serialized);
            return wrapper;
        }

        private static void SetField(object target, string name, object value)
        {
            for (var type = target.GetType(); type != null; type = type.BaseType)
            {
                FieldInfo field = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                if (field == null)
                    continue;

                field.SetValue(target, value);
                return;
            }
            Assert.Fail($"Missing field {name}");
        }

        // The fields are set while the object is inactive, so OnEnable sees them.
        private DefaultInteractor NewInteractor(params IInteractorBehavior[] behaviors)
        {
            var go = new GameObject("Interactor");
            created.Add(go);
            go.SetActive(false);
            var interactor = go.AddComponent<DefaultInteractor>();
            var list = new List<InteractorBehavior>();
            foreach (var behavior in behaviors)
                list.Add(Wrap<InteractorBehavior, IInteractorBehavior>(behavior));
            SetField(interactor, "behaviors", list);
            go.SetActive(true);
            return interactor;
        }

        private InteractiveObject NewObject(Vector3 position, params IObjectInteraction[] interactions)
        {
            var go = new GameObject("Object");
            created.Add(go);
            go.SetActive(false);
            go.transform.position = position;
            go.AddComponent<BoxCollider>();
            var interactiveObject = go.AddComponent<InteractiveObject>();
            var list = new List<ObjectInteraction>();
            foreach (var interaction in interactions)
                list.Add(Wrap<ObjectInteraction, IObjectInteraction>(interaction));
            SetField(interactiveObject, "interactions", list);
            go.SetActive(true);
            return interactiveObject;
        }

        private GameObject NewWall(Vector3 position)
        {
            var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            created.Add(wall);
            wall.transform.position = position;
            return wall;
        }

        // Lets physics see the new colliders and lets the detectors run a few times.
        private static IEnumerator Settle()
        {
            Physics.SyncTransforms();
            yield return new WaitForFixedUpdate();
            yield return null;
            yield return null;
        }

        private T NewDetector<T>(DefaultInteractor interactor, Vector3 position, System.Action<T> configure = null)
            where T : InteractiveObjectDetector
        {
            var go = new GameObject("Detector");
            created.Add(go);
            go.SetActive(false);
            go.transform.position = position;
            var detector = go.AddComponent<T>();
            SetField(detector, "interactor", interactor);
            configure?.Invoke(detector);
            go.SetActive(true);
            return detector;
        }

        // ---- Interactor ----

        [Test]
        public void Interactor_InteractsOnlyWithTypesBothSidesKnow()
        {
            var open = NewType("Open");
            var talk = NewType("Talk");
            var interactor = NewInteractor(open);
            var target = NewObject(Vector3.zero, open, talk);

            interactor.InteractiveObject = target;

            CollectionAssert.AreEqual(new[] { open }, interactor.AvailableInteractions);
            Assert.IsTrue(interactor.TryInteract(open));
            Assert.AreEqual(1, open.Count);
            Assert.IsFalse(interactor.TryInteract(talk));
            Assert.AreEqual(0, talk.Count);
        }

        [Test]
        public void Interactor_PassesItselfAndItsBehaviorToTheInteraction()
        {
            var open = NewType("Open");
            var interactor = NewInteractor(open);
            interactor.InteractiveObject = NewObject(Vector3.zero, open);

            interactor.TryInteract(open);

            Assert.AreSame(interactor, open.LastInteractor);
            Assert.IsNotNull(open.LastBehavior);
            CollectionAssert.Contains(new List<InteractionType>(open.LastBehavior.GetInteractionTypes()), open);
        }

        [Test]
        public void Interactor_WhenTheInteractionRefuses_ReportsFailure()
        {
            var open = NewType("Open");
            open.Refuses = true;
            var interactor = NewInteractor(open);
            interactor.InteractiveObject = NewObject(Vector3.zero, open);

            Assert.IsFalse(interactor.TryInteract(open));
            Assert.AreEqual(1, open.Count, "The interaction was asked");

            open.Refuses = false;
            Assert.IsTrue(interactor.TryInteract(open));
        }

        [Test]
        public void CustomInteractor_ReceivesTheInteractionThatWasCarriedOut()
        {
            var open = NewType("Open");
            var locked = NewType("Locked");
            locked.Refuses = true;
            var go = new GameObject("Custom");
            created.Add(go);
            var interactor = go.AddComponent<CustomInteractor>();
            interactor.Target = NewObject(Vector3.zero, open, locked);

            Assert.IsTrue(interactor.TryInteract(open));
            Assert.AreSame(open, interactor.LastPerformed);

            Assert.IsFalse(interactor.TryInteract(locked));
            Assert.IsNull(interactor.LastPerformed, "A refusal gives null");

            Assert.IsFalse(interactor.TryInteract(NewType("Unknown")));
            Assert.IsNull(interactor.LastPerformed, "An unknown type gives null");
        }

        [Test]
        public void Interactor_WithoutAnObject_RefusesToInteract()
        {
            var open = NewType("Open");
            var interactor = NewInteractor(open);

            Assert.IsFalse(interactor.TryInteract(open));
            Assert.AreEqual(0, open.Count);
            Assert.IsEmpty(interactor.AvailableInteractions);
        }

        [Test]
        public void Interactor_WithoutBehaviors_RefusesInsteadOfThrowing()
        {
            var open = NewType("Open");
            var interactor = NewInteractor();
            interactor.InteractiveObject = NewObject(Vector3.zero, open);

            Assert.IsFalse(interactor.TryInteract(open));
            Assert.IsFalse(interactor.TryInteract(null));
            Assert.IsEmpty(interactor.AvailableInteractions);
        }

        [Test]
        public void Interactor_ReportsTheChangeOfTheObject()
        {
            var interactor = NewInteractor();
            var first = NewObject(Vector3.zero);
            var second = NewObject(Vector3.one);
            var changes = new List<(InteractiveObject, InteractiveObject)>();
            interactor.OnInteractiveObjectChanged += (sender, oldObject, newObject) => changes.Add((oldObject, newObject));

            interactor.InteractiveObject = first;
            interactor.InteractiveObject = first;
            interactor.InteractiveObject = second;

            CollectionAssert.AreEqual(new[] { (null as InteractiveObject, first), (first, second) }, changes);
        }

        [Test]
        public void Interactor_ASecondBehaviorForTheSameType_IsReportedAndIgnored()
        {
            var open = NewType("Open");
            var other = NewType("Other");
            LogAssert.Expect(LogType.Warning, new Regex("handled twice"));

            var interactor = NewInteractor(open, open, other);
            interactor.InteractiveObject = NewObject(Vector3.zero, open, other);

            CollectionAssert.AreEquivalent(new[] { open, other }, interactor.AvailableInteractions);
        }

        // ---- Detector contract ----

        [UnityTest]
        public IEnumerator Detector_WritesItsAnswerToTheInteractor_AndClearsItWhenThereIsNone()
        {
            var interactor = NewInteractor();
            var target = NewObject(Vector3.zero);
            var detector = NewDetector<ScriptedDetector>(interactor, Vector3.zero);

            detector.Result = target;
            yield return Settle();
            Assert.AreSame(target, interactor.InteractiveObject);

            detector.Result = null;
            yield return Settle();
            Assert.IsNull(interactor.InteractiveObject);
        }

        [UnityTest]
        public IEnumerator Detector_WhenSwitchedOff_ClearsTheInteractor()
        {
            var interactor = NewInteractor();
            var detector = NewDetector<ScriptedDetector>(interactor, Vector3.zero);
            detector.Result = NewObject(Vector3.zero);
            yield return Settle();
            Assert.IsNotNull(interactor.InteractiveObject);

            detector.enabled = false;

            Assert.IsNull(interactor.InteractiveObject);
        }

        [Test]
        public void Detector_WithoutAnInteractor_ReportsAnErrorAndSwitchesOff()
        {
            LogAssert.Expect(LogType.Error, new Regex("needs an interactor"));

            var detector = NewDetector<ScriptedDetector>(null, Vector3.zero);

            Assert.IsFalse(detector.enabled);
        }

        // ---- Nearest ----

        [UnityTest]
        public IEnumerator Nearest_ChoosesTheClosestAndLetsGoWhenItLeaves()
        {
            var interactor = NewInteractor();
            var near = NewObject(new Vector3(0, 0, 1));
            var far = NewObject(new Vector3(0, 0, 3));
            NewWall(new Vector3(0, 0, -1));   // a collider that is not interactive
            NewDetector<NearestInteractiveObjectDetector>(interactor, Vector3.zero, d => SetField(d, "detectionRadius", 5f));

            yield return Settle();
            Assert.AreSame(near, interactor.InteractiveObject);

            near.transform.position = new Vector3(0, 0, 50);
            yield return Settle();
            Assert.AreSame(far, interactor.InteractiveObject);

            far.transform.position = new Vector3(0, 0, 50);
            yield return Settle();
            Assert.IsNull(interactor.InteractiveObject);
        }

        // ---- Pointed ----

        [UnityTest]
        public IEnumerator Pointed_ChoosesTheObjectInFrontWithinReach()
        {
            var interactor = NewInteractor();
            var target = NewObject(new Vector3(0, 0, 1.5f));   // its front face is at z = 1
            NewDetector<PointedInRangeObjectDetector>(interactor, Vector3.zero, d => SetField(d, "maxObjectDistance", 2f));

            yield return Settle();
            Assert.AreSame(target, interactor.InteractiveObject);

            target.transform.position = new Vector3(0, 0, 5);
            yield return Settle();
            Assert.IsNull(interactor.InteractiveObject, "Out of reach");

            target.transform.position = new Vector3(0, 0, -1.5f);
            yield return Settle();
            Assert.IsNull(interactor.InteractiveObject, "Behind the detector");
        }

        [UnityTest]
        public IEnumerator Pointed_IsBlockedByAnythingInBetween()
        {
            var interactor = NewInteractor();
            NewObject(new Vector3(0, 0, 1.8f));
            NewWall(new Vector3(0, 0, 0.8f));
            NewDetector<PointedInRangeObjectDetector>(interactor, Vector3.zero, d => SetField(d, "maxObjectDistance", 3f));

            yield return Settle();

            Assert.IsNull(interactor.InteractiveObject);
        }

        [UnityTest]
        public IEnumerator Pointed_AimsFromTheRayOriginTransform()
        {
            var interactor = NewInteractor();
            var target = NewObject(new Vector3(5, 0, 1.5f));
            var eye = new GameObject("Eye").transform;
            created.Add(eye.gameObject);
            eye.position = new Vector3(5, 0, 0);

            // The detector is the player's body, a step behind the eye; the ray starts at the eye.
            NewDetector<PointedInRangeObjectDetector>(interactor, new Vector3(5, 0, -1), d =>
            {
                SetField(d, "rayOrigin", eye);
                SetField(d, "maxObjectDistance", 2.5f);
            });

            yield return Settle();
            Assert.AreSame(target, interactor.InteractiveObject);

            // The body still faces the object, so only the eye can explain the change.
            eye.rotation = Quaternion.Euler(0, 180, 0);
            yield return Settle();
            Assert.IsNull(interactor.InteractiveObject);
        }
    }
}
