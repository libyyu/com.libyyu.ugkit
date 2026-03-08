
using System;

namespace UGKit.UI.Runtime
{
    /// <summary>
    /// 界面辅助器接口。
    /// </summary>
    public interface IUIFormHelper
    {
        /// <summary>
        /// 实例化界面。
        /// </summary>
        /// <param name="uiFormAsset">要实例化的界面资源。</param>
        /// <returns>实例化后的界面。</returns>
        object InstantiateUIForm(object uiFormAsset);

        /// <summary>
        /// 创建界面。
        /// </summary>
        /// <param name="uiFormInstance">界面实例。</param>
        /// <param name="uiFormType">界面逻辑类型</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>界面。</returns>
        IUIForm CreateUIForm(object uiFormInstance, Type uiFormType, UIType uiType, object userData);

        /// <summary>
        /// 释放界面。
        /// </summary>
        /// <param name="uiFormAsset">要释放的界面资源。</param>
        /// <param name="uiFormInstance">要释放的界面实例。</param>
        /// <param name="assetHandle">界面资源句柄。</param>
        void ReleaseUIForm(object uiFormAsset, object uiFormInstance, object assetHandle);
    }
}