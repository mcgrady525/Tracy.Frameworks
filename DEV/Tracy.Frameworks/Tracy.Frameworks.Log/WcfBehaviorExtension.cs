using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Text;

namespace Tracy.Frameworks.Log
{
    /// <summary>
    /// Wcf行为扩展
    /// </summary>
    public class WcfBehaviorExtension : BehaviorExtensionElement
    {

        /// <summary>
        /// 重写
        /// </summary>
        public override Type BehaviorType
        {
            get
            {
                return typeof(WcfServiceCounterAttribute);
            }
        }

        /// <summary>
        /// 重写新建行为方法
        /// </summary>
        /// <returns></returns>
        protected override object CreateBehavior()
        {
            return new WcfServiceCounterAttribute();
        }
    }
}
