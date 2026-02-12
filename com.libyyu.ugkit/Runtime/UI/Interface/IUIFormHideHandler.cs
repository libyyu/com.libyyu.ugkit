
using System;

namespace UGKit.UI.Runtime
{
    /// <summary>
    /// 界面显示处理接口定义，用于处理界面显示时的逻辑
    /// </summary>
    public interface IUIFormHideHandler
    {
        /// <summary>
        /// 界面隐藏处理
        /// </summary>
        /// <param name="uiForm">界面表单</param>
        /// <param name="enableAnimation">是否启用动画</param>
        /// <param name="animationName">动画名称</param>
        /// <param name="complete">完成回调</param>
        void Handler(object uiForm, bool enableAnimation, string animationName, Action complete);
    }
}