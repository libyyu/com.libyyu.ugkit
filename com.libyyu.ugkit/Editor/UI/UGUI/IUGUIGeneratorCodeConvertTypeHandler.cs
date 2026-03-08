
using UnityEngine;

namespace UGKit.UI.UGUI.Editor
{
    /// <summary>
    /// UI控件类型转换接口
    /// </summary>
    public interface IUGUIGeneratorCodeConvertTypeHandler
    {
        /// <summary>
        /// 处理器优先级,数值越小优先级越高
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// 执行类型转换
        /// </summary>
        /// <param name="transform">要转换的Transform</param>
        /// <returns>转换后的类型名称</returns>
        string Run(Transform transform);
    }
}