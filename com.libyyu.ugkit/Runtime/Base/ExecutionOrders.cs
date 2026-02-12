
namespace UGKit.Runtime
{
    /// <summary>
    /// Unity脚本执行顺序常量定义中心
    /// 原则：数字越小，执行越早
    /// </summary>
    internal static class ExecutionOrders
    {
        // ========== 系统级（-10,000 ~ -5,000）==========
        // 留给极底层、必须第一个初始化的系统（极少使用）
        public const int BOOTSTRAP          = -10000;   // 启动引导器
        public const int DI_CONTAINER       = -9000;    // 依赖注入容器

        // ========== 框架/管理器（-4,999 ~ -1,000）==========
        // 全局单例管理器、独立于场景的服务
        public const int TIME_MANAGER       = -5000;    // 时间系统（时间缩放等）
        public const int EVENT_MANAGER      = -4500;    // 事件系统
        public const int SETTINGS_GLOBAL    = -4200;    // 配置管理（必须在最前）
        public const int SETTINGS_LOCAL     = -4000;    // 配置管理（必须在最前）
        public const int POOL_MANAGER       = -3500;    // 对象池
        public const int FSM_MANAGER        = -3200;    // 状态机管理器
        public const int PROCEDURE_MANAGER  = -3100;    // 流程管理器
        public const int NETWORK_MANAGER    = -3000;    // 网络系统
        public const int WEBNETWORK_MANAGER = -2900;    // Web网络系统
        public const int WEBPROTOBUF_MANAGER = -2880;   // Web Protobuf系统
        public const int DOWNLOAD_MANAGER   = -2850;    // 下载系统
        public const int ASSET_MANAGER      = -2800;    // 资源系统
        public const int ENTITY_MANAGER     = -2750;    // 实体系统
        public const int AUDIO_MANAGER      = -2700;    // 音频系统
        public const int INPUT_MANAGER      = -2600;    // 输入系统
        public const int SCENE_MANAGER      = -2500;    // 场景加载管理器
        public const int UI_MANAGER         = -1500;    // UI框架根节点
        public const int LOCALIZATION       = -1000;    // 本地化系统

        // ========== 数据层（-999 ~ -100）==========
        // 数据模型、状态管理、持久化
        public const int GLOBAL_STATE = -900;         // 全局状态机
        public const int PLAYER_DATA = -800;          // 玩家数据
        public const int CURRENCY_SYSTEM = -700;      // 货币/资源系统
        public const int INVENTORY = -600;            // 背包数据

        // ========== 核心逻辑（-99 ~ 0）==========
        // 默认值0附近的业务逻辑组件
        public const int CHARACTER = -80;             // 角色基类
        public const int ABILITY = -60;              // 能力组件
        public const int BUFF = -40;                 // Buff系统（需在角色后）
        public const int COLLECTIBLE = -20;          // 可拾取物
        public const int DEFAULT = 0;                // 显式标记默认顺序

        // ========== 表现层（1 ~ 500）==========
        // 视觉、特效、动画、UI控件
        public const int ANIMATOR = 50;              // 动画控制器
        public const int VFX = 100;                  // 特效系统
        public const int UI_WIDGET = 150;            // UI具体控件
        public const int HUD = 200;                  // 抬头显示
        public const int TIPS = 250;                 // 飘字/提示

        // ========== 摄像机与视图（501 ~ 2000）==========
        // 后期处理、视图相关
        public const int CAMERA_FOLLOW = 800;        // 摄像机跟随（LateUpdate专用）
        public const int CINEMACHINE = 850;          // 虚拟相机
        public const int POST_PROCESSING = 900;      // 后处理（渲染前）

        // ========== 清理与销毁（2001 ~ 5000）==========
        // 需要最后执行的操作
        public const int GARBAGE_COLLECTOR = 3000;   // 自定义回收
        public const int ANALYTICS = 4000;           // 数据分析（记录最后状态）
        public const int LOGGER = 5000;              // 日志输出
    }
}
