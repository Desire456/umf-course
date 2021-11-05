using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace course_work_1
{
    public class Layer
    {
        public double Point { get; private set; }
        public int Index { get; private set; }
        public double[] Array { get; private set; }
        public double Length { get; private set; }

        public Layer(double Point, double End, double Length, int PointsCount, int ArraySize)
        {
            this.Length = Length;
            this.Point = Point;
            Array = new double[ArraySize];
            double index = Point * (PointsCount - 1) / End;
            Index = (int)(index + 0.5);
        }

        public double[] swapArray(double[] arr)
        {
            double[] t = Array;
            Array = arr;
            return t;
        }
    }
}
