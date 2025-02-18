using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;
using System.Xml.Linq;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace _1054___Cotoi___Corina_Daniela_
{
    public partial class Form1 : Form

    {

        SqlConnection connection = new SqlConnection("Data Source=(localdb)\\ProjectModels;" +
            "Initial Catalog=Proiect-daniela;Integrated Security=True;" +
            "Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;" +
            "ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        private List<Tranzactii> listaTranzactii = new List<Tranzactii>();
        private List<Actiuni> listaActiuni = new List<Actiuni>();
        private Portofolii portofoliu; // Instanță a clasei Portofolii

        const int margine = 10;
        Color culoareBars = Color.Blue;
        Font fontText = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold);
        Color culoareText = Color.Black;

        private float CalculeazaValoareTotala()
        {
            float valoareTotala = 0;
            foreach (Actiuni actiune in listaActiuni)
            {
                valoareTotala += actiune.CalculeazaValoareTotala();
            }
            return valoareTotala;
        }

        public Form1()
        {
            InitializeComponent();
            portofoliu = new Portofolii("Portofoliu Default", listaActiuni, listaTranzactii);

            //Adăugați câteva obiecte Actiuni în listaActiuni
            listaActiuni.Add(new Actiuni("Firma1", 2, new[] { "Daniela", "Ruxandra" }, new[] { 10000.5f, 15000.2f }));
            listaActiuni.Add(new Actiuni("Firma2", 3, new[] { "Catalin", "Daniela", "Denisa" }, new[] { 8000.7f, 12000.1f, 200000.3f }));

            DataTable dt = new DataTable();
            dt.Columns.Add("NumeFirmaDetinatoare");
            dt.Columns.Add("NrFirme");
            dt.Columns.Add("ListaFirme");
            dt.Columns.Add("PretCumparareActiuni");

            // Foreach(Actiuni actiune in listaActiuni)
            // {
            //     dt.Rows.Add(actiune.NumeFirmaDetinatoare, actiune.NrFirme, string.Join(",", actiune.ListaFirme), string.Join(",", actiune.PretCumparareActiuni));
            // }

            // dgvActiuni.DataSource = dt;

            dvgActiunii.DataSource = listaActiuni;

        }



        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder tranzactiiText = new StringBuilder();
            foreach (Tranzactii tranzactie in listaTranzactii)
            {
                tranzactiiText.AppendLine(tranzactie.ToString());
            }

            tbFormular.Text = tranzactiiText.ToString();


        }

        private void btnAfisareInTv_Click(object sender, EventArgs e)
        {
            tvPortofoliu.Nodes.Clear();
            portofoliu.NumePortofoliu = tbNumePortofoliu.Text;

            TreeNode portfolioNode = new TreeNode(portofoliu.NumePortofoliu);
            tvPortofoliu.Nodes.Add(portfolioNode);

            TreeNode actiuniNode = new TreeNode("Lista Actiuni");
            portfolioNode.Nodes.Add(actiuniNode);
            foreach (Actiuni actiune in listaActiuni)
            {
                TreeNode actiuneNode = new TreeNode(actiune.ToString());
                actiuniNode.Nodes.Add(actiuneNode);
            }

            // Adăugare nod pentru valoarea totală a acțiunilor
            TreeNode valoareTotalaNode = new TreeNode($"Valoarea totală: {CalculeazaValoareTotala()}"); // Folosește metoda pentru a calcula valoarea totală
            actiuniNode.Nodes.Add(valoareTotalaNode);

            TreeNode tranzactiiNode = new TreeNode("Lista Tranzactii");
            portfolioNode.Nodes.Add(tranzactiiNode);
            foreach (Tranzactii tranzactie in listaTranzactii)
            {
                TreeNode tranzactieNode = new TreeNode(tranzactie.ToString());
                tranzactiiNode.Nodes.Add(tranzactieNode);
            }

            tvPortofoliu.ExpandAll();
        }

        private void salveazaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                    {
                        foreach (TreeNode node in tvPortofoliu.Nodes)
                        {
                            SaveNode(node, writer, 0);
                        }
                    }
                }
            }
        }

        private void SaveNode(TreeNode node, StreamWriter writer, int level)
        {
            string indent = new string('\t', level);
            writer.WriteLine($"{indent}{node.Text}");
            foreach (TreeNode childNode in node.Nodes)
            {
                SaveNode(childNode, writer, level + 1);
            }
        }

        private void restaureazaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (StreamReader reader = new StreamReader(openFileDialog.FileName))
                    {
                        tvPortofoliu.Nodes.Clear();
                        TreeNode currentNode = null;
                        Dictionary<int, TreeNode> levelNodes = new Dictionary<int, TreeNode>();

                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            int level = line.TakeWhile(char.IsWhiteSpace).Count();
                            string nodeText = line.Trim();

                            TreeNode newNode = new TreeNode(nodeText);
                            if (level == 0)
                            {
                                tvPortofoliu.Nodes.Add(newNode);
                            }
                            else
                            {
                                levelNodes[level - 1].Nodes.Add(newNode);
                            }

                            levelNodes[level] = newNode;
                        }

                        tvPortofoliu.ExpandAll();
                    }
                }
            }
        }

        private void salveazaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Binary Files (*.bin)|*.bin|All Files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                        {
                            BinaryWriter writer = new BinaryWriter(fileStream);

                            // Salvarea datelor
                            writer.Write(listaTranzactii.Count);
                            foreach (Tranzactii tranzactie in listaTranzactii)
                            {
                                writer.Write(tranzactie.TipTranzactie);
                                writer.Write(tranzactie.NumeFirma);
                                writer.Write(tranzactie.Cantitate);
                                writer.Write(tranzactie.Pret);
                                writer.Write(tranzactie.Data.ToBinary());
                            }

                            writer.Write(listaActiuni.Count);
                            foreach (Actiuni actiune in listaActiuni)
                            {
                                writer.Write(actiune.NumeFirmaDetinatoare);
                                writer.Write(actiune.NrFirme);
                                foreach (string firma in actiune.ListaFirme)
                                {
                                    writer.Write(firma);
                                }
                                foreach (float pret in actiune.PretCumparareActiuni)
                                {
                                    writer.Write(pret);
                                }
                            }

                            writer.Close();
                        }

                        MessageBox.Show("Datele au fost salvate cu succes.", "Salvare reușită", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Eroare la salvare: " + ex.Message, "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void restaureazaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Binary Files (*.bin)|*.bin|All Files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (FileStream fileStream = new FileStream(openFileDialog.FileName, FileMode.Open))
                        {
                            BinaryReader reader = new BinaryReader(fileStream);

                            // Restaurarea datelor
                            listaTranzactii.Clear();
                            int numTranzactii = reader.ReadInt32();
                            for (int i = 0; i < numTranzactii; i++)
                            {
                                string tipTranzactie = reader.ReadString();
                                string numeFirma = reader.ReadString();
                                int cantitate = reader.ReadInt32();
                                float pret = reader.ReadSingle();
                                DateTime data = DateTime.FromBinary(reader.ReadInt64());
                                Tranzactii tranzactie = new Tranzactii(tipTranzactie, numeFirma, cantitate, pret, data);
                                listaTranzactii.Add(tranzactie);
                            }

                            listaActiuni.Clear();
                            int numActiuni = reader.ReadInt32();
                            for (int i = 0; i < numActiuni; i++)
                            {
                                string numeFirmaDetinatoare = reader.ReadString();
                                int numarFirme = reader.ReadInt32();
                                string[] listaFirme = new string[numarFirme];
                                for (int j = 0; j < numarFirme; j++)
                                {
                                    listaFirme[j] = reader.ReadString();
                                }
                                float[] pretCumparareActiuni = new float[numarFirme];
                                for (int j = 0; j < numarFirme; j++)
                                {
                                    pretCumparareActiuni[j] = reader.ReadSingle();
                                }
                                Actiuni actiune = new Actiuni(numeFirmaDetinatoare, numarFirme, listaFirme, pretCumparareActiuni);
                                listaActiuni.Add(actiune);
                            }

                            reader.Close();
                        }

                        MessageBox.Show("Datele au fost restaurate cu succes.", "Restaurare reușită", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Eroare la restaurare: " + ex.Message, "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void adaugaTranzactieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Validare și preluare date
                string numeFirma = tbNumeFirma.Text.Trim();
                if (string.IsNullOrEmpty(numeFirma))
                {
                    throw new Exception("Numele firmei nu este valid.");
                }

                if (!float.TryParse(tbPret.Text, out float pret))
                {
                    throw new Exception("Prețul introdus nu este valid.");
                }

                if (!int.TryParse(tbCantitate.Text, out int cantitate))
                {
                    throw new Exception("Cantitatea introdusă nu este validă.");
                }

                string tipTranzactie = rbTipTranzactie1.Checked ? "Cumpărare" : (rbTipTranzactie2.Checked ? "Vânzare" : null);
                if (tipTranzactie == null)
                {
                    throw new Exception("Tipul tranzacției nu este selectat.");
                }

                DateTime data = dtpData.Value;

                // Crearea obiectului Tranzactii
                Tranzactii tranzactie = new Tranzactii(tipTranzactie, numeFirma, cantitate, pret, data);
                listaTranzactii.Add(tranzactie);

                MessageBox.Show("Tranzacția a fost adăugată cu succes.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Resetarea câmpurilor formularului
                tbNumeFirma.Clear();
                tbCantitate.Clear();
                tbPret.Clear();
                rbTipTranzactie1.Checked = false;
                rbTipTranzactie2.Checked = false;
                tbNumeFirma.Focus();
            }
        }

        private void stergeTranzactieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listaTranzactii.Count == 0)
            {
                MessageBox.Show("Nu există nicio tranzacție de șters.", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Ștergeți ultima tranzacție
            listaTranzactii.RemoveAt(listaTranzactii.Count - 1);

            // Actualizați textBox-ul
            StringBuilder tranzactiiText = new StringBuilder();
            foreach (Tranzactii tranzactie in listaTranzactii)
            {
                tranzactiiText.AppendLine(tranzactie.ToString());
                tranzactiiText.AppendLine("--------------------------------------------");
            }
            tbFormular.Text = tranzactiiText.ToString();

            MessageBox.Show("Ultima tranzacție a fost ștearsă cu succes.");
        }

        private void adaugaActiuneLaPortofoliuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Validare și preluare date
                string numeFirmaDetinatoare = tbNumeFirmaDetinatoare.Text.Trim();
                if (string.IsNullOrEmpty(numeFirmaDetinatoare))
                {
                    throw new Exception("Numele firmei deținătoare nu este valid.");
                }

                if (!int.TryParse(tbNumarFirmeDetineActiuni.Text, out int nrFirme))
                {
                    throw new Exception("Numărul firmelor care dețin acțiuni nu este valid.");
                }

                string[] listaFirme = tbListaFirme.Text.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (listaFirme.Length != nrFirme)
                {
                    throw new Exception("Numărul firmelor din lista de firme nu corespunde cu numărul specificat.");
                }

                string[] preturiString = tbPretActiunePeFirma.Text.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (preturiString.Length != nrFirme)
                {
                    throw new Exception("Numărul prețurilor nu corespunde cu numărul specificat.");
                }

                float[] pretCumparareActiuni = new float[nrFirme];
                for (int i = 0; i < nrFirme; i++)
                {
                    if (!float.TryParse(preturiString[i], out pretCumparareActiuni[i]))
                    {
                        throw new Exception($"Prețul pentru firma {listaFirme[i]} nu este valid.");
                    }
                }

                // Crearea obiectului Actiuni
                Actiuni actiune = new Actiuni(numeFirmaDetinatoare, nrFirme, listaFirme, pretCumparareActiuni);
                listaActiuni.Add(actiune);

                MessageBox.Show("Acțiunea a fost adăugată cu succes.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Resetarea câmpurilor formularului
                tbNumeFirmaDetinatoare.Clear();
                tbNumarFirmeDetineActiuni.Clear();
                tbListaFirme.Clear();
                tbPretActiunePeFirma.Clear();
                tbNumeFirmaDetinatoare.Focus();
            }
        }

        private void stergereActiuneDinPortofoliuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listaActiuni.Count == 0)
            {
                MessageBox.Show("Nu există nicio acțiune de șters.", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Ștergeți ultima acțiune
            listaActiuni.RemoveAt(listaActiuni.Count - 1);

            // Actualizați TreeView-ul
            TreeNode portfolioNode = tvPortofoliu.Nodes[0]; // Obținem nodul portofoliului
            TreeNode actiuniNode = portfolioNode.Nodes[0]; // Obținem nodul pentru lista de acțiuni

            // Ștergem toți copiii nodului cu acțiuni
            actiuniNode.Nodes.Clear();

            // Ștergem și nodul pentru valoarea totală (dacă există)
            foreach (TreeNode node in portfolioNode.Nodes)
            {
                if (node.Text.StartsWith("Valoarea totală"))
                {
                    portfolioNode.Nodes.Remove(node);
                    break; // Ne oprim după ce am găsit și șters nodul
                }
            }

            MessageBox.Show("Ultima acțiune și valoarea totală au fost șterse cu succes din TreeView.", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

       

        private void incarcaDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                StreamReader sr = new StreamReader("DateCantitatePretActiuni.txt");
                string linie = null;
                List<Tranzactii> listaVanzari = new List<Tranzactii>();

                while ((linie = sr.ReadLine()) != null)
                {
                    string[] valori = linie.Split(',');

                    if (valori.Length == 2)
                    {
                        int cantitate;
                        float pret;

                        if (int.TryParse(valori[0], out cantitate) && float.TryParse(valori[1], out pret))
                        {
                            Tranzactii tranzactie = new Tranzactii("Vânzare", "", cantitate, pret, DateTime.Now);
                            listaVanzari.Add(tranzactie);
                        }
                        else
                        {
                            MessageBox.Show($"Eroare la conversia valorilor de pe linia: {linie}", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Linia {linie} nu are formatul corect (cantitate,pret).", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                sr.Close();

                // Adăugați tranzacțiile în lista principală
                listaTranzactii.AddRange(listaVanzari);

                MessageBox.Show("Datele au fost încărcate cu succes!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la încărcarea datelor: {ex.Message}", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            chart1.Visible = false;
            if (listaTranzactii.Count > 0) // Verificăm dacă există tranzacții
            {
                Graphics g = e.Graphics;
                Rectangle rectangle = new Rectangle(panel1.ClientRectangle.X + margine, panel1.ClientRectangle.Y + 4 * margine,
                     panel1.ClientRectangle.Width - 2 * margine, panel1.ClientRectangle.Height - 5 * margine);
                Pen pen = new Pen(Color.Red, 3);
                g.DrawRectangle(pen, rectangle);

                double latime = rectangle.Width / listaTranzactii.Count / 3;
                double distanta = (rectangle.Width - listaTranzactii.Count * latime) / (listaTranzactii.Count + 1);
                float valoareMaxima = listaTranzactii.Max(t => t.Pret); // Presupunem că vrei să afișezi prețul maxim

                Brush brBars = new SolidBrush(culoareBars);
                Brush brFont = new SolidBrush(culoareText);

                Rectangle[] rectangles = new Rectangle[listaTranzactii.Count];
                for (int i = 0; i < listaTranzactii.Count; i++)
                {
                    rectangles[i] = new Rectangle((int)(rectangle.Location.X + (i + 1) * distanta + i * latime),
                    (int)(rectangle.Location.Y + rectangle.Height - listaTranzactii[i].Pret / valoareMaxima * rectangle.Height),
                    (int)latime,
                    (int)(listaTranzactii[i].Pret / valoareMaxima * rectangle.Height));

                    g.DrawString(listaTranzactii[i].NumeFirma, fontText, brFont, new Point((int)(rectangles[i].Location.X),
                        (int)(rectangles[i].Location.Y - fontText.Height)));
                }
                g.FillRectangles(brBars, rectangles);

                for (int i = 0; i < listaTranzactii.Count - 1; i++)
                {
                    g.DrawLine(pen, new Point((int)(rectangles[i].Location.X + latime / 2), (int)(rectangles[i].Location.Y)),
                        new Point((int)(rectangles[i + 1].Location.X + latime / 2), (int)(rectangles[i + 1].Location.Y)));
                }
            }
        }

        private void graficDesenatBarsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(listaTranzactii.Count > 0)
            {
                chart1.Visible = false;
                panel1.Invalidate();
            }
            else
            {
                MessageBox.Show("Datele nu au fost incarcate");
            }
        }

        private void graphicBarsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.Visible = true;
            chart1.Series.Clear();
            chart1.Titles.Clear();

            if (listaTranzactii.Count > 0)
            {
                // Adăugați o serie de date pentru grafic
                Series series = chart1.Series.Add("Tranzactii");
                series.ChartType = SeriesChartType.Column;

                // Adăugați datele din listaTranzactii la seria de date
                foreach (Tranzactii tranzactie in listaTranzactii)
                {
                    series.Points.AddXY(tranzactie.Cantitate, tranzactie.Pret);
                }

                // Configurați titlul graficului
                chart1.Titles.Add("Grafic cu bare pentru tranzacții");
            }
            else
            {
                MessageBox.Show("Datele nu au fost încărcate");
            }
        }

        private void graphicPieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listaTranzactii.Count > 0)
            {
                chart1.Series["Vanzari actiuni"].Points.Clear();
                chart1.Titles.Clear();
                chart1.Visible = true;
                chart1.Series[0].ChartType = SeriesChartType.Pie;

                List<Tranzactii> listaVanzari = listaTranzactii.Where(t => t.TipTranzactie == "Vânzare").ToList();

                foreach (Tranzactii tranzactie in listaVanzari)
                {
                    chart1.Series["Vanzari actiuni"].Points.AddXY(tranzactie.NumeFirma, tranzactie.Pret);
                }

                chart1.Titles.Add("Vânzări actiuni");
            }
            else
            {
                MessageBox.Show("Datele nu au fost încărcate");
            }
        }

        private void salvare_bmp(Control c, string nume_fisier)
        {
            Bitmap img = new Bitmap(c.Width, c.Height);
            c.DrawToBitmap(img, new Rectangle(c.ClientRectangle.X, c.ClientRectangle.Y, c.ClientRectangle.Width,
                c.ClientRectangle.Height));
            img.Save(nume_fisier);
            img.Dispose();
        }

        private void salvareBmpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            salvare_bmp(panel1, "Grafic_" + DateTime.Now.ToString("dd-MM-yyyy") + ".bmp");
            MessageBox.Show("S-a salvat imaginea!");
        }

        private void pp(object sender, PrintPageEventArgs e)
        {
            if (listaTranzactii.Count > 0)
            {
                Graphics g = e.Graphics;
                Rectangle rectangle = new Rectangle(e.PageBounds.X + margine, e.PageBounds.Y + 4 * margine,
                    e.PageBounds.Width - 2 * margine, e.PageBounds.Height - 5 * margine);
                Pen pen = new Pen(Color.Red, 3);
                g.DrawRectangle(pen, rectangle);

                double latime = rectangle.Width / listaTranzactii.Count / 3;
                double distanta = (rectangle.Width - listaTranzactii.Count * latime) / (listaTranzactii.Count + 1);
                double valoareMaximaPret = listaTranzactii.Max(t => t.Pret);
                double valoareMaximaCantitate = listaTranzactii.Max(t => t.Cantitate);

                Brush brBars = new SolidBrush(culoareBars);
                Brush brFont = new SolidBrush(culoareText);

                Rectangle[] rectangles = new Rectangle[listaTranzactii.Count];
                for (int i = 0; i < rectangles.Length; i++)
                {
                    rectangles[i] = new Rectangle((int)(rectangle.Location.X + (i + 1) * distanta + i * latime),
                        (int)(rectangle.Location.Y + rectangle.Height - listaTranzactii[i].Cantitate / valoareMaximaCantitate * rectangle.Height),
                        (int)latime, (int)(listaTranzactii[i].Cantitate / valoareMaximaCantitate * rectangle.Height));

                    g.DrawString($"{listaTranzactii[i].NumeFirma} - Pret: {listaTranzactii[i].Pret}, Cantitate: {listaTranzactii[i].Cantitate}", fontText, brFont, new Point((int)(rectangles[i].Location.X), (int)(rectangles[i].Location.Y - fontText.Height)));
                }

                g.FillRectangles(brBars, rectangles);

                for (int i = 0; i < listaTranzactii.Count - 1; i++)
                {
                    g.DrawLine(pen, new Point((int)(rectangles[i].Location.X + latime / 2), (int)(rectangles[i].Location.Y)),
                        new Point((int)(rectangles[i + 1].Location.X + latime / 2), (int)(rectangles[i + 1].Location.Y)));
                }
            }
        }
        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(pp);
            PrintPreviewDialog pdlg = new PrintPreviewDialog
            {
                Document = pd
            };
            pdlg.ShowDialog();
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

        private void textToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                culoareText = cd.Color;
            }
            panel1.Invalidate();
        }

        private void modificareFontToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                fontText = fd.Font;
            }
            panel1.Invalidate();
        }

        private void btnPopuleaza_Click(object sender, EventArgs e)
        {
            lbActiuni.DataSource = listaActiuni;
            tbActiuni.DataBindings.Add(new Binding("Text", listaActiuni, ""));
        }


        private void btnAfiseazaDVG_Click(object sender, EventArgs e)
        {
            Actiuni actiune = (Actiuni)dvgActiunii.CurrentRow.DataBoundItem;
            MessageBox.Show(actiune.ToString());
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (((System.Windows.Forms.Button)sender).Tag as string == "Prev")
            {
                BindingContext[listaActiuni].Position -= 1;
            }
            else
            {
                BindingContext[listaActiuni].Position += 1;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (((System.Windows.Forms.Button)sender).Tag as string == "Prev")
            {
                BindingContext[listaActiuni].Position -= 1;
            }
            else
            {
                BindingContext[listaActiuni].Position += 1;
            }
        }

        private void adaugaTranzactiaIntrunXLMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Validare și preluare date
                string numeFirma = tbNumeFirma.Text.Trim();
                if (string.IsNullOrEmpty(numeFirma))
                {
                    throw new Exception("Numele firmei nu este valid.");
                }

                if (!float.TryParse(tbPret.Text, out float pret))
                {
                    throw new Exception("Prețul introdus nu este valid.");
                }

                if (!int.TryParse(tbCantitate.Text, out int cantitate))
                {
                    throw new Exception("Cantitatea introdusă nu este validă.");
                }

                string tipTranzactie = rbTipTranzactie1.Checked ? "Cumpărare" : (rbTipTranzactie2.Checked ? "Vânzare" : null);
                if (tipTranzactie == null)
                {
                    throw new Exception("Tipul tranzacției nu este selectat.");
                }

                DateTime data = dtpData.Value;

                // Crearea obiectului Tranzactii
                Tranzactii tranzactie = new Tranzactii(tipTranzactie, numeFirma, cantitate, pret, data);

                // Salvarea tranzacției în fișierul XML
                XmlWriterSettings settings = new XmlWriterSettings { Indent = true };
                using (XmlWriter writer = XmlWriter.Create("tranzactii.xml", settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("Tranzactii");
                    writer.WriteStartElement("Tranzactie");
                    writer.WriteElementString("TipTranzactie", tranzactie.TipTranzactie);
                    writer.WriteElementString("NumeFirma", tranzactie.NumeFirma);
                    writer.WriteElementString("Cantitate", tranzactie.Cantitate.ToString());
                    writer.WriteElementString("Pret", tranzactie.Pret.ToString());
                    writer.WriteElementString("Data", tranzactie.Data.ToString());
                    writer.WriteEndElement(); // Tranzactie
                    writer.WriteEndElement(); // Tranzactii
                    writer.WriteEndDocument();
                }

                MessageBox.Show("Tranzacția a fost salvată în fișierul XML.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void afiseazaTranzactiaPreluataDinXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                XDocument doc = XDocument.Load("tranzactii.xml");
                XElement tranzactieElement = doc.Root.Element("Tranzactie");

                string tipTranzactie = tranzactieElement.Element("TipTranzactie").Value;
                string numeFirma = tranzactieElement.Element("NumeFirma").Value;
                int cantitate = int.Parse(tranzactieElement.Element("Cantitate").Value);
                float pret = float.Parse(tranzactieElement.Element("Pret").Value);
                DateTime data = DateTime.Parse(tranzactieElement.Element("Data").Value);

                Tranzactii tranzactie = new Tranzactii(tipTranzactie, numeFirma, cantitate, pret, data);

                tbFormular.Text = tranzactie.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void incarcaDateleInBdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            connection.Open();
            string tipTranzactie;
            if (rbTipTranzactie1.Checked)
            {
                tipTranzactie = rbTipTranzactie1.Text;
            }
            else if (rbTipTranzactie2.Checked)
            {
                tipTranzactie = rbTipTranzactie2.Text;
            }
            else
            {
                
                tipTranzactie = "Valoare implicită sau mesaj de eroare";
            }
            SqlCommand comand = new SqlCommand("INSERT INTO Tranzactii (tipTranzactie,numeFirma,cantitate,pret,data)" +
                "VALUES(@tipTran,@nume,@cantitate,@pret,@date)", connection);
            comand.Parameters.AddWithValue("@tipTran", tipTranzactie);
            comand.Parameters.AddWithValue("@nume", tbNumeFirma.Text);
            comand.Parameters.AddWithValue("@cantitate", tbCantitate.Text);
            comand.Parameters.AddWithValue("@pret", tbPret.Text);
            string data = dtpData.Value.ToString("yyyy-MM-dd HH:mm:ss");
            comand.Parameters.AddWithValue("@date", data);
            
            comand.ExecuteNonQuery();
            connection.Close();
            MessageBox.Show("Tranzactia a fost inserata!");
        }

        private void afisareDateDinBdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            connection.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM tranzactii", connection);
            adapter.Fill(dt);
            dgvBazaDeDate.DataSource = dt;
            connection.Close();
        }

        private void incarcaDateInBdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            connection.Open();
            SqlCommand comand = new SqlCommand("INSERT INTO actiuni (numeFirmaDetinatoare, nrFirme, listaFirme) " +
                                                "VALUES (@numeFirma, @nrFirme, @listFirme)", connection);
            comand.Parameters.AddWithValue("@numeFirma" ,tbNumeFirmaDetinatoare.Text);
            comand.Parameters.AddWithValue("@nrFirme", tbNumarFirmeDetineActiuni.Text);

            // Extrage lista de firme din tbListaFirme și lista de prețuri din tbPretActiunePeFirma
            List<string> firme = tbListaFirme.Text.Split(',').ToList();
            List<string> preturi = tbPretActiunePeFirma.Text.Split(',').ToList();

            // Verifică dacă toate valorile din lista de prețuri pot fi convertite la float
            bool preturiValide = preturi.All(p => float.TryParse(p, out _));

            if (preturiValide)
            {
                // Afisează conținutul listelor înainte de adăugare
                MessageBox.Show("Lista de firme: " + string.Join(",", firme));
                MessageBox.Show("Lista de prețuri: " + string.Join(",", preturi));

                // Adaugă lista de firme și lista de prețuri în comanda SQL
                comand.Parameters.AddWithValue("@listFirme", string.Join(",", firme));
               // comand.Parameters.AddWithValue("@pretFirme", string.Join(",", preturi));

                comand.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Actiunea a fost inserata!");
            }
            else
            {
                MessageBox.Show("Valorile prețurilor introduse nu sunt valide!");
            }
        }

        private void preluareDateDinBdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            connection.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM actiuni", connection);
            adapter.Fill(dt);
            dgvBazaDeDate.DataSource = dt;
            connection.Close();
        }
    }
}
