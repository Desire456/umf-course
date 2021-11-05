using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ZedGraph;

namespace course_work_1
{
    public partial class FormShow : Form
    {
        List<Layer> horLayers;
        List<Layer> vertLayers;

        public FormShow(List<Layer> horLayers, List<Layer> vertLayers)
        {
            InitializeComponent();

            this.horLayers = horLayers;
            this.vertLayers = vertLayers;
            if (vertLayers.Count == 0) radioButton2.Enabled = false;
            else if(horLayers.Count == 0)
            {
                radioButton1.Enabled = false;
                radioButton2.Checked = true;
            }

            GraphPane pane = zedGraph.GraphPane;
            pane.YAxis.Title.Text = "u";
            pane.Title.Text = "";
            pane.XAxis.MinorGrid.IsVisible = true;
            pane.YAxis.MinorGrid.IsVisible = true;
            pane.XAxis.MajorGrid.IsVisible = true;
            pane.YAxis.MajorGrid.IsVisible = true;
            radioButton1_CheckedChanged(null, null);
        }

        private void DrawGraphics(List<Layer> list)
        {
            GraphPane pane = zedGraph.GraphPane;
            pane.CurveList.Clear();
            if(list.Count != 0)
            {
                double[] x = new double[list[0].Array.Length];
                for (int i = 0; i < x.Length; i++)
                    x[i] = list[0].Length * i / (x.Length - 1);

                var sel = listBox1.SelectedIndex;
                for (int i = 0; i < list.Count; i++)
                {
                    PointPairList graph = new PointPairList(x, list[i].Array);
                    if (i != sel)
                        pane.AddCurve(listBox1.Items[i].ToString(), graph, Color.DarkBlue, SymbolType.None);
                    else
                    {
                        LineItem t = pane.AddCurve(listBox1.Items[i].ToString(), graph, Color.Red, SymbolType.None);
                        t.Line.Width = 2;
                    }
                }

                zedGraph.AxisChange();
                zedGraph.Invalidate();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            if (index == -1) return;
            List<Layer> list = radioButton1.Checked ? horLayers : vertLayers;
            using(BinaryWriter writer = new BinaryWriter(File.Open("data " + listBox1.SelectedItem.ToString() + ".bin", FileMode.Create)))
            {
                writer.Write(list[index].Array.Length);
                writer.Write(list[index].Length);
                writer.Write(list[index].Array.SelectMany(val => BitConverter.GetBytes(val)).ToArray());
            }
            MessageBox.Show("Данные успешно сохранены!");
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            GraphPane pane = zedGraph.GraphPane;
            listBox1.Items.Clear();
            if (radioButton1.Checked)
            {
                pane.XAxis.Title.Text = "r";
                foreach (var x in horLayers)
                    listBox1.Items.Add("t = " + x.Point);
                DrawGraphics(horLayers);
            } 
            else
            {
                pane.XAxis.Title.Text = "t";
                foreach (var x in vertLayers)
                    listBox1.Items.Add("r = " + x.Point);
                DrawGraphics(vertLayers);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                DrawGraphics(horLayers);
            else
                DrawGraphics(vertLayers);
        }
    }
}
