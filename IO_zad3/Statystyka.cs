using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    class Statystyka
    {
        public double srednia(double[] tablica, int n)
        {
            double suma=0;
            double wynik = 0;
            foreach (int i in tablica)
            {
                suma += tablica[i];
            }

            wynik = suma / n;
            return wynik;
        }

        public double mediana(double[] tablica, int n)
        {
            Array.Sort(tablica);
            if (n % 2 == 1)
            {
                return tablica[(n + 1) / 2];
            }
            else
            {
                return tablica[n / 2];
            }
        }

        public double odchylenie(double[] tablica, int n)
        {
            double sr = srednia(tablica, n);
            double suma = 0;
            foreach (int i in tablica)
            {
                suma = Math.Pow((tablica[i] - sr), 2);
            }
            double wynik = suma / (n - 1);
            return Math.Sqrt(wynik);
        }

        public double skosnosc(double[] tablica, int n)
        {
            double sk;
            return sk = 3 * (srednia(tablica, n) - mediana(tablica, n)) / odchylenie(tablica, n);
        }
    }
}
