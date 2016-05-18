using System;
using System.ServiceModel.Configuration;

namespace Tracy.Frameworks.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class ValidateFaultBehaviorExtension : BehaviorExtensionElement {
        /// <summary>
        /// 
        /// </summary>
        public override Type BehaviorType {
            get {
                return typeof ( ValidateFaultBehaviorAttribute );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override object CreateBehavior() {
            return new ValidateFaultBehaviorAttribute ( );
        }
    }
}
