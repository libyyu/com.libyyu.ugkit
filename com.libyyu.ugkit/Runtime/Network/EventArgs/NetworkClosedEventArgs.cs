using UGKit.Event.Runtime;
using UGKit.Runtime;

namespace UGKit.Network.Runtime
{
    /// <summary>
    /// 网络连接关闭事件。
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    public sealed class NetworkClosedEventArgs : GameEventArgs
    {
        /// <summary>
        /// 网络连接关闭事件编号。
        /// </summary>
        public static readonly string EventId = typeof(NetworkClosedEventArgs).FullName;

        /// <summary>
        /// 获取网络连接关闭事件编号。
        /// </summary>
        public override string Id
        {
            get { return EventId; }
        }

        /// <summary>
        /// 初始化网络连接关闭事件的新实例。
        /// </summary>
        public NetworkClosedEventArgs()
        {
            NetworkChannel = null;
        }

        /// <summary>
        /// 获取网络频道。
        /// </summary>
        public INetworkChannel NetworkChannel { get; private set; }

        /// <summary>
        /// 创建网络连接关闭事件。
        /// </summary>
        /// <param name="networkChannel">网络频道。</param>
        /// <param name="reason">关闭原因。</param>
        /// <param name="errorCode">错误码。</param>
        /// <returns>创建的网络连接关闭事件。</returns>
        public static NetworkClosedEventArgs Create(INetworkChannel networkChannel, string reason, ushort errorCode)
        {
            NetworkClosedEventArgs networkClosedEventArgs = ReferencePool.Acquire<NetworkClosedEventArgs>();
            networkClosedEventArgs.NetworkChannel = networkChannel;
            networkClosedEventArgs.Reason = reason;
            networkClosedEventArgs.ErrorCode = errorCode;
            return networkClosedEventArgs;
        }

        /// <summary>
        /// 获取关闭原因。
        /// </summary>
        public string Reason { get; private set; }

        /// <summary>
        /// 获取错误码。
        /// </summary>
        public ushort ErrorCode { get; private set; }


        /// <summary>
        /// 清理网络连接关闭事件。
        /// </summary>
        public override void Clear()
        {
            NetworkChannel = null;
        }
    }
}