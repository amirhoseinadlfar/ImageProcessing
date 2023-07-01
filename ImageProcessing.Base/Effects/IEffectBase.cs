using System;
using System.Numerics;

namespace ImageProcessing.Base.Effects
{
    public interface IEffectBase<T> : IEffectBase
    {
        void Process(T arg);
    }

    public interface IEffectBase
    {
        byte[,,] Image { get; set; }
        byte[,,]? Output { get; }
        void Init();
    }
}