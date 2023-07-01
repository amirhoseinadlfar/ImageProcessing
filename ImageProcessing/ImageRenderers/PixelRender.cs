using ImageProcessing.Base.Effects;
using ImageProcessing.Base;
using ImageProcessing.EffectSystem;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;

namespace ImageProcessing.ImageRenderers
{
    class PixelRender : RenderBase
    {
        public PixelRender(ImageProcessor processor, MultiThreadRenderMode trenderMode, byte threadCount):base(processor, trenderMode, threadCount)
        {
            if (processor.EffectType != EffectType.Pixel)
                throw new ArgumentException("processor can i only be Area type", nameof(processor));

        }

        protected override void RunProcess(AreaInfo areaInfo)
        {
            RenderLoop(areaInfo);
        }
        void RenderLoop(AreaInfo areaInfo)
        {
            Vector2D<uint> vector2D;
            if (effectBase is IEffectBase<Vector2D<uint>> eff)
            {
                for (int x = areaInfo.ProcessArea.X; x < areaInfo.ProcessArea.Width; x++)
                {
                    for (int y = areaInfo.ProcessArea.Y; y < areaInfo.ProcessArea.Height; y++)
                    {
                        vector2D = new Vector2D<uint>((uint)x, (uint)y);
                        eff.Process(vector2D);
                        areaInfo.CompeletedPixels++;
                    }
                }
            }
        }
    }
    public enum MultiThreadRenderMode
    {
        Task,
        Thread,
    }
}
