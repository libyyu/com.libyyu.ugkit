using UnityEngine.Scripting; // 确保引入命名空间

namespace UGKit.Runtime
{
    /// <summary>
    /// 事件基类。
    /// </summary>
    public abstract class BaseEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 获取事件ID。
        /// </summary>
        [Preserve] // 添加 Preserve 标签
        public abstract string Id { get; }
    }
}