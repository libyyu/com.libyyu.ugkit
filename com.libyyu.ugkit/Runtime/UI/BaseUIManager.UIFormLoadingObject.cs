using System;
using System.Threading.Tasks;
using UGKit.ObjectPool;
using UGKit.Runtime;

namespace UGKit.UI.Runtime
{
    public partial class BaseUIManager
    {
        /// <summary>
        /// 界面加载对象。
        /// </summary>
        public sealed class UIFormLoadingObject : IReference
        {
            /// <summary>
            /// 界面资源路径。
            /// </summary>
            public string UIFormAssetPath { get; private set; }

            /// <summary>
            /// 界面资源名称。
            /// </summary>
            public string UIFormAssetName { get; private set; }

            /// <summary>
            /// 界面类型。
            /// </summary>
            public Type UIFormType { get; private set; }

            /// <summary>
            /// 界面加载任务。
            /// </summary>
            public Task<IUIForm> Task { get; private set; }

            /// <summary>
            /// 创建界面实例对象。
            /// </summary>
            /// <param name="uiFormAssetPath">界面资源路径。</param>
            /// <param name="uiFormAssetName">界面资源名称。</param>
            /// <param name="uiFormType">界面类型。</param>
            /// <param name="task">界面加载任务。</param>
            /// <returns>界面实例对象。</returns>
            public static UIFormLoadingObject Create(string uiFormAssetPath, string uiFormAssetName, Type uiFormType, Task<IUIForm> task)
            {
                var uiFormLoadingObject = ReferencePool.Acquire<UIFormLoadingObject>();
                uiFormLoadingObject.UIFormAssetPath = uiFormAssetPath;
                uiFormLoadingObject.UIFormAssetName = uiFormAssetName;
                uiFormLoadingObject.UIFormType = uiFormType;
                uiFormLoadingObject.Task = task;
                return uiFormLoadingObject;
            }

            public void Clear()
            {
                UIFormAssetPath = null;
                UIFormAssetName = null;
                UIFormType = null;
                Task = null;
            }
        }
    }
}