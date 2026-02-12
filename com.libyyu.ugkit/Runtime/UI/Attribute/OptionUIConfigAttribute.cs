
using System;

namespace UGKit.UI.Runtime
{
    /// <summary>
    /// 用于标记 UI 配置的特性类，支持 FairyGUI 和 UGUI 两种 UI 框架。
    /// 通过指定包名或路径，实现 UI 资源的自动定位和加载。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class OptionUIConfigAttribute : Attribute
    {
        /// <summary>
        /// FairyGUI 使用的包名。用于定位 FairyGUI 的 UI 资源包。
        /// </summary>
        public string PackageName { get; private set; }

        /// <summary>
        /// UGUI 使用的资源路径。用于定位 UGUI 的 UI 预制体或资源。
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// 构造 UI 配置特性。
        /// </summary>
        /// <param name="packageName">FairyGUI 使用的包名，若为 null 则不使用 FairyGUI。</param>
        /// <param name="path">UGUI 使用的资源路径，若为 null 则不使用 UGUI。</param>
        /// <exception cref="Exception">当 packageName 和 path 均为 null 或空字符串时抛出异常。</exception>
        public OptionUIConfigAttribute(string packageName = null, string path = null)
        {
            PackageName = packageName;
            Path = path;
            if (string.IsNullOrEmpty(PackageName) && string.IsNullOrEmpty(Path))
            {
                throw new Exception("PackageName or Path is null");
            }
        }
    }
}