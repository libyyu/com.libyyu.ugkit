#if ENABLE_UGKIT_TENCENT_XLUA
using System.Collections.Generic;
using UGKit.Editor;
using UGKit.Runtime;
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

            // 显示并允许编辑序列化数组（显示子属性）
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_LuaPackages, m_LuaPackagesGUIContent, true);
            bool changed = EditorGUI.EndChangeCheck();
            EditorGUI.EndDisabledGroup();

            /*
            // 只有在用户编辑了数组时才去重并重写数组，避免每帧重写导致 Inspector 控件失效
            if (changed)
            {
                //去重
                Dictionary<string, bool> tempDict = new Dictionary<string, bool>();
                List<XLuaComponent.LuaLoader> tempList = new List<XLuaComponent.LuaLoader>();
                int arrayCount = m_LuaPackages.arraySize;
                for (int i = 0; i < arrayCount; i++)
                {
                    var element = m_LuaPackages.GetArrayElementAtIndex(i);
                    var packageProp = element.FindPropertyRelative("PackageName");
                    var prefixProp = element.FindPropertyRelative("Prefix");
                    string packageName = packageProp.stringValue ?? string.Empty;
                    string prefix = prefixProp.stringValue ?? string.Empty;

                    prefix = prefix.Replace("\\", "/");
                    if (prefix.EndsWith("/"))
                    {
                        prefix = prefix.Substring(0, prefix.Length - 1);
                    }
                    var key = (packageName + "|" + prefix).ToLower();
                    if (tempDict.ContainsKey(key))
                    {
                        UnityEngine.Debug.LogWarning($"Duplicate Lua Package: {packageName} with Prefix: {prefixProp.stringValue}");
                    }
                    else
                    {
                        tempDict[key] = true;
                        tempList.Add(new XLuaComponent.LuaLoader
                        {
                            PackageName = packageName,
                            Prefix = prefix,
                        });
                    }
                }

                // 重写序列化数组
                // 重写序列化数组（使用 arraySize + Set/FindPropertyRelative 以避免 Unity 的复制行为）
                m_LuaPackages.arraySize = tempList.Count;
                for (int i = 0; i < tempList.Count; i++)
                {
                    var element = m_LuaPackages.GetArrayElementAtIndex(i);
                    element.FindPropertyRelative("PackageName").stringValue = tempList[i].PackageName;
                    element.FindPropertyRelative("Prefix").stringValue = tempList[i].Prefix;
                }
            }*/

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