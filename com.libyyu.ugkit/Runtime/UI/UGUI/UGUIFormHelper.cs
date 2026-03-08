using System;
using System.Reflection;
using UGKit.Asset.Runtime;
using UGKit.Runtime;
using UGKit.UI.Runtime;
using UnityEngine;
using UnityEngine.Scripting;
using Object = UnityEngine.Object;

namespace UGKit.UI.UGUI.Runtime
{
    /// <summary>
    /// 默认界面辅助器。
    /// </summary>
    [Preserve]
    public sealed class UGUIFormHelper : UIFormHelperBase
    {
        private UIComponent m_UIComponent = null;
        private AssetComponent m_AssetComponent = null;

        /// <summary>
        /// 实例化界面。
        /// </summary>
        /// <param name="uiFormAsset">要实例化的界面资源。</param>
        /// <returns>实例化后的界面。</returns>
        public override object InstantiateUIForm(object uiFormAsset)
        {
            return (Object)uiFormAsset;
        }

        /// <summary>
        /// 创建界面。
        /// </summary>
        /// <param name="uiFormInstance">界面实例。</param>
        /// <param name="uiFormType">界面逻辑类</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>界面。</returns>
        public override IUIForm CreateUIForm(object uiFormInstance, Type uiFormType, UIType uiType, object userData)
        {
            var uiGameObject = uiFormInstance as GameObject;
            if (uiGameObject == null)
            {
                Log.Error($"UI form instance type {uiFormType} is invalid.");
                return null;
            }

            var componentType = uiGameObject.GetOrAddComponent(uiFormType);
            if (!(componentType is IUIForm uiForm))
            {
                Log.Error($"UI form instance type {uiFormType} is invalid.");
                return null;
            }

            if (uiForm.IsAwake == false)
            {
                uiForm.OnAwake();
            }

            var uiGroup = uiForm.UIGroup;
            if (uiGroup == null)
            {
                var attribute = uiFormType.GetCustomAttribute(typeof(OptionUIGroupAttribute));
                if (attribute is OptionUIGroupAttribute optionUIGroup)
                {
                    uiGroup = m_UIComponent.GetUIGroup(optionUIGroup.GroupName);
                }

                uiForm.UIGroup = uiGroup;
            }


            var showAnimationAttribute = uiFormType.GetCustomAttribute(typeof(OptionUIShowAnimationAttribute));
            if (showAnimationAttribute is OptionUIShowAnimationAttribute optionShowAnimation)
            {
                uiForm.EnableShowAnimation = optionShowAnimation.Enable;
                uiForm.ShowAnimationName = optionShowAnimation.AnimationName;
            }
            else
            {
                uiForm.EnableShowAnimation = m_UIComponent.IsEnableUIShowAnimation;
            }

            var hideAnimationAttribute = uiFormType.GetCustomAttribute(typeof(OptionUIHideAnimationAttribute));
            if (hideAnimationAttribute is OptionUIHideAnimationAttribute optionHideAnimation)
            {
                uiForm.EnableHideAnimation = optionHideAnimation.Enable;
                uiForm.HideAnimationName = optionHideAnimation.AnimationName;
            }
            else
            {
                uiForm.EnableHideAnimation = m_UIComponent.IsEnableUIHideAnimation;
            }

            if (uiGroup == null)
            {
                Log.Error("UI group is invalid.");
                return null;
            }

            var uiTransform = uiGameObject.transform;
            uiTransform.SetParent(((MonoBehaviour)uiGroup.Helper).transform);
            uiTransform.localScale = Vector3.one;
            return uiForm;
        }

        /// <summary>
        /// 释放界面。
        /// </summary>
        /// <param name="uiFormAsset">要释放的界面资源。</param>
        /// <param name="uiFormInstance">要释放的界面实例。</param>
        /// <param name="assetHandle">资源句柄。</param>
        public override void ReleaseUIForm(object uiFormAsset, object uiFormInstance, object assetHandle)
        {
            m_AssetComponent.UnloadAssetHandle(assetHandle);
            Destroy((Object)uiFormInstance);
        }

        private void Awake()
        {
            m_AssetComponent = GameEntry.GetComponent<AssetComponent>();
            if (m_AssetComponent == null)
            {
                Log.Fatal("Asset component is invalid.");
                return;
            }

            m_UIComponent = GameEntry.GetComponent<UIComponent>();
            if (m_UIComponent == null)
            {
                Log.Fatal("UI component is invalid.");
                return;
            }
        }
    }
}