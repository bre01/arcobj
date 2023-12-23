using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EX3
{
    public partial class ModificationForm : Form
    {
        public bool Save=false;
        public ModificationForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Save = true;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Save = false;
            this.Close();
        }
    }
}
