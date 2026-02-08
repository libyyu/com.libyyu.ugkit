using System;

namespace UGKit.Runtime
{
    public partial class TimerHelper
    {
        /// <summary>
        /// 获取指定日期所在月的开始时间
        /// </summary>
        /// <param name="date">指定日期</param>
        /// <returns>所在月1号零点时间</returns>
        /// <remarks>
        /// 此方法返回指定日期所在月份的1号零点时间
        /// 例如:输入2024-01-10,返回2024-01-01 00:00:00
        /// 保持原有时区不变
        /// </remarks>
        public static DateTime GetStartTimeOfMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        /// <summary>
        /// 获取指定日期所在月的开始时间戳
        /// </summary>
        /// <param name="date">指定日期</param>
        /// <returns>所在月1号零点时间戳(秒)</returns>
        /// <remarks>
        /// 此方法返回指定日期所在月份的1号零点时间的Unix时间戳
        /// 例如:输入2024-01-10,返回2024-01-01 00:00:00的时间戳
        /// 会将时间转换为UTC时间后再计算时间戳
        /// </remarks>
        public static long GetStartTimestampOfMonth(DateTime date)
        {
            return new DateTimeOffset(GetStartTimeOfMonth(date)).ToUnixTimeSeconds();
        }

        /// <summary>
        /// 获取指定日期所在月的结束时间
        /// </summary>
        /// <param name="date">指定日期</param>
        /// <returns>所在月最后一天23:59:59的时间</returns>
        /// <remarks>
        /// 此方法返回指定日期所在月份的最后一天的最后一秒
        /// 例如:输入2024-01-10,返回2024-01-31 23:59:59
        /// 保持原有时区不变
        /// 自动处理大小月份和闰年
        /// </remarks>
        public static DateTime GetEndTimeOfMonth(DateTime date)
        {
            return GetStartTimeOfMonth(date).AddMonths(1).AddSeconds(-1);
        }

        /// <summary>
        /// 获取指定日期所在月的结束时间戳
        /// </summary>
        /// <param name="date">指定日期</param>
        /// <returns>所在月最后一天23:59:59的时间戳(秒)</returns>
        /// <remarks>
        /// 此方法返回指定日期所在月份的最后一天最后一秒的Unix时间戳
        /// 例如:输入2024-01-10,返回2024-01-31 23:59:59的时间戳
        /// 会将时间转换为UTC时间后再计算时间戳
        /// </remarks>
        public static long GetEndTimestampOfMonth(DateTime date)
        {
            return new DateTimeOffset(GetEndTimeOfMonth(date)).ToUnixTimeSeconds();
        }

        /// <summary>
        /// 获取下月开始时间戳
        /// </summary>
        /// <returns>下月1号零点时间戳(秒)</returns>
        /// <remarks>
        /// 此方法返回下个月1号零点时间的Unix时间戳
        /// 例如:当前是2024-01-10,返回2024-02-01 00:00:00的时间戳
        /// 会将时间转换为UTC时间后再计算时间戳
        /// </remarks>
        public static long GetNextMonthStartTimestamp()
        {
            return new DateTimeOffset(GetNextMonthStartTime()).ToUnixTimeSeconds();
        }

        /// <summary>
        /// 获取下月结束时间
        /// </summary>
        /// <returns>下月最后一天23:59:59的时间</returns>
        /// <remarks>
        /// 此方法返回下个月最后一天的最后一秒
        /// 例如:当前是2024-01-10,返回2024-02-29 23:59:59
        /// 使用本地时区计算时间
        /// 自动处理大小月份和闰年
        /// </remarks>
        public static DateTime GetNextMonthEndTime()
        {
            return GetNextMonthStartTime().AddMonths(1).AddSeconds(-1);
        }

        /// <summary>
        /// 获取下月结束时间戳
        /// </summary>
        /// <returns>下月最后一天23:59:59的时间戳(秒)</returns>
        /// <remarks>
        /// 此方法返回下个月最后一天最后一秒的Unix时间戳
        /// 例如:当前是2024-01-10,返回2024-02-29 23:59:59的时间戳
        /// 会将时间转换为UTC时间后再计算时间戳
        /// </remarks>
        public static long GetNextMonthEndTimestamp()
        {
            return new DateTimeOffset(GetNextMonthEndTime()).ToUnixTimeSeconds();
        }

        /// <summary>
        /// 获取下月开始时间
        /// </summary>
        /// <returns>下月1号零点时间</returns>
        /// <remarks>
        /// 此方法返回下个月1号的零点时间
        /// 例如:当前是2024-01-10,返回2024-02-01 00:00:00
        /// 使用本地时区计算时间
        /// </remarks>
        public static DateTime GetNextMonthStartTime()
        {
            return GetMonthStartTime().AddMonths(1);
        }

        /// <summary>
        /// 获取本月开始时间
        /// </summary>
        /// <returns>本月1号零点时间</returns>
        /// <remarks>
        /// 此方法基于UTC时间计算本月开始时间:
        /// 1. 获取当前UTC时间的年份和月份
        /// 2. 创建一个新的DateTime对象,设置为本月1号零点
        /// 3. 返回的时间为UTC时区的时间
        /// 
        /// 示例:
        /// - 当前UTC时间为2024-01-15 14:30:00
        /// - 返回时间为2024-01-01 00:00:00 (UTC)
        /// 
        /// 注意:
        /// - 返回的是UTC时区的时间,如需本地时间请使用TimeZoneInfo.ConvertTimeFromUtc转换
        /// - 返回时间的Hour/Minute/Second/Millisecond均为0
        /// </remarks>
        public static DateTime GetMonthStartTime()
        {
            return new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
        }

        /// <summary>
        /// 获取本月开始时间戳
        /// </summary>
        /// <returns>本月1号零点时间戳(秒)</returns>
        public static long GetMonthStartTimestamp()
        {
            return new DateTimeOffset(GetMonthStartTime()).ToUnixTimeSeconds();
        }

        /// <summary>
        /// 获取本月结束时间
        /// </summary>
        /// <returns>本月最后一天23:59:59的时间</returns>
        public static DateTime GetMonthEndTime()
        {
            return GetMonthStartTime().AddMonths(1).AddSeconds(-1);
        }

        /// <summary>
        /// 获取本月结束时间戳
        /// </summary>
        /// <returns>本月最后一天23:59:59的时间戳(秒)</returns>
        public static long GetMonthEndTimestamp()
        {
            return new DateTimeOffset(GetMonthEndTime()).ToUnixTimeSeconds();
        }
    }
}