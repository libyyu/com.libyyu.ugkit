using UGKit.Runtime;
using UGKit.UI.Runtime;
using UnityEngine;

namespace UGKit.UI.UGUI.Runtime
{
    /// <summary>
    /// 界面管理器。
    /// </summary>
    internal sealed partial class UIManager
    {
        /// <summary>
        /// 回收界面实例对象。
        /// </summary>
        /// <param name="uiForm"></param>
        /// <param name="isDispose">是否销毁释放</param>
        protected override void RecycleUIForm(IUIForm uiForm, bool isDispose = false)
        {
            uiForm.OnRecycle();
            var formHandle = uiForm.Handle as GameObject;
            if (!formHandle)
            {
                return;
            }

            m_InstancePool.Unspawn(uiForm.Handle);
            if (isDispose)
            {
                m_InstancePool.ReleaseObject(uiForm.Handle);
            }
        }
    }
}
