using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UGKit.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;
using Object = UnityEngine.Object;

namespace UGKit.Asset.Runtime
{
    internal class YooAssetsLogger : YooAsset.ILogger
    {
        public void Log(string message)
        {
            UGKit.Runtime.Log.Info($"[YooAsset] {message}");
        }
        public void Warning(string message)
        {
            UGKit.Runtime.Log.Warning($"[YooAsset] {message}");
        }
        public void Error(string message)
        {
            UGKit.Runtime.Log.Error($"[YooAsset] {message}");
        }
        public void Exception(Exception exception)
        {
            UGKit.Runtime.Log.Fatal($"[YooAsset] {exception}");
        }
    }

    /// <summary>
    /// 资源组件。
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("UGKit/Framework/Asset")]
    [UnityEngine.Scripting.Preserve]
    [DefaultExecutionOrder(ExecutionOrders.ASSET_MANAGER)]
    public sealed class AssetComponent : GameFrameworkComponent
    {
        [Tooltip("当目标平台为Web平台时，将会强制设置为" + nameof(EPlayMode.WebPlayMode))] [SerializeField]
        private EPlayMode m_GamePlayMode;

        /// <summary>
        /// 资源的运行模式
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public EPlayMode GamePlayMode
        {
            get { return m_GamePlayMode; }
            set { m_GamePlayMode = value; }
        }
#if UNITY_EDITOR
        [SerializeField] private List<AssetResourcePackageInfo> m_assetResourcePackages = new List<AssetResourcePackageInfo>();
#endif

        public const string BuildInPackageName = "DefaultPackage";

        private IAssetManager _assetManager;

        [UnityEngine.Scripting.Preserve]
        protected override void OnPreCreate()
        {
#if !UNITY_EDITOR
            if (GamePlayMode == EPlayMode.EditorSimulateMode)
            {
                GamePlayMode = EPlayMode.HostPlayMode;
            }
#if UNITY_WEBGL
            GamePlayMode = EPlayMode.WebPlayMode;
#endif
#endif
            ImplementationComponentType = Utility.Assembly.GetType(componentType);
            InterfaceComponentType = typeof(IAssetManager);
            base.OnPreCreate();
            _assetManager = GameFrameworkEntry.GetModule<IAssetManager>();
            if (_assetManager == null)
            {
                Log.Fatal("Asset manager is invalid.");
                return;
            }

            _assetManager.SetPlayMode(GamePlayMode);
            _assetManager.Initialize(new YooAssetsLogger());
        }

        protected override async UniTask OnStart()
        {
            await UniTask.CompletedTask;
        }

        public bool Initialized { get { return _assetManager != null && _assetManager.Initialized; } }

        /// <summary>
        /// 初始化资源包
        /// </summary>
        /// <param name="packageName">包名称</param>
        /// <param name="host">主下载地址</param>
        /// <param name="fallbackHostServer">备用下载地址</param>
        /// <param name="isDefaultPackage">是否是默认包</param>
        [UnityEngine.Scripting.Preserve]
        public async Task<bool> InitPackageAsync(string packageName, string host, string fallbackHostServer, bool isDefaultPackage = false, bool updatePackage = true)
        {
#if UNITY_EDITOR
            var assetResourcePackageInfo = new AssetResourcePackageInfo()
            {
                PackageName = packageName,
                DownloadURL = host,
                FallbackDownloadURL = fallbackHostServer
            };
            if (!m_assetResourcePackages.Exists(m => m.PackageName == packageName))
            {
                m_assetResourcePackages.Add(assetResourcePackageInfo);
            }
#endif
            return await _assetManager.InitPackageAsync(packageName, host, fallbackHostServer, isDefaultPackage);
        }

        [UnityEngine.Scripting.Preserve]
        public async Task<bool> InitPackageAsync(List<AppPackageInfo> presetAppPackageInfos)
        {
            foreach (var packageInfo in presetAppPackageInfos)
            {
                // updatePackage = true, 因為新版 Yoo 必須獲取版號與 manifest 才能進行資源加載
                bool isInitialized = await InitPackageAsync(packageInfo.packageName, packageInfo.hostServer, packageInfo.fallbackHostServer, packageInfo.isDefault, packageInfo.initUpdate);
                if (isInitialized)
                {
                    Log.Info($"Successfully initialized preset App package: {packageInfo.packageName}.");
                }
                else
                {
                    Log.Error($"Initialization failed for preset App package: {packageInfo.packageName}.");
                    return false;
                }
            }
            return true;
        }

        #region 异步加载子资源对象

        /// <summary>
        /// 异步加载子资源对象
        /// </summary>
        /// <param name="assetInfo">资源信息</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public Task<SubAssetsHandle> LoadSubAssetsAsync(AssetInfo assetInfo)
        {
            return _assetManager.LoadSubAssetsAsync(assetInfo);
        }

        /// <summary>
        /// 异步加载子资源对象
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <param name="type"></param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public Task<SubAssetsHandle> LoadSubAssetsAsync(string path, Type type)
        {
            return _assetManager.LoadSubAssetsAsync(path, type);
        }

        /// <summary>
        /// 异步加载子资源对象
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public Task<SubAssetsHandle> LoadSubAssetsAsync<T>(string path) where T : Object
        {
            return _assetManager.LoadSubAssetsAsync<T>(path);
        }

        #endregion

        #region 同步加载子资源对象

        /// <summary>
        /// 同步加载子资源对象
        /// </summary>
        /// <param name="assetInfo">资源信息</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public SubAssetsHandle LoadSubAssetSync(AssetInfo assetInfo)
        {
            return _assetManager.LoadSubAssetSync(assetInfo);
        }

        /// <summary>
        /// 同步加载子资源对象
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <param name="type"></param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public SubAssetsHandle LoadSubAssetSync(string path, Type type)
        {
            return _assetManager.LoadSubAssetSync(path, type);
        }

        /// <summary>
        /// 同步加载子资源对象
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public SubAssetsHandle LoadSubAssetSync<T>(string path) where T : Object
        {
            return _assetManager.LoadSubAssetSync<T>(path);
        }

        #endregion

        #region 异步加载原生文件

        /// <summary>
        /// 异步加载原生文件
        /// </summary>
        /// <param name="assetInfo">资源信息</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public Task<RawFileHandle> LoadRawFileAsync(AssetInfo assetInfo)
        {
            return _assetManager.LoadRawFileAsync(assetInfo);
        }

        /// <summary>
        /// 异步加载原生文件
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public Task<RawFileHandle> LoadRawFileAsync(string path)
        {
            return _assetManager.LoadRawFileAsync(path);
        }

        #endregion

        #region 同步加载原生文件

        /// <summary>
        /// 同步加载原生文件
        /// </summary>
        /// <param name="assetInfo">资源信息</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public RawFileHandle LoadRawFileSync(AssetInfo assetInfo)
        {
            return _assetManager.LoadRawFileSync(assetInfo);
        }

        /// <summary>
        /// 同步加载原生文件
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public RawFileHandle LoadRawFileSync(string path)
        {
            return _assetManager.LoadRawFileSync(path);
        }

        #endregion


        #region 异步加载资源

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="assetInfo">资源信息</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public Task<AssetHandle> LoadAssetAsync(AssetInfo assetInfo)
        {
            return _assetManager.LoadAssetAsync(assetInfo);
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <param name="type">资源类型</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public Task<AssetHandle> LoadAssetAsync(string path, Type type)
        {
            return _assetManager.LoadAssetAsync(path, type);
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <typeparam name="T">资源类型</typeparam>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public Task<AssetHandle> LoadAssetAsync<T>(string path) where T : Object
        {
            return _assetManager.LoadAssetAsync<T>(path);
        }

        /// <summary>
        /// 异步加载全部资源
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <typeparam name="T">资源类型</typeparam>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public Task<AllAssetsHandle> LoadAllAssetsAsync<T>(string path) where T : Object
        {
            return _assetManager.LoadAllAssetsAsync<T>(path);
        }

        /// <summary>
        /// 异步加载全部资源
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <param name="type">资源类型</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public Task<AllAssetsHandle> LoadAllAssetsAsync(string path, Type type)
        {
            return _assetManager.LoadAllAssetsAsync(path, type);
        }

        /// <summary>
        /// 异步加载资源包内所有资源对象
        /// </summary>
        /// <param name="path">资源的定位地址</param>
        [UnityEngine.Scripting.Preserve]
        public Task<AllAssetsHandle> LoadAllAssetsAsync(string path)
        {
            return _assetManager.LoadAllAssetsAsync(path);
        }

        /// <summary>
        /// 异步加载资源包内所有资源对象
        /// </summary>
        /// <param name="assetInfo">资源信息</param>
        [UnityEngine.Scripting.Preserve]
        public Task<AllAssetsHandle> LoadAllAssetsAsync(AssetInfo assetInfo)
        {
            return _assetManager.LoadAllAssetsAsync(assetInfo);
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public Task<AssetHandle> LoadAssetAsync(string path)
        {
            return _assetManager.LoadAssetAsync(path);
        }

        /// <summary>
        /// 异步加载子资源对象
        /// </summary>
        /// <param name="path">资源的定位地址</param>
        [UnityEngine.Scripting.Preserve]
        public SubAssetsHandle LoadSubAssetsAsync(string path)
        {
            return _assetManager.LoadSubAssetsAsync(path);
        }

        #endregion

        #region 同步加载资源

        /// <summary>
        /// 同步加载资源包内所有资源对象
        /// </summary>
        /// <param name="path">资源的定位地址</param>
        [UnityEngine.Scripting.Preserve]
        public AllAssetsHandle LoadAllAssetsSync(string path)
        {
            return _assetManager.LoadAllAssetsSync(path);
        }

        /// <summary>
        /// 同步加载资源包内所有资源对象
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="path">资源的定位地址</param>
        [UnityEngine.Scripting.Preserve]
        public AllAssetsHandle LoadAllAssetsSync<T>(string path) where T : Object
        {
            return _assetManager.LoadAllAssetsSync<T>(path);
        }

        /// <summary>
        /// 同步加载资源包内所有资源对象
        /// </summary>
        /// <param name="path">资源的定位地址</param>
        /// <param name="type">子对象类型</param>
        [UnityEngine.Scripting.Preserve]
        public AllAssetsHandle LoadAllAssetsSync(string path, Type type)
        {
            return _assetManager.LoadAllAssetsSync(path, type);
        }

        /// <summary>
        /// 同步加载包内全部资源对象
        /// </summary>
        /// <param name="assetInfo">资源信息</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public AllAssetsHandle LoadAllAssetsSync(AssetInfo assetInfo)
        {
            return _assetManager.LoadAllAssetsSync(assetInfo);
        }

        /// <summary>
        /// 同步加载子资源
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public SubAssetsHandle LoadSubAssetsSync(string path)
        {
            return _assetManager.LoadSubAssetSync(path);
        }

        /// <summary>
        /// 同步加载资源
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public AssetHandle LoadAssetsSync(string path)
        {
            return _assetManager.LoadAssetSync(path);
        }

        /// <summary>
        /// 同步加载资源
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <param name="type"></param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public AssetHandle LoadAssetSync(string path, Type type)
        {
            return _assetManager.LoadAssetSync(path, type);
        }

        /// <summary>
        /// 同步加载资源
        /// </summary>
        /// <param name="assetInfo">资源信息</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public AssetHandle LoadAssetSync(AssetInfo assetInfo)
        {
            return _assetManager.LoadAssetSync(assetInfo);
        }

        /// <summary>
        /// 同步加载资源
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public AssetHandle LoadAssetSync<T>(string path) where T : Object
        {
            return _assetManager.LoadAssetSync<T>(path);
        }

        #endregion

        #region 加载场景

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <param name="sceneMode">场景模式</param>
        /// <param name="activateOnLoad">是否加载完成自动激活</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public Task<SceneHandle> LoadSceneAsync(string path, LoadSceneMode sceneMode, bool activateOnLoad = true)
        {
            return _assetManager.LoadSceneAsync(path, sceneMode, activateOnLoad);
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="assetInfo">资源路径</param>
        /// <param name="sceneMode">场景模式</param>
        /// <param name="activateOnLoad">是否加载完成自动激活</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public Task<SceneHandle> LoadSceneAsync(AssetInfo assetInfo, LoadSceneMode sceneMode, bool activateOnLoad = true)
        {
            return _assetManager.LoadSceneAsync(assetInfo, sceneMode, activateOnLoad);
        }

        #endregion

        #region 资源包

        /// <summary>
        /// 创建资源包
        /// </summary>
        /// <param name="packageName">资源包名称</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public ResourcePackage CreateAssetsPackage(string packageName)
        {
            return _assetManager.CreateAssetsPackage(packageName);
        }

        /// <summary>
        /// 尝试获取资源包
        /// </summary>
        /// <param name="packageName">资源包名称</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public ResourcePackage TryGetAssetsPackage(string packageName)
        {
            return _assetManager.TryGetAssetsPackage(packageName);
        }

        /// <summary>
        /// 检查资源包是否存在
        /// </summary>
        /// <param name="packageName">资源包名称</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public bool HasAssetsPackage(string packageName)
        {
            return _assetManager.HasAssetsPackage(packageName);
        }

        /// <summary>
        /// 获取资源包
        /// </summary>
        /// <param name="packageName">资源包名称</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public ResourcePackage GetAssetsPackage(string packageName)
        {
            return _assetManager.GetAssetsPackage(packageName);
        }

        #endregion

        /// <summary>
        /// 是否需要下载
        /// </summary>
        /// <param name="assetInfo">资源信息</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public bool IsNeedDownload(AssetInfo assetInfo)
        {
            return _assetManager.IsNeedDownload(assetInfo);
        }

        /// <summary>
        /// 是否需要下载
        /// </summary>
        /// <param name="path">资源地址</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public bool IsNeedDownload(string path)
        {
            return _assetManager.IsNeedDownload(path);
        }

        /// <summary>
        /// 获取资源信息
        /// </summary>
        /// <param name="assetTags">资源标签列表</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public AssetInfo[] GetAssetInfos(string[] assetTags)
        {
            return _assetManager.GetAssetInfos(assetTags);
        }

        /// <summary>
        /// 获取资源信息
        /// </summary>
        /// <param name="assetTag">资源标签</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public AssetInfo[] GetAssetInfos(string assetTag)
        {
            return _assetManager.GetAssetInfos(assetTag);
        }

        /// <summary>
        /// 获取资源信息
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public AssetInfo GetAssetInfo(string path)
        {
            return _assetManager.GetAssetInfo(path);
        }

        /// <summary>
        /// 检查指定的资源路径是否存在。
        /// </summary>
        /// <param name="assetPath">要检查的资源路径。</param>
        /// <returns>如果存在指定的资源路径，则返回 true；否则返回 false。</returns>
        [UnityEngine.Scripting.Preserve]
        public bool HasAssetPath(string assetPath)
        {
            return _assetManager.HasAssetPath(assetPath);
        }

        /// <summary>
        /// 设置默认资源包
        /// </summary>
        /// <param name="assetsPackage">资源信息</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public void SetDefaultAssetsPackage(ResourcePackage assetsPackage)
        {
            _assetManager.SetDefaultAssetsPackage(assetsPackage);
        }

        [UnityEngine.Scripting.Preserve]
        public ResourcePackage GetDefaultAssetsPackage()
        {             
            return _assetManager.GetDefaultAssetsPackage();
        }

        public List<string> GetPackageBundleList()
        {
            return _assetManager.GetPackageBundleList();
        }
        public List<string> GetPackageAssetList()
        {
            return _assetManager.GetPackageAssetList();
        }

        public ResourceDownloaderOperation CreateTagResourceDownloader(string tag, int downloadingMaxNumber, int failedTryAgain)
        {
            return _assetManager.CreateTagResourceDownloader(tag, downloadingMaxNumber, failedTryAgain);
        }
        public ResourceDownloaderOperation CreateTagResourceDownloader(string[] tags, int downloadingMaxNumber, int failedTryAgain)
        {
            return _assetManager.CreateTagResourceDownloader(tags, downloadingMaxNumber, failedTryAgain);
        }

        /// <summary>
        /// 强制回收所有资源
        /// </summary>
        /// <param name="packageName">资源包名称</param>
        [UnityEngine.Scripting.Preserve]
        public void UnloadAllAssetsAsync(string packageName)
        {
            _assetManager.UnloadAllAssetsAsync(packageName);
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="packageName">资源包名称</param>
        /// <param name="assetPath">资源路径</param>
        [UnityEngine.Scripting.Preserve]
        public void UnloadAsset(string packageName, string assetPath)
        {
            _assetManager.UnloadAsset(packageName, assetPath);
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="assetPath">资源路径</param>
        [UnityEngine.Scripting.Preserve]
        public void UnloadAsset(string assetPath)
        {
            _assetManager.UnloadAsset(assetPath);
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="assetHandle">资源句柄</param>
        [UnityEngine.Scripting.Preserve]
        public void UnloadAssetHandle(object assetHandle)
        {
            if (assetHandle is AssetHandle handle)
            {
                handle.Release();
            }
        }

        /// <summary>
        /// 卸载无用资源
        /// </summary>
        /// <param name="packageName">资源包名称</param>
        [UnityEngine.Scripting.Preserve]
        public void UnloadUnusedAssetsAsync(string packageName)
        {
            _assetManager.UnloadUnusedAssetsAsync(packageName);
        }

        /// <summary>
        /// 清理所有资源
        /// </summary>
        /// <param name="packageName">资源包名称</param>
        [UnityEngine.Scripting.Preserve]
        public void ClearAllBundleFilesAsync(string packageName)
        {
            _assetManager.ClearAllBundleFilesAsync(packageName);
        }

        /// <summary>
        /// 清理无用资源
        /// </summary>
        /// <param name="packageName">资源包名称</param>
        [UnityEngine.Scripting.Preserve]
        public void ClearUnusedBundleFilesAsync(string packageName)
        {
            _assetManager.ClearUnusedBundleFilesAsync(packageName);
        }
    }
#if UNITY_EDITOR
    [Serializable]
    public sealed class AssetResourcePackageInfo
    {
        [SerializeField] public string PackageName;
        [SerializeField] public string DownloadURL;
        [SerializeField] public string FallbackDownloadURL;
    }
#endif
}