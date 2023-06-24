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
          
            string searchValue = tbPlayer.Text;

            PlayerServiceSoapClient client = new PlayerServiceSoapClient();
           
            string result = client.SearchPlayer(searchValue);

            if (!string.IsNullOrEmpty(result))
            {
                lblResult.Text = result;
            }
            else
            {
                lblResult.Text = "No player found.";
            }

        }
    }
}
