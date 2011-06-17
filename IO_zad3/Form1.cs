using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Globalization;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public String file_name = "";
        public String som_file_name = "";
        public SOM som;

        public Form1()
        {
            InitializeComponent();
            this.label5.Text = "";
            this.label6.Text = "";
            this.label7.Text = "";
            this.label8.Text = "";
            this.label9.Text = "";
            this.label20.Text = "";
            this.label21.Text = "";
            this.label22.Text = "";
            this.label23.Text = "";
            this.label25.Text = "";
            this.label24.Text = "";
            this.label26.Text = "";
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;
                this.file_name = openFileDialog1.FileName;
                this.toolStripProgressBar1.Enabled = true;

                CSV csv_file = new CSV(this.file_name);
                ArrayList collection = csv_file.GetCollection();
                this.toolStripProgressBar1.Maximum = collection.Count;

                String[] first_row = (String[])collection[0];
                for (int i = 0; i < first_row.Length; i++)
                {
                    this.listBox1.Items.Add(first_row[i]);
                    this.listBox2.Items.Add(first_row[i]);
                    this.listBox3.Items.Add(first_row[i]);
                    dataGridView1.Columns.Add((String)first_row[i], (String)first_row[i]);
                }

                for (int i = 1; i < collection.Count; i++)
                {
                    String[] row = (String[])collection[i];
                    dataGridView1.Rows.Add(row);
                    this.toolStripProgressBar1.Value++;
                }

                FileInfo fInfo = new FileInfo(file_name);
                this.label5.Text = this.file_name;
                this.label6.Text = fInfo.Length.ToString();
                this.label7.Text = (collection.Count - 1).ToString();
                this.label8.Text = (first_row.Length - 1).ToString();
                this.label9.Text = fInfo.Attributes.ToString();

                this.toolStripProgressBar1.Enabled = false;
                this.toolStripProgressBar1.Value = 0;
                this.Cursor = Cursors.Default;
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            // Wczytywanie sieci
            DialogResult result = openFileDialog2.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.som_file_name = this.openFileDialog2.FileName;
                this.som = SOM.wczytajsiec(this.som_file_name);
                this.numericUpDown1.Value = this.som.neurony.Length;
                this.textBox1.Text = this.som.wsp_uczenia.ToString();
                this.textBox2.Text = this.som.promien.ToString();
            }
            this.toolStripButton3.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Uczenie sieci
            // przygotowywanie danych
            this.Cursor = Cursors.WaitCursor;
            this.toolStripProgressBar1.Enabled = true;
            this.toolStripProgressBar1.Maximum = (int)this.numericUpDown2.Value;
            double [][] array;

            if (this.listBox1.SelectedIndices.Count == this.listBox3.SelectedIndices.Count)
                array = new double[this.dataGridView1.Rows.Count * 2][];
            else
                array = new double[this.dataGridView1.Rows.Count][];
            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                array[i] = new double[this.listBox1.SelectedIndices.Count];
                for (int j = 0; j < this.listBox1.SelectedIndices.Count; j++ )
                {
                    String tmp = this.dataGridView1.Rows[i].Cells[this.listBox1.SelectedIndices[j]].FormattedValue.ToString().Replace('.',',');
                    if(tmp == "")
                        tmp = "0,0";
                    array[i][j] = double.Parse(tmp);
                }
            }

            if (this.listBox1.SelectedIndices.Count == this.listBox3.SelectedIndices.Count)
            {
                for (int i = this.dataGridView1.Rows.Count; i < this.dataGridView1.Rows.Count * 2; i++)
                {
                    array[i] = new double[this.listBox3.SelectedIndices.Count];
                    for (int j = 0; j < this.listBox3.SelectedIndices.Count; j++)
                    {
                        String tmp = this.dataGridView1.Rows[i - this.dataGridView1.Rows.Count].Cells[this.listBox3.SelectedIndices[j]].FormattedValue.ToString().Replace('.', ',');
                        if (tmp == "")
                            tmp = "0,0";
                        array[i][j] = double.Parse(tmp);
                    }
                }
            }

            // tworzenie sieci neuronowej
            this.som = new SOM((int)this.numericUpDown1.Value, (int)this.listBox1.SelectedItems.Count, double.Parse(this.textBox1.Text, CultureInfo.InvariantCulture), double.Parse(this.textBox2.Text, CultureInfo.InvariantCulture));

            // uczenie sieci
            this.som.uczsiec(array, (int)this.numericUpDown2.Value, this.toolStripProgressBar1);

            // sprzątanie
            this.richTextBox1.Text = this.som.ToString();
            this.toolStripProgressBar1.Value = 0;
            this.toolStripProgressBar1.Enabled = false;
            this.Cursor = Cursors.Default;
            this.toolStripButton3.Enabled = true;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.button1.Enabled = true;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            DialogResult result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.som.zapiszsiec(this.saveFileDialog1.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            double [] array = new double[this.listBox2.SelectedIndices.Count];
            for (int i = 0; i < this.listBox2.SelectedIndices.Count; i++ )
                array[i] = double.Parse(this.dataGridView1.SelectedRows[0].Cells[this.listBox2.SelectedIndices[i]].FormattedValue.ToString().Replace('.', ','));

            this.label20.Text = Statystyka.srednia(array).ToString();
            this.label21.Text = Statystyka.mediana(array).ToString();
            this.label22.Text = Statystyka.odchylenie(array).ToString();
            this.label23.Text = Statystyka.skosnosc(array).ToString();
            int pos = 0;

            if (this.som != null && this.som.neurony[0].wagi.Length == array.Length)
            {
                Neuron winner = this.som.zwyciezca(array);
                pos = this.som.zwyciezca_index(array);
                this.label24.Text = "Neuron nr: " + (pos + 1).ToString();
                this.label25.Text = "Odległość: " + winner.odleglosc(array).ToString();
            }

            int stat = 0;
            for (int i = 0; i < this.dataGridView1.RowCount; i++ )
            {
                double[] tab = new double[this.listBox2.SelectedIndices.Count];
                for (int j = 0; j < this.listBox2.SelectedIndices.Count; j++)
                {
                    string s = this.dataGridView1.Rows[i].Cells[this.listBox2.SelectedIndices[j]].FormattedValue.ToString().Replace('.', ',');
                    if (s == "")
                        s = "0,0";
                    tab[j] = double.Parse(s);
                }
                if (tab != null && this.som.zwyciezca_index(tab) == pos)
                    stat++;
                int color = this.som.zwyciezca_index(tab) * 255 / (int)this.numericUpDown1.Value;
                for (int j = 0; j < this.dataGridView1.ColumnCount; j++)
                {
                    this.dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.FromArgb(color, color, color);
                    if(color < 130)
                        this.dataGridView1.Rows[i].Cells[j].Style.ForeColor = Color.White;
                    else
                        this.dataGridView1.Rows[i].Cells[j].Style.ForeColor = Color.Black;
                }
            }
            this.label26.Text = "Wraz z " + stat.ToString() + " danymi.";

            this.Cursor = Cursors.Default;
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBox2.SelectedItems.Count > 0 && this.dataGridView1.SelectedRows.Count > 0)
            {
                this.button2.Enabled = true;
            }
            else
            {
                this.button2.Enabled = false;
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (this.listBox2.SelectedItems.Count > 0 && this.dataGridView1.SelectedRows.Count > 0)
            {
                this.button2.Enabled = true;
            }
            else
            {
                this.button2.Enabled = false;
            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBox3.SelectedIndices.Count > 0)
                this.button1.Enabled = true;
            else
                this.button1.Enabled = false;
        }

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedIndices.Count > 0)
                this.button1.Enabled = true;
            else
                this.button1.Enabled = false;
        }

        double odleglosc(double[] t1, double[] t2)
        {
            double result = 0.0;
            for (int i = 0; i < t1.Length; i++)
            {
                result += Math.Pow(t1[i] - t2[i], 2);
            }
            return result;
        }
    }
}
