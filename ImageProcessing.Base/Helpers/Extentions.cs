using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing.Helpers
{
    public static class Extentions
    {
        public static TNum[,,] CloneEmpty<TNum>(this TNum[,,] nums)
            
        {
            return new TNum[nums.GetLength(0), nums.GetLength(1), nums.GetLength(2)];
        }
        public static TNum[,,] CloneWithAlpha<TNum>(this TNum[,,] nums)
            
        {
            var val = new TNum[nums.GetLength(0), nums.GetLength(1), nums.GetLength(2)];
            for (int x = 0; x < val.GetLength(0); x++)
            {
                for (int y = 0; y < val.GetLength(1); y++)
                {
                    val[x, y, 0] = nums[x, y, 0];
                }
            }
            return val;
        }
    }
}
