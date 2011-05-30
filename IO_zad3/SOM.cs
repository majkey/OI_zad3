using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

namespace WindowsFormsApplication1
{
    class SOM
    {
        Neuron[] neurony;
        double wsp_uczenia=0.3;
        double promien=0.5;

        SOM(int n, int nwag)
        {
            this.neurony = new Neuron[n];
            for (int i=0; i<n; i++){
                this.neurony[i]=new Neuron(nwag);
            }
        }

        SOM(int n, int nwag, double wu, double r)
        {
            this.neurony = new Neuron[n];
            for (int i = 0; i < n; i++)
            {
                this.neurony[i] = new Neuron(nwag);
            }
            this.wsp_uczenia = wu;
            this.promien = r;
        }

        void uczsiec(double [][] dane, int epoki)
        {
            for (int h = 0; h < epoki; h++)
            {
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

        void zapiszsiec(string nazwapliku)
        {
            Stream stream = null;
            try
            {
                IFormatter formatter = new BinaryFormatter();
                stream = new FileStream(nazwapliku, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, this);
            }
            catch
            {
                // do nothing, just ignore any possible errors
            }
            finally
            {
                if (null != stream)
                    stream.Close();
            }
        }

        public static SOM wczytajsiec(string nazwapliku)
        {
            Stream stream = null;
            SOM siec = null;
            try
            {
                IFormatter formatter = new BinaryFormatter();
                stream = new FileStream(nazwapliku, FileMode.Open, FileAccess.Read, FileShare.None);
                siec = (SOM)formatter.Deserialize(stream);
            }
            catch
            {
                // do nothing, just ignore any possible errors
            }
            finally
            {
                if (null != stream)
                    stream.Close();
            }
            return siec;
        }
    }
}
