using UGKit.Editor;
using UGKit.Timer.Runtime;
using UnityEditor;

namespace UGKit.Timer.Editor
{
    [CustomEditor(typeof(TimerComponent))]
    internal sealed class TimerComponentInspector : ComponentTypeComponentInspector
    {
        protected override void RefreshTypeNames()
        {
            RefreshComponentTypeNames(typeof(ITimerManager));
        }
    }
}
