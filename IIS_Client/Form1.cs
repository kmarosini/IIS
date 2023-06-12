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

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private async void ValidateXSDAsync()
        {
            //Pronalazak XSD i XML datoteke
            string path = AppDomain.CurrentDomain.BaseDirectory; // Gets the bin/debug directory path
            string solutionDirectoryPath = Directory.GetParent(path).Parent.Parent.Parent.FullName; // Navigates up to the solution directory
            string fixedFilePath = Path.Combine(solutionDirectoryPath, "FootballPlayers.xml"); // Combines the solution directory path with the filename
            string newFilePath = Path.GetTempFileName(); // Generate a unique temporary file path

            //Copy the original file into a temp one
            File.Copy(fixedFilePath, newFilePath, true);

            // Open the editor for the temp file
            Process.Start("notepad++.exe", newFilePath)?.WaitForExit();

            // Read the contents of the fixed file
            string fileContent = File.ReadAllText(newFilePath);

            // Save the modified content to the new file
            File.WriteAllText(newFilePath, fileContent);

            using (var client = new HttpClient())
            {
                var formContent = new MultipartFormDataContent();

                var fileContentTemp = new ByteArrayContent(File.ReadAllBytes(newFilePath));
                fileContentTemp.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

                // Add the new file to the POST request
                formContent.Add(fileContentTemp, "file", Path.GetFileName(newFilePath));

                // Send the POST request to the server
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
            //Pronalazak XSD i XML datoteke
            string path = AppDomain.CurrentDomain.BaseDirectory; // Gets the bin/debug directory path
            string solutionDirectoryPath = Directory.GetParent(path).Parent.Parent.Parent.FullName; // Navigates up to the solution directory
            string fixedFilePath = Path.Combine(solutionDirectoryPath, "FootballPlayers.xml"); // Combines the solution directory path with the filename
            string newFilePath = Path.GetTempFileName(); // Generate a unique temporary file path

            //Copy the original file into a temp one
            File.Copy(fixedFilePath, newFilePath, true);

            // Open the editor for the temp file
            Process.Start("notepad++.exe", newFilePath)?.WaitForExit();

            // Read the contents of the fixed file
            string fileContent = File.ReadAllText(newFilePath);

            // Save the modified content to the new file
            File.WriteAllText(newFilePath, fileContent);

            using (var client = new HttpClient())
            {
                var formContent = new MultipartFormDataContent();

                var fileContentTemp = new ByteArrayContent(File.ReadAllBytes(newFilePath));
                fileContentTemp.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

                // Add the new file to the POST request
                formContent.Add(fileContentTemp, "file", Path.GetFileName(newFilePath));

                // Send the POST request to the server
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
