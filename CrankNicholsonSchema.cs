using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace course_work_1
{
    class CrankNicholsonSchema : Solver
    {
        double[] a;
        double[] b;
        double B;
        double B0;
        double kof;

        public override void Init(double c, double k, double alpha, double R, double T, int I, int K)
        {
            base.Init(c, k, alpha, R, T, I, K);
            kof = k * ht / c / hr / hr / 2;
            B = 2 * kof;
            B0 = 6 * kof;

            a = new double[I];
            b = new double[I];

            a[0] = 6 * kof / (1 + B0);
            for (int i = 1; i < I; i++)
                a[i] = -C(i) / (A(i) * a[i - 1] + 1 + B);
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
            b[0] = (layer_1[0] * (1 - B0) + 6 * kof * layer_1[1]) / (1 + B0);
            for (int i = 1; i < I; i++)
            {
                double Fi = -A(i) * layer_1[i- 1] + layer_1[i] * (1 - B) - C(i) * layer_1[i + 1];
                b[i] = (Fi - A(i) * b[i - 1]) / (A(i) * a[i - 1] + 1 + B);
            }

            {
                double BI = 0.5 / hr + alpha / 2 / k * (1 + hr / R);
                double BI_plus = c * hr / 2 / k / ht;
                double AI = -0.5 / hr;
                layer_2[I] = (layer_1[I] * (BI_plus - BI) - AI * layer_1[I - 1] - AI * b[I - 1]) / (AI * a[I - 1] + BI_plus + BI);
            }
            for (int i = I; i > 0; i--)
                layer_2[i-1] = a[i - 1] * layer_2[i] + b[i - 1];
        }
    }
}
