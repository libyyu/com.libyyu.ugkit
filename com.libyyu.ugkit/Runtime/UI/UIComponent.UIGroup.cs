using System;
using UGKit.Runtime;
using UnityEngine;

namespace UGKit.UI.Runtime
{
    public partial class UIComponent : GameFrameworkComponent
    {
        [Serializable]
        private sealed class UIGroup
        {
            [SerializeField] private string m_Name = null;

            [SerializeField] private int m_Depth = 0;

            public string Name
            {
                get { return m_Name; }
            }

            public int Depth
            {
                get { return m_Depth; }
            }

            public UIGroup(int depth, string name)
            {
                this.m_Depth = depth;
                this.m_Name = name;
            }
        }
    }
}