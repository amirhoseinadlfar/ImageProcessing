using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ImageProcessing.Base
{
    public class AreaInfo
    {
        public Rectangle ProcessArea { get; }
        public long CompeletedPixels { get; set; }

        public AreaInfo(Rectangle processArea)
        {
            ProcessArea = processArea;
            CompeletedPixels = 0;
        }
    }
}
