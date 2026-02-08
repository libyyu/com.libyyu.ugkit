using UGKit.Editor;
using UnityEditor;

namespace UGKit.Config.Editor
{
    /// <summary>
    /// 配置表二进制功能脚本宏定义。
    /// </summary>
    public static class ConfigDefineSymbols
    {
        public const string EnableBinaryConfigScriptingDefineSymbol = "ENABLE_BINARY_CONFIG";

        /// <summary>
        /// 禁用配置表为二进制的脚本宏定义。
        /// </summary>
        [MenuItem("UGKit/Scripting Define Symbols/Config/Disable Binary Config(关闭二进制配置表)", false, 500)]
        public static void DisableBinaryConfig()
        {
            ScriptingDefineSymbols.RemoveScriptingDefineSymbol(EnableBinaryConfigScriptingDefineSymbol);
        }

        /// <summary>
        /// 开启配置表为二进制的脚本宏定义。
        /// </summary>
        [MenuItem("UGKit/Scripting Define Symbols/Config/Enable Binary Config(开启二进制配置表)", false, 501)]
        public static void EnableBinaryConfig()
        {
            ScriptingDefineSymbols.AddScriptingDefineSymbol(EnableBinaryConfigScriptingDefineSymbol);
        }
    }
}