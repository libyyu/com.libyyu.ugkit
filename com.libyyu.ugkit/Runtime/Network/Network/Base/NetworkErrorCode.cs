namespace UGKit.Network.Runtime
{
    /// <summary>
    /// 网络关闭原因。
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    public static class NetworkCloseReason
    {
        /// <summary>
        /// 正常关闭。
        /// </summary>
        public const string Normal = "Normal";

        /// <summary>
        /// 超时关闭。
        /// </summary>
        public const string Timeout = "Timeout";

        /// <summary>
        /// 资源释放关闭。
        /// </summary>
        public const string Dispose = "Dispose";

        /// <summary>
        /// 连接关闭。
        /// </summary>
        public const string ConnectClose = "ConnectClose";

        /// <summary>
        /// 连接地址错误关闭。
        /// </summary>
        public const string ConnectAddressError = "ConnectAddressError";

        /// <summary>
        /// 连接地址异常错误关闭。
        /// </summary>
        public const string ConnectAddressExceptionError = "ConnectAddressExceptionError";

        /// <summary>
        /// 缺失心跳关闭。
        /// </summary>
        public const string MissHeartBeat = "MissHeartBeat";
    }

    /// <summary>
    /// 网络错误码。
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    public enum NetworkErrorCode : byte
    {
        /// <summary>
        /// 未知错误。
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// 地址族错误。
        /// </summary>
        AddressFamilyError,

        /// <summary>
        /// Socket 错误。
        /// </summary>
        SocketError,

        /// <summary>
        /// 连接错误。
        /// </summary>
        ConnectError,

        /// <summary>
        /// 发送错误。
        /// </summary>
        SendError,

        /// <summary>
        /// 接收错误。
        /// </summary>
        ReceiveError,

        /// <summary>
        /// 序列化错误。
        /// </summary>
        SerializeError,

        /// <summary>
        /// 反序列化消息包头错误。
        /// </summary>
        DeserializePacketHeaderError,

        /// <summary>
        /// 反序列化消息包错误。
        /// </summary>
        DeserializePacketError,

        /// <summary>
        /// 缺失心跳错误。
        /// </summary>
        MissHeartBeatError,

        /// <summary>
        /// 资源释放错误。
        /// </summary>
        DisposeError,
    }
}