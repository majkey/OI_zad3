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
            this.label15.Text = "";
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
                this.toolStripProgressBar1.Maximum = collection.Count + ((String[])collection[0]).Length;

                String[] first_row = (String[])collection[0];
                for (int i = 0; i < first_row.Length; i++)
                {
                    if (i != 0)
                        this.listBox1.Items.Add(first_row[i]);
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

                for (int i = 1; i < this.dataGridView1.Columns.Count; i++)
                {
                    double[] array = new double[this.dataGridView1.Rows.Count];
                    this.toolStripProgressBar1.Value++;
                    for (int j = 0; j < this.dataGridView1.Rows.Count; j++)
                    {
                        String tmp = this.dataGridView1.Rows[j].Cells[i].FormattedValue.ToString();
                        if (tmp == "")
                            tmp = "0,0";
                        array[j] = Double.Parse(tmp, CultureInfo.InvariantCulture);
                    }
                    String[] row = new String[5];
                    row[0] = first_row[i];
                    row[1] = Statystyka.srednia(array).ToString();
                    row[2] = Statystyka.mediana(array).ToString();
                    row[3] = Statystyka.odchylenie(array).ToString();
                    row[4] = Statystyka.skosnosc(array).ToString();
                    this.dataGridView2.Rows.Add(row);
                }
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
            this.toolStripProgressBar1.Maximum = this.dataGridView1.Rows.Count + (int)this.numericUpDown2.Value;
            this.label15.Text = "Przygotowywanie danych...";

            double[][] array = new double[this.dataGridView1.Rows.Count][];
            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                array[i] = new double[this.listBox1.SelectedItems.Count];
                this.toolStripProgressBar1.Value++;
                int l = 0;
                foreach (int j in this.listBox1.SelectedIndices)
                {
                    String tmp = this.dataGridView1.Rows[i].Cells[j + 1].FormattedValue.ToString();
                    if (tmp == "")
                        tmp = "0.0";
                    array[i][l] = Double.Parse(tmp, CultureInfo.InvariantCulture);
                    l++;
                }
            }

            // tworzenie sieci neuronowej
            this.som = new SOM((int)this.numericUpDown1.Value, (int)this.listBox1.SelectedItems.Count, double.Parse(this.textBox1.Text, CultureInfo.InvariantCulture), double.Parse(this.textBox2.Text, CultureInfo.InvariantCulture));

            // uczenie sieci
            this.label15.Text = "Uczenie sieci...";
            this.som.uczsiec(array, (int)this.numericUpDown2.Value, this.toolStripProgressBar1);

            // sprzątanie
            this.label15.Text = "Sieć nauczona!";
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
    }
}
