using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data; 
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageToBase64
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            if (txtIDEvento.Text.Trim() != string.Empty)
            {
                for (int i = 0; i < txtIDEvento.Text.Length; i++)
                {
                    if (!Char.IsNumber(txtIDEvento.Text[i]))
                    {
                        MessageBox.Show("Digite apenas números.");
                        txtIDEvento.Text = string.Empty;
                        return;
                    }
                }

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string base64 = ImageToBase64(ofd.FileName);

                    FileInfo f = new FileInfo(ofd.FileName);

                    Save(Application.StartupPath + "\\" + "INSERT_" + f.Name.Replace(f.Extension, string.Empty) + ".SQL",
                          "UPDATE eventos SET imagem64 = '" + base64 + "' WHERE idEvento = " + txtIDEvento.Text);

                    txtIDEvento.Text = string.Empty;
                }
            }
            else
            {
                MessageBox.Show("Informe o ID do evento.");
            }
        }

        private bool Save(string strPath, string base64)
        {
            try
            {
                FileStream fs = new FileStream(strPath, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(base64);
                sw.Close();
                fs.Close();
                
                Process.Start(Application.StartupPath);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string ImageToBase64(string strPath)
        {
            using (Image image = Image.FromFile(strPath))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    string base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
        }
    }
}
