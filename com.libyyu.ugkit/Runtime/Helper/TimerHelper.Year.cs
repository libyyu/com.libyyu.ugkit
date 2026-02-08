using System;

namespace UGKit.Runtime
{
    public partial class TimerHelper
    {
        /// <summary>
        /// 获取本年开始时间
        /// </summary>
        /// <returns>本年1月1日零点时间</returns>
        /// <remarks>
        /// 此方法基于UTC时间计算年份:
        /// 1. 获取当前UTC时间的年份
        /// 2. 返回该年份1月1日零点时间
        /// 
        /// 示例:
        /// - 当前UTC时间为2024-03-15 14:30:00
        /// - 返回2024-01-01 00:00:00
        /// 
        /// 注意:
        /// - 返回的是UTC时间,不考虑本地时区
        /// - 返回时间的时分秒毫秒都为0
        /// - 使用DateTime.UtcNow避免时区转换带来的问题
        /// </remarks>
        public static DateTime GetYearStartTime()
        {
            return new DateTime(DateTime.UtcNow.Year, 1, 1);
        }

        /// <summary>
        /// 获取本年开始时间戳
        /// </summary>
        /// <returns>本年1月1日零点时间戳(秒)</returns>
        /// <remarks>
        /// 此方法返回当前年份1月1日零点的Unix时间戳
        /// 使用本地时区计算时间
        /// 例如:2024年返回2024-01-01 00:00:00的时间戳
        /// </remarks>
        public static long GetYearStartTimestamp()
        {
            return new DateTimeOffset(GetYearStartTime()).ToUnixTimeSeconds();
        }

        /// <summary>
        /// 获取本年结束时间
        /// </summary>
        /// <returns>本年12月31日23:59:59的时间</returns>
        /// <remarks>
        /// 此方法返回当前年份最后一天的最后一秒
        /// 使用本地时区计算时间
        /// 例如:2024年返回2024-12-31 23:59:59
        /// </remarks>
        public static DateTime GetYearEndTime()
        {
            return GetYearStartTime().AddYears(1).AddSeconds(-1);
        }

        /// <summary>
        /// 获取本年结束时间戳
        /// </summary>
        /// <returns>本年12月31日23:59:59的时间戳(秒)</returns>
        /// <remarks>
        /// 此方法返回当前年份最后一天的最后一秒的Unix时间戳
        /// 使用本地时区计算时间
        /// 例如:2024年返回2024-12-31 23:59:59的时间戳
        /// </remarks>
        public static long GetYearEndTimestamp()
        {
            return new DateTimeOffset(GetYearEndTime()).ToUnixTimeSeconds();
        }

        /// <summary>
        /// 获取指定日期所在年的开始时间
        /// </summary>
        /// <param name="date">指定日期</param>
        /// <returns>所在年1月1日零点时间</returns>
        /// <remarks>
        /// 此方法返回指定日期所在年份的1月1日零点时间
        /// 例如:输入2024-01-10,返回2024-01-01 00:00:00
        /// 保持原有时区不变
        /// </remarks>
        public static DateTime GetStartTimeOfYear(DateTime date)
        {
            return new DateTime(date.Year, 1, 1);
        }

        /// <summary>
        /// 获取指定日期所在年的开始时间戳
        /// </summary>
        /// <param name="date">指定日期</param>
        /// <returns>所在年1月1日零点时间戳(秒)</returns>
        /// <remarks>
        /// 此方法返回指定日期所在年份的1月1日零点时间的Unix时间戳
        /// 例如:输入2024-01-10,返回2024-01-01 00:00:00的时间戳
        /// 会将时间转换为UTC时间后再计算时间戳
        /// </remarks>
        public static long GetStartTimestampOfYear(DateTime date)
        {
            return new DateTimeOffset(GetStartTimeOfYear(date)).ToUnixTimeSeconds();
        }

        /// <summary>
        /// 获取指定日期所在年的结束时间
        /// </summary>
        /// <param name="date">指定日期</param>
        /// <returns>所在年12月31日23:59:59的时间</returns>
        /// <remarks>
        /// 此方法返回指定日期所在年份的12月31日最后一秒
        /// 例如:输入2024-01-10,返回2024-12-31 23:59:59
        /// 保持原有时区不变
        /// </remarks>
        public static DateTime GetEndTimeOfYear(DateTime date)
        {
            return GetStartTimeOfYear(date).AddYears(1).AddSeconds(-1);
        }

        /// <summary>
        /// 获取指定日期所在年的结束时间戳
        /// </summary>
        /// <param name="date">指定日期</param>
        /// <returns>所在年12月31日23:59:59的时间戳(秒)</returns>
        /// <remarks>
        /// 此方法返回指定日期所在年份的12月31日最后一秒的Unix时间戳
        /// 例如:输入2024-01-10,返回2024-12-31 23:59:59的时间戳
        /// 会将时间转换为UTC时间后再计算时间戳
        /// </remarks>
        public static long GetEndTimestampOfYear(DateTime date)
        {
            return new DateTimeOffset(GetEndTimeOfYear(date)).ToUnixTimeSeconds();
        }
    }
}