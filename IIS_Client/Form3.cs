using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace IIS_Client
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            lblCity.Text = tbSearch.Text + " = " + await GetCurrentTemperature(tbSearch.Text) + " °C";
        }

        public async Task<string> GetCurrentTemperature(string cityName)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string xmlContent = await client.GetStringAsync("https://vrijeme.hr/hrvatska_n.xml");
                    XDocument xmlDoc = XDocument.Parse(xmlContent);

                    var cityTemperature = xmlDoc.Descendants("Grad")
                        .FirstOrDefault(g => (string)g.Element("GradIme") == cityName)?
                        .Element("Podatci")?
                        .Element("Temp")?.Value;

                    if (cityTemperature != null)
                    {
                        return cityTemperature.ToString();
                    }
                    else
                    {
                        return lblCity.Text = $"Temperature for {cityName} doesn't exist.";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }
    }
}
