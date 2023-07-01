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
    [Effect("سیاه سفید")]
    [Description("این افکت برای سیاه و سفید کردن تصویر استفاده می شود")]
    public class BlackWhiteEffect : PixelEffectBase
    {
        public override void Init()
        {
            Output = Image.CloneEmpty();
            Output = Image.CloneWithAlpha();
        }

        public override void Process(Vector2D<uint> pixel)
        {
            int avg = 0;
            for (int i = 0; i < 4; i++)
            {
                avg += Image[pixel.X, pixel.Y, i];
            }
            avg /= 4;
            for (int i = 0; i < 4; i++)
            {
                Output[pixel.X, pixel.Y, i] = (byte)avg;
            }
        }
    }
}
