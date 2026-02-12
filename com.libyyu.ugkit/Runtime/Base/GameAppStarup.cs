/*
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UGKit.Runtime
{ 
    [DisallowMultipleComponent]
    [AddComponentMenu("UGKit/Framework/GameAppStarup")]
    [UnityEngine.Scripting.Preserve]
    [DefaultExecutionOrder(-500)]
    public class GameAppStarup : MonoBehaviour
    {
        public static GameAppStarup Instance { get; private set; }

        /// <summary>
        /// 游戏框架所在的场景编号。
        /// </summary>
        public const int GameFrameworkSceneId = 0;

        private const int DefaultDpi = 96; // default windows dpi

        private float m_GameSpeedBeforePause = 1f;

        [SerializeField] private int m_FrameRate = 30;

        [SerializeField] private float m_GameSpeed = 1f;

        [SerializeField] private bool m_RunInBackground = true;

        [SerializeField] private bool m_NeverSleep = true;

        /// <summary>
        /// 获取或设置游戏帧率。
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public int FrameRate
        {
            get { return m_FrameRate; }
            set { Application.targetFrameRate = m_FrameRate = value; }
        }

        /// <summary>
        /// 获取或设置游戏速度。
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public float GameSpeed
        {
            get { return m_GameSpeed; }
            set { Time.timeScale = m_GameSpeed = value >= 0f ? value : 0f; }
        }

        /// <summary>
        /// 获取游戏是否暂停。
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public bool IsGamePaused
        {
            get { return m_GameSpeed <= 0f; }
        }

        /// <summary>
        /// 获取是否正常游戏速度。
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public bool IsNormalGameSpeed
        {
            get { return m_GameSpeed == 1f; }
        }

        /// <summary>
        /// 获取或设置是否允许后台运行。
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public bool RunInBackground
        {
            get { return m_RunInBackground; }
            set { Application.runInBackground = m_RunInBackground = value; }
        }

        /// <summary>
        /// 获取或设置是否禁止休眠。
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public bool NeverSleep
        {
            get { return m_NeverSleep; }
            set
            {
                m_NeverSleep = value;
                Screen.sleepTimeout = value ? SleepTimeout.NeverSleep : SleepTimeout.SystemSetting;
            }
        }

        void Awake()
        {
            if (Instance != null) throw new GameFrameworkException("Multi GameApp is found");
            Instance = this;
            DontDestroyOnLoad(this);
            OnPreSetup();
            Log.Info("Game Version: {0}, Unity Version: {1}", Version.GameVersion, Application.unityVersion);

#if UNITY_5_3_OR_NEWER || UNITY_5_3
            Utility.Converter.ScreenDpi = Screen.dpi;
            if (Utility.Converter.ScreenDpi <= 0)
            {
                Utility.Converter.ScreenDpi = DefaultDpi;
            }

            Application.targetFrameRate = m_FrameRate;
            Time.timeScale = m_GameSpeed;
            Application.runInBackground = m_RunInBackground;
            Screen.sleepTimeout = m_NeverSleep ? SleepTimeout.NeverSleep : SleepTimeout.SystemSetting;
#else
            Log.Error("Game Framework only applies with Unity 5.3 and above, but current Unity version is {0}.", Application.unityVersion);
            Shutdown(ShutdownType.Quit);
#endif
#if UNITY_5_6_OR_NEWER
            Application.lowMemory += OnLowMemory;
#endif

            OnPostSetup();
        }

        protected virtual void OnPreSetup()
        {
            Utility.Text.SetTextHelper(new DefaultTextHelper());
            Version.SetVersionHelper(new DefaultVersionHelper());
            GameFrameworkLog.SetLogHelper(new DefaultLogHelper());
#if UNITY_5_3_OR_NEWER || UNITY_5_3
            Utility.Compression.SetCompressionHelper(new DefaultCompressionHelper());
            Utility.Json.SetJsonHelper(new NewtonsoftJsonHelper());
#endif


        }

        protected virtual void OnPostSetup()
        {
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            GameFrameworkEntry.Update(Time.deltaTime, Time.unscaledDeltaTime);
        }

        void OnDestroy()
        {
            GameFrameworkEntry.Shutdown();
            Instance = null;
        }

        void OnApplicationQuit()
        {
#if UNITY_5_6_OR_NEWER
            Application.lowMemory -= OnLowMemory;
#endif
            StopAllCoroutines();
        }

        /// <summary>
        /// 暂停游戏。
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public void PauseGame()
        {
            if (IsGamePaused)
            {
                return;
            }

            m_GameSpeedBeforePause = GameSpeed;
            GameSpeed = 0f;
        }

        /// <summary>
        /// 恢复游戏。
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public void ResumeGame()
        {
            if (!IsGamePaused)
            {
                return;
            }

            GameSpeed = m_GameSpeedBeforePause;
        }

        /// <summary>
        /// 重置为正常游戏速度。
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public void ResetNormalGameSpeed()
        {
            if (IsNormalGameSpeed)
            {
                return;
            }

            GameSpeed = 1f;
        }

        /// <summary>
        /// 关闭游戏框架。
        /// </summary>
        /// <param name="shutdownType">关闭游戏框架类型。</param>
        [UnityEngine.Scripting.Preserve]
        public void Shutdown(ShutdownType shutdownType)
        {
            Log.Info($"Shutdown Game Framework ({shutdownType})...");
            Destroy(this);

            if (shutdownType == ShutdownType.None)
            {
                return;
            }

            if (shutdownType == ShutdownType.Restart)
            {
                SceneManager.LoadScene(GameFrameworkSceneId);
                return;
            }

            if (shutdownType == ShutdownType.Quit)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }

        void OnLowMemory()
        {
            Log.Info("Low memory reported...");

            //ObjectPoolComponent objectPoolComponent = GameEntry.GetComponent<ObjectPoolComponent>();
            //if (objectPoolComponent != null)
            //{
            //    objectPoolComponent.ReleaseAllUnused();
            //}

            //AssetComponent resourceComponent = GameEntry.GetComponent<AssetComponent>();
            //if (resourceComponent != null)
            //{
            //    // resourceComponent.ForceUnloadUnusedAssets(true);
            //}
        }
    }


    public static partial class GameApp
    {
        public static GameAppStarup App
        {
            get
            {
                return GameAppStarup.Instance;
            }
        }
    }
}
*/