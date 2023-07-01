using ImageProcessing.Base;
using ImageProcessing.Base.Effects;
using ImageProcessing.EffectSystem;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace ImageProcessing.ImageRenderers
{
    internal abstract class RenderBase : IRender
    {
        protected readonly IEffectBase effectBase;
        protected readonly ImageProcessor processor;
        protected readonly byte threadCount;
        protected readonly MultiThreadRenderMode trenderMode;
        protected Dictionary<object, AreaInfo>? activeThreads;
        public RenderBase(ImageProcessor processor, MultiThreadRenderMode trenderMode, byte threadCount)
        {

            this.processor = processor;
            this.effectBase = processor.Effect;
            this.trenderMode = trenderMode;
            this.threadCount = threadCount;
        }
        public double CalculateComp(bool fromHoundred)
        {
            if (activeThreads == null)
                throw new Exception("process must be started");
            double all = 0;
            foreach (var item in activeThreads)
            {
                all += item.Value.CompeletedPixels;
            }
            return all / (effectBase.Image.GetLength(0) * effectBase.Image.GetLength(1)) * (fromHoundred ? 100 : 1);

        }
        public byte[,,] GetOutput()
        {
            if (effectBase.Output == null)
                throw new Exception("effect did not Init");
            return effectBase.Output;
        }

        public void SetImage(byte[,,] bytes)
        {
            effectBase.Image = bytes;
        }

        public void StartRender()
        {
            if (activeThreads != null)
                throw new Exception("process already started");
            if (effectBase.Image == null)
                throw new Exception("input image is invalid");
            activeThreads = new Dictionary<object, AreaInfo>();
            int width = effectBase.Image.GetLength(0);
            int height = effectBase.Image.GetLength(1);

            int allWidth = (width / threadCount);
            int allHeight = (height / threadCount);
            effectBase.Init();
            for (int i = 0; i < threadCount; i++)
            {
                Rectangle rectangle = new Rectangle();
                rectangle.X = allWidth * i;
                rectangle.Y = 0;
                rectangle.Width = rectangle.X + allWidth;
                rectangle.Height = height;
                if (i == threadCount - 1)
                {
                    rectangle.Width = width;
                }
                AreaInfo areaInfo = new AreaInfo(rectangle);
                if (trenderMode == MultiThreadRenderMode.Task)
                {
                    var task = Task.Run(() => RunProcess(areaInfo));
                    activeThreads.Add(task, areaInfo);
                }
                else if (trenderMode == MultiThreadRenderMode.Thread)
                {
                    var thread = new Thread(() => RunProcess(areaInfo));
                    thread.Start();
                    activeThreads.Add(thread, areaInfo);
                }
            }
        }
        protected abstract void RunProcess(AreaInfo areaInfo);
        
    }
}