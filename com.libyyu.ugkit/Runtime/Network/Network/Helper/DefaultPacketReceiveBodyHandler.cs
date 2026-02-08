#if ENABLE_UGKIT_PROTOBUF
using ProtoBuf;
using UGKit.Runtime;
#endif

namespace UGKit.Network.Runtime
{
    /// <summary>
    /// 默认消息接收内容处理器
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    public sealed class DefaultPacketReceiveBodyHandler : IPacketReceiveBodyHandler, IPacketHandler
    {
        public bool Handler<T>(byte[] source, int messageId, out T messageObject) where T : MessageObject
        {
#if ENABLE_UGKIT_PROTOBUF
            var messageType = ProtoMessageIdHandler.GetRespTypeById(messageId);
            messageObject = (T)SerializerHelper.Deserialize(source, messageType);
            return true;
#else
            Log.Warning($"[{nameof(DefaultPacketReceiveBodyHandler)}]请先确认是否安装ProtobufBuf运行库");
            messageObject = default;    
            return false;  
#endif
        }
    }
}