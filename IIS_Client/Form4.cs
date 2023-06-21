using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IIS_Client
{
    public partial class Form4 : Form
    {
        public Form4(string temp)
        {
            InitializeComponent();
            lblResult.Text = temp;
        }
    }
}
