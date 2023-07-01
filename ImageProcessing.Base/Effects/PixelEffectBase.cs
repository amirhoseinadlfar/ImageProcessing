using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing.Base.Effects
{
    public abstract class PixelEffectBase : IEffectBase<Vector2D<uint>>
    {
        public byte[,,] Image { get; set; }
        public byte[,,] Output { get; set; }
        public abstract void Init();
        public abstract void Process(Vector2D<uint> pixel);
    }
}
