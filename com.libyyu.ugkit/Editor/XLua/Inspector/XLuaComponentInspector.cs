#if ENABLE_UGKIT_TENCENT_XLUA
using UGKit.Editor;
using UGKit.XLua.Runtime;
using UnityEditor;

namespace UGKit.XLua.Editor
{
    [CustomEditor(typeof(XLuaComponent))]
    internal sealed class XLuaComponentInspector : ComponentTypeComponentInspector
    {
        protected override void RefreshTypeNames()
        {
            RefreshComponentTypeNames(typeof(IXLuaManager));
        }

        protected override void Enable()
        {
            base.Enable();

            // m_Timeout = serializedObject.FindProperty("m_Timeout");
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif