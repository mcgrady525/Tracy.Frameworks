using System;
using System.Linq.Expressions;

namespace Tracy.Frameworks.MongoDb
{
    public class OrderBy<T>
    {
        public OrderBy()
        {
            this.isAscending = true;
        }

        /// <summary>
        /// 獲取排序字段的表達式
        /// </summary>
        public Expression<Func<T, object>> Field { get; set; }

        /// <summary>
        /// 是否正序排序
        /// </summary>
        //c#6.0自动属性默认初始化
        //public bool IsAscending { get; set; } = true;

        private bool isAscending;

        public bool IsAscending
        {
            get
            {
                return isAscending;
            }
            set 
            {
                isAscending = value;
            }
        }
    }
}