using ImageProcessing.Base;
using ImageProcessing.Base.Effects;
using ImageProcessing.EffectSystem;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageProcessing.ImageRenderers
{
    internal class AreaRender : RenderBase
    {
        public AreaRender(ImageProcessor processor, MultiThreadRenderMode trenderMode, byte threadCount) : base(processor, trenderMode, threadCount)
        {
            if (processor.EffectType != EffectType.Area)
                throw new ArgumentException("processor can i only be Area type", nameof(processor));
        }

        protected override void RunProcess(AreaInfo areaInfo)
        {
            if(effectBase is IEffectBase<AreaInfo> eff)
                eff.Process(areaInfo);
        }
    }
}
