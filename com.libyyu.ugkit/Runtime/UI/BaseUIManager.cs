using System.Collections.Generic;
using UGKit.Asset.Runtime;
using UGKit.ObjectPool;
using UGKit.Runtime;

namespace UGKit.UI.Runtime
{
    [UnityEngine.Scripting.Preserve]
    public abstract partial class BaseUIManager : GameFrameworkModule, IUIManager
    {
        /// <summary>
        /// 当前加载的界面实例对象池。
        /// </summary>
        protected readonly Dictionary<int, string> m_UIFormsBeingLoaded = new Dictionary<int, string>();

        /// <summary>
        /// 需要释放的界面实例对象池。
        /// </summary>
        protected readonly HashSet<int> m_UIFormsToReleaseOnLoad = new HashSet<int>();

        /// <summary>
        /// 待释放的界面实例队列。
        /// </summary>
        private Queue<IUIForm> m_RecycleQueue = new Queue<IUIForm>();

        /// <summary>
        /// 界面实例对象池回收间隔秒数。
        /// </summary>
        protected int m_RecycleInterval = 60;

        /// <summary>
        /// 界面实例对象池回收时间。
        /// </summary>
        protected float m_RecycleTime = 0;

        protected int m_Serial;

        /// <summary>
        /// 资源管理器。
        /// </summary>
        protected IAssetManager m_AssetManager;

        /// <summary>
        /// 界面辅助器。
        /// </summary>
        protected IUIFormHelper m_UIFormHelper;

        /// <summary>
        /// 对象池管理器。
        /// </summary>
        protected IObjectPoolManager m_ObjectPoolManager;

        /// <summary>
        /// 获取或设置界面实例对象池自动释放可释放对象的间隔秒数。
        /// </summary>
        public float InstanceAutoReleaseInterval
        {
            get { return m_InstancePool.AutoReleaseInterval; }
            set { m_InstancePool.AutoReleaseInterval = value; }
        }

        /// <summary>
        /// 获取或设置界面实例对象池的回收间隔秒数。
        /// </summary>
        public int RecycleInterval
        {
            get { return m_RecycleInterval; }
            set { m_RecycleInterval = value; }
        }

        /// <summary>
        /// 获取或设置界面实例对象池的容量。
        /// </summary>
        public int InstanceCapacity
        {
            get { return m_InstancePool.Capacity; }
            set { m_InstancePool.Capacity = value; }
        }

        private bool m_IsEnableUIHideAnimation = false;

        /// <summary>
        /// 获取或设置是否启用界面隐藏动画。
        /// </summary>
        public bool IsEnableUIHideAnimation
        {
            get { return m_IsEnableUIHideAnimation; }
            set { m_IsEnableUIHideAnimation = value; }
        }

        /// <summary>
        /// 获取或设置界面实例对象池对象过期秒数。
        /// </summary>
        public float InstanceExpireTime
        {
            get { return m_InstancePool.ExpireTime; }
            set { m_InstancePool.ExpireTime = value; }
        }


        /// <summary>
        /// 获取或设置是否启用界面显示动画。
        /// </summary>
        private bool m_IsEnableUIShowAnimation = false;

        /// <summary>
        /// 获取或设置是否启用界面显示动画。
        /// </summary>
        public bool IsEnableUIShowAnimation
        {
            get { return m_IsEnableUIShowAnimation; }
            set { m_IsEnableUIShowAnimation = value; }
        }

        protected IObjectPool<UIFormInstanceObject> m_InstancePool = null;
        protected bool m_IsShutdown = false;
        protected IUIFormShowHandler m_UIFormShowHandler;
        private IUIFormHideHandler m_UIFormHideHandler;

        /// <summary>
        /// 界面管理器轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        protected internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
            while (m_RecycleQueue.Count > 0)
            {
                var uiForm = m_RecycleQueue.Dequeue();
                RecycleUIForm(uiForm);
            }

            foreach (var uiGroup in m_UIGroups)
            {
                uiGroup.Value.Update(elapseSeconds, realElapseSeconds);
            }
        }

        /// <summary>
        /// 关闭并清理界面管理器。
        /// </summary>
        protected internal override void Shutdown()
        {
            m_IsShutdown = true;
            CloseAllLoadedUIForms();
            m_UIGroups.Clear();
            m_UIFormsBeingLoaded.Clear();
            m_UIFormsToReleaseOnLoad.Clear();
            m_RecycleQueue.Clear();
        }

        /// <summary>
        /// 设置对象池管理器。
        /// </summary>
        /// <param name="objectPoolManager">对象池管理器。</param>
        public void SetObjectPoolManager(IObjectPoolManager objectPoolManager)
        {
            GameFrameworkGuard.NotNull(objectPoolManager, nameof(objectPoolManager));

            m_ObjectPoolManager = objectPoolManager;
            m_InstancePool = m_ObjectPoolManager.CreateMultiSpawnObjectPool<UIFormInstanceObject>("UI Instance Pool");
        }

        /// <summary>
        /// 设置资源管理器。
        /// </summary>
        /// <param name="assetManager">资源管理器。</param>
        public virtual void SetResourceManager(IAssetManager assetManager)
        {
            GameFrameworkGuard.NotNull(assetManager, nameof(assetManager));

            m_AssetManager = assetManager;
        }

        /// <summary>
        /// 设置界面辅助器。
        /// </summary>
        /// <param name="uiFormHelper">界面辅助器。</param>
        public void SetUIFormHelper(IUIFormHelper uiFormHelper)
        {
            GameFrameworkGuard.NotNull(uiFormHelper, nameof(uiFormHelper));

            m_UIFormHelper = uiFormHelper;
        }

        /// <summary>
        /// 设置界面显示处理接口
        /// </summary>
        /// <param name="handler">界面显示处理接口</param>
        public void SetUIFormShowHandler(IUIFormShowHandler handler)
        {
            m_UIFormShowHandler = handler;
        }

        /// <summary>
        /// 设置界面隐藏处理接口
        /// </summary>
        /// <param name="handler">界面隐藏处理接口</param>
        public void SetUIFormHideHandler(IUIFormHideHandler handler)
        {
            m_UIFormHideHandler = handler;
        }

        /// <summary>
        /// 解析區分 Resources 或 Bundle 加載名稱規則
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static bool RefineResourcesPath(ref string assetName)
        {
            if (string.IsNullOrEmpty(assetName))
                return false;

            string prefix = "res#";
            if (assetName.Length > prefix.Length)
            {
                if (assetName.Substring(0, prefix.Length).Equals(prefix))
                {
                    var count = assetName.Length - prefix.Length;
                    assetName = assetName.Substring(prefix.Length, count);
                    return true;
                }
            }
            return false;
        }
    }
}