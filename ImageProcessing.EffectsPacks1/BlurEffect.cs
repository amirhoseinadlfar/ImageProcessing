using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ImageProcessing.Base;
using ImageProcessing.Base.Attributes;
using ImageProcessing.Base.Effects;
using ImageProcessing.Helpers;

namespace ImageProcessing.EffectsPacks1
{
    [Effect("بلور")]
    [Description("این افکت برای تار کردن صفحه استفاده می شود")]
    public class BlurEffect : AreaEffectBase
    {
        [EffectProperty("سطح")]
        public int Level { get; set; }

        public override void Init()
        {
            Output = Image.CloneEmpty();
        }
        public override void Process(AreaInfo areaInfo)
        {
            int imageW = Image.GetLength(0);
            int imageH = Image.GetLength(1);
            var area = areaInfo.ProcessArea;
            for (int x = area.X; x < area.Width; x++)
            {
                for (int y = area.Y; y < area.Height; y++)
                {
                    int[] argbAvg = new int[Image.GetLength(2)];
                    int validPixels = 0;
                    for (int xAlpha = -Level; xAlpha < Level; xAlpha++)
                    {
                        int xLoc = x - xAlpha;
                        if (xLoc < 0 || xLoc > imageW - 1)
                            continue;
                        for (int yAlpha = -Level; yAlpha < Level; yAlpha++)
                        {
                            int yLoc = y - yAlpha;
                            if (yLoc < 0 || yLoc > imageH - 1)
                                continue;

                            validPixels++;
                            for (int colorIndex = 0; colorIndex < argbAvg.Length; colorIndex++)
                            {
                                argbAvg[colorIndex] += Image[xLoc,yLoc,colorIndex];
                            }

                        }
                    }
                    for (int c = 0; c < argbAvg.Length; c++)
                        Output[x, y, c] = (byte)(argbAvg[c] / validPixels);
                }
                int xr = x - area.X + 1;
                areaInfo.CompeletedPixels = xr * area.Height;
            }
        }
    }
}
