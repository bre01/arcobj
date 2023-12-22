using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
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
    public partial class GDBForm : Form
    {
        string _folder;
        public GDBForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog folderLog = new FolderBrowserDialog();
            folderLog.ShowNewFolderButton = true;
            DialogResult res = folderLog.ShowDialog();
            _folder = folderLog.SelectedPath;
            label2.Text = _folder;
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            CreateGDBWorkspace(_folder, textBox1.Text);
            
        }
        public IWorkspace CreateGDBWorkspace(string folderPath,string dbName)
        {
            var factor = new FileGDBWorkspaceFactory();
            IWorkspaceName workspaceName = factor.Create(folderPath, dbName, null, 0);
            IName name = (IName)workspaceName;
            IWorkspace workspace = (IWorkspace)name.Open();
            return workspace;


        }
    }
}
