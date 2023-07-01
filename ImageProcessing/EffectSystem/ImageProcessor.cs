using ImageProcessing.Base;
using ImageProcessing.Base.Effects;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing.EffectSystem
{
    public class ImageProcessor
    {

        public ImageProcessor(EffectType effectType, IEffectBase effect, Type type, ImageProcessorUnit imageProcessorUnit)
        {
            EffectType = effectType;
            Effect = effect;
            Type = type;
            ImageProcessorUnit = imageProcessorUnit;
        }
        public EffectType EffectType { get; }
        public IEffectBase Effect { get; }
        public Type Type { get; }
        public ImageProcessorUnit ImageProcessorUnit { get; }
    }
}
