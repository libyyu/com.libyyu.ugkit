
using UGKit.Runtime;
using UGKit.Web.ProtoBuff.Runtime;

public static partial class GameApp
{
    /// <summary>
    /// 获取Web ProtoBuff 组件。
    /// </summary>
    public static WebProtoBuffComponent WebProtoBuff
    {
        get
        {
            if (_webProtoBuff == null)
            {
                _webProtoBuff = GameEntry.GetComponent<WebProtoBuffComponent>();
            }

            return _webProtoBuff;
        }
    }

    private static WebProtoBuffComponent _webProtoBuff;
}