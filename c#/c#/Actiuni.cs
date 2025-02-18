using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1054___Cotoi___Corina_Daniela_
{
    internal class Actiuni : ICloneable, IComparable<Actiuni>, ICalculValoare
    {
        private string numeFirmaDetinatoare; // detinatoare
        private int nrFirme; // număr firme la care deține acțiuni
        private string[] listaFirme; // lista firmelor la care deține acțiuni
        private float[] pretCumparareActiuni;

        // Constructor implicit
        public Actiuni()
        {
            numeFirmaDetinatoare = string.Empty;
            nrFirme = 0;
            listaFirme = new string[0];
            pretCumparareActiuni = new float[0];
        }

        // Constructor cu parametri
        public Actiuni(string numeFirmaDetinatoare, int nrFirme, string[] listaFirme, float[] pretCumparareActiuni)
        {
            this.numeFirmaDetinatoare = numeFirmaDetinatoare;
            this.nrFirme = nrFirme;
            this.listaFirme = new string[nrFirme];
            this.pretCumparareActiuni = new float[nrFirme];
            for (int i = 0; i < nrFirme; i++)
            {
                this.listaFirme[i] = listaFirme[i];
                this.pretCumparareActiuni[i] = pretCumparareActiuni[i];
            }
        }

        // Proprietăți
        public string NumeFirmaDetinatoare
        {
            get => numeFirmaDetinatoare;
            set => numeFirmaDetinatoare = value;
        }

        public int NrFirme
        {
            get => nrFirme;
            set => nrFirme = value;
        }

        public string[] ListaFirme
        {
            get => listaFirme;
            set => listaFirme = value;
        }

        public float[] PretCumparareActiuni
        {
            get => pretCumparareActiuni;
            set => pretCumparareActiuni = value;
        }

        // Suprasarcină operator < pentru a compara prețurile de cumpărare între două firme la același index
        public static bool operator <(Actiuni a1, Actiuni a2)
        {
            if (a1.nrFirme > 0 && a2.nrFirme > 0)
            {
                for (int i = 0; i < Math.Min(a1.nrFirme, a2.nrFirme); i++)
                {
                    if (a1.pretCumparareActiuni[i] >= a2.pretCumparareActiuni[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                throw new InvalidOperationException("Compararea nu este posibilă deoarece unul dintre obiecte nu conține acțiuni.");
            }
        }

        // Suprasarcină operator > pentru a compara prețurile de cumpărare între două firme la același index
        public static bool operator >(Actiuni a1, Actiuni a2)
        {
            if (a1.nrFirme > 0 && a2.nrFirme > 0)
            {
                for (int i = 0; i < Math.Min(a1.nrFirme, a2.nrFirme); i++)
                {
                    if (a1.pretCumparareActiuni[i] <= a2.pretCumparareActiuni[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                throw new InvalidOperationException("Compararea nu este posibilă deoarece unul dintre obiecte nu conține acțiuni.");
            }
        }
        public static Actiuni operator +(Actiuni a, float nouPret)
        {
            float[] pretCumparareNou = new float[a.nrFirme + 1];

            for (int i = 0; i < a.nrFirme; i++)
            {
                pretCumparareNou[i] = a.pretCumparareActiuni[i];
            }

            pretCumparareNou[a.nrFirme] = nouPret;

            Actiuni rezultat = new Actiuni(a.numeFirmaDetinatoare, a.nrFirme + 1, a.listaFirme, pretCumparareNou);
            return rezultat;
        }

        public object Clone()
        {
            return new Actiuni(
                this.numeFirmaDetinatoare,
                this.nrFirme,
                (string[])this.listaFirme.Clone(),
                (float[])this.pretCumparareActiuni.Clone()
            );
        }

        public int CompareTo(Actiuni other)
        {
            if (other == null)
                return 1;

            int compareNumeFirma = string.Compare(this.numeFirmaDetinatoare, other.numeFirmaDetinatoare, StringComparison.OrdinalIgnoreCase);
            if (compareNumeFirma != 0)
                return compareNumeFirma;

            int compareNrFirme = this.nrFirme.CompareTo(other.nrFirme);
            if (compareNrFirme != 0)
                return compareNrFirme;

            for (int i = 0; i < this.nrFirme; i++)
            {
                int compareFirma = string.Compare(this.listaFirme[i], other.listaFirme[i], StringComparison.OrdinalIgnoreCase);
                if (compareFirma != 0)
                    return compareFirma;

                int comparePret = this.pretCumparareActiuni[i].CompareTo(other.pretCumparareActiuni[i]);
                if (comparePret != 0)
                    return comparePret;
            }

            return 0;
        }

        public float CalculeazaValoareTotala()
        {
            float valoare = 0;
            for (int i = 0; i < nrFirme; i++)
            {
                valoare += pretCumparareActiuni[i];
            }
            return valoare;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Firma deținătoare: {NumeFirmaDetinatoare} ");
            sb.AppendLine($"Număr de firme: {NrFirme} ");
            sb.AppendLine("Lista firmelor și prețurile de cumpărare: ");

            for (int i = 0; i < NrFirme; i++)
            {
                sb.AppendLine($"- {ListaFirme[i]} : {PretCumparareActiuni[i]} ");
            }

            return sb.ToString();
        }

    }


}
