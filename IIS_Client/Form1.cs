using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IIS_Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ValidateXSDAsync();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ValidateRNGAsync();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PrikaziFormuUPanelu(new Form5());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            PrikaziFormuUPanelu(new Form2());
        }

        private void button6_Click(object sender, EventArgs e)
        {
            PrikaziFormuUPanelu(new Form3());
        }

        public void PrikaziFormuUPanelu(Form forma)
        {
            pnlGlavniPanel.Controls.Clear();
            forma.TopLevel = false;
            pnlGlavniPanel.Controls.Add(forma);
            forma.Show();
        }

        private async void ValidateXSDAsync()
        {
            //Pronalazak XSD i XML datoteke
            string path = AppDomain.CurrentDomain.BaseDirectory; 
            string solutionDirectoryPath = Directory.GetParent(path).Parent.Parent.Parent.FullName; 
            string fixedFilePath = Path.Combine(solutionDirectoryPath, "FootballPlayers.xml"); 
            string newFilePath = Path.GetTempFileName(); 

            //Copy originala u temp
            File.Copy(fixedFilePath, newFilePath, true);

            Process.Start("notepad++.exe", newFilePath)?.WaitForExit();

            string fileContent = File.ReadAllText(newFilePath);

            File.WriteAllText(newFilePath, fileContent);

            using (var client = new HttpClient())
            {
                // holdam datu koju saljem u request
                var formContent = new MultipartFormDataContent();

                var fileContentTemp = new ByteArrayContent(File.ReadAllBytes(newFilePath));
                fileContentTemp.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

                formContent.Add(fileContentTemp, "file", Path.GetFileName(newFilePath));

                var response = await client.PostAsync("http://localhost:5285/api/Player/SaveWithXSD", formContent);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("File successfully validated and uploaded.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }
                else
                {
                    MessageBox.Show("There was an error validating and uploading your file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }
            }
        }

        private async void ValidateRNGAsync()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string solutionDirectoryPath = Directory.GetParent(path).Parent.Parent.Parent.FullName; 
            string fixedFilePath = Path.Combine(solutionDirectoryPath, "FootballPlayers.xml"); 
            string newFilePath = Path.GetTempFileName(); // unique file path

            File.Copy(fixedFilePath, newFilePath, true);

            Process.Start("notepad++.exe", newFilePath)?.WaitForExit();

            string fileContent = File.ReadAllText(newFilePath);

            File.WriteAllText(newFilePath, fileContent);

            using (var client = new HttpClient())
            {
                var formContent = new MultipartFormDataContent();

                var fileContentTemp = new ByteArrayContent(File.ReadAllBytes(newFilePath));
                fileContentTemp.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

                formContent.Add(fileContentTemp, "file", Path.GetFileName(newFilePath));

                var response = await client.PostAsync("http://localhost:5285/api/Player/SaveWithRNG", formContent);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("File successfully validated and uploaded.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }
                else
                {
                    MessageBox.Show("There was an error validating and uploading your file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }
            }
        }

    }
}
