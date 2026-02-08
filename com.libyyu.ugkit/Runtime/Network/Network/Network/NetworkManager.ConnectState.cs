namespace UGKit.Network.Runtime
{
    public sealed partial class NetworkManager
    {
        public sealed class ConnectState
        {
            private readonly INetworkSocket m_Socket;

            [UnityEngine.Scripting.Preserve]
            public ConnectState(INetworkSocket socket, object userData)
            {
                m_Socket = socket;
                UserData = userData;
            }

            /// <summary>
            /// Socket
            /// </summary>
            public INetworkSocket Socket
            {
                get { return m_Socket; }
            }

            /// <summary>
            /// 用户自定义数据
            /// </summary>
            public object UserData { get; }
        }
    }
}