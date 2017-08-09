using EmitMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tracy.Frameworks.Common.Extends
{
    /// <summary>
    /// DTO mapping转换(基于EmitMapper)
    /// </summary>
    public static class DTOExtension
    {
        /// <summary>
        /// DTO mapping
        /// </summary>
        /// <typeparam name="TFrom">源实体</typeparam>
        /// <typeparam name="TTo">目标实体</typeparam>
        /// <param name="tFrom">源实体输入</param>
        /// <returns></returns>
        public static TTo ToDto<TFrom, TTo>(this TFrom tFrom)
        {
            var mapper = ObjectMapperManager.DefaultInstance.GetMapper<TFrom, TTo>();
            return mapper.Map(tFrom);
        }
    }
}
