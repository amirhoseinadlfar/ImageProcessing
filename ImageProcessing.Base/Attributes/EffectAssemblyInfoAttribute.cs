using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing.Base.Attributes
{
    [System.AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
    public sealed class EffectAssemblyInfoAttribute : Attribute
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Creator { get; set; }
    }
}
