using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing.Base
{
    public struct Vector2D<T> where T : struct
    {
        public Vector2D()
        {
            X = default;
            Y = default;
        }
        public Vector2D(T x,T y)
        {
            X = x;
            Y = y;
        }
        public T X { get; set; }
        public T Y { get; set; }
    }
}
