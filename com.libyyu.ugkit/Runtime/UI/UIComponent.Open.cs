using System.Threading.Tasks;
using System;
using System.Reflection;
using UGKit.Runtime;

namespace UGKit.UI.Runtime
{
    public partial class UIComponent
    {
        /// <summary>
        /// 异步打开全屏UI。
        /// </summary>
        /// <param name="uiFormAssetPath">界面所在路径</param>
        /// <typeparam name="T">UI的具体类型。</typeparam>
        /// <param name="userData">传递给UI的用户数据。</param>
        /// <returns>返回打开的UI实例。</returns>
        public async Task<T> OpenFullScreenAsync<T>(string uiFormAssetPath, UIType uiType, object userData = null) where T : class, IUIForm
        {
            return await OpenUIFormAsync<T>(uiFormAssetPath, true, uiType, userData, true);
        }

        /// <summary>
        /// 异步打开全屏UI。
        /// </summary>
        /// <typeparam name="T">UI的具体类型。</typeparam>
        /// <param name="userData">传递给UI的用户数据。</param>
        /// <returns>返回打开的UI实例。</returns>
        public async Task<T> OpenFullScreenAsync<T>(UIType uiType, object userData = null) where T : class, IUIForm
        {
            var uiFormAssetName = typeof(T).Name;
            var uiFormAssetPath = Utility.Asset.Path.GetUIPath(uiFormAssetName);
            return await OpenFullScreenAsync<T>(uiFormAssetPath, uiType, userData);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormAssetPath">界面所在路径</param>
        /// <param name="uiFormType">界面逻辑类型。</param>
        /// <param name="pauseCoveredUIForm">是否暂停被覆盖的界面。</param>
        /// <param name="isFullScreen">是否全屏</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>界面的序列编号。</returns>
        public async Task<IUIForm> OpenUIAsync(string uiFormAssetPath, Type uiFormType, bool pauseCoveredUIForm, UIType uiType, object userData = null, bool isFullScreen = false)
        {
            return await m_UIManager.OpenUIFormAsync(uiFormAssetPath, uiFormType, pauseCoveredUIForm, uiType, userData, isFullScreen);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormAssetPath">界面所在路径</param>
        /// <param name="pauseCoveredUIForm">是否暂停被覆盖的界面。</param>
        /// <param name="isFullScreen">是否全屏</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>界面的序列编号。</returns>
        private async Task<T> OpenUIFormAsync<T>(string uiFormAssetPath, bool pauseCoveredUIForm, UIType uiType, object userData = null, bool isFullScreen = false) where T : class, IUIForm
        {
            var ui = await m_UIManager.OpenUIFormAsync<T>(uiFormAssetPath, pauseCoveredUIForm, uiType, userData, isFullScreen);
            return ui as T;
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="pauseCoveredUIForm">是否暂停被覆盖的界面。</param>
        /// <param name="isFullScreen">是否全屏</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>界面的序列编号。</returns>
        public async Task<T> OpenUIFormAsync<T>(bool pauseCoveredUIForm, UIType uiType, object userData = null, bool isFullScreen = false) where T : class, IUIForm
        {
            var uiFormAssetName = typeof(T).Name;
            var uiFormAssetPath = Utility.Asset.Path.GetUIPath(uiFormAssetName);
            return await OpenUIFormAsync<T>(uiFormAssetPath, pauseCoveredUIForm, uiType, userData, isFullScreen);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormAssetPath">界面所在路径</param>
        /// <param name="isFullScreen">是否全屏</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>界面的序列编号。</returns>
        public async Task<T> OpenAsync<T>(string uiFormAssetPath, UIType uiType, object userData = null, bool isFullScreen = false) where T : class, IUIForm
        {
            return await OpenUIFormAsync<T>(uiFormAssetPath, false, uiType, userData, isFullScreen);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormAssetPath">界面所在路径</param>
        /// <param name="pauseCoveredUIForm">是否暂停覆盖的UI</param>
        /// <param name="isFullScreen">是否全屏</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>界面的序列编号。</returns>
        public async Task<T> OpenAsync<T>(string uiFormAssetPath, bool pauseCoveredUIForm, UIType uiType, object userData = null, bool isFullScreen = false) where T : class, IUIForm
        {
            return await OpenUIFormAsync<T>(uiFormAssetPath, pauseCoveredUIForm, uiType, userData, isFullScreen);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="isFullScreen">是否全屏</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>界面的序列编号。</returns>
        public async Task<T> OpenAsync<T>(UIType uiType, object userData = null, bool isFullScreen = false) where T : class, IUIForm
        {
            var uiFormAssetName = typeof(T).Name;

            var uiFormAssetPath = Utility.Asset.Path.GetUIPath(uiFormAssetName);
            var attribute = typeof(T).GetCustomAttribute(typeof(OptionUIConfigAttribute));
            if (attribute is OptionUIConfigAttribute optionUIConfig)
            {
                if (optionUIConfig.Path.IsNullOrWhiteSpace())
                {
                    uiFormAssetPath = Utility.Asset.Path.GetUIPath(optionUIConfig.PackageName);
                }
                else
                {
                    uiFormAssetPath = optionUIConfig.Path;
                }
            }

            return await OpenAsync<T>(uiFormAssetPath, uiType, userData, isFullScreen);
        }
    }
}