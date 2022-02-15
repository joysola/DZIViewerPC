using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DST.Common.Extensions
{
    public static class TaskExtensions
    {
        /// <summary>
        /// Task任务(不await) 调用此方法后不产生警告
        /// </summary>
        /// <param name="task"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NoWarning(this Task task) { }
    }
}
