using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UGKit.Fsm.Runtime;
using UGKit.Procedure;
using UGKit.Runtime;
using UnityEngine;

namespace UGKit.Procedure.Runtime
{
    /// <summary>
    /// 流程组件。
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("UGKit/Framework/Procedure")]
    [UnityEngine.Scripting.Preserve]
    [DefaultExecutionOrder(ExecutionOrders.PROCEDURE_MANAGER)]
    public sealed class ProcedureComponent : GameFrameworkComponent
    {
        private IProcedureManager m_ProcedureManager = null;
        private ProcedureBase m_EntranceProcedure = null;

        [SerializeField] private string[] m_AvailableProcedureTypeNames = null;

        [SerializeField] private string m_EntranceProcedureTypeName = null;

        /// <summary>
        /// 获取当前流程。
        /// </summary>
        public IProcedureManager Procedure
        {
            get { return m_ProcedureManager; }
        }

        /// <summary>
        /// 获取当前流程。
        /// </summary>
        public ProcedureBase CurrentProcedure
        {
            get { return m_ProcedureManager.CurrentProcedure; }
        }

        /// <summary>
        /// 获取当前流程持续时间。
        /// </summary>
        public float CurrentProcedureTime
        {
            get { return m_ProcedureManager.CurrentProcedureTime; }
        }

        [HideInInspector] public bool IsRunning { get; private set; }

        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected override void OnPreCreate()
        {
            ImplementationComponentType = Utility.Assembly.GetType(componentType);
            InterfaceComponentType = typeof(IProcedureManager);
            base.OnPreCreate();
            m_ProcedureManager = GameFrameworkEntry.GetModule<IProcedureManager>();
            if (m_ProcedureManager == null)
            {
                Log.Fatal("Procedure manager is invalid.");
                return;
            }
        }

        protected override async UniTask OnStart()
        {
            ProcedureBase[] procedures = new ProcedureBase[m_AvailableProcedureTypeNames.Length];
            for (int i = 0; i < m_AvailableProcedureTypeNames.Length; i++)
            {
                Type procedureType = Utility.Assembly.GetType(m_AvailableProcedureTypeNames[i]);
                if (procedureType == null)
                {
                    Log.Error("Can not find procedure type '{0}'.", m_AvailableProcedureTypeNames[i]);
                    return;
                }

                procedures[i] = (ProcedureBase) Activator.CreateInstance(procedureType);
                if (procedures[i] == null)
                {
                    Log.Error("Can not create procedure instance '{0}'.", m_AvailableProcedureTypeNames[i]);
                    return;
                }

                if (m_EntranceProcedureTypeName == m_AvailableProcedureTypeNames[i])
                {
                    m_EntranceProcedure = procedures[i];
                }
            }

            if (m_EntranceProcedure == null)
            {
                Log.Error("Entrance procedure is invalid.");
                return;
            }

            m_ProcedureManager.Initialize(GameFrameworkEntry.GetModule<IFsmManager>(), procedures);

            await UniTask.WaitForEndOfFrame(this);

            IsRunning = true;
            m_ProcedureManager.StartProcedure(m_EntranceProcedure.GetType());
        }

        /// <summary>
        /// 是否存在流程。
        /// </summary>
        /// <typeparam name="T">要检查的流程类型。</typeparam>
        /// <returns>是否存在流程。</returns>
        public bool HasProcedure<T>() where T : ProcedureBase
        {
            return m_ProcedureManager.HasProcedure<T>();
        }

        /// <summary>
        /// 是否存在流程。
        /// </summary>
        /// <param name="procedureType">要检查的流程类型。</param>
        /// <returns>是否存在流程。</returns>
        public bool HasProcedure(Type procedureType)
        {
            return m_ProcedureManager.HasProcedure(procedureType);
        }

        /// <summary>
        /// 获取流程。
        /// </summary>
        /// <typeparam name="T">要获取的流程类型。</typeparam>
        /// <returns>要获取的流程。</returns>
        public ProcedureBase GetProcedure<T>() where T : ProcedureBase
        {
            return m_ProcedureManager.GetProcedure<T>();
        }

        /// <summary>
        /// 获取流程。
        /// </summary>
        /// <param name="procedureType">要获取的流程类型。</param>
        /// <returns>要获取的流程。</returns>
        public ProcedureBase GetProcedure(Type procedureType)
        {
            return m_ProcedureManager.GetProcedure(procedureType);
        }
    }
}