
using System;

namespace UGKit.UI.Runtime
{
    /// <summary>
    /// 用于为类指定 UI 选项分组的特性。
    /// 只能应用于类，分组名称不能为空。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class OptionUIGroupAttribute : Attribute
    {
        /// <summary>
        /// 分组名称
        /// </summary>
        public string GroupName { get; private set; }

        /// <summary>
        /// 构造函数，初始化分组名称
        /// </summary>
        /// <param name="groupName">分组名称，不能为空</param>
        /// <exception cref="Exception">分组名称为空时抛出异常</exception>
        public OptionUIGroupAttribute(string groupName)
        {
            GroupName = groupName;
            if (string.IsNullOrEmpty(GroupName))
            {
                throw new Exception("GroupName is null");
            }
        }
    }
}