using System;
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
    [Serializable]
    public class SOM
    {
        [XmlArray("neurony")]
        public Neuron[] neurony { get; set; }
        [XmlElement("wspolczynnik_uczenia")]
        public double wsp_uczenia { get; set; }
        [XmlElement("promien")]
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
            for (int i=0; i<n; i++){
                this.neurony[i]=new Neuron(nwag);
            }
            this.wsp_uczenia = 0.3;
            this.promien = 0.5;
        }

        public SOM(int n, int nwag, double wu, double r)
        {
            this.neurony = new Neuron[n];
            for (int i = 0; i < n; i++)
            {
                this.neurony[i] = new Neuron(nwag);
            }
            this.wsp_uczenia = wu;
            this.promien = r;
        }

        public void uczsiec(double [][] dane, int epoki, System.Windows.Forms.ToolStripProgressBar pb)
        {
            for (int h = 0; h < epoki; h++)
            {
                pb.Value++;
                for (int i = 0; i < dane.Length; i++)
                {
                    for (int j = 0; j < this.neurony.Length; j++)
                    {
                        this.neurony[j].ucz(this.wsp_uczenia, dane[i]);
                    }

                }
            }
        }

        Neuron zwyciezca(double [] dane)
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
