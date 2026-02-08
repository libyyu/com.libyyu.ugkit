using System;

namespace UGKit.Runtime
{
    public partial class TimerHelper
    {
        /// <summary>
        /// 获取指定时间是否在指定的时间范围内
        /// </summary>
        /// <param name="time">指定时间。例如：2024-01-10 14:30:00</param>
        /// <param name="startTime">开始时间。例如：2024-01-10 00:00:00</param>
        /// <param name="endTime">结束时间。例如：2024-01-10 23:59:59</param>
        /// <returns>如果指定时间在开始时间和结束时间之间（包含边界），则返回true；否则返回false</returns>
        /// <remarks>
        /// 此方法使用闭区间比较，即time等于startTime或endTime时也返回true
        /// 不会对startTime和endTime的先后顺序做检查，调用方需确保startTime不晚于endTime
        /// </remarks>
        public static bool IsTimeInRange(DateTime time, DateTime startTime, DateTime endTime)
        {
            return time >= startTime && time <= endTime;
        }

        /// <summary>
        /// 获取指定时间戳是否在指定的时间戳范围内
        /// </summary>
        /// <param name="timestamp">指定时间戳（Unix秒级时间戳）。例如：1704857400</param>
        /// <param name="startTimestamp">开始时间戳（Unix秒级时间戳）。例如：1704816000</param>
        /// <param name="endTimestamp">结束时间戳（Unix秒级时间戳）。例如：1704902399</param>
        /// <returns>如果指定时间戳在开始时间戳和结束时间戳之间（包含边界），则返回true；否则返回false</returns>
        /// <remarks>
        /// 此方法使用闭区间比较，即timestamp等于startTimestamp或endTimestamp时也返回true
        /// 不会对startTimestamp和endTimestamp的先后顺序做检查，调用方需确保startTimestamp不大于endTimestamp
        /// 时间戳应为Unix秒级时间戳（自1970年1月1日UTC零点以来的秒数）
        /// </remarks>
        public static bool IsTimestampInRange(long timestamp, long startTimestamp, long endTimestamp)
        {
            return timestamp >= startTimestamp && timestamp <= endTimestamp;
        }
    }
}