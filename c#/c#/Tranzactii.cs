using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1054___Cotoi___Corina_Daniela_
{
    internal class Tranzactii : ICloneable, IComparable<Tranzactii>
    {
        private string tipTranzactie; // Tipul tranzacției: "cumpărare" sau "vânzare"
        private string numeFirma; // Numele firmei pentru care s-a efectuat tranzacția
        private int cantitate; // Cantitatea tranzacționată
        private float pret; // Prețul tranzacției
        private DateTime data; // Data tranzacției


        // Constructori
        public Tranzactii()
        {
            tipTranzactie = string.Empty;
            numeFirma = string.Empty;
            cantitate = 0;
            pret = 0.0f;
            data = DateTime.MinValue;
        }

        public Tranzactii(string tipTranzactie, string numeFirma, int cantitate, float pret, DateTime data)
        {
            this.tipTranzactie = tipTranzactie;
            this.numeFirma = numeFirma;
            this.cantitate = cantitate;
            this.pret = pret;
            this.data = data;
        }

        // Proprietăți
        public string TipTranzactie
        {
            get { return tipTranzactie; }
            set { tipTranzactie = value; }
        }

        public string NumeFirma
        {
            get { return numeFirma; }
            set { numeFirma = value; }
        }

        public int Cantitate
        {
            get { return cantitate; }
            set { cantitate = value; }
        }

        public float Pret
        {
            get { return pret; }
            set { pret = value; }
        }

        public DateTime Data
        {
            get { return data; }
            set { data = value; }
        }

        // Supraîncărcare operator+ --> aduna două obiecte Tranzactii daca au aceeasi nume si tip de tranzactie
        public static Tranzactii operator +(Tranzactii t1, Tranzactii t2)
        {
            if (string.Equals(t1.numeFirma, t2.numeFirma, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(t1.tipTranzactie, t2.tipTranzactie, StringComparison.OrdinalIgnoreCase))
            {
                int cantitateNoua = t1.cantitate + t2.cantitate;
                float pretNou = (t1.pret * t1.cantitate + t2.pret * t2.cantitate) / cantitateNoua;
                return new Tranzactii(t1.tipTranzactie, t1.numeFirma, cantitateNoua, pretNou, t1.data);
            }
            else
            {
                throw new InvalidOperationException("Tranzacțiile nu pot fi adunate deoarece au firme sau tipuri de tranzacții diferite.");
            }
        }


        //Supraîncărcare operator ++
        public static Tranzactii operator ++(Tranzactii t)
        {
            t.cantitate++;
            return t;
        }

        // Metoda care calculează valoarea totală a tranzacțiilor de un anumit tip
        public static float CalculeazaValoareTotala(List<Tranzactii> listaTranzactii, string tipTranzactie)
        {
            float valoareTotala = 0;

            foreach (Tranzactii tranzactie in listaTranzactii)
            {
                if (tranzactie.TipTranzactie == tipTranzactie)
                {
                    valoareTotala += tranzactie.Pret * tranzactie.Cantitate;
                }
            }

            return valoareTotala;
        }

        public object Clone()
        {
            Tranzactii clona = (Tranzactii)this.MemberwiseClone();

            clona.tipTranzactie = string.Copy(this.tipTranzactie);
            clona.numeFirma = string.Copy(this.numeFirma);
            clona.cantitate = this.cantitate;
            clona.pret = this.pret;
            clona.data = this.data;

            return clona;
        }

        public int CompareTo(Tranzactii other)
        {
            int compareTip = string.Compare(this.tipTranzactie, other.tipTranzactie, StringComparison.OrdinalIgnoreCase);
            if (compareTip != 0)
                return compareTip;

            int compareFirma = string.Compare(this.numeFirma, other.numeFirma, StringComparison.OrdinalIgnoreCase);
            if (compareFirma != 0)
                return compareFirma;

            int compareCantitate = this.cantitate.CompareTo(other.cantitate);
            if (compareCantitate != 0)
                return compareCantitate;

            int comparePret = this.pret.CompareTo(other.pret);
            if (comparePret != 0)
                return comparePret;

            return this.data.CompareTo(other.data);
        }

        public override string ToString()
        {
            string tipTranzactieAfisat;
            if (string.Equals(TipTranzactie, "cumpărare", StringComparison.OrdinalIgnoreCase))
            {
                tipTranzactieAfisat = "Cumpărare";
            }
            else if (string.Equals(TipTranzactie, "vânzare", StringComparison.OrdinalIgnoreCase))
            {
                tipTranzactieAfisat = "Vânzare";
            }
            else
            {
                tipTranzactieAfisat = TipTranzactie;
            }

            return $"Tip tranzacție: {tipTranzactieAfisat}, Firma: {NumeFirma}, Cantitate: {Cantitate}%, Preț: {Pret}, Data: {Data:d}";
        }

    }
}
