
using UnityEditor;

namespace UGKit.Editor
{
    /// <summary>
    /// 在构建前执行
    /// </summary>
    public interface IBuilderPreHookHandler
    {
        /// <summary>
        /// 优先级,数值越大优先级越低
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="target">构建目标平台</param>
        /// <param name="path">路径或者目录</param>
        void Run(BuildTarget target, string path);
    }
}