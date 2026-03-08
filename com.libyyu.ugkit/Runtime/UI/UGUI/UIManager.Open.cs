using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UGKit.Runtime;
using UGKit.UI.Runtime;
using UnityEngine;
using YooAsset;

namespace UGKit.UI.UGUI.Runtime
{
    /// <summary>
    /// 界面管理器。
    /// </summary>
    internal sealed partial class UIManager
    {
        private readonly List<UIFormLoadingObject> m_LoadingUIForms = new List<UIFormLoadingObject>(64);
        private readonly List<UIFormLoadingObject> m_UIFormsRemoveList = new List<UIFormLoadingObject>(64);

        protected override async Task<IUIForm> InnerOpenUIFormAsync(string uiFormAssetPath, Type uiFormType, bool pauseCoveredUIForm, UIType uiType, object userData, bool isFullScreen = false)
        {
            GameFrameworkGuard.NotNull(m_AssetManager, nameof(m_AssetManager));
            GameFrameworkGuard.NotNull(m_UIFormHelper, nameof(m_UIFormHelper));
            GameFrameworkGuard.NotNull(uiFormType, nameof(uiFormType));
            var uiFormAssetName = uiFormType.Name;
            string assetPath = PathHelper.Combine(uiFormAssetPath, uiFormAssetName);
            var uiFormInstanceObject = m_InstancePool.Spawn(assetPath);
            if (uiFormInstanceObject != null)
            {
                // 如果对象池存在
                return InternalOpenUIForm(-1, uiFormAssetName, uiFormType, uiFormInstanceObject.Target, pauseCoveredUIForm, uiType, false, 0f, userData, isFullScreen);
            }

            var uiForm = InnerLoadUIFormAsync(uiFormAssetPath, uiFormType, pauseCoveredUIForm, uiType, userData, isFullScreen, uiFormAssetName, assetPath);
            UIFormLoadingObject uiFormLoadingObject = UIFormLoadingObject.Create(uiFormAssetPath, uiFormAssetName, uiFormType, uiForm);
            m_LoadingUIForms.Add(uiFormLoadingObject);
            var result = await uiForm;

            foreach (var value in m_LoadingUIForms)
            {
                if (value.UIFormAssetPath == uiFormAssetPath && value.UIFormAssetName == uiFormAssetName && value.UIFormType == uiFormType)
                {
                    m_UIFormsRemoveList.Add(value);
                }
            }

            foreach (var value in m_UIFormsRemoveList)
            {
                m_LoadingUIForms.Remove(value);
                ReferencePool.Release(value);
            }

            m_UIFormsRemoveList.Clear();

            return result;
        }

        /// <summary>
        /// 异步加载界面。
        /// </summary>
        /// <param name="uiFormAssetPath">界面资源路径。</param>
        /// <param name="uiFormType">界面类型。</param>
        /// <param name="pauseCoveredUIForm">是否暂停被覆盖的界面。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <param name="isFullScreen">是否全屏显示。</param>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="assetPath">界面资源路径。</param>
        /// <returns>界面实例。</returns>
        private async Task<IUIForm> InnerLoadUIFormAsync(string uiFormAssetPath, Type uiFormType, bool pauseCoveredUIForm, UIType uiType, object userData, bool isFullScreen, string uiFormAssetName, string assetPath)
        {
            int serialId = ++m_Serial;
            m_UIFormsBeingLoaded.Add(serialId, uiFormAssetName);
            OpenUIFormInfo openUIFormInfo = OpenUIFormInfo.Create(serialId, uiFormType, pauseCoveredUIForm, userData, isFullScreen, uiType);
            //if (uiFormAssetPath.IndexOf(Utility.Asset.Path.BundlesDirectoryName, StringComparison.OrdinalIgnoreCase) < 0)
            if(RefineResourcesPath(ref assetPath))
            {
                // 从Resources 中加载
                var original = (GameObject)Resources.Load(assetPath);
                var gameObject = UnityEngine.Object.Instantiate(original);
                gameObject.name = uiFormAssetName;
                return LoadAssetSuccessCallback(assetPath, gameObject, 0, openUIFormInfo);
            }

            // 从包中加载
            var assetHandle = await m_AssetManager.LoadAssetAsync<UnityEngine.Object>(assetPath);
            if (assetHandle.IsSucceed())
            {
                // 加载成功
                var gameObject = assetHandle.InstantiateSync();
                gameObject.name = uiFormAssetName;
                openUIFormInfo.SetAssetHandle(assetHandle);
                return LoadAssetSuccessCallback(assetPath, gameObject, assetHandle.Progress, openUIFormInfo);
            }

            // 加载失败
            return LoadAssetFailureCallback(assetPath, assetHandle.LastError, openUIFormInfo);
        }

        private IUIForm InternalOpenUIForm(int serialId, string uiFormAssetName, Type uiFormType, object uiFormInstance, bool pauseCoveredUIForm, UIType uiType, bool isNewInstance, float duration, object userData, bool isFullScreen)
        {
            try
            {
                IUIForm uiForm = m_UIFormHelper.CreateUIForm(uiFormInstance, uiFormType, uiType, userData);
                if (uiForm == null)
                {
                    throw new GameFrameworkException("Can not create UI form in UI form helper.");
                }

                var uiGroup = uiForm.UIGroup;
                uiForm.Init(serialId, uiFormAssetName, uiGroup, null, pauseCoveredUIForm, isNewInstance, userData, RecycleInterval, uiType, isFullScreen);

                if (!uiGroup.InternalHasInstanceUIForm(uiFormAssetName, uiForm))
                {
                    uiGroup.AddUIForm(uiForm);
                }

                uiForm.OnOpen(userData);
                uiForm.BindEvent();
                uiForm.LoadData();
                uiForm.UpdateLocalization();
                if (uiForm.EnableShowAnimation)
                {
                    uiForm.Show(m_UIFormShowHandler, null);
                }

                uiGroup.Refresh();

                if (m_OpenUIFormSuccessEventHandler != null)
                {
                    OpenUIFormSuccessEventArgs openUIFormSuccessEventArgs = OpenUIFormSuccessEventArgs.Create(uiForm, duration, userData);
                    m_OpenUIFormSuccessEventHandler(this, openUIFormSuccessEventArgs);
                    // ReferencePool.Release(openUIFormSuccessEventArgs);
                }

                return uiForm;
            }
            catch (Exception exception)
            {
                if (m_OpenUIFormFailureEventHandler != null)
                {
                    OpenUIFormFailureEventArgs openUIFormFailureEventArgs = OpenUIFormFailureEventArgs.Create(serialId, uiFormAssetName, pauseCoveredUIForm, exception.ToString(), userData);
                    m_OpenUIFormFailureEventHandler(this, openUIFormFailureEventArgs);
                    return GetUIForm(openUIFormFailureEventArgs.SerialId);
                }

                throw;
            }
        }

        private IUIForm LoadAssetSuccessCallback(string uiFormAssetName, object uiFormAsset, float duration, object userData)
        {
            OpenUIFormInfo openUIFormInfo = (OpenUIFormInfo)userData;
            if (openUIFormInfo == null)
            {
                throw new GameFrameworkException("Open UI form info is invalid.");
            }

            if (m_UIFormsToReleaseOnLoad.Contains(openUIFormInfo.SerialId))
            {
                var form = GetUIForm(openUIFormInfo.SerialId);
                m_UIFormsToReleaseOnLoad.Remove(openUIFormInfo.SerialId);
                m_UIFormHelper.ReleaseUIForm(uiFormAsset, null, openUIFormInfo.AssetHandle);
                ReferencePool.Release(openUIFormInfo);
                return form;
            }

            m_UIFormsBeingLoaded.Remove(openUIFormInfo.SerialId);
            var uiFormInstanceObject = UIFormInstanceObject.Create(uiFormAssetName, uiFormAsset, m_UIFormHelper.InstantiateUIForm(uiFormAsset), m_UIFormHelper, openUIFormInfo.AssetHandle);
            m_InstancePool.Register(uiFormInstanceObject, true);

            var uiForm = InternalOpenUIForm(openUIFormInfo.SerialId, uiFormAssetName, openUIFormInfo.FormType, uiFormInstanceObject.Target, openUIFormInfo.PauseCoveredUIForm, openUIFormInfo.uiType, true, duration, openUIFormInfo.UserData, openUIFormInfo.IsFullScreen);
            ReferencePool.Release(openUIFormInfo);
            return uiForm;
        }

        private IUIForm LoadAssetFailureCallback(string uiFormAssetName, string errorMessage, object userData)
        {
            OpenUIFormInfo openUIFormInfo = (OpenUIFormInfo)userData;
            if (openUIFormInfo == null)
            {
                throw new GameFrameworkException("Open UI form info is invalid.");
            }

            if (m_UIFormsToReleaseOnLoad.Contains(openUIFormInfo.SerialId))
            {
                var uiForm = GetUIForm(openUIFormInfo.SerialId);
                m_UIFormsToReleaseOnLoad.Remove(openUIFormInfo.SerialId);
                ReferencePool.Release(openUIFormInfo);
                return uiForm;
            }

            m_UIFormsBeingLoaded.Remove(openUIFormInfo.SerialId);
            string appendErrorMessage = Utility.Text.Format("Load UI form failure, asset name '{0}', error message '{2}'.", uiFormAssetName, errorMessage);
            if (m_OpenUIFormFailureEventHandler != null)
            {
                OpenUIFormFailureEventArgs openUIFormFailureEventArgs = OpenUIFormFailureEventArgs.Create(openUIFormInfo.SerialId, uiFormAssetName, openUIFormInfo.PauseCoveredUIForm, appendErrorMessage, openUIFormInfo.UserData);
                m_OpenUIFormFailureEventHandler(this, openUIFormFailureEventArgs);
                return GetUIForm(openUIFormInfo.SerialId);
            }

            throw new GameFrameworkException(appendErrorMessage);
        }
    }
}