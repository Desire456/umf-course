using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace course_work_1
{
    public class ImplicitSchema : Solver
    {
        double[] a;
        double[] b;
        double B;
        double B0;
        double kof;


        public override void Init(double c, double k, double alpha, double R, double T, int I, int K)
        {
            base.Init(c, k, alpha, R, T, I, K);
            kof = k * ht / c / hr / hr;
            B = 1 + 2 * kof;
            B0 = 1 + 6 * kof;

            a = new double[I - 1];
            b = new double[I - 1];

            a[0] = 6 * kof / B0;
            for (int i = 1; i < I - 1; i++)
                a[i] = -C(i) / (A(i) * a[i - 1] + B);
        }
        private double A(int i)
        {
            return -kof * (1 - 1.0 / i);
        }
        private double C(int i)
        {
            return -kof * (1 + 1.0 / i);
        }
        protected override void Step()
        {
            b[0] = layer_1[0] / B0;
            for (int i = 1; i < I - 1; i++)
                b[i] = (layer_1[i] - A(i)*b[i-1]) / (A(i) * a[i - 1] + B);

            layer_2[I-1] = (layer_1[I-1] - A(I-1) * b[I-2])/(A(I - 1) * a[I-2] + B + C(I-1)/ (1 + alpha * hr / k));
            for(int i = I - 2; i >= 0; i--)
                layer_2[i] = a[i] * layer_2[i + 1] + b[i];

            layer_2[I] = layer_2[I - 1] / (1 + alpha * hr / k);
        }
    }
}
