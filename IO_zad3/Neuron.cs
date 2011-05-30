using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    public class Neuron
    {
        double[] wagi;

        public Neuron(int rozmiar)
        {
            this.wagi=new double[rozmiar];
            for (int i = 0; i < wagi.Length; i++)
            {
                Random numer = new Random();
                wagi[i] = numer.NextDouble();
            }
        }

        public Neuron(double[] tablica)
        {
            this.wagi = tablica;
        }
        

        public double odleglosc(double[] tablica1)
        {
            int wymiar = tablica1.Length;
            double sumacalosc = 0;

            for (int i = 0; i < wymiar; i++)
            {
                sumacalosc += Math.Pow(tablica1[i] - this.wagi[i], 2);
            }

            return Math.Sqrt(sumacalosc);
        }

        double sasiedztwo(double[] tablica1, double promien)
        {
            double rezultat = 0.0;
            double odl = odleglosc(tablica1);

            rezultat = Math.Exp(-(Math.Pow(odl, 2)) / (2 * Math.Pow(promien, 2)));

            return rezultat;
        }

        public double ucz(double wsp_uczenia, double[] tablica)
        {
            double s = sasiedztwo(tablica, wsp_uczenia);

            for (int i = 0; i < this.wagi.Length; i++)
            {
                double roznica = this.wagi[i] - tablica[i];
                this.wagi[i] += roznica * s * wsp_uczenia;
            }
            return odleglosc(tablica);
        }
    }
}
