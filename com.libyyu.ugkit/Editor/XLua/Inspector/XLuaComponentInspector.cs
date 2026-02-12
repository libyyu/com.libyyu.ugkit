#if ENABLE_UGKIT_TENCENT_XLUA
using System.Threading;
using UGKit.Editor;
using UGKit.Web.Runtime;
using UGKit.XLua.Runtime;
using UnityEditor;
using UnityEngine;

namespace UGKit.XLua.Editor
{
    [CustomEditor(typeof(XLuaComponent))]
    internal sealed class XLuaComponentInspector : ComponentTypeComponentInspector
    {
        private GUIContent m_LuaPackagesGUIContent = new GUIContent("Lua包列表");
        private SerializedProperty m_LuaPackages;

        protected override void RefreshTypeNames()
        {
            RefreshComponentTypeNames(typeof(IXLuaManager));
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();


            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                GUI.enabled = false;
                EditorGUILayout.PropertyField(m_LuaPackages, m_LuaPackagesGUIContent);
                GUI.enabled = true;
            }
            EditorGUI.EndDisabledGroup();

            serializedObject.ApplyModifiedProperties();

            Repaint();

            serializedObject.ApplyModifiedProperties();
        }

        protected override void Enable()
        {
            base.Enable();

            // m_Timeout = serializedObject.FindProperty("m_Timeout");
            m_LuaPackages = serializedObject.FindProperty("m_LuaPackageList");
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif