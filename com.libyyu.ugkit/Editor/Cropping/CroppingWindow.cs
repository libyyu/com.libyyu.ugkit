using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UGKit.Runtime;
using UnityEditor;
using UnityEngine;

namespace UGKit.Editor
{
    /// <summary>
    /// 防裁剪代码生成窗口
    /// </summary>
    public sealed class CroppingWindow : EditorWindow
    {
        [MenuItem("UGKit/Cropping(防止裁剪代码生成)", false, 2001)]
        public static void ShowWindow()
        {
            var window = GetWindow<CroppingWindow>("Cropping");
            window.minSize = new UnityEngine.Vector2(800, 600);
            window.maxSize = window.minSize;
            window.maximized = false;
            window.Show();
        }

        private string[] _dropdownOptions = new string[] { "Empty" };
        private readonly string[] _ignoredTypes = new string[] { "UnityEngine".ToLower(), "UnityEditor".ToLower(), "Mono".ToLower(), "System".ToLower(), "dnlib".ToLower(), "Unity.Hotfix".ToLower(), "Unity.Baselib".ToLower(), ".Editor".ToLower(), "JetBrains".ToLower(), "NUnit".ToLower() };
        private int _selectedDropdownIndex = 0;
        private string _searchText = string.Empty;

        private void OnGUI()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            {
                GUILayout.Label("查询类型:", EditorStyles.label, GUILayout.Width(100)); // 设置宽度以确保一致性
                _searchText = EditorGUILayout.TextField(_searchText, EditorStyles.toolbarTextField, GUILayout.Width(600));
                GUILayout.FlexibleSpace(); // 使中间部分可以自动伸缩
                if (GUILayout.Button("Search(查询)", EditorStyles.toolbarButton))
                {
                    if (string.IsNullOrWhiteSpace(_searchText))
                    {
                        ShowNotification(new GUIContent() { text = "搜索内容不能为空" });
                    }
                    else
                    {
                        string searchText = _searchText.ToLower();
                        var types = Utility.Assembly.GetTypes();
                        var result = new List<string>();
                        foreach (var type in types)
                        {
                            var fullName = type.FullName.ToLower();
                            var isFind = false;
                            foreach (var ignoredType in _ignoredTypes)
                            {
                                if (fullName.Contains(ignoredType))
                                {
                                    isFind = true;
                                    break;
                                }
                            }

                            if (isFind)
                            {
                                continue;
                            }

                            if (fullName.Contains(searchText))
                            {
                                result.Add(type.FullName);
                            }
                        }

                        _dropdownOptions = result.ToArray();
                    }
                }
            }

            GUILayout.EndHorizontal();
            GUILayout.Space(5); // 使中间部分可以自动伸缩
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("类型选择:", EditorStyles.label, GUILayout.Width(100));
                int newDropdownIndex = EditorGUILayout.Popup(_selectedDropdownIndex, _dropdownOptions, EditorStyles.toolbarDropDown, GUILayout.Width(600));
                if (newDropdownIndex != _selectedDropdownIndex)
                {
                    _selectedDropdownIndex = newDropdownIndex;
                }

                GUILayout.FlexibleSpace(); // 使中间部分可以自动伸缩
                if (GUILayout.Button("Generate(生成)", EditorStyles.toolbarButton))
                {
                    Generate(_dropdownOptions[_selectedDropdownIndex]);
                }
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.TextArea(_generatedText, GUILayout.ExpandHeight(true));
        }

        private string _generatedText = string.Empty;

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="targetTypeName"></param>
        private void Generate(string targetTypeName)
        {
            _generatedText = string.Empty;
            var targetType = Utility.Assembly.GetType(targetTypeName);
            if (targetType != null)
            {
                var types = targetType.Assembly.GetTypes();
                types = types.OrderBy(m => m.FullName).ToArray();
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var type in types)
                {
                    if (type.IsNestedPrivate)
                    {
                        continue;
                    }

                    if (type.FullName.Contains("PrivateImplementationDetails"))
                    {
                        continue;
                    }

                    stringBuilder.AppendLine(" _ = typeof(" + type.FullName.Replace("+", ".").Replace("`1", "<>").Replace("`2", "<,>") + ");");
                }

                _generatedText = stringBuilder.ToString();
                ShowNotification(new GUIContent() { text = "请将代码复制到CroppingHelper.cs 中" });
            }
        }
    }
}