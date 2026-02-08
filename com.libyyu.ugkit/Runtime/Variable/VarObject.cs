using UnityEngine.Scripting;

namespace UGKit.Runtime
{
    /// <summary>
    /// System.Object 变量类。
    /// </summary>
    [Preserve]
    public sealed class VarObject : Variable<object>
    {
        /// <summary>
        /// 初始化 System.Object 变量类的新实例。
        /// </summary>
        [Preserve]
        public VarObject()
        {
        }
    }
}