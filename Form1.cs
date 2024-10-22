using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Xml;
using System.Text.RegularExpressions;
using System.Runtime.Remoting.Contexts;
using System.Drawing.Printing;
using System.Data.SqlClient;

namespace PROIECT_C_
{
    public partial class Form1 : Form
    {


        SqlConnection connection = new SqlConnection("Data Source=(localdb)\\ProjectModels;Initial Catalog=Proiectt;" +
            "Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;" +
            "ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        private Camere camere;
        
        bool dateIncarcate;

        const int margine = 10;
        Color culoareBars = Color.Blue;
        Font fontText = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold);
        Color culoareText = Color.Black;
        List<Camere> listCamere;
        List<Camere> listaCamereRezervate;
        List<Rezervari> listaRezerv;
        Hotel h;
        public Form1()
        {
            

            // Inițializarea obiectelor
            h = new Hotel();
            h.NumeFotel = "Hotel Atlantis";
            h.NrLocuriParcare = 30;
            h.NrFacilitati = 9;
            InitializeComponent();
            camere = new Camere(4, "king");


            tbNumarPaturi.DataBindings.Add("Text", camere, "NrPaturi", true, DataSourceUpdateMode.OnPropertyChanged);
            tbDenumire.DataBindings.Add("Text", camere, "DenumireCamera");

           
            btnModifica.Click += btnModifica_Click;

            listCamere = new List<Camere>();
            listaRezerv = new List<Rezervari>();
            listaCamereRezervate= new List<Camere>();
        }

       


        private void fisierTextToolStripMenuItem_Click(object sender, EventArgs e)
        {

            saveFileDialog1.Filter = "(*.txt)|*.txt";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {

                StreamWriter sw = new StreamWriter(saveFileDialog1.FileName);

                foreach (Camere c in listaCamereRezervate)
                {
                    sw.Write(c.ToString());
                }
                foreach (Rezervari r in listaRezerv)
                {
                    sw.Write(r.ToString());
                }
                sw.Close();
                MessageBox.Show($"S a reusit salvarea:{saveFileDialog1.FileName}");
            }
        }



        private bool IsValidHour(string hour)
        {
            // Pattern pentru a valida orele între 1 și 23
            string pattern = @"^(0?[1-9]|1[0-9]|2[0-3])$";
            return Regex.IsMatch(hour, pattern);
        }

        private void btnAdaugaCamera_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tbDenumire.Text))
                {
                    throw new Exception("Denumirea camerei nu este validă.");
                }

                int nrPaturi = int.Parse(tbNumarPaturi.Text);
                string denumire = tbDenumire.Text.Trim().ToLower();

                if (!(denumire.Equals("single") || denumire.Equals("double") || denumire.Equals("king")))
                {
                    throw new Exception("Denumirea camerei trebuie să fie 'single', 'double' sau 'king'.");
                }

                int oraCheckIn, oraCheckOut;
                // Verificare validitate ore de check-in și check-out
                if (!IsValidHour(tbOraIn.Text))
                {
                    throw new Exception("Ora de check-in nu este validă. Introduceți o oră între 1 și 23.");
                }
                else
                {
                    oraCheckIn = int.Parse(tbOraIn.Text);
                }

                if (!IsValidHour(tbOraOut.Text))
                {
                    throw new Exception("Ora de check-out nu este validă. Introduceți o oră între 1 și 23.");
                }
                else
                {
                    oraCheckOut = int.Parse(tbOraOut.Text);
                }

                int pretCamera = int.Parse(tbPret.Text);
                if (pretCamera < 150)
                {
                    throw new Exception("Prețul camerei trebuie să fie de cel puțin 150 de lei.");
                }
                int nrNoptii = int.Parse(tbNopti.Text);

                Rezervari r = new Rezervari(oraCheckIn, oraCheckOut, pretCamera, nrNoptii);
                int pretTotal;

                if (nrPaturi > 2)
                {
                    int taxe = 100;
                    pretTotal = r.PretTotal2(taxe);
                    MessageBox.Show("Pretul total este=" + r.PretTotal2(taxe) + " .A mai fost adaugata o taxa de 100 de lei pentru patul/paturile suplimentate");
                }
                else
                {
                    pretTotal = r.PretTotal();
                    MessageBox.Show("Pretul total este=" + r.PretTotal());
                }

                listaRezerv.Add(r);

                Camere c = new Camere(nrPaturi, denumire);
                listaCamereRezervate.Add(c);

                MessageBox.Show("Camera a fost rezervată");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Eroare", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
            finally
            {
                tbNumarPaturi.Clear();
                tbDenumire.Clear();
                tbOraIn.Clear();
                tbOraOut.Clear();
                tbPret.Clear();
                tbNopti.Clear();
                tbNumarPaturi.Focus();
            }
        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            lvRezeravri.Items.Clear();
            foreach (Rezervari r in listaRezerv)
            {
                ListViewItem lvi = new ListViewItem(r.OraCkeckIn.ToString());
                lvi.SubItems.Add(r.OraCkeckOut.ToString());
                lvi.SubItems.Add(r.PretCamera.ToString());
                lvi.SubItems.Add(r.NrNoptii.ToString());
                // lvi.SubItems.Add(r.Camera.ToString());
                lvRezeravri.Items.Add(lvi);
            }
        }

        




        private void button1_Click_1(object sender, EventArgs e)
        {
            ListViewItem lvi1 = new ListViewItem("2");

            lvi1.SubItems.Add("single");
            lvCameree.Items.Add(lvi1);

            ListViewItem lvi2 = new ListViewItem("2");

            lvi2.SubItems.Add("double");
            lvCameree.Items.Add(lvi2);

            ListViewItem lvi3 = new ListViewItem("2");

            lvi3.SubItems.Add("king");
            lvCameree.Items.Add(lvi3);
        }

        private void fisierBinarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileStream fs = new FileStream("hotel.dat", FileMode.Create, FileAccess.Write);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, listaCamereRezervate);
            bf.Serialize(fs, listaRezerv);
            fs.Close();
            MessageBox.Show("s a salvat fisierul hotel.dat!");
        }

        private void salvareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "(*.txt)|*.txt";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                tbCamera.Clear();
                StreamReader sr = new StreamReader(openFileDialog1.FileName);
                tbCamera.Text += sr.ReadToEnd();
                sr.Close();
            }
        }

        private void restaurareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileStream fs = new FileStream("hotel.dat", FileMode.Open, FileAccess.Read);
            BinaryFormatter bf = new BinaryFormatter();
            List<Camere> listaCameraDeserializati = (List<Camere>)bf.Deserialize(fs);
            List<Rezervari> listaRezervariDeserializati = (List<Rezervari>)bf.Deserialize(fs);
            tbCamera.Clear();
            foreach (Camere c in listaCameraDeserializati)
            {
                tbCamera.Text += c.ToString() + Environment.NewLine;
            }
            foreach (Rezervari r in listaRezervariDeserializati)
            {
                tbCamera.Text += r.ToString() + Environment.NewLine;
            }
            fs.Close();
        }

        private void btnAnuleaza_Click(object sender, EventArgs e)
        {
            if (listaRezerv.Count > 0)
            {
                Rezervari ultimaRezervare = listaRezerv[listaRezerv.Count - 1];
                listaRezerv.Remove(ultimaRezervare);
                MessageBox.Show("Ultima rezervare a fost anulată cu succes!");
            }
            else
            {
                MessageBox.Show("Nu există nicio rezervare de anulat.");
            }
        }

        private void afisareRezervareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvRezeravri.Items.Clear();
            foreach (Rezervari r in listaRezerv)
            {
                ListViewItem lvi = new ListViewItem(r.OraCkeckIn.ToString());
                lvi.SubItems.Add(r.OraCkeckOut.ToString());
                lvi.SubItems.Add(r.PretCamera.ToString());
                lvi.SubItems.Add(r.NrNoptii.ToString());
                // lvi.SubItems.Add(r.Camera.ToString());
                lvRezeravri.Items.Add(lvi);
            }
        }

        private void anulareRezervareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listaRezerv.Count > 0)
            {
                Rezervari ultimaRezervare = listaRezerv[listaRezerv.Count - 1];
                listaRezerv.Remove(ultimaRezervare);
                MessageBox.Show("Ultima rezervare a fost anulată cu succes!");
            }
            else
            {
                MessageBox.Show("Nu există nicio rezervare de anulat.");
            }
        }

        private void afisareListaCuCamereToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem lvi1 = new ListViewItem("2");

            lvi1.SubItems.Add("single");
            lvCameree.Items.Add(lvi1);

            ListViewItem lvi2 = new ListViewItem("2");

            lvi2.SubItems.Add("double");
            lvCameree.Items.Add(lvi2);

            ListViewItem lvi3 = new ListViewItem("2");

            lvi3.SubItems.Add("king");
            lvCameree.Items.Add(lvi3);
        }

       

        private void camereRezervateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tbCamera.Clear();
            foreach (Camere c in listCamere)
            {
                tbCamera.Text += c.ToString() + Environment.NewLine;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(@"ListaCamere.xml");

            XmlNodeList roomList = xmlDoc.SelectNodes("/catalog/room");

            foreach (XmlNode roomNode in roomList)
            {
   
                string type = roomNode.SelectSingleNode("type").InnerText;
                int beds = int.Parse(roomNode.SelectSingleNode("beds").InnerText);
                Camere camera = new Camere( beds,type);
                listCamere.Add(camera);


                lbCamere.Items.Add($" Tip: {type}, Paturi: {beds}");
            }
        }

        

        private void incarcaDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            StreamReader sr = new StreamReader("Camere.txt");
            string linie = null;
            listCamere.Clear(); 
            while ((linie = sr.ReadLine()) != null)
            {
                try
                {
                    string denumire = linie.Split(',')[0];
                    int nrPaturi = Convert.ToInt32(linie.Split(',')[1]);

                    Camere c = new Camere(nrPaturi,denumire);
                    listCamere.Add(c);
                    dateIncarcate = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            sr.Close();
            MessageBox.Show("S-au incarcat datele de rezervare!");
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            chart1.Visible = false;
            if (dateIncarcate)
            {
                Graphics g = e.Graphics;
                Rectangle rectangle = new Rectangle(panel1.ClientRectangle.X + margine, panel1.ClientRectangle.Y + 4 * margine,
                    panel1.ClientRectangle.Width - 2 * margine, panel1.ClientRectangle.Height - 5 * margine);
                Pen pen = new Pen(Color.Red, 3);
                g.DrawRectangle(pen, rectangle);

                double latime = rectangle.Width / listCamere.Count / 3;
                double distanta = (rectangle.Width - listCamere.Count * latime) / (listCamere.Count + 1);
                double vMax = listCamere.Max(max => max.NrPaturi);
                Brush brBars = new SolidBrush(culoareBars);
                Brush brFont = new SolidBrush(culoareText);

                Rectangle[] rectangles = new Rectangle[listCamere.Count];
                for (int i = 0; i < rectangles.Length; i++)
                {
                    rectangles[i] = new Rectangle((int)(rectangle.Location.X + (i + 1) * distanta + i * latime),
                        (int)(rectangle.Location.Y + rectangle.Height - listCamere[i].NrPaturi / vMax * rectangle.Height),
                        (int)latime, (int)(listCamere[i].NrPaturi / vMax * rectangle.Height));
                    g.DrawString(listCamere[i].DenumireCamera, fontText, brFont, new Point((int)(rectangles[i].Location.X),
                        (int)(rectangles[i].Location.Y - fontText.Height)));
                }
                g.FillRectangles(brBars, rectangles);
                for (int i = 0; i < listCamere.Count - 1; i++)
                {
                    g.DrawLine(pen, new Point((int)(rectangles[i].Location.X + latime / 2),
                        (int)(rectangles[i].Location.Y)), new Point((int)(rectangles[i + 1].Location.X + latime / 2),
                        (int)(rectangles[i + 1].Location.Y)));
                }

            }
        }


        //private void salvare_bmp(Control c, string nume_fisier)
        //{
        //    Bitmap img = new Bitmap(c.Width, c.Height);
        //    c.DrawToBitmap(img, new Rectangle(c.ClientRectangle.X, c.ClientRectangle.Y, c.ClientRectangle.Width,
        //        c.ClientRectangle.Height));
        //    img.Save(nume_fisier);
        //    img.Dispose();
        //}

        //private void salvareToolStripMenuItem1_Click(object sender, EventArgs e)
        //{
        //    salvare_bmp(panel1, "Grafic_" + DateTime.Now.ToString("dd-MM-yyyy") + ".bmp");
        //    MessageBox.Show("S-a salvat imaginea!");
        //}

        private void pp(object sender, PrintPageEventArgs e)
        {
            if (dateIncarcate)
            {
                Graphics g = e.Graphics;
                Rectangle rectangle = new Rectangle(e.PageBounds.X + margine, e.PageBounds.Y + 4 * margine,
                   e.PageBounds.Width - 2 * margine, e.PageBounds.Height - 5 * margine);
                Pen pen = new Pen(Color.Red, 3);
                g.DrawRectangle(pen, rectangle);

                double latime = rectangle.Width / listCamere.Count / 3;
                double distanta = (rectangle.Width - listCamere.Count * latime) / (listCamere.Count + 1);
                double vMax = listCamere.Max(max => max.NrPaturi);
                Brush brBars = new SolidBrush(culoareBars);
                Brush brFont = new SolidBrush(culoareText);

                Rectangle[] rectangles = new Rectangle[listCamere.Count];
                for (int i = 0; i < rectangles.Length; i++)
                {
                    rectangles[i] = new Rectangle((int)(rectangle.Location.X + (i + 1) * distanta + i * latime),
                        (int)(rectangle.Location.Y + rectangle.Height - listCamere[i].NrPaturi / vMax * rectangle.Height),
                        (int)latime, (int)(listCamere[i].NrPaturi / vMax * rectangle.Height));
                    g.DrawString(listCamere[i].DenumireCamera, fontText, brFont, new Point((int)(rectangles[i].Location.X),
                        (int)(rectangles[i].Location.Y - fontText.Height)));
                }
                g.FillRectangles(brBars, rectangles);
                for (int i = 0; i < listCamere.Count - 1; i++)
                {
                    g.DrawLine(pen, new Point((int)(rectangles[i].Location.X + latime / 2),
                        (int)(rectangles[i].Location.Y)), new Point((int)(rectangles[i + 1].Location.X + latime / 2),
                        (int)(rectangles[i + 1].Location.Y)));
                }

            }
        }

        private void printeazaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(pp);
            PrintPreviewDialog pdlg = new PrintPreviewDialog
            {
                Document = pd
            };
            pdlg.ShowDialog();
        }

        private void graficDesenatBarsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dateIncarcate)
            {
                chart1.Visible = false;
                panel1.Invalidate();
            }
            else
            {
                MessageBox.Show("Datele nu au fost incarcate");
            }
        }

        private void graficBarsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dateIncarcate == true)
            {
                chart1.Series["Camere"].Points.Clear();
                chart1.Titles.Clear();
                chart1.Visible = true;
                chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                for (int i = 0; i < listCamere.Count; i++)
                {
                    chart1.Series["Camere"].Points.AddXY(listCamere[i].DenumireCamera, listCamere[i].NrPaturi);
                }
                chart1.Titles.Add("Camere hotel");
            }
            else
            {
                MessageBox.Show("Datele nu au fost incarcate");
            }
        }

        private void graficPieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dateIncarcate == true)
            {
                chart1.Series["Camere"].Points.Clear();
                chart1.Titles.Clear();
                chart1.Visible = true;
                chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
                for (int i = 0; i < listCamere.Count; i++)
                {
                    chart1.Series["Camere"].Points.AddXY(listCamere[i].DenumireCamera, listCamere[i].NrPaturi);
                }
                chart1.Titles.Add("Camere hotel");
            }
            else
            {
                MessageBox.Show("Datele nu au fost incarcate");
            }
        }

        private void barsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                culoareBars = cd.Color;
            }
            panel1.Invalidate();
        }

        private void textToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                culoareText = cd.Color;
            }
            panel1.Invalidate();
        }

        private void modficareFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                fontText = fd.Font;
            }
            panel1.Invalidate();
        }


        private void InitializeComponentt()
        {
            
            tbNumarPaturi = new TextBox();
            tbDenumire = new TextBox();
            btnModifica = new Button();
            this.Controls.Add(tbNumarPaturi);
            this.Controls.Add(tbDenumire);
            this.Controls.Add(btnModifica);
        }


        private void btnModifica_Click(object sender, EventArgs e)
        {
            camere.DenumireCamera = tbDenumire.Text;
            if (int.TryParse(tbNumarPaturi.Text, out int nr))
            {
                camere.NrPaturi = nr;
            }
            
            tbDenumire.DataBindings["Text"].ReadValue();
            tbNumarPaturi.DataBindings["Text"].ReadValue();
        }

        private void afiseazaCameraRezervataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tbCamera.Clear();
            foreach (Camere c in listaCamereRezervate)
            {
                tbCamera.Text += c.ToString() + Environment.NewLine;
            }
        }

        

        private void informatiiHotelDinBDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            connection.Open();
            //SqlCommand comand = new SqlCommand("INSERT INTO Hotel (NumeHotel,NrLocuriDeParcare,NrFacilitati)" +
            //    "VALUES('Hotel Atlantis',30,9)", connection);
            //comand.ExecuteNonQuery();
           // MessageBox.Show("Camera a fost inserata!");
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM HOTEL", connection);
            adapter.Fill(dt);
            dgvHotel.DataSource = dt;
            connection.Close();
        }

        private void afiseazaListaCuCamereBDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            connection.Open();
            SqlCommand comand = new SqlCommand("INSERT INTO Camere (NumarPaturi,DenumireCamera)" +
                "VALUES(@nrPaturi,@denumireCamera)", connection);
            comand.Parameters.AddWithValue("@nrPaturi",tbNumarPaturi.Text);
            comand.Parameters.AddWithValue("@denumireCamera",tbDenumire.Text);
            comand.ExecuteNonQuery();
            connection.Close();
            MessageBox.Show("Camera a fost inserata!");
            //this.Close();
        }

        private void afiseazaListaCuCamereBDToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            connection.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Camere", connection);
            adapter.Fill(dt);
            dgvHotel.DataSource = dt;
            connection.Close();
        }

        private void afiseazaRezervareBDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            connection.Open();
            SqlCommand comand = new SqlCommand("INSERT INTO Rezervari (oraCheckin,oraCheckout,PretCamera,NumarNopti)" +
                "VALUES(@oraCheckin,@oraCheckout,@pretCamera,@numarNopti)", connection);
            comand.Parameters.AddWithValue("@oraCheckin", tbOraIn.Text);
            comand.Parameters.AddWithValue("@oraCheckout",tbOraOut.Text);
            comand.Parameters.AddWithValue("@pretCamera", tbPret.Text);
            comand.Parameters.AddWithValue("@numarNopti", tbNopti.Text);
            comand.ExecuteNonQuery();
            connection.Close();
            MessageBox.Show("Rezervarea  a fost inserata!");
           // this.Close();
        }

        private void afiseazaRezervareBDToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            connection.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Rezervari", connection);
            adapter.Fill(dt);
            dgvHotel.DataSource = dt;
            connection.Close();
        }


        private void informatiiHotelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tvHotel.Nodes.Clear();


            TreeNode hotelNode = new TreeNode($"Numele Hotelului: {h.NumeFotel}");


            TreeNode camereNodeList = new TreeNode("Lista Camere:");
            foreach (Camere c in listaCamereRezervate)
            {

                TreeNode cameraNode = new TreeNode($"- Nr. Paturi: {c.NrPaturi}");
                TreeNode denumireCameraNode = new TreeNode($"- Denumire Camera: {c.DenumireCamera}");


                cameraNode.Nodes.Add(cameraNode);
                cameraNode.Nodes.Add(denumireCameraNode);


                camereNodeList.Nodes.Add(cameraNode);
            }

            hotelNode.Nodes.Add(camereNodeList);


            TreeNode rezervariNode = new TreeNode("Lista Rezervări:");
            foreach (Rezervari r in listaRezerv)
            {

                TreeNode rezervareNode = new TreeNode("- Rezervare");


                TreeNode oraCheckInNode = new TreeNode($"- Ora Check-In: {r.OraCkeckIn}");
                TreeNode oraCheckOutNode = new TreeNode($"- Ora Check-Out: {r.OraCkeckOut}");
                TreeNode pretCameraNode = new TreeNode($"- Preț Cameră: {r.PretCamera}");
                TreeNode nrNoptiNode = new TreeNode($"- Număr Nopți: {r.NrNoptii}");

                rezervareNode.Nodes.Add(oraCheckInNode);
                rezervareNode.Nodes.Add(oraCheckOutNode);
                rezervareNode.Nodes.Add(pretCameraNode);
                rezervareNode.Nodes.Add(nrNoptiNode);


                rezervariNode.Nodes.Add(rezervareNode);
            }

            hotelNode.Nodes.Add(rezervariNode);


            TreeNode locuriParcareNode = new TreeNode($"Număr Locuri de Parcare: {h.NrLocuriParcare}");
            TreeNode facilitatiNode = new TreeNode($"Număr Facilități: {h.NrFacilitati}");


            tvHotel.Nodes.Add(hotelNode);
            tvHotel.Nodes.Add(locuriParcareNode);
            tvHotel.Nodes.Add(facilitatiNode);


            tvHotel.ExpandAll();

        }
    }
    
}
