using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing.Base.Effects
{
    public abstract class AreaEffectBase : IEffectBase<AreaInfo>
    {
        public byte[,,] Image { get; set; }
        public byte[,,] Output { get; set; }
        public abstract void Init();
        public abstract void Process(AreaInfo area);
    }
}
