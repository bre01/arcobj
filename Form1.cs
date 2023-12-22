using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EX3;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;

namespace EX3
{
    public partial class Form1 : Form
    {
        IFeatureWorkspace _ws;
        public IFeatureLayer _editingLayer;
        IWorkspaceEdit _editSpan;
        Flag _flag = new Flag();
        public void SetEdit(IWorkspaceEdit edit, IFeatureLayer layer)
        {
            _editSpan = edit;
            _editingLayer = layer;


        }

        public Form1()
        {
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Desktop);
            InitializeComponent();


        }
        public class Flag
        {
            public bool able = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            axToolbarControl1.AddItem(new OpenDocument(axMapControl1), -1, -1, true, 0,
            esriCommandStyles.esriCommandStyleIconOnly);

            FullExtent fullExtentTool = new FullExtent();
            fullExtentTool.OnCreate(axMapControl1.Object);
            axToolbarControl1.AddItem(fullExtentTool, -1, -1, false, 0, esriCommandStyles.esriCommandStyleIconOnly);

            ZoomIn zoomInTool = new ZoomIn();
            zoomInTool.OnCreate(axMapControl1.Object); // 关联地图控件
            axToolbarControl1.AddItem(zoomInTool, -1, -1, true, 0,
            esriCommandStyles.esriCommandStyleIconOnly);

            axToolbarControl1.AddItem(new SaveAsDocument(axMapControl1), -1, -1, true,
                0, esriCommandStyles.esriCommandStyleIconOnly);

            ICommand loadLayerCommand = new OpenCommand();
            // 将LoadLayerCommand添加到ToolbarControl1
            axToolbarControl1.AddItem(loadLayerCommand, -1, -1, false, 0,
              esriCommandStyles.esriCommandStyleIconOnly);
            axToolbarControl1.AddItem(new OpenQuery(axMapControl1), -1, -1, false, 0,
              esriCommandStyles.esriCommandStyleIconOnly);
            axToolbarControl1.AddItem(new IdentifyTool(axMapControl1), -1, -1, false, 0,
              esriCommandStyles.esriCommandStyleIconOnly);

            axToolbarControl1.AddItem(new IdentifyTool(axMapControl1), -1, -1, false, 0,
              esriCommandStyles.esriCommandStyleIconOnly);
            axToolbarControl1.AddItem(new CreateGDBCommand(_ws as IFeatureWorkspace), -1, -1, false, 0,
              esriCommandStyles.esriCommandStyleIconOnly);
            axToolbarControl1.AddItem(new CreateFeatureCommand(axMapControl1, _ws as IFeatureWorkspace), -1, -1, false, 0,
              esriCommandStyles.esriCommandStyleIconOnly);
            axToolbarControl1.AddItem(new EditStartCommand(axMapControl1, _editingLayer, _editSpan, this), -1, -1, false, 0,
              esriCommandStyles.esriCommandStyleIconOnly);

            /*axToolbarControl1.AddItem(new EditStopCommand(axMapControl1,_ws as IFeatureWorkspace), -1, -1, false, 0, 
              esriCommandStyles.esriCommandStyleIconOnly);*/

        }
        public void AddEdit()
        {
            axToolbarControl1.AddItem(new EditTool(_editSpan,  _editingLayer, axMapControl1), -1, -1, false, 0,
          esriCommandStyles.esriCommandStyleIconOnly);

        }

        private void axToolbarControl1_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IToolbarControlEvents_OnMouseDownEvent e)
        {

        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void Form1_Click(object sender, EventArgs e)
        {

        }
    }
}
