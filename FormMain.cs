using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace course_work_1
{
    public partial class FormMain : Form
    {
        double getDouble(TextBox tb)
        {
            return Convert.ToDouble(tb.Text);
        }
        int getInt(TextBox tb)
        {
            return Convert.ToInt32(tb.Text);
        }

        bool checkInputs()
        {
            try
            {
                if (getDouble(textBox_c) <= 0) return false;
                if (getDouble(textBox_k) <= 0) return false;
                if (getDouble(textBox_R) <= 0) return false;
                if (getDouble(textBox_T) <= 0) return false;

                if (getInt(textBox_size_I) < 1) return false;
                if (getInt(textBox_size_K) < 1) return false;
            }
            catch
            {
                return false;
            }

            if (listBox_t.Items.Count == 0 && listBox_r.Items.Count == 0) return false;

            return true;
        }

        public FormMain()
        {
            InitializeComponent();

            textBox_size_I_TextChanged(null, null);
            textBox_size_K_TextChanged(null, null);
        }

        private void button_start_Click(object sender, EventArgs e)
        {
            if (!checkInputs())
            {
                MessageBox.Show("Не корректно введены данные", "Не могу запустить расчет", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int I = getInt(textBox_size_I);
            int K = getInt(textBox_size_K);
            double T = getDouble(textBox_T);
            double R = getDouble(textBox_R);
            double c = getDouble(textBox_c);
            double k = getDouble(textBox_k);
            double alpha = getDouble(textBox_alpha);
            List<Layer> horLayers = new List<Layer>();
            foreach (var x in listBox_t.Items)
                if (Convert.ToDouble(x) <= T && Convert.ToDouble(x) >= 0)
                    horLayers.Add(new Layer(Convert.ToDouble(x), T, R, K + 1, I + 1));
            List<Layer> vertLayers = new List<Layer>();
            foreach (var x in listBox_r.Items)
                if (Convert.ToDouble(x) <= R && Convert.ToDouble(x) >= 0)
                    vertLayers.Add(new Layer(Convert.ToDouble(x), R, T, I + 1, K + 1));

            Solver solv = null;
            if (radioButton1.Checked)
                solv = new ImplicitSchema();
            else if (radioButton2.Checked)
                solv = new CrankNicholsonSchema();

            solv.Init(c, k, alpha, R, T, I, K);
            solv.SetCollectors(horLayers, vertLayers);
            FormLoad form = new FormLoad(solv);
            form.Show();
        }

        private void textBox_size_I_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int I = getInt(textBox_size_I);
                double R = getDouble(textBox_R);
                if (I > 0 && R > 0)
                    label_hr.Text = "hr = " + R / I;
                else
                    label_hr.Text = "hr = NaN";
            }
            catch
            {
                label_hr.Text = "hr = NaN";
            }
        }

        private void textBox_size_K_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int K = getInt(textBox_size_K);
                double T = getDouble(textBox_T);
                if (K > 0 && T > 0)
                    label_ht.Text = "ht = " + T / K;
                else
                    label_ht.Text = "ht = NaN";
            }
            catch
            {
                label_ht.Text = "ht = NaN";
            }
        }

        private void textBox_T_TextChanged(object sender, EventArgs e)
        {
            textBox_size_K_TextChanged(null, null);
        }

        private void textBox_L_TextChanged(object sender, EventArgs e)
        {
            textBox_size_I_TextChanged(null, null);
        }

        private void textBox_new_z_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                try
                {
                    e.SuppressKeyPress = true;
                    listBox_r.Items.Add(textBox_new_r.Text);
                    SortListBox(listBox_r);
                }
                catch { }
        }

        private void textBox_new_t_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                try
                {
                    e.SuppressKeyPress = true;
                    listBox_t.Items.Add(textBox_new_t.Text);
                    SortListBox(listBox_t);
                }
                catch { }
        }

        private void SortListBox(ListBox list)
        {
            List<double> Sorting = new List<double>();
            foreach (var o in list.Items)
                if (!Sorting.Contains(Convert.ToDouble(o)))
                    Sorting.Add(Convert.ToDouble(o));
            Sorting.Sort();
            list.Items.Clear();
            foreach (var o in Sorting)
                list.Items.Add(o);
        }

        private void listBox_r_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                e.SuppressKeyPress = true;
                listBox_r.Items.Remove(listBox_r.SelectedItem);
            }
        }

        private void listBox_t_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                e.SuppressKeyPress = true;
                listBox_t.Items.Remove(listBox_t.SelectedItem);
            }
        }
    }
}
