using UnityEngine;

namespace UGKit.Runtime
{
    /// <summary>
    /// 对 Unity 的扩展方法。
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    public static class UnityEngineVector3Extension
    {
        /// <summary>
        /// 取 <see cref="Vector3" /> 的 (x, y, z) 转换为 <see cref="Vector2" /> 的 (x, z)。
        /// </summary>
        /// <param name="self">要转换的 Vector3。</param>
        /// <returns>转换后的 Vector2。</returns>
        [UnityEngine.Scripting.Preserve]
        public static Vector2 ToVector2(this Vector3 self)
        {
            return new Vector2(self.x, self.z);
        }

        /// <summary>
        /// 取 <see cref="Vector2" /> 的 (x, y) 转换为 <see cref="Vector3" /> 的 (x, 0, y)。
        /// </summary>
        /// <param name="self">要转换的 Vector3。</param>
        /// <returns>转换后的 Vector3。</returns>
        [UnityEngine.Scripting.Preserve]
        public static Vector3 ToVector3(this Vector3Int self)
        {
            return new Vector3(self.x, self.y, self.z);
        }

        /// <summary>
        /// 将 Vector3 转换为 Vector4，在 w 分量上设置为 0
        /// </summary>
        /// <param name="self">源 Vector3 对象</param>
        /// <returns>包含源 Vector3 的 x、y、z 分量且 w 分量为 0 的新 Vector4 对象</returns>
        public static Vector4 ToVector4(this Vector3 self)
        {
            return new Vector4(self.x, self.y, self.z, 0);
        }
    }
}