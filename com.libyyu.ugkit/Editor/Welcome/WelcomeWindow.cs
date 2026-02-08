using UnityEditor;
using UnityEngine;

namespace UGKit.Editor
{
    public class WelcomeWindow : EditorWindow
    {
        private static readonly string FirstRunKey = "UGKit_WelcomeWindow_Shown";
        private static readonly string ShowOnStartupKey = "UGKit_ShowOnStartup";

        private bool showOnStartup = true;
        private Vector2 scrollPosition;

        [MenuItem("UGKit/Welcome Window", false, 99999999)]
        public static void ShowWindow()
        {
            var window = GetWindow<WelcomeWindow>("UGKit 欢迎界面");
            window.minSize = new Vector2(640, 800);
            window.maxSize = new Vector2(640, 800);
            window.ShowModalUtility();
        }

        [InitializeOnLoadMethod]
        static void InitializeOnLoad()
        {
            EditorApplication.delayCall += ShowWelcomeWindowOnFirstRun;
        }

        /// <summary>
        /// 在第一次运行时显示欢迎窗口
        /// </summary>
        static void ShowWelcomeWindowOnFirstRun()
        {
            // 检查是否已经显示过欢迎窗口
            if (!SessionState.GetBool(FirstRunKey, false))
            {
                // 设置标记，表示已经显示过
                SessionState.SetBool(FirstRunKey, true);

                // 检查用户设置是否应该显示
                if (EditorPrefs.GetBool(ShowOnStartupKey, true))
                {
                    var window = GetWindow<WelcomeWindow>("UGKit 欢迎界面");
                    window.minSize = new Vector2(640, 800);
                    window.maxSize = new Vector2(640, 800);
                    window.ShowModalUtility();
                }
            }
        }

        private void OnEnable()
        {
            showOnStartup = EditorPrefs.GetBool(ShowOnStartupKey, true);
            // 加载Logo纹理
            LoadLogoTexture();
        }

        private Texture2D _logoTexture;

        private void LoadLogoTexture()
        {
            // 方式1: 从Resources文件夹加载（如果Logo放在Resources文件夹中）
            _logoTexture = Resources.Load<Texture2D>("gameframex_logo");
        }

        private void OnGUI()
        {
            // 头部标题
            GUILayout.Space(10);
            // Logo和标题区域
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (_logoTexture != null)
            {
                // 显示Logo
                GUILayout.Label(_logoTexture, GUILayout.Width(80), GUILayout.Height(80));
                GUILayout.Space(10);
            }

            // 标题垂直排列
            GUILayout.BeginVertical();
            GUIStyle titleStyle = new GUIStyle(EditorStyles.largeLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 20,
                fontStyle = FontStyle.Bold
            };

            GUIStyle subTitleStyle = new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 12
            };

            GUILayout.Label("欢迎使用 UGKit", titleStyle);
            GUILayout.Label("独立游戏前后端一体化解决方案,独立游戏开发者的圆梦大使", subTitleStyle);
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(20);

            // 内容区域
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.ExpandHeight(true));

            // 视频教程部分
            DrawSection("视频教程", "观看我们的入门视频教程", "https://space.bilibili.com/101356690/lists/3861146?type=season");

            // 文档链接部分
            DrawSection("官方文档", "查看详细的开发文档", "https://gameframex.doc.alianblank.com/");

            // 常见问题部分
            DrawSection("常见问题", "查看常见问题及解决方案", "https://gameframex.doc.alianblank.com/faq");

            // 配置表部分
            DrawSection("配置表", "查看配置表的规范和要求相关内容", "https://gameframex.doc.alianblank.com/config/");

            // 通信协议部分
            DrawSection("通信协议", "查看通信协议的规范和要求相关内容", "https://gameframex.doc.alianblank.com/protobuf/note.html");

            // UI 系统部分
            DrawSection("UI 系统", "查看 UI 系统相关内容", "https://gameframex.doc.alianblank.com/unity/component/ui.html");

            //  BUG 反馈部分
            DrawSection("BUG 反馈", "反馈 BUG", "https://github.com/GameFrameX/GameFrameX/issues/new");

            GUILayout.EndScrollView();

            GUILayout.FlexibleSpace();

            // 底部复选框
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            showOnStartup = EditorGUILayout.ToggleLeft("下次启动时显示此窗口", showOnStartup, GUILayout.Width(200));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
        }

        private void DrawSection(string title, string description, string url)
        {
            GUIStyle sectionStyle = new GUIStyle(GUI.skin.box)
            {
                padding = new RectOffset(10, 10, 10, 10),
                margin = new RectOffset(10, 10, 5, 5)
            };

            GUILayout.BeginVertical(sectionStyle);

            GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 14
            };

            GUILayout.Label(title, headerStyle);
            GUILayout.Label(description, EditorStyles.label);

            GUILayout.Space(5);

            if (GUILayout.Button("立即访问"))
            {
                Application.OpenURL(url);
            }

            GUILayout.EndVertical();

            GUILayout.Space(10);
        }

        private void OnDisable()
        {
            // 保存用户的设置
            EditorPrefs.SetBool(ShowOnStartupKey, showOnStartup);
        }
    }
}