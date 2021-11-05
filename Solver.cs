using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace course_work_1
{
    public abstract class Solver
    {
        protected double c;
        protected double k;
        protected double alpha;
        protected double R;
        protected double T;
        protected int I;
        protected int K;

        protected double ht;
        protected double hr;

        List<Layer> horLayers;
        List<Layer> vertLayers;

        protected double[] layer_1;
        protected double[] layer_2;

        public virtual void Init(double c, double k, double alpha, double R, double T, int I, int K)
        {
            this.c = c;
            this.k = k;
            this.alpha = alpha;
            this.R = R;
            this.T = T;
            this.I = I;
            this.K = K;

            hr = R / I;
            ht = T / K;

            layer_1 = new double[I + 1];
            layer_2 = new double[I + 1];
            setFirstLayer();
        }
        public void SetCollectors(List<Layer> horLayers, List<Layer> vertLayers)
        {
            this.horLayers = horLayers;
            this.vertLayers = vertLayers;
        }

        public List<Layer> getHorizontalLayers()
        {
            return horLayers;
        }
        public List<Layer> getVerticalLayers()
        {
            return vertLayers;
        }

        public event Action Finish;
        public event Action<double> Process;

        public void Start()
        {
            int index = 0;
            int numHorColl = 0;

            while(index <= K)
            {
                CollectVertData(index);
                Step();
                if(numHorColl < horLayers.Count && index == horLayers[numHorColl].Index)
                    layer_1 = horLayers[numHorColl++].swapArray(layer_1);
                swapLayers();
                Process?.Invoke(index * 1.0 / K);
                index++;
            }
            Finish?.Invoke();
        }
        protected abstract void Step();
        protected void CollectVertData(int index)
        {
            foreach(var x in vertLayers)
                x.Array[index] = layer_1[x.Index];
        }
        protected void swapLayers()
        {
            double[] t = layer_1;
            layer_1 = layer_2;
            layer_2 = t;
        }
        protected void setFirstLayer()
        {
            for (int i = 0; i <= I; i++) {
                double r = hr * i / 0.1 / R;
                layer_1[i] = 180 * Math.Exp(-r*r);
            }
        }
    }
}
