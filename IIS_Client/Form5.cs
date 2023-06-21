using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IIS_Client.ServiceReference1;

namespace IIS_Client
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            // Get the search value from the TextBox
            string searchValue = tbPlayer.Text;

            // Create an instance of the web service client
            PlayerServiceSoapClient client = new PlayerServiceSoapClient();
           
            // Call the SearchPlayer method of the web service
            string result = client.SearchPlayer(searchValue);

            // Display the result or handle it as needed
            if (!string.IsNullOrEmpty(result))
            {
                //MessageBox.Show(result, "Search Result");
                lblResult.Text = result;
            }
            else
            {
                //MessageBox.Show("No matching player found.", "Search Result");
                lblResult.Text = "No player found.";
            }

        }
    }
}
