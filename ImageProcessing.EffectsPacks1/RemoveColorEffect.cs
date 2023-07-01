using ImageProcessing.Base;
using ImageProcessing.Base.Attributes;
using ImageProcessing.Base.Effects;
using ImageProcessing.Helpers;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing.EffectsPacks1
{
    [Effect("حذف رنگ")]
    [Description("این افکت برای حذف یکی از 3 رنگ اصلی استفاده میشود")]
    public class RemoveColorEffect : PixelEffectBase
    {
        public override void Init()
        {
            Output = Image.CloneEmpty();
        }
        [EffectProperty("R")]
        [Description("حذف رنگ قرمز")]
        public bool ColorRed { get; set; }
        [EffectProperty("G")]
        [Description("حذف رنگ سبز")]
        public bool ColorGreen { get; set; }
        [EffectProperty("B")]
        [Description("حذف رنگ آبی")]
        public bool ColorBlue { get; set; }
        public override void Process(Vector2D<uint> pixel)
        {
            Output[pixel.X, pixel.Y, 0] = Image[pixel.X, pixel.Y, 0];
            Output[pixel.X, pixel.Y, 1] = ColorRed ? byte.MinValue : Image[pixel.X, pixel.Y, 1];
            Output[pixel.X, pixel.Y, 2] = ColorGreen ? byte.MinValue : Image[pixel.X, pixel.Y, 2];
            Output[pixel.X, pixel.Y, 3] = ColorBlue ? byte.MinValue : Image[pixel.X, pixel.Y, 3];
        }
    }
}
