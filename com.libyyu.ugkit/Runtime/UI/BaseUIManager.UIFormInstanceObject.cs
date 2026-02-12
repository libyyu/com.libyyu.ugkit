using UGKit.ObjectPool;
using UGKit.Runtime;

namespace UGKit.UI.Runtime
{
    public partial class BaseUIManager
    {
        /// <summary>
        /// 界面实例对象。
        /// </summary>
        public sealed class UIFormInstanceObject : ObjectBase
        {
            private object m_UIFormAsset = null;
            private IUIFormHelper m_UIFormHelper = null;
            private object m_AssetHandle = null;

            public static UIFormInstanceObject Create(string name, object uiFormAsset, object uiFormInstance, IUIFormHelper uiFormHelper, object assetHandle)
            {
                if (uiFormAsset == null)
                {
                    throw new GameFrameworkException("UI form asset is invalid.");
                }

                if (uiFormHelper == null)
                {
                    throw new GameFrameworkException("UI form helper is invalid.");
                }

                var uiFormInstanceObject = ReferencePool.Acquire<UIFormInstanceObject>();
                uiFormInstanceObject.Initialize(name, uiFormInstance);
                uiFormInstanceObject.m_UIFormAsset = uiFormAsset;
                uiFormInstanceObject.m_UIFormHelper = uiFormHelper;
                uiFormInstanceObject.m_AssetHandle = assetHandle;
                return uiFormInstanceObject;
            }

            public override void Clear()
            {
                base.Clear();
                m_UIFormAsset = null;
                m_UIFormHelper = null;
                m_AssetHandle = null;
            }

            protected internal override void Release(bool isShutdown)
            {
                m_UIFormHelper.ReleaseUIForm(m_UIFormAsset, Target, m_AssetHandle);
            }
        }
    }
}