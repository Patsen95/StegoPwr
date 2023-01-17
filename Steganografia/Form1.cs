namespace Steganografia
{
    public partial class Form1 : Form
    {
        private Bitmap bmp;
        private string msg;

        public Form1()
        {
            InitializeComponent();

            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            #region Podpisy kontrolek
            tabPage1.Text = "Kodowanie";
            tabPage2.Text = "Dekodowanie";
            groupBox1.Text = "Podgl¹d obrazu";
            groupBox2.Text = "Informacje";
            groupBox3.Text = "Ustawienia";

            label1.Text = "Nazwa pliku:";
            label2.Text = "Rozmiar[px]:";
            label3.Text = "Rozmiar na dysku[kB]:";
            label3.Text = "Rozmiar na dysku[kB]:";

            btn_LoadImg.Text = "Wczytaj obraz...";
            btn_Export.Text = "Eksportuj";
            #endregion
            

        }
        
        // Wczytywanie obrazka
        private void btn_LoadImg_Click(object sender, EventArgs e)
        {
            using(OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Wczytaj obraz...";
                ofd.Filter = "Image files (*.bmp, *.png)|*.bmp;*.png";
                ofd.RestoreDirectory = true;

                if(ofd.ShowDialog() == DialogResult.OK)
                {

                }
            }
        }
    }
}