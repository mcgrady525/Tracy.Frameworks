using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;

namespace Tracy.Frameworks.Common.Extends
{
    /// <summary>
    /// 枚举(enum)扩展
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// 将对象转换为指定类型的枚举变量
        /// </summary>
        /// <param name="value"></param>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static object ToEnum(this object value, Type enumType)
        {
            if (!enumType.IsEnum) return null;
            try
            {
                object newObject;
                var fields = enumType.GetFields();
                value = Convert.ChangeType(value, fields[0].FieldType);
                if (Enum.IsDefined(enumType, value))
                    newObject = Enum.Parse(enumType, value.ToString(), true);
                else if (fields.Length < 2)
                    newObject = Enum.Parse(enumType, "0");
                else
                    newObject = Enum.Parse(enumType, fields[1].Name);
                return newObject;
            }
            catch
            {
            }
            return Enum.Parse(enumType, "0");
        }

        /// <summary>
        /// 将对象转换为指定类型的枚举变量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this object value)
            where T : struct
        {
            var res = value.ToEnum(typeof(T));
            return res == null ? default(T) : (T)res;
        }

        /// <summary>
        /// 将对象转换为指定类型的可空枚举变量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Nullable<T> ToEnumOrNull<T>(this object value)
            where T : struct
        {
            var enumType = typeof(T);
            if (!enumType.IsEnum) return null;
            try
            {
                var fields = enumType.GetFields();
                value = Convert.ChangeType(value, fields[0].FieldType);
                if (Enum.IsDefined(enumType, value))
                    return (T)Enum.Parse(enumType, value.ToString(), true);
            }
            catch
            {
            }
            return null;
        }

        /// <summary>
        /// 尝试将值类型变量转换为指定枚举类型，并获取枚举变量值的 Description 、 DisplayName 或 Display(Name= 特性值
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <param name="obj">值类型变量</param>
        /// <returns>如果包含特性值则返回，否则返回枚举项的名称；对有Flags位标识的枚举，且输入参数值是多个枚举项基本值按位或结果的，返回多个枚举项的特性或名称，以逗号分隔</returns>
        public static string GetDescription<TEnum>(this object obj)
            where TEnum : struct
        {
            return obj.ToEnum<TEnum>().GetDescription();
        }

        /// <summary>
        /// 获取枚举变量值的 Description 、 DisplayName 或 Display(Name= 特性值
        /// </summary>
        /// <param name="obj">枚举变量</param>
        /// <returns>如果包含特性值则返回，否则返回枚举项的名称；对有Flags位标识的枚举，且输入参数值是多个枚举项基本值按位或结果的，返回多个枚举项的特性或名称，以逗号分隔</returns>
        public static string GetDescription(this object obj)
        {
            return GetDescription(obj, false);
        }

        /// <summary>
        /// 获取枚举变量值的 Description 、 DisplayName 或 Display(Name= 特性值
        /// </summary>
        /// <param name="obj">枚举变量</param>
        /// <param name="isTop">是否改变为返回该类、枚举类型的头 Description 属性，而不是当前的属性或枚举变量值的 Description 属性</param>
        /// <returns>如果包含特性值则返回，否则返回枚举项的名称；对有Flags位标识的枚举，且输入参数值是多个枚举项基本值按位或结果的，返回多个枚举项的特性或名称，以逗号分隔</returns>
        public static string GetDescription(this object obj, bool isTop)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            Type type = obj.GetType();
            try
            {
                if ((!type.IsEnum && !type.IsValueType) || (type.IsEnum && isTop))
                {
                    var da = (DescriptionAttribute)Attribute.GetCustomAttribute(type, typeof(DescriptionAttribute));
                    if (da != null && !string.IsNullOrEmpty(da.Description))
                        return da.Description;
                    var dna = (DisplayNameAttribute)Attribute.GetCustomAttribute(type, typeof(DisplayNameAttribute));
                    if (dna != null && !string.IsNullOrEmpty(dna.DisplayName))
                        return dna.DisplayName;
                    var na = (DisplayAttribute)Attribute.GetCustomAttribute(type, typeof(DisplayAttribute));
                    if (na != null && !string.IsNullOrEmpty(na.Name))
                        return na.Name;
                }
                else
                {
                    if (Attribute.GetCustomAttribute(type, typeof(FlagsAttribute)) != null && !Enum.IsDefined(type, obj))
                    {
                        List<string> lst = new List<string>();
                        var values = Enum.GetValues(type);
                        if (values.Length > 0)
                        {
                            var objValue = Convert.ToUInt64(obj);
                            foreach (var value in values)
                            {
                                if ((objValue & Convert.ToUInt64(value)) > 0)
                                {
                                    lst.Add(GetDescription(type, value, isTop));
                                }
                            }
                        }
                        return string.Join(",", lst);
                    }
                    else
                    {
                        return GetDescription(type, obj, isTop);
                    }
                }
            }
            catch { }
            return obj.ToString();
        }
        private static string GetDescription(Type type, object obj, bool isTop)
        {
            try
            {
                FieldInfo fi = type.GetField(Enum.GetName(type, obj));
                var da = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));
                if (da != null && !string.IsNullOrEmpty(da.Description))
                    return da.Description;
                var dna = (DisplayNameAttribute)Attribute.GetCustomAttribute(fi, typeof(DisplayNameAttribute));
                if (dna != null && !string.IsNullOrEmpty(dna.DisplayName))
                    return dna.DisplayName;
                var na = (DisplayAttribute)Attribute.GetCustomAttribute(fi, typeof(DisplayAttribute));
                if (na != null && !string.IsNullOrEmpty(na.Name))
                    return na.Name;
            }
            catch { }
            return obj.ToString();
        }

        /// <summary>
        /// 根据枚举变量获取对应枚举类型的所有值的 Description 、 DisplayName 或 Display(Name= 特性值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="spliter"></param>
        /// <returns></returns>
        public static string GetMutiEnumDescription(this object obj, string spliter = ",")
        {
            if (obj == null)
            {
                return string.Empty;
            }
            try
            {
                Type type = obj.GetType();

                if (type.IsEnum)
                {
                    var flagValues = Enum.GetValues(type);
                    Dictionary<object, string> vd = new Dictionary<object, string>();
                    DescriptionAttribute da = null;
                    //FieldInfo fi = type.GetField(Enum.GetName(type, obj));
                    //    da = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));
                    foreach (var v in flagValues)
                    {
                        FieldInfo fi = type.GetField(Enum.GetName(type, obj));
                        var attrs = fi.GetCustomAttributes(false);

                        if (attrs.Any(i => i.GetType() == typeof(DescriptionAttribute)))
                        {
                            DescriptionAttribute desc = (DescriptionAttribute)attrs.Find(i => i.GetType() == typeof(DescriptionAttribute));
                            vd.Add(v, desc.Description);
                        }
                        else if (attrs.Any(i => i.GetType() == typeof(DisplayNameAttribute)))
                        {
                            DisplayNameAttribute desc = (DisplayNameAttribute)attrs.Find(i => i.GetType() == typeof(DisplayNameAttribute));
                            vd.Add(v, desc.DisplayName);
                        }
                        else if (attrs.Any(i => i.GetType() == typeof(DisplayAttribute)))
                        {
                            DisplayAttribute desc = (DisplayAttribute)attrs.Find(i => i.GetType() == typeof(DisplayAttribute));
                            vd.Add(v, desc.Name);
                        }
                    }
                    List<string> descs = new List<string>();
                    int realValue = (int)obj;
                    foreach (var x in flagValues)
                    {
                        var temp = (int)x;
                        if ((realValue & temp) == temp)
                        {
                            if (vd.ContainsKey(x))
                                descs.Add(vd[x]);
                            else
                                descs.Add(x.ToString());
                        }
                    }
                    return descs.Aggregate((s, r) => { return string.Format("{0}{1}{2}", s, spliter, r); });

                }
            }
            catch
            {
            }
            return string.Empty;
        }

        /// <summary>
        /// 调用方法：EnumHelper.GetEnumInfoHtml(this.GetType().Namespace, "Hello", "Hotel.");
        /// </summary>
        /// <param name="startsWith"></param>
        /// <returns></returns>
        public static string GetEnumInfoHtml(params string[] startsWith)
        {
            var sb = new StringBuilder();
            sb.Append(@"
<style>
.CommonTable { width: 100%; height: 20px; line-height: 200%; border-collapse: collapse; border-top:1px solid #CCCCCC;border-left:1px solid #CCCCCC; margin-top:2px }
.CommonTable th,.CommonTable tr td { padding-left: 5px; border-right:1px solid #CCCCCC;border-bottom:1px solid #CCCCCC;}
.CommonTable th { background-color: #507CD1; color: #ffffff; line-height: 200%;border-bottom:1px solid #CCCCCC;  }
.CommonTable th,.CommonTable td img { vertical-align: middle; }
.CommonTable a { color:#FFF }
body{min-height:120%}
</style>
");
            sb.Append("<div id='EnumInfoHtml'>");


            var temp = GetEnumInfo(startsWith);
            foreach (var ds in temp)
            {
                sb.AppendFormat("<h1>{0}:</h1>", ds.Key);
                foreach (DataTable table in ds.Value.Tables)
                {
                    var talbeNames = table.TableName.Split(new string[] { "-_-" }, StringSplitOptions.None);
                    sb.AppendFormat("<table id='{0}' cellspacing='0'  border='1'  style='border-collapse:collapse;' class='CommonTable'><tr><th colspan='3'><a href='#{0}'>{0}<a> {1} {2}</th></tr>", talbeNames[0], !string.IsNullOrWhiteSpace(talbeNames[1]) ? string.Format("[{0}]", talbeNames[1]) : "", Convert.ToBoolean(talbeNames[2]) ? "[Flags]" : "");
                    sb.Append("<tr><th>Name</th><th>Desc</th><th>Value</th></tr>");
                    foreach (DataRow row in table.Rows)
                    {
                        sb.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", row["Name"], row["Desc"], row["Value"]);
                    }

                    sb.Append("</table><br />");
                }
                sb.Append("<br /><br /><br />");
            }
            sb.Append("</div>");


            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startsWith"></param>
        /// <returns>key:eg:Hotel.Entity.dll, Value:多个datatable</returns>
        public static Dictionary<string, DataSet> GetEnumInfo(params string[] startsWith)
        {

            var result = new Dictionary<string, DataSet>();

            //var lstAssembly = AppDomain.CurrentDomain.GetAssemblies().Where(p =>
            //    p.GetTypes().Any(j => j.IsEnum) && (startsWith == null || startsWith.Length == 0 || startsWith.Any(x => p.FullName.StartsWith(x)))
            //    ).ToList();

            var lstAssembly = AppDomain.CurrentDomain.GetAssemblies().ToList().Where(p => (startsWith == null || startsWith.Length == 0 || startsWith.Any(x => p.FullName.StartsWith(x))));

            foreach (var assembly in lstAssembly)
            {

                var ds = new DataSet();

                var lstEnumType = assembly.GetTypes().Where(p => p.IsEnum).ToList();
                foreach (var enumType in lstEnumType)
                {
                    var table = enumType.GetEnumInfo();
                    table.TableName = enumType.FullName + "-_-" + enumType.GetAttributeDesc() + "-_-" + (Attribute.GetCustomAttribute(enumType, typeof(FlagsAttribute)) != null);

                    ds.Tables.Add(table);
                }
                result.Add(Path.GetFileName(assembly.Location), ds);
            }

            return result;
        }

        /// <summary>
        /// 取得Enum的Name,Desc,Value
        /// </summary>
        /// <param name="enumType"></param>
        public static DataTable GetEnumInfo(this Type enumType)
        {
            if (!enumType.IsEnum)
            {
                return null;
            }

            //建立DataTable的列信息
            var dt = new DataTable();
            dt.Columns.Add("Name", typeof(String));
            dt.Columns.Add("Desc", typeof(String));
            dt.Columns.Add("Value", typeof(String));


            //获得枚举的字段信息（因为枚举的值实际上是一个static的字段的值）
            var fields = enumType.GetFields().Where(p => p.FieldType.IsEnum);

            //检索所有字段
            foreach (var field in fields)
            {
                var dr = dt.NewRow();
                dr["Name"] = field.Name;
                dr["Desc"] = field.GetAttributeDesc();
                dr["Value"] = Convert.ToInt64(field.GetValue(null));

                dt.Rows.Add(dr);
            }

            return dt;
        }

        /// <summary>
        /// 取得DescriptionAttribute,DisplayNameAttribute,DisplayAttribute的说明文字
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public static string GetAttributeDesc(this MemberInfo memberInfo)
        {
            var da = (DescriptionAttribute)Attribute.GetCustomAttribute(memberInfo, typeof(DescriptionAttribute));
            if (da != null && !string.IsNullOrEmpty(da.Description))
            {
                return da.Description;
            }
            var dna = (DisplayNameAttribute)Attribute.GetCustomAttribute(memberInfo, typeof(DisplayNameAttribute));
            if (dna != null && !string.IsNullOrEmpty(dna.DisplayName))
            {
                return dna.DisplayName;
            }
            var na = (DisplayAttribute)Attribute.GetCustomAttribute(memberInfo, typeof(DisplayAttribute));
            if (na != null && !string.IsNullOrEmpty(na.Name))
            {
                return na.Name;
            }

            return "";
        }
    }
}
