using System;
using UnityEngine;

namespace UGKit.Runtime
{
    /// <summary>
    /// 默认游戏框架日志辅助器。
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    public class DefaultLogHelper : GameFrameworkLog.ILogHelper
    {
        /// <summary>
        /// 记录日志。
        /// </summary>
        /// <param name="level">日志等级。</param>
        /// <param name="message">日志内容。</param>
        [UnityEngine.Scripting.Preserve] // Added Preserve attribute
        public void Log(GameFrameworkLogLevel level, object message)
        {
            var time = $"[Unity]:[{DateTime.Now:HH:mm:ss.fff}]:";

            switch (level)
            {
                case GameFrameworkLogLevel.Debug:
                    Debug.Log(ColorizeLogMessage(level, $"{time}{message}"));
                    break;

                case GameFrameworkLogLevel.Info:
                    Debug.Log(ColorizeLogMessage(level, $"{time}{message}"));
                    break;

                case GameFrameworkLogLevel.Warning:
                    Debug.LogWarning(ColorizeLogMessage(level, $"{time}{message}"));
                    break;

                case GameFrameworkLogLevel.Error:
                    Debug.LogError(ColorizeLogMessage(level, $"{time}{message}"));
                    break;

                case GameFrameworkLogLevel.Fatal:
                default:
                    throw new GameFrameworkException($"{time}{message}");
            }
        }

        static object ColorizeLogMessage(GameFrameworkLogLevel logLevel, object message)
        {
#if UNITY_EDITOR
            switch (logLevel)
            {
                case GameFrameworkLogLevel.Debug:
                    return $"<color=#00ffd8><b>[DEBUG] ► </b></color><color=#72ffe9>{message}</color>";
                case GameFrameworkLogLevel.Info:
                    return $"<color=#00ff73><b>[INFO] ► </b></color><color=#72ffb2>{message}</color>";
                case GameFrameworkLogLevel.Warning:
                    return $"<color=#ffc400><b>[WARNING] ► </b></color><color=#ffdf71>{message}</color>";
                case GameFrameworkLogLevel.Error:
                    return $"<color=#ff003c><b>[ERROR] ► </b></color><color=#ff7293>{message}</color>";
                case GameFrameworkLogLevel.Fatal:
                    return $"<color=#ff003c><b>[FATAL] ► </b></color><color=#ff7293>{message}</color>";
            }

            return message;
#else
            return message;
#endif
        }
    }
}