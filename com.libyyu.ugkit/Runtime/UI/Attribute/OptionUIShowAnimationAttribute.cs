
using System;

namespace UGKit.UI.Runtime
{
    /// <summary>
    /// 用于指定UI打开时播放的动画特性
    /// 可以应用于类，指定动画名称和是否启用动画
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class OptionUIShowAnimationAttribute : Attribute
    {
        /// <summary>
        /// 动画名称
        /// </summary>
        public string AnimationName { get; private set; }

        /// <summary>
        /// 是否启用动画
        /// </summary>
        public bool Enable { get; private set; }

        /// <summary>
        /// 构造函数，初始化动画名称和启用状态
        /// </summary>
        /// <param name="animationName">动画名称</param>
        /// <param name="enable">是否启用动画，默认为true</param>
        public OptionUIShowAnimationAttribute(string animationName, bool enable = true)
        {
            AnimationName = animationName;
            Enable = enable;
        }

        /// <summary>
        /// 构造函数，初始化动画名称和启用状态
        /// </summary>
        /// <param name="enable">是否启用动画，默认为true</param>
        public OptionUIShowAnimationAttribute(bool enable = true)
        {
            AnimationName = string.Empty;
            Enable = enable;
        }
    }
}