using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Tracy.Frameworks.Common.Exceptions.ValidationException
{
    /// <summary>
    /// 验证对象状态
    /// </summary>
    [Flags]
    [Serializable]
    [DataContract]
    public enum ValidationEntityState
    {
        /// <summary>
        /// 对象存在，但未由对象服务跟踪。在创建实体之后、但将其添加到对象上下文之前，该实体处于此状态。通过调用 System.Data.Objects.ObjectContext.Detach(System.Object)
        /// 方法从上下文中移除实体后，或者使用 System.Data.Objects.MergeOption.NoTrackingSystem.Data.Objects.MergeOption
        /// 加载实体后，该实体也会处于此状态。
        /// </summary>
        [EnumMember]
        Detached = 1,

        /// <summary>
        /// 自对象加载到上下文中后，或自上次调用 System.Data.Objects.ObjectContext.SaveChanges() 方法后，此对象尚未经过修改。
        /// </summary>
        [EnumMember]
        Unchanged = 2,

        /// <summary>
        /// 对象已添加到对象上下文，但尚未调用 System.Data.Objects.ObjectContext.SaveChanges() 方法。对象是通过调用
        /// System.Data.Objects.ObjectContext.AddObject(System.String,System.Object)
        /// 方法添加到对象上下文中的。
        /// </summary>
        [EnumMember]
        Added = 4,

        /// <summary>
        /// 使用 System.Data.Objects.ObjectContext.DeleteObject(System.Object) 方法从对象上下文中删除了对象。
        /// </summary>
        [EnumMember]
        Deleted = 8,

        /// <summary>
        /// 对象已更改，但尚未调用 System.Data.Objects.ObjectContext.SaveChanges() 方法。
        /// </summary>
        [EnumMember]
        Modified = 16
    }
}
