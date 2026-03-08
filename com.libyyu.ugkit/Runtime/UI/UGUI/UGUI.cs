
using System;
using UGKit.Runtime;
using UGKit.UI.Runtime;
using UnityEngine;
using UnityEngine.Scripting;

namespace UGKit.UI.UGUI.Runtime
{
    [Preserve]
    [DisallowMultipleComponent]
    public class UGUI : UIForm
    {
        /// <summary>
        /// 界面显示。
        /// </summary>
        /// <param name="handler">界面显示处理接口</param>
        /// <param name="complete">完成回调</param>
        public override void Show(IUIFormShowHandler handler, Action complete)
        {
            if (handler != null)
            {
                handler.Handler(Handle, EnableShowAnimation, ShowAnimationName, complete);
            }
            else
            {
                complete?.Invoke();
            }
        }


        /// <summary>
        /// 界面隐藏。
        /// </summary>
        /// <param name="handler">界面隐藏处理接口</param>
        /// <param name="complete">完成回调</param>
        public override void Hide(IUIFormHideHandler handler, Action complete)
        {
            if (handler != null)
            {
                handler.Handler(Handle, EnableHideAnimation, HideAnimationName, complete);
            }
            else
            {
                complete?.Invoke();
            }
        }

        /// <summary>
        /// 设置UI的显示状态，不发出事件
        /// </summary>
        /// <param name="value"></param>
        protected override void InternalSetVisible(bool value)
        {
            if (gameObject.activeSelf == value)
            {
                return;
            }

            gameObject.SetActive(value);
        }

        public override bool Visible
        {
            get
            {
                if (gameObject == null)
                {
                    return false;
                }

                return gameObject.activeSelf;
            }
            protected set
            {
                if (gameObject == null)
                {
                    return;
                }

                if (gameObject.activeSelf == value)
                {
                    return;
                }

                if (value == false)
                {
                    // OnHideBeforeAction?.Invoke(this);
                    // foreach (var child in Children)
                    // {
                    //     ((FUI)child.Value).Visible = value;
                    // }

                    // OnHide();
                    // OnHideAfterAction?.Invoke(this);
                }

                gameObject.SetActive(value);
                if (value)
                {
                    // OnShowBeforeAction?.Invoke(this);
                    // foreach (var child in Children)
                    // {
                    //     ((FUI)child.Value).Visible = value;
                    // }
                    //
                    // OnShow();
                    // OnShowAfterAction?.Invoke(this);
                    // Refresh();
                }
            }
        }

        /// <summary>
        /// 设置当前UI对象为全屏
        /// </summary>
        protected internal override void MakeFullScreen()
        {
            gameObject?.GetOrAddComponent<RectTransform>()?.MakeFullScreen();
        }
    }
}