using Analogy.LogViewer.Template;

namespace Analogy.LogViewer.Intuitive.IAnalogy
{
    public class IntuitiveAnalogyPolicyEnforcer : AnalogyPolicyEnforcer
    {
        public override bool DisableUpdates { get; set; } = true;
    }
}