using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    class Statystyka
    {
        public double srednia(double[] tablica)
        {
            double suma=0;
            double wynik = 0;
            foreach (int i in tablica)
            {
                suma += tablica[i];
            }

            wynik = suma / tablica.Length;
            return wynik;
        }

        public double mediana(double[] tablica)
        {
            Array.Sort(tablica);
            if (tablica.Length % 2 == 1)
            {
                return tablica[(tablica.Length + 1) / 2];
            }
            else
            {
                return tablica[tablica.Length / 2];
            }
        }

        public double odchylenie(double[] tablica)
        {
            double sr = srednia(tablica);
            double suma = 0;
            foreach (int i in tablica)
            {
                suma = Math.Pow((tablica[i] - sr), 2);
            }
            double wynik = suma / (tablica.Length - 1);
            return Math.Sqrt(wynik);
        }

        public double skosnosc(double[] tablica, int n)
        {
            double sk;
            return sk = 3 * (srednia(tablica) - mediana(tablica)) / odchylenie(tablica);
        }
    }
}
