﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace WindowsFormsApplication1
{
    public class SOM
    {
        public Neuron[] neurony { get; set; }
        public double wsp_uczenia { get; set; }
        public double promien { get; set; }

        public SOM()
        {
            this.neurony = null;
            this.wsp_uczenia = 0.3;
            this.promien = 0.5;
        }

        SOM(int n, int nwag)
        {
            this.neurony = new Neuron[n];
            Random seed = new Random();
            for (int i=0; i<n; i++)
                this.neurony[i]=new Neuron(nwag, seed);
            this.wsp_uczenia = 0.3;
            this.promien = 0.5;
        }

        public SOM(int n, int nwag, double wu, double r)
        {
            this.neurony = new Neuron[n];
            Random seed = new Random();
            for (int i = 0; i < n; i++)
                this.neurony[i] = new Neuron(nwag, seed);
            this.wsp_uczenia = wu;
            this.promien = r;
        }

        public string ToString()
        {
            string result = "Sieć SOM:\n";
            result += "\tWspółczynnik uczenia: " + this.wsp_uczenia.ToString() + "\n";
            result += "\tPromień sąsiedztwa: " + this.promien.ToString() + "\n";
            result += "\tNeurony:\n";
            int k = 0;
            foreach (Neuron n in this.neurony)
            {
                k++;
                result += "\t\tNeuron " + k.ToString() + "\n";
                foreach (double v in n.wagi)
                {
                    result += "\t\t\t" + v.ToString() + "\n";
                }
            }
            return result;
        }

        double promien_sasiedztwa(double p0, int epoka)
        {
            return p0 / (epoka + 1);
        }

        public void uczsiec(double [][] dane, int epoki, System.Windows.Forms.ToolStripProgressBar pb)
        {
            for (int i = 0; i < this.neurony.Length; i++)
                this.neurony[i].wagi = dane[dane.Length / (i + 1) % dane.Length];
            for (int h = 0; h < epoki; h++)
            {
                pb.Value++;
                for (int i = 0; i < dane.Length; i++)
                {
                    for (int j = 0; j < this.neurony.Length; j++)
                    {
                        if(dane[i] != null)
                            this.neurony[j].ucz(this.wsp_uczenia, promien_sasiedztwa(this.promien, i), dane[i]);
                    }

                }
            }
        }

        public Neuron zwyciezca(double [] dane)
        {
            Neuron rezultat = this.neurony[0];
            for (int i = 0; i < neurony.Length; i++)
            {
                if (neurony[i].odleglosc(dane) < rezultat.odleglosc(dane))
                {
                    rezultat = neurony[i];
                }
            }
            return rezultat;
        }

        public int zwyciezca_index(double[] dane)
        {
            int rezultat = 0;
            for (int i = 0; i < neurony.Length; i++)
            {
                if (neurony[i].odleglosc(dane) < neurony[rezultat].odleglosc(dane))
                {
                    rezultat = i;
                }
            }
            return rezultat;
        }

        public void zapiszsiec(string nazwapliku)
        {
            string lines = SerializeToXml();
            System.IO.StreamWriter file = new System.IO.StreamWriter(nazwapliku);
            file.WriteLine(lines);
            file.Close();
        }

        public static SOM wczytajsiec(string nazwapliku)
        {
            TextReader tr = new StreamReader(nazwapliku);
            SOM som = DeserializeFromXml(tr.ReadToEnd());
            tr.Close();
            return som;
        }

        string SerializeToXml()
        {
            StringWriter writer = new StringWriter();
            XmlSerializer serializer = new XmlSerializer(typeof(SOM));
            serializer.Serialize(writer, this);
            return writer.ToString();
        }

        static SOM DeserializeFromXml(string s)
        {
            StringReader reader = new StringReader(s);
            XmlSerializer serializer = new XmlSerializer(typeof(SOM));
            SOM som = (SOM)serializer.Deserialize(reader);
            return som;
        }
    }
}
