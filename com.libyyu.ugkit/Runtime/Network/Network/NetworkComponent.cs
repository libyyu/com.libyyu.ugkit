using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UGKit.Event.Runtime;
using UGKit.Runtime;
using UnityEngine;

namespace UGKit.Network.Runtime
{
    /// <summary>
    /// 网络组件。
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("UGKit/Framework/Network")]
    [UnityEngine.Scripting.Preserve]
    public sealed class NetworkComponent : GameFrameworkComponent
    {
        private INetworkManager m_NetworkManager = null;
        private EventComponent m_EventComponent = null;

        /// <summary>
        /// 忽略发送的网络消息ID的日志打印
        /// </summary>
        [SerializeField] private List<int> m_IgnoredSendNetworkIds = new List<int>();

        /// <summary>
        /// 忽略接收的网络消息ID的日志打印
        /// </summary>
        [SerializeField] private List<int> m_IgnoredReceiveNetworkIds = new List<int>();

        /// <summary>
        /// RPC超时时间，以毫秒为单位,默认为5秒
        /// </summary>
        [SerializeField] private int m_rpcTimeout = 5000;

        /// <summary>
        /// 是否在应用程序获得焦点时发送心跳包,默认为false
        /// </summary>
        [SerializeField] private bool m_FocusHeartbeat = false;

        /// <summary>
        /// 获取网络频道数量。
        /// </summary>
        public int NetworkChannelCount
        {
            get { return m_NetworkManager.NetworkChannelCount; }
        }


        /// <summary>
        /// 应用程序获得或失去焦点时调用。
        /// </summary>
        /// <param name="hasFocus">应用程序是否获得焦点。</param>
        private void OnApplicationFocus(bool hasFocus)
        {
            if (!m_FocusHeartbeat)
            {
                return;
            }

            m_NetworkManager.SetFocusHeartbeat(hasFocus);
            Log.Info($"SetFocusHeartbeat: {hasFocus}");
        }

        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected override void OnPreCreate()
        {
            ImplementationComponentType = Utility.Assembly.GetType(componentType);
            InterfaceComponentType = typeof(INetworkManager);
            base.OnPreCreate();
            m_NetworkManager = GameFrameworkEntry.GetModule<INetworkManager>();
            if (m_NetworkManager == null)
            {
                Log.Fatal("Network manager is invalid.");
                return;
            }

            m_NetworkManager.NetworkConnected += OnNetworkConnected;
            m_NetworkManager.NetworkClosed += OnNetworkClosed;
            m_NetworkManager.NetworkMissHeartBeat += OnNetworkMissHeartBeat;
            m_NetworkManager.NetworkError += OnNetworkError;
        }

        protected override async UniTask OnStart()
        {
            await UniTask.CompletedTask;
            m_EventComponent = GameEntry.GetComponent<EventComponent>();
            if (m_EventComponent == null)
            {
                Log.Fatal("Event component is invalid.");
                return;
            }
        }

        /// <summary>
        /// 检查是否存在网络频道。
        /// </summary>
        /// <param name="channelName">网络频道名称。</param>
        /// <returns>是否存在网络频道。</returns>
        [UnityEngine.Scripting.Preserve]
        public bool HasNetworkChannel(string channelName)
        {
            GameFrameworkGuard.NotNullOrEmpty(channelName, nameof(channelName));
            return m_NetworkManager.HasNetworkChannel(channelName);
        }

        /// <summary>
        /// 获取网络频道。
        /// </summary>
        /// <param name="channelName">网络频道名称。</param>
        /// <returns>要获取的网络频道。</returns>
        [UnityEngine.Scripting.Preserve]
        public INetworkChannel GetNetworkChannel(string channelName)
        {
            GameFrameworkGuard.NotNullOrEmpty(channelName, nameof(channelName));
            return m_NetworkManager.GetNetworkChannel(channelName);
        }

        /// <summary>
        /// 获取所有网络频道。
        /// </summary>
        /// <returns>所有网络频道。</returns>
        [UnityEngine.Scripting.Preserve]
        public INetworkChannel[] GetAllNetworkChannels()
        {
            return m_NetworkManager.GetAllNetworkChannels();
        }

        /// <summary>
        /// 获取所有网络频道。
        /// </summary>
        /// <param name="results">所有网络频道。</param>
        [UnityEngine.Scripting.Preserve]
        public void GetAllNetworkChannels(List<INetworkChannel> results)
        {
            m_NetworkManager.GetAllNetworkChannels(results);
        }

        /// <summary>
        /// 创建网络频道。
        /// </summary>
        /// <param name="channelName">网络频道名称。</param>
        /// <param name="networkChannelHelper">网络频道辅助器。</param>
        /// <returns>要创建的网络频道。</returns>
        [UnityEngine.Scripting.Preserve]
        public INetworkChannel CreateNetworkChannel(string channelName, INetworkChannelHelper networkChannelHelper)
        {
            GameFrameworkGuard.NotNullOrEmpty(channelName, nameof(channelName));
            var networkChannel = m_NetworkManager.CreateNetworkChannel(channelName, networkChannelHelper, m_rpcTimeout);
            networkChannel.SetIgnoreLogNetworkIds(m_IgnoredSendNetworkIds, m_IgnoredReceiveNetworkIds);
            return networkChannel;
        }

        /// <summary>
        /// 销毁网络频道。
        /// </summary>
        /// <param name="channelName">网络频道名称。</param>
        /// <returns>是否销毁网络频道成功。</returns>
        [UnityEngine.Scripting.Preserve]
        public bool DestroyNetworkChannel(string channelName)
        {
            GameFrameworkGuard.NotNullOrEmpty(channelName, nameof(channelName));
            return m_NetworkManager.DestroyNetworkChannel(channelName);
        }

        private void OnNetworkConnected(object sender, NetworkConnectedEventArgs eventArgs)
        {
            m_EventComponent.Fire(this, eventArgs);
        }

        private void OnNetworkClosed(object sender, NetworkClosedEventArgs eventArgs)
        {
            m_EventComponent.Fire(this, eventArgs);
        }

        private void OnNetworkMissHeartBeat(object sender, NetworkMissHeartBeatEventArgs eventArgs)
        {
            m_EventComponent.Fire(this, eventArgs);
        }

        private void OnNetworkError(object sender, NetworkErrorEventArgs eventArgs)
        {
            m_EventComponent.Fire(this, eventArgs);
        }
    }
}