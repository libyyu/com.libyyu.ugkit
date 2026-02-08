using UnityEngine;

namespace UGKit.Runtime
{
    /// <summary>
    /// 默认版本号辅助器。
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    public class DefaultVersionHelper : Version.IVersionHelper
    {
        /// <summary>
        /// 获取游戏版本号。
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public string GameVersion
        {
            get { return Application.version; }
        }
    }
}