using UGKit.UI.Runtime;

namespace UGKit.UI.UGUI.Runtime
{
    /// <summary>
    /// 界面管理器。
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    internal sealed partial class UIManager : BaseUIManager, IUIManager
    {
        /// <summary>
        /// 初始化界面管理器的新实例。
        /// </summary>
        public UIManager()
        {
            m_AssetManager = null;
            m_InstancePool = null;
            m_UIFormHelper = null;
            m_Serial = 0;
            m_RecycleTime = 0;
            if (m_RecycleInterval < 10)
            {
                m_RecycleInterval = 10;
            }

            m_IsShutdown = false;
            m_OpenUIFormSuccessEventHandler = null;
            m_OpenUIFormFailureEventHandler = null;
            // m_OpenUIFormUpdateEventHandler = null;
            // m_OpenUIFormDependencyAssetEventHandler = null;
            m_CloseUIFormCompleteEventHandler = null;
        }

        /*/// <summary>
        /// 获取或设置界面实例对象池的优先级。
        /// </summary>
        public int InstancePriority
        {
            get { return m_InstancePool.Priority; }
            set { m_InstancePool.Priority = value; }
        }*/
    }
}