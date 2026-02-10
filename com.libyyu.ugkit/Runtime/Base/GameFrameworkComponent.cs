
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;

namespace UGKit.Runtime
{
    /// <summary>
    /// 游戏框架组件抽象类。
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    public abstract class GameFrameworkComponent : MonoBehaviour
    {
        [HideInInspector] public bool isCreated { get; set; }
        [HideInInspector] public bool isStarted { get; set; }
        [HideInInspector] public bool isReleased { get; set; }
        [HideInInspector] public bool isPostCreated { get; set; }

        /// <summary>
        /// 是否自动注册
        /// </summary>
        protected bool IsAutoRegister { get; set; } = true;

        /// <summary>
        /// 实现类的类型
        /// </summary>
        protected Type ImplementationComponentType = null;

        /// <summary>
        /// 接口类的类型
        /// </summary>
        protected Type InterfaceComponentType = null;

        /// <summary>
        /// 游戏框架组件类型。
        /// </summary>
        [SerializeField] protected string componentType = string.Empty;

        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        private async void Awake()
        {
            isReleased = false;
            isPostCreated = false;

            if (!isCreated)
            {
                OnPreCreate();
                isCreated = true;
                await OnCreate();
                isPostCreated = true;
            }
        }

        [UnityEngine.Scripting.Preserve]
        private async void Start()
        {
            while (!isPostCreated) await UniTask.Yield();
            if (!isStarted)
            {
                isStarted = true;
                await OnStart();
            }
        }

        [UnityEngine.Scripting.Preserve]
        private async void OnDestroy()
        {
            isCreated = false;
            isStarted = false;
            isReleased = true;
            await OnRelease();
        }

        protected virtual void OnPreCreate()
        {
            GameEntry.RegisterComponent(this);
            if (IsAutoRegister)
            {
                GameFrameworkGuard.NotNull(ImplementationComponentType, nameof(ImplementationComponentType));
                GameFrameworkGuard.NotNull(InterfaceComponentType, nameof(InterfaceComponentType));
                GameFrameworkEntry.RegisterModule(InterfaceComponentType, ImplementationComponentType);
            }
        }

        protected virtual async UniTask OnCreate() { }
        protected virtual async UniTask OnStart() { }
        protected virtual async UniTask OnRelease() { }

        public async UniTask RunCoroutineAsync(IEnumerator coroutine)
        {
            var completionSource = new UniTaskCompletionSource<bool>();

            this.StartCoroutine(RunCoroutine(coroutine, completionSource));

            await completionSource.Task;
        }

        private IEnumerator RunCoroutine(IEnumerator coroutine, UniTaskCompletionSource<bool> completionSource)
        {
            yield return coroutine;
            completionSource.TrySetResult(true);
        }
    }
}