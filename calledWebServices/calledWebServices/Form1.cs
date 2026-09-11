using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;

namespace calledWebServices
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ServiceReference1.WsRecruitmentSoap a = new ServiceReference1.WsRecruitmentSoapClient();
            DataTable dt = a.GetEmail();
            string str = a.HelloWorld();

            for(int i = 0; i < dt.Rows.Count; i++)
            {
                listBox1.Items.Add(dt.Rows[i][0].ToString());
            }
            listBox1.Items.Add(str);
        }
    }
}
