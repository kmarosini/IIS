using IIS_Client.Models;
using Newtonsoft.Json;
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
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace IIS_Client
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            GetPlayers();
        }

        public async void GetPlayers()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://api-football-v1.p.rapidapi.com/v3/countries"),
                Headers =
                {
                    { "X-RapidAPI-Key", "8b79006348msh7aa4de3682922dep16db12jsnb3ecd83c34d8" },
                    { "X-RapidAPI-Host", "api-football-v1.p.rapidapi.com" },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);

                List<CountryRapid> countries = ExtractCountriesFromJson(body);
                dgvPlayers.DataSource = countries;
                foreach (CountryRapid country in countries)
                {
                    Console.WriteLine($"Name: {country.Name}, Code: {country.Code}, Flag: {country.Flag}");
                }
            }

        }

        public static List<CountryRapid> ExtractCountriesFromJson(string json)
        {
            JObject jsonObject = JObject.Parse(json);
            JArray responseArray = (JArray)jsonObject["response"];

            List<CountryRapid> countries = new List<CountryRapid>();
            foreach (JObject countryObject in responseArray)
            {
                CountryRapid country = new CountryRapid
                {
                    Name = (string)countryObject["name"],
                    Code = (string)countryObject["code"],
                    Flag = (string)countryObject["flag"]
                };
                countries.Add(country);
            }

            return countries;
        }

        public async void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
