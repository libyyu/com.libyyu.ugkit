
using System;

namespace UGKit.Runtime
{
    /// <summary>
    /// 游戏框架中包含事件数据的类的基类。
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    public abstract class GameFrameworkEventArgs : EventArgs, IReference
    {
        /// <summary>
        /// 初始化游戏框架中包含事件数据的类的新实例。
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        protected GameFrameworkEventArgs()
        {
        }

        /// <summary>
        /// 清理引用。
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public abstract void Clear();
    }
}