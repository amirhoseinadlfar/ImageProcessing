using ImageProcessing.Base.Attributes;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing.EffectSystem
{
    public class ImageProcessorProperty
    {
        public ImageProcessorProperty(PropertyInfo propertyInfo,EffectPropertyAttribute effectPropertyAttribute)
        {
            Name = effectPropertyAttribute.Name;
            var des = propertyInfo.GetCustomAttribute<DescriptionAttribute>();
            Description = des?.Description ?? string.Empty;
            PropertyInfo = propertyInfo;
        }
        public string Name { get; }
        public string Description { get; }
        public PropertyInfo PropertyInfo { get; }
    }
}
