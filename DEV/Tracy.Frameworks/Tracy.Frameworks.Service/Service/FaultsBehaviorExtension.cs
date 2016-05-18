using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Configuration;

namespace Tracy.Frameworks.Service
{
    /// <summary>
    /// 自定义WCF异常处理行为扩展
    /// </summary>
    public class FaultsBehaviorExtension : BehaviorExtensionElement
    {
        /// <summary>
        /// 行为扩展绑定类型
        /// </summary>
        public override Type BehaviorType
        {
            get { return typeof(FaultsBehaviorAttribute); }
        }

        /// <summary>
        /// 创建对应类型实例
        /// </summary>
        /// <returns></returns>
        protected override object CreateBehavior()
        {
            return new FaultsBehaviorAttribute();
        }
    }
}
