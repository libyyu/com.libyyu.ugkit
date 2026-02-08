using System;

namespace UGKit.Runtime
{
    public partial class TimerHelper
    {
        /// <summary>
        /// 时区偏移秒数。用于调整时间计算的偏移量。
        /// 正值表示向未来偏移,负值表示向过去偏移。
        /// </summary>
        public static long TimeOffsetSeconds { get; private set; } = 0;

        /// <summary>
        /// 时区偏移毫秒数。用于调整时间计算的偏移量。
        /// 正值表示向未来偏移,负值表示向过去偏移。
        /// </summary>
        public static long TimeOffsetMilliseconds { get; private set; } = 0;

        /// <summary>
        /// 设置时区偏移值
        /// </summary>
        /// <param name="offsetSeconds">秒级偏移量</param>
        /// <param name="offsetMilliseconds">毫秒级偏移量</param>
        /// <remarks>
        /// 此方法用于调整时间计算的基准。
        /// 例如要模拟未来时间,可以传入正数;要模拟过去时间,可以传入负数。
        /// 通常用于调试和测试场景。
        /// </remarks>
        public static void SetTimeOffset(long offsetSeconds, long offsetMilliseconds)
        {
            TimeOffsetSeconds = offsetSeconds;
            TimeOffsetMilliseconds = offsetMilliseconds;
        }

        /// <summary>
        /// 重置时区偏移值为默认值(0)
        /// </summary>
        /// <remarks>
        /// 此方法会将秒级和毫秒级的偏移量都重置为0,
        /// 使时间计算恢复到未经调整的状态。
        /// </remarks>
        public static void ResetTimeOffset()
        {
            TimeOffsetSeconds = default;
            TimeOffsetMilliseconds = default;
        }

        /// <summary>
        /// 获取当前UTC时间的秒级时间戳
        /// </summary>
        /// <returns>返回自1970年1月1日 00:00:00 UTC以来经过的秒数,加上时区偏移量</returns>
        /// <remarks>
        /// 此方法:
        /// 1. 获取当前UTC时间
        /// 2. 转换为Unix时间戳(秒)
        /// 3. 加上TimeOffsetSeconds偏移量
        /// 主要用于需要UTC时间戳的场景,如跨时区业务
        /// </remarks>
        public static long UnixTimeSecondsWithOffset()
        {
            return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds() + TimeOffsetSeconds;
        }

        /// <summary>
        /// 获取当前UTC时间的毫秒级时间戳
        /// </summary>
        /// <returns>返回自1970年1月1日 00:00:00 UTC以来经过的毫秒数,加上时区偏移量</returns>
        /// <remarks>
        /// 此方法:
        /// 1. 获取当前UTC时间
        /// 2. 转换为Unix时间戳(毫秒)
        /// 3. 加上TimeOffsetMilliseconds偏移量
        /// 相比秒级时间戳提供更高的精度,适用于需要精确时间计算的场景
        /// </remarks>
        public static long UnixTimeMillisecondsWithOffset()
        {
            return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds() + TimeOffsetMilliseconds;
        }

        /// <summary>
        /// 获取当前本地时区时间的秒级时间戳
        /// </summary>
        /// <returns>返回自1970年1月1日 00:00:00以来经过的秒数(本地时区),加上时区偏移量</returns>
        /// <remarks>
        /// 此方法:
        /// 1. 获取当前本地时区时间
        /// 2. 转换为Unix时间戳(秒)
        /// 3. 加上TimeOffsetSeconds偏移量
        /// 主要用于需要本地时区时间戳的场景
        /// </remarks>
        public static long TimeSecondsWithOffset()
        {
            return new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds() + TimeOffsetSeconds;
        }

        /// <summary>
        /// 获取当前本地时区时间的毫秒级时间戳
        /// </summary>
        /// <returns>返回自1970年1月1日 00:00:00以来经过的毫秒数(本地时区),加上时区偏移量</returns>
        /// <remarks>
        /// 此方法:
        /// 1. 获取当前本地时区时间
        /// 2. 转换为Unix时间戳(毫秒)
        /// 3. 加上TimeOffsetMilliseconds偏移量
        /// 相比秒级时间戳提供更高的精度,适用于需要精确时间计算的场景
        /// </remarks>
        public static long TimeMillisecondsWithOffset()
        {
            return new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds() + TimeOffsetMilliseconds;
        }
    }
}