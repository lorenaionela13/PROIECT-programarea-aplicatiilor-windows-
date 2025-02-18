using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1054___Cotoi___Corina_Daniela_
{
    internal class Portofolii
    {
        private string numePortofoliu; // numele portofoliului
        private List<Actiuni> listaActiuni; // lista de acțiuni din portofoliu
        private List<Tranzactii> listaTranzactii; // lista de tranzacții din portofoliu


        // Constructor implicit
        public Portofolii()
        {
            numePortofoliu = string.Empty;
            listaActiuni = new List<Actiuni>();
            listaTranzactii = new List<Tranzactii>();
        }

        // Constructor cu parametri
        public Portofolii(string numePortofoliu, List<Actiuni> listaActiuni, List<Tranzactii> listaTranzactii)
        {
            this.numePortofoliu = numePortofoliu;
            this.listaActiuni = new List<Actiuni>(listaActiuni);
            this.listaTranzactii = new List<Tranzactii>(listaTranzactii);
        }

        // Proprietăți
        public string NumePortofoliu
        {
            get { return numePortofoliu; }
            set { numePortofoliu = value; }
        }

        public List<Actiuni> ListaActiuni
        {
            get { return listaActiuni; }
            set { listaActiuni = value; }
        }

        public List<Tranzactii> ListaTranzactii
        {
            get { return listaTranzactii; }
            set { listaTranzactii = value; }
        }

        // Supraîncărcare operator ++
        public static Portofolii operator ++(Portofolii p)
        {
            p.listaActiuni.Add(new Actiuni()); // Adaugă o nouă acțiune goală
            return p;
        }

        // Indexer pentru listaActiuni
        public Actiuni this[int index]
        {
            get
            {
                if (index >= 0 && index < listaActiuni.Count)
                {
                    return listaActiuni[index];
                }
                else
                {
                    throw new IndexOutOfRangeException("Index invalid pentru listaActiuni.");
                }
            }
            set
            {
                if (index >= 0 && index < listaActiuni.Count)
                {
                    listaActiuni[index] = value;
                }
                else
                {
                    throw new IndexOutOfRangeException("Index invalid pentru listaActiuni.");
                }
            }
        }

        // Metoda 1: Calculează valoarea totală a acțiunilor din portofoliu
        public float CalculeazaValoareTotalaActiuni()
        {
            float valoareTotala = 0;

            foreach (Actiuni actiune in listaActiuni)
            {
                for (int i = 0; i < actiune.NrFirme; i++)
                {
                    valoareTotala += actiune.PretCumparareActiuni[i];
                }
            }

            return valoareTotala;
        }

        // Metoda 2: Calculează profitul/pierderea totală din tranzacții
        public float CalculeazaProfitPierdere()
        {
            float profitPierdere = 0;

            foreach (Tranzactii tranzactie in listaTranzactii)
            {
                if (tranzactie.TipTranzactie == "cumpărare")
                {
                    profitPierdere -= tranzactie.Pret * tranzactie.Cantitate;
                }
                else
                {
                    profitPierdere += tranzactie.Pret * tranzactie.Cantitate;
                }
            }

            return profitPierdere;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Nume portofoliu: {NumePortofoliu}");
            sb.AppendLine("Acțiuni din portofoliu:");

            foreach (Actiuni actiune in ListaActiuni)
            {
                sb.AppendLine(actiune.ToString());
                sb.AppendLine();
            }

            sb.AppendLine("Tranzacții din portofoliu:");

            foreach (Tranzactii tranzactie in ListaTranzactii)
            {
                sb.AppendLine(tranzactie.ToString());
            }

            return sb.ToString();
        }
    }


}
