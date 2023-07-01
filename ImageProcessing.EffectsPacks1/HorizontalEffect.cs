using ImageProcessing.Base;
using ImageProcessing.Base.Attributes;
using ImageProcessing.Base.Effects;
using ImageProcessing.Helpers;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing.EffectsPacks1
{
    [Effect("لبه های عمودی")]
    public class HorizontalEffect : PixelEffectBase
    {
        public override void Init()
        {
            Output = Image.CloneWithAlpha();
        }

        public override void Process(Vector2D<uint> pixel)
        {
            uint x = pixel.X;
            uint y = pixel.Y;
            if (x == 0 || x + 1 >= Output.GetLength(0))
                return;
            if (y == 0 || y + 1 >= Output.GetLength(1))
                return;
            for(int i = 1;i< 4;i++)
            {
                Output[x, y, i] = (byte)Math.Clamp(Math.Abs((-Image[x - 1, y - 1, i] - Image[x, y - 1, i] - Image[x + 1, y - 1, i] + Image[x - 1, y + 1, i] + Image[x, y + 1, i] + Image[x + 1, y + 1, i])),byte.MinValue,byte.MaxValue);
            }
        }
    }
}
