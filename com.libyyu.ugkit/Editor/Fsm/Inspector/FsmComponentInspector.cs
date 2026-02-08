using UGKit.Editor;
using UGKit.Fsm.Runtime;
using UnityEditor;

namespace UGKit.Fsm.Editor
{
    [CustomEditor(typeof(FsmComponent))]
    internal sealed class FsmComponentInspector : ComponentTypeComponentInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox("Available during runtime only.", MessageType.Info);
                return;
            }

            var t = (FsmComponent)target;

            if (IsPrefabInHierarchy(t.gameObject))
            {
                EditorGUILayout.LabelField("FSM Count", t.Count.ToString());

                var fsms = t.GetAllFsmList();
                foreach (FsmBase fsm in fsms)
                {
                    DrawFsm(fsm);
                }
            }

            Repaint();
        }

        protected override void RefreshTypeNames()
        {
            RefreshComponentTypeNames(typeof(IFsmManager));
        }

        private void DrawFsm(FsmBase fsm)
        {
            EditorGUILayout.LabelField(fsm.FullName, fsm.IsRunning ? string.Format("{0}, {1:F1} s", fsm.CurrentStateName, fsm.CurrentStateTime) : (fsm.IsDestroyed ? "Destroyed" : "Not Running"));
        }
    }
}