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
            if (radioButton1.Checked)
            {
                CreateAcWorkspace(_folder, textBox1.Text);

            }
            else if (radioButton2.Checked)
            {
                CreateGDBWorkspace(_folder, textBox1.Text);
            }
            else if (radioButton3.Checked)
            {
                CreateShapeWorkspace(_folder, textBox1.Text);
            }

        }
        public IWorkspace CreateGDBWorkspace(string folderPath, string dbName)
        {
            var factor = new FileGDBWorkspaceFactory();
            IWorkspaceName workspaceName = factor.Create(folderPath, dbName, null, 0);
            IName name = (IName)workspaceName;
            IWorkspace workspace = (IWorkspace)name.Open();
            return workspace;
        }
        public IWorkspace CreateAcWorkspace(string foldPath, string dbName)
        {
            IWorkspaceFactory workspaceFactory = new AccessWorkspaceFactoryClass();
            IWorkspaceName workspaceName = workspaceFactory.Create(foldPath, dbName, null, 0);
            // 通过接口转换到 IName 接口，调用 Open 方法得到工作区对象实例。
            IName Name = (IName)workspaceName;
            IWorkspace workspace = (IWorkspace)Name.Open();
            return workspace;
        }
        public IWorkspace CreateShapeWorkspace(string foldPath, string dbName)
        {
            /*
            IWorkspaceFactory2 workspaceFactory = (IWorkspaceFactory2)new ShapefileWorkspaceFactoryClass();
            IWorkspaceName workspaceName = workspaceFactory.Create("c:\\gisdata\\", "YunnanSF", null, 0);
            IName Name = (IName)workspaceName;
            IWorkspace workspace = (IWorkspace)Name.Open();
            return workspace
            */
            return null;
        }
    }
}
