using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UGKit.Runtime;
using UnityEngine;

namespace UGKit.Web.ProtoBuff.Runtime
{
    /// <summary>
    /// Web 请求组件。
    /// 提供HTTP GET和POST请求功能的Unity组件。
    /// 支持字符串和字节数组格式的请求结果。
    /// 可以设置请求超时时间。
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("UGKit/Framework/Web ProtoBuff")]
    [UnityEngine.Scripting.Preserve]
    public sealed class WebProtoBuffComponent : GameFrameworkComponent
    {
        /// <summary>
        /// Web请求管理器实例
        /// </summary>
        private IWebProtoBuffManager m_WebProtoBuffManager;

        /// <summary>
        /// 请求超时时间配置
        /// </summary>
        [SerializeField] [Tooltip("超时时间.单位：秒")]
        private float m_Timeout = 5f;

        /// <summary>
        /// 获取或设置下载超时时长，以秒为单位。
        /// 当请求超过此时间未完成时会自动终止。
        /// </summary>
        public float Timeout
        {
            get { return m_WebProtoBuffManager.Timeout; }
            set { m_WebProtoBuffManager.Timeout = m_Timeout = value; }
        }

        /// <summary>
        /// 游戏框架组件初始化。
        /// 在此方法中初始化Web管理器并设置超时时间。
        /// </summary>
        protected override void Awake()
        {
            ImplementationComponentType = Utility.Assembly.GetType(componentType);
            InterfaceComponentType = typeof(IWebProtoBuffManager);
            base.Awake();
            m_WebProtoBuffManager = GameFrameworkEntry.GetModule<IWebProtoBuffManager>();
            if (m_WebProtoBuffManager == null)
            {
                Log.Fatal("Web manager is invalid.");
                return;
            }

            m_WebProtoBuffManager.Timeout = m_Timeout;
        }

#if ENABLE_UGKIT_PROTOBUF
        /// <summary>
        /// 发送Post请求，用于发送和接收Protocol Buffer消息。
        /// 此方法仅在启用ENABLE_UGKIT_PROTOBUF宏定义时可用。
        /// </summary>
        /// <param name="url">目标服务器的URL地址</param>
        /// <param name="message">要发送的Protocol Buffer消息对象，必须继承自MessageObject</param>
        /// <typeparam name="T">返回的数据类型，必须继承自MessageObject并且实现IResponseMessage接口</typeparam>
        /// <returns>返回一个任务对象，该任务完成时将包含从服务器接收到的Protocol Buffer响应数据，数据类型为T</returns>
        /// <remarks>
        /// 此方法专门用于处理Protocol Buffer格式的请求和响应。
        /// 发送的消息和接收的响应都必须是Protocol Buffer消息类型。
        /// </remarks>
        public Task<T> Post<T>(string url, UGKit.Network.Runtime.MessageObject message) where T : UGKit.Network.Runtime.MessageObject, UGKit.Network.Runtime.IResponseMessage
        {
            return m_WebProtoBuffManager.Post<T>(url, message);
        }
#endif
    }
}