
using UGKit.XLua.Runtime;
using UGKit.Runtime;

public static partial class GameApp
{
    /// <summary>
    /// 获取XLua组件。
    /// </summary>
    public static XLuaComponent XLua
    {
        get
        {
            if (_xlua == null)
            {
                _xlua = GameEntry.GetComponent<XLuaComponent>();
            }

            return _xlua;
        }
    }

    private static XLuaComponent _xlua;
}