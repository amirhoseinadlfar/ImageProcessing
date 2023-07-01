using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing.Base.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class EffectAttribute : Attribute
    {
        public string Name { get; }

        public EffectAttribute(string name)
        {
            Name = name;
        }
    }
}
