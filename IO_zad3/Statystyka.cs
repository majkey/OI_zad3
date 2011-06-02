using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    class Statystyka
    {
        public static double srednia(double[] tablica)
        {
            double suma=0;
            double wynik = 0;
            for (int i = 0; i < tablica.Length; i++)
            {
                suma += tablica[i];
            }

            wynik = suma / tablica.Length;
            return wynik;
        }

        public static double mediana(double[] tablica)
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

        public static double odchylenie(double[] tablica)
        {
            double sr = srednia(tablica);
            double suma = 0;
            for (int i = 0; i < tablica.Length; i++)
            {
                suma = Math.Pow((tablica[i] - sr), 2);
            }
            double wynik = suma / (tablica.Length - 1);
            return Math.Sqrt(wynik);
        }

        public static double skosnosc(double[] tablica)
        {
            double sk;
            return sk = 3 * (srednia(tablica) - mediana(tablica)) / odchylenie(tablica);
        }
    }
}
