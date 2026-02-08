using UnityEngine;

namespace UGKit.Runtime
{
    /// <summary>
    /// 对 Unity 的扩展方法。
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    public static class UnityEngineVector4Extension
    {
        /// <summary>
        /// 将 Vector4 转换为 Vector2，丢弃 z 和 w 分量
        /// </summary>
        /// <param name="self">源 Vector4 对象</param>
        /// <returns>包含源 Vector4 的 x 和 y 分量的新 Vector2 对象</returns>
        public static Vector2 ToVector2(this Vector4 self)
        {
            return new Vector2(self.x, self.y);
        }

        /// <summary>
        /// 将 Vector4 转换为 Vector3，丢弃 w 分量
        /// </summary>
        /// <param name="self">源 Vector4 对象</param>
        /// <returns>包含源 Vector4 的 x、y 和 z 分量的新 Vector3 对象</returns>
        public static Vector3 ToVector3(this Vector4 self)
        {
            return new Vector3(self.x, self.y, self.z);
        }
    }
}