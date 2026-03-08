
using System;
using UGKit.Runtime;

namespace UGKit.UI.Runtime
{
    /// <summary>
    /// 打开界面的信息。
    /// </summary>
    public sealed class OpenUIFormInfo : IReference
    {
        private int m_SerialId = 0;
        private bool m_PauseCoveredUIForm = false;
        private object m_UserData = null;
        private Type m_FormType;
        private object m_AssetHandle;
        private bool m_IsFullScreen = false;
        private UIType m_UIType;

        /// <summary>
        /// 获取界面是否全屏。
        /// </summary>
        public bool IsFullScreen
        {
            get { return m_IsFullScreen; }
        }

        /// <summary>
        /// 获取界面类型。
        /// </summary>
        public Type FormType
        {
            get { return m_FormType; }
        }

        public UIType uiType
        {
            get { return m_UIType; }
        }

        /// <summary>
        /// 获取界面序列编号。
        /// </summary>
        public int SerialId
        {
            get { return m_SerialId; }
        }

        /// <summary>
        /// 获取是否暂停被覆盖的界面。
        /// </summary>
        public bool PauseCoveredUIForm
        {
            get { return m_PauseCoveredUIForm; }
        }

        /// <summary>
        /// 获取用户自定义数据。
        /// </summary>
        public object UserData
        {
            get { return m_UserData; }
        }

        /// <summary>
        /// 获取用户自定义数据。
        /// </summary>
        public object AssetHandle
        {
            get { return m_AssetHandle; }
        }

        /// <summary>
        /// 设置界面资源句柄。
        /// </summary>
        /// <param name="assetHandle">界面资源句柄。</param>
        public void SetAssetHandle(object assetHandle)
        {
            m_AssetHandle = assetHandle;
        }

        /// <summary>
        /// 创建打开界面的信息。
        /// </summary>
        /// <param name="serialId">界面序列编号。</param>
        /// <param name="uiFormType">界面类型。</param>
        /// <param name="pauseCoveredUIForm">是否暂停被覆盖的界面。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <param name="isFullScreen">界面是否全屏。</param>
        /// <returns>创建的打开界面的信息。</returns>
        public static OpenUIFormInfo Create(int serialId, Type uiFormType, bool pauseCoveredUIForm, object userData, bool isFullScreen, UIType uiType)
        {
            OpenUIFormInfo openUIFormInfo = ReferencePool.Acquire<OpenUIFormInfo>();
            openUIFormInfo.m_SerialId = serialId;
            openUIFormInfo.m_PauseCoveredUIForm = pauseCoveredUIForm;
            openUIFormInfo.m_UserData = userData;
            openUIFormInfo.m_FormType = uiFormType;
            openUIFormInfo.m_IsFullScreen = isFullScreen;
            openUIFormInfo.m_UIType = uiType;
            return openUIFormInfo;
        }

        /// <summary>
        /// 清理打开界面的信息。
        /// </summary>
        public void Clear()
        {
            m_SerialId = default;
            m_PauseCoveredUIForm = default;
            m_UserData = default;
            m_FormType = default;
            m_AssetHandle = default;
            m_IsFullScreen = default;
        }
    }
}