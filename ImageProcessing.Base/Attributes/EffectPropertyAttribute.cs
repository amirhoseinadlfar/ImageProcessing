using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing.Base.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class EffectPropertyAttribute : Attribute
    {
        public string Name { get; }
        public EffectPropertyAttribute(string name)
        {
            Name = name;
        }
    }
}
