namespace Bipolar.InteractionSystem.Tests
{
    /// <summary>A detector whose answer the test decides.</summary>
    public class ScriptedDetector : InteractiveObjectDetector
    {
        public InteractiveObject Result { get; set; }

        protected override InteractiveObject Detect() => Result;
    }
}
