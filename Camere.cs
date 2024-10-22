using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROIECT_C_
{
    [Serializable]
    internal class Camere:IComparable<Camere>,ICloneable
    {
        int nrPaturi;
       // int idCamera;
        string denumireCamera;

        public Camere(int nrPaturi,  string denumireCamera)
        {
            this.nrPaturi = nrPaturi;
           
            this.denumireCamera = denumireCamera;
        }


        public int NrPaturi { get => nrPaturi; set => nrPaturi = value; }
       
        public string DenumireCamera { get => denumireCamera; set => denumireCamera = value; }

        public object Clone()
        {
            return new Camere(this.nrPaturi,  this.denumireCamera);
        }
   
        public int CompareTo(Camere other)
        {
            if (this.nrPaturi == other.nrPaturi)
            {
                return this.denumireCamera.CompareTo(other.denumireCamera);
            }
            else
            {
                return other.nrPaturi.CompareTo(this.nrPaturi);
            }
        }

        public static Camere operator +(Camere c1, Camere c2)
        {
            return new Camere(c1.nrPaturi + c2.nrPaturi, c1.denumireCamera + " " + c2.denumireCamera);
        }

        public static Camere operator -(Camere c1, Camere c2)
        {
            return new Camere(c1.nrPaturi - c2.nrPaturi,  c1.denumireCamera + " - " + c2.denumireCamera);
        }

        public static Camere operator ++(Camere c)
        {
            c.nrPaturi++;
            return c;
        }

        public void ActualizareInformatii(int nrPaturiNou, string denumireNoua)
        {
            nrPaturi = nrPaturiNou;
            denumireCamera = denumireNoua;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\nNumarul de paturi: ");
            sb.Append(nrPaturi);

            sb.Append("\n Denumirea camerei:");
            sb.Append(denumireCamera);
            return sb.ToString();
        }

    }
}
