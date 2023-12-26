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
        public IFeatureLayer _editingLayer;
        IWorkspaceEdit _editSpan;
        IFeatureWorkspace _newlyAddedWorkspace;
        private IToolbarMenu m_menuLayer;
        private IMapControl3 m_mapControl;


        public Form1()
        {
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Desktop);
            InitializeComponent();
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
            axToolbarControl1.AddItem(new CreateGDBCommand(_newlyAddedWorkspace), -1, -1, false, 0,
              esriCommandStyles.esriCommandStyleIconOnly);
            axToolbarControl1.AddItem(new CreateFeatureCommand(axMapControl1, _newlyAddedWorkspace), -1, -1, false, 0,
              esriCommandStyles.esriCommandStyleIconOnly);
            axToolbarControl1.AddItem(new EditStartCommand(axMapControl1, _editingLayer, _editSpan, this), -1, -1, false, 0,
              esriCommandStyles.esriCommandStyleIconOnly);
            axToolbarControl1.AddItem(new EditTool(_editSpan, _editingLayer, axMapControl1), -1, -1, false, 0,
                    esriCommandStyles.esriCommandStyleIconOnly);
            axToolbarControl1.AddItem(new EditStopCommand(axMapControl1), -1, -1, false, 0,
              esriCommandStyles.esriCommandStyleIconOnly);

            m_mapControl = (IMapControl3)axMapControl1.Object;
            m_menuLayer = new ToolbarMenuClass();
            m_menuLayer.AddItem(new RemoveLayer(), -1, 0, false, ESRI.ArcGIS.SystemUI.esriCommandStyles.esriCommandStyleTextOnly);
            m_menuLayer.AddItem(new ZoomToLayer_New(), -1, 0, true, ESRI.ArcGIS.SystemUI.esriCommandStyles.esriCommandStyleTextOnly);
            m_menuLayer.AddItem(new CmdOpenLayerRender(), -1, 0, true, ESRI.ArcGIS.SystemUI.esriCommandStyles.esriCommandStyleTextOnly);
            m_menuLayer.AddItem(new ShowAttribute(), -1, 0, true, ESRI.ArcGIS.SystemUI.esriCommandStyles.esriCommandStyleTextOnly);
            m_menuLayer.SetHook(m_mapControl);

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

        /// <summary>
        /// 双击打开符号选择器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axTOCControl1_OnDoubleClick(object sender, ITOCControlEvents_OnDoubleClickEvent e)
        {
            esriTOCControlItem itemType = esriTOCControlItem.esriTOCControlItemNone;
            IBasicMap basicMap = null;
            ILayer layer = null;
            object unk = null;
            object data = null;

            //HitTest(鼠标点击的X坐标，鼠标点击的Y坐标，esriTOCControlItem枚举常量，绑定MapControl的IBasicMap接口
            //被点击的图层，TOCControl的LegendGroup对象，LegendClass在LegendGroup中的Index)
            axTOCControl1.HitTest(e.x, e.y, ref itemType, ref basicMap, ref layer, ref unk, ref data);
            if (e.button == 1)
            {
                if (itemType == esriTOCControlItem.esriTOCControlItemLegendClass)
                {
                    //取得图例
                    ILegendClass pLegendClass = ((ILegendGroup)unk).get_Class((int)data);
                    //创建符号选择器SymbolSelector实例
                    frmSymbolSelector SymbolSelectorFrm = new frmSymbolSelector(pLegendClass, layer);
                    if (SymbolSelectorFrm.ShowDialog() == DialogResult.OK)
                    {
                        axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
                        pLegendClass.Symbol = SymbolSelectorFrm.pSymbol;
                        axMapControl1.Refresh();
                        axTOCControl1.Update();
                    }
                }
            }
        }


        /// <summary>
        /// 右击打开菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axTOCControl1_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {
            esriTOCControlItem item = esriTOCControlItem.esriTOCControlItemNone;
            IBasicMap map = null;
            object unk = null;
            object data = null;
            ILayer layer = null;
            //右击图层名
            if (e.button == 2)
            {
                //定义被选中的对象
                axTOCControl1.HitTest(e.x, e.y, ref item, ref map, ref layer, ref unk, ref data);
                //确保对象被选中
                if (item == esriTOCControlItem.esriTOCControlItemMap)
                    axTOCControl1.SelectItem(map, null);
                else
                    axTOCControl1.SelectItem(layer, null);
                m_mapControl.CustomProperty = layer;
                m_menuLayer.PopupMenu(e.x, e.y, axTOCControl1.hWnd);
            }
        }
    }
}
