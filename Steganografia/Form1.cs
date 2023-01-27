namespace Steganografia
{
    public partial class Form1 : Form
    {
        private Bitmap bmp = null;
        private string theText = string.Empty;
        private int maxChars = 0;        

        public Form1()
        {
            InitializeComponent();

            #region Ustawienia kontrolek
            FormBorderStyle = FormBorderStyle.FixedSingle;
            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            Text = "Steganosaur";
            
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            textBox1.ScrollBars = ScrollBars.Vertical;
            btn_Export.Enabled = false;
            btn_Encode.Enabled = false;
            btn_Decode.Enabled = false;

            FormClosed += Form1_FormClosed;
            btn_LoadImg.Click += Btn_LoadImg_Click;
            btn_Clear.Click += Btn_Clear_Click;
            btn_Export.Click += Btn_Export_Click;
            btn_Encode.Click += Btn_Encode_Click;
            btn_Decode.Click += Btn_Decode_Click;
            btn_Author.Click += Btn_Author_Click;
            textBox1.TextChanged += TextBox1_TextChanged;

            groupBox1.Text = "Podgl¹d obrazu";
            groupBox2.Text = "Informacje";
            groupBox3.Text = "Wiadomoœæ";

            label3.Text = string.Empty;
            label4.Text = string.Empty;
            label6.Text = string.Empty;
            label8.Text = "0";

            btn_LoadImg.Text = "Wczytaj obraz...";
            btn_Export.Text = "Eksportuj";
            btn_Clear.Text = "Wyczyœæ";
            btn_Encode.Text = "Zakoduj";
            btn_Decode.Text = "Odkoduj";
            btn_Author.Text = "?";
            #endregion
        }

        // Sprz¹tanie zasobów po zamkniêciu g³ównej formatki
        private void Form1_FormClosed(object? sender, FormClosedEventArgs e)
        {
            if(bmp != null)
                bmp.Dispose();
        }

        // Wczytywanie obrazka
        private void Btn_LoadImg_Click(object? sender, EventArgs e)
        {
            using(OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Wczytaj obraz...";
                ofd.Filter = "Image files (*.bmp, *.png)|*.bmp;*.png";
                ofd.RestoreDirectory = true;

                if(ofd.ShowDialog() == DialogResult.OK)
                {
                    bmp = new Bitmap(ofd.FileName);
                    maxChars = GetMaxCodingLength(bmp);

                    pictureBox1.Image = bmp;
                    label3.Text = ofd.SafeFileName;
                    label4.Text = string.Format("{0} x {1}", bmp.Size.Width, bmp.Size.Height);
                    label6.Text = string.Format("{0:N0} ({1} kB)", maxChars, (maxChars / 1000));
                    btn_Decode.Enabled = true;
                }
            }
        }

        // Przycisk eksportu
        private void Btn_Export_Click(object? sender, EventArgs e)
        {
            using(SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "Zapisz obraz";
                sfd.Filter = "PNG (*.png)|*.png|BMP (*.bmp)|*.bmp";
                sfd.RestoreDirectory = true;

                if(sfd.ShowDialog() == DialogResult.OK)
                {
                    bmp.Save(sfd.FileName);
                    MessageBox.Show("Obraz pomyœlnie wyeksportowano!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        // Zakodowanie danych
        private void Btn_Encode_Click(object? sender, EventArgs e)
        {
            if(textBox1.TextLength >= maxChars)
            {
                MessageBox.Show(string.Format("D³ugoœæ tekstu przekracza maksymalny dostêpny zakres: {0}.", maxChars), "B³¹d kodowania", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            theText = textBox1.Text;
            bmp = Steganosaur.Encode(bmp, theText);
        }

        // Odkodowanie danych
        private void Btn_Decode_Click(object? sender, EventArgs e)
        {
            textBox1.Text = Steganosaur.Decode(bmp);
        }

        // Czyszczenie okienka tekstowego
        private void Btn_Clear_Click(object? sender, EventArgs e)
        {
            if(textBox1.TextLength > 0)
                textBox1.Clear();
        }

        // Informacja o autorze
        private void Btn_Author_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("\"Steganosaur\" - aplikacja pozwalaj¹ca ukryæ dowolny tekst w obrazku." +
                            "\nPatryk Sienkiewicz (221158)\nPolitechnika Wroc³awska, WEFiM 2023",
                            "Autor", MessageBoxButtons.OK, MessageBoxIcon.Information
                        );
        }

        // Zdarzenie podczas zmiany tekstu
        private void TextBox1_TextChanged(object? sender, EventArgs e)
        {
            label8.Text = textBox1.TextLength.ToString("N0");
            if((textBox1.TextLength > 0) && (bmp != null))
            {
                btn_Encode.Enabled = true;
                btn_Export.Enabled = true;
            }
            else
            {
                btn_Encode.Enabled = false;
                btn_Export.Enabled = false;
            }
        }

        // Dodatkowa metoda pozwalaj¹ca obliczyæ przybli¿on¹ liczbê bajtów (znaków),
        // które da siê zmieœciæ we wczytanym obrazku
        private int GetMaxCodingLength(Bitmap bmp)
        {
            int _pixels = bmp.Width * bmp.Height;
            return (int)(_pixels / 3);
        }
    }
}