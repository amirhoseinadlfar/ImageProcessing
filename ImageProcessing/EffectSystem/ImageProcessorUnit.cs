using ImageProcessing.Base;
using ImageProcessing.Base.Attributes;
using ImageProcessing.Base.Effects;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing.EffectSystem
{
    public enum EffectType
    {
        Pixel,
        Area
    }
    public class ImageProcessorUnit
    {
        public EffectAssembly EffectAssembly { get; }
        public ImageProcessorUnit(Type type, EffectAssembly effectAssembly)
        {

            Type = type;
            if (type.GetInterfaces().Any(x => x == typeof(IEffectBase<Vector2D<uint>>)))
            {
                EffectType = EffectType.Pixel;
            }
            else if (type.GetInterfaces().Any(x => x == typeof(IEffectBase<AreaInfo>)))
            {
                EffectType = EffectType.Area;
            }
            EffectAssembly = effectAssembly;
            var att = type.GetCustomAttribute<EffectAttribute>();
            Name = att?.Name ?? string.Empty;
            var des = type.GetCustomAttribute<DescriptionAttribute>();
            Description = des?.Description ?? string.Empty;

            Properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(x =>
                {
                    var patt = x.GetCustomAttribute<EffectPropertyAttribute>();
                    if (patt is null)
                        return null;
                    return new ImageProcessorProperty(x, patt);
                })
                .Where(x=>x!=null)
                .ToArray()!;

        }
        public ImageProcessor CreateInstance()
        {
            IEffectBase insance = Type.GetConstructor(Array.Empty<Type>())!.Invoke(null) as IEffectBase;
            return new ImageProcessor(EffectType, insance, Type,this);
        }
        public string Name { get; }
        public string Description { get; }
        public EffectType EffectType { get; }
        public Type Type { get; }
        public ImageProcessorProperty[] Properties { get; }
    }
}
