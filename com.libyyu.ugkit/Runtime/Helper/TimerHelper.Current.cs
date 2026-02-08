using System;

namespace UGKit.Runtime
{
    public partial class TimerHelper
    {
        /// <summary>
        /// 获取当前UTC时间，格式为HHmmss的字符串
        /// </summary>
        /// <returns>返回一个6位字符串，表示当前UTC时间。例如：143045表示14:30:45</returns>
        /// <remarks>
        /// 此方法将当前UTC时间转换为6位时间字符串:
        /// - 前2位表示小时(24小时制)
        /// - 中间2位表示分钟
        /// - 最后2位表示秒
        /// 使用DateTime.UtcNow获取UTC时间
        /// </remarks>
        public static string CurrentTimeWithUtcFullString()
        {
            return DateTime.UtcNow.ToString("HHmmss");
        }

        /// <summary>
        /// 获取当前本地时间，格式为HHmmss的字符串
        /// </summary>
        /// <returns>返回一个6位字符串，表示当前本地时间。例如：143045表示14:30:45</returns>
        /// <remarks>
        /// 此方法将当前本地时间转换为6位时间字符串:
        /// - 前2位表示小时(24小时制)
        /// - 中间2位表示分钟
        /// - 最后2位表示秒
        /// 使用DateTime.Now获取本地时间
        /// </remarks>
        public static string CurrentTimeWithLocalFullString()
        {
            return DateTime.Now.ToString("HHmmss");
        }

        /// <summary>
        /// 获取当前UTC时间，格式为HHmmss的整数
        /// </summary>
        /// <returns>返回一个6位整数，表示当前UTC时间。例如：143045表示14:30:45</returns>
        /// <remarks>
        /// 此方法将当前UTC时间转换为6位整数:
        /// - 前2位表示小时(24小时制)
        /// - 中间2位表示分钟
        /// - 最后2位表示秒
        /// 内部调用CurrentTimeWithUtcFullString()获取字符串后转换为整数
        /// </remarks>
        public static int CurrentTimeWithUtcTime()
        {
            return Convert.ToInt32(CurrentTimeWithUtcFullString());
        }

        /// <summary>
        /// 获取当前本地时间，格式为HHmmss的整数
        /// </summary>
        /// <returns>返回一个6位整数，表示当前本地时间。例如：143045表示14:30:45</returns>
        /// <remarks>
        /// 此方法将当前本地时间转换为6位整数:
        /// - 前2位表示小时(24小时制)
        /// - 中间2位表示分钟
        /// - 最后2位表示秒
        /// 内部调用CurrentTimeWithLocalFullString()获取字符串后转换为整数
        /// </remarks>
        public static int CurrentTimeWithLocalTime()
        {
            return Convert.ToInt32(CurrentTimeWithLocalFullString());
        }

        /// <summary>
        /// 获取当前本地时区时间的自定义格式字符串
        /// </summary>
        /// <param name="format">时间格式字符串，默认为"yyyy-MM-dd HH:mm:ss.fff K"</param>
        /// <returns>返回指定格式的本地时间字符串。例如默认格式返回："2023-12-25 14:30:45.123 +08:00"</returns>
        /// <remarks>
        /// 此方法允许自定义时间格式字符串:
        /// - 默认格式包含年月日时分秒毫秒和时区信息
        /// - 可以通过format参数指定其他格式
        /// - 使用DateTime.Now获取本地时间
        /// 支持标准的.NET日期时间格式说明符
        /// </remarks>
        public static string CurrentDateTimeWithFormat(string format = "yyyy-MM-dd HH:mm:ss.fff K")
        {
            return DateTime.Now.ToString(format);
        }

        /// <summary>
        /// 获取当前UTC时区时间的自定义格式字符串
        /// </summary>
        /// <param name="format">时间格式字符串，默认为"yyyy-MM-dd HH:mm:ss.fff K"</param>
        /// <returns>返回指定格式的UTC时间字符串。例如默认格式返回："2023-12-25 06:30:45.123 +00:00"</returns>
        /// <remarks>
        /// 此方法允许自定义UTC时间格式字符串:
        /// - 默认格式包含年月日时分秒毫秒和时区信息
        /// - 可以通过format参数指定其他格式
        /// - 使用DateTime.UtcNow获取UTC时间
        /// 支持标准的.NET日期时间格式说明符
        /// </remarks>
        public static string CurrentDateTimeWithUtcFormat(string format = "yyyy-MM-dd HH:mm:ss.fff K")
        {
            return DateTime.UtcNow.ToString(format);
        }

        /// <summary>
        /// 获取当前UTC时间
        /// </summary>
        /// <returns>当前UTC时间</returns>
        /// <remarks>
        /// 此方法返回当前的UTC时间(协调世界时)
        /// 与本地时间相比会有时区偏移
        /// 主要用于需要统一时间标准的场景
        /// </remarks>
        public static DateTime GetUtcNow()
        {
            return DateTime.UtcNow;
        }

        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <returns>当前时间</returns>
        /// <remarks>
        /// 此方法返回当前的本地时间
        /// 会根据系统设置的时区自动调整
        /// 主要用于需要显示本地时间的场景
        /// </remarks>
        public static DateTime GetNow()
        {
            return DateTime.Now;
        }
    }
}