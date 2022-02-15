using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DST.Common.Expressions
{
    public class ReflectionExpression
    {
        public static ReflectionExpression Singleton { get; } = new ReflectionExpression();
        /// <summary>
        /// 根据 属性名称 获取 对象属性值
        /// </summary>
        public Func<object, string, object> GetPropValueFunc { get; } = new Lazy<Func<object, string, object>>(() =>
        {
            var param_item = Expression.Parameter(typeof(object), "item");
            var param_valuePath = Expression.Parameter(typeof(string), "selectedValuePath");
            var const_Null = Expression.Constant(null, typeof(object));
            // GetType
            var methodGetType = typeof(object).GetMethod(nameof(object.GetType), new Type[] { });
            var method_exp1 = Expression.Call(param_item, methodGetType);
            // GetProperty
            var methodGetProperty = typeof(Type).GetMethod(nameof(Type.GetProperty), new Type[] { typeof(string) });
            var method_exp2 = Expression.Call(method_exp1, methodGetProperty, param_valuePath);
            // GetProperty的结果 == null
            var euqalNull_exp = Expression.Equal(method_exp2, const_Null);
            // GetValue
            var methodGetValue = typeof(PropertyInfo).GetMethod(nameof(PropertyInfo.GetValue), new Type[] { typeof(object) });
            var method_exp3 = Expression.Call(method_exp2, methodGetValue, param_item);
            // 如果 GetProperty的结果 == null，则 返回 null 否则 调用 GetValue
            var exp = Expression.Condition(euqalNull_exp, const_Null, method_exp3);

            var action = Expression.Lambda<Func<object, string, object>>(exp, param_item, param_valuePath).Compile();
            return action;
        }).Value;
    }
}
