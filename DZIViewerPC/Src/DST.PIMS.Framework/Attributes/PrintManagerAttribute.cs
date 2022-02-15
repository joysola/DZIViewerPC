using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Framework.Attributes
{
    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    sealed class PrintManagerAttribute : Attribute
    {
        
        public PrintManagerAttribute(string printType)
        {
            PrintType = printType;
        }

        public string PrintType { get; set; }
    }
}
