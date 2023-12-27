using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Geodatabase;

namespace EX3
{
    /// <summary>
    /// 渲染方式选择窗口，能够为要素选择单一符号、唯一值符号或分级符号
    /// </summary>
    public partial class frmLayerRender : Form
    {
        ISimpleRenderer m_sRen;
        IUniqueValueRenderer m_UVRen;
        ISymbol[] m_Symbols;
        string[] m_Labels;
        ILayer layer;
        ILegendClass pLegendClass;
        IHookHelper m_Hookhelper;
        AxMapControl axMapControl1;
        ILayerFields m_layerfields;
        IGeoFeatureLayer geoFeatureLayer;


        public frmLayerRender(IHookHelper hookHelper)
        {
            InitializeComponent();
            this.m_Hookhelper = hookHelper;
            //找到当前操作的layer
            IMapControl3 m_mapControl = (IMapControl3)hookHelper.Hook;
            layer = (ILayer)m_mapControl.CustomProperty;
            //通过一系列的接口把ILayer转为ILegendClass
            IFeatureLayer pFeatureLayer = layer as IFeatureLayer;
            ILegendInfo lengendInfo = (ILegendInfo)pFeatureLayer;
            ILegendGroup legendGroup = lengendInfo.get_LegendGroup(0);
            pLegendClass = legendGroup.get_Class(0); //获取到LegendClass  
            //获取axmapcontrol
            axMapControl1 = Control.FromHandle(new IntPtr(this.m_Hookhelper.ActiveView.ScreenDisplay.hWnd)) as AxMapControl;
            geoFeatureLayer = layer as IGeoFeatureLayer;
            //为m_sRen赋初值
            if ((geoFeatureLayer.Renderer is SimpleRenderer))
            {
                m_sRen = geoFeatureLayer.Renderer as ISimpleRenderer;
            }

            
        }


        /// <summary>
        /// “选择符号”按钮点击事件，用于调用frmSymbolSelector并将选择的Symbol应用于选定的图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBmp_Click(object sender, EventArgs e)
        {
            if(pLegendClass is UniqueValueRendererClass)
            {
                m_sRen = new SimpleRendererClass();
                m_sRen.Symbol = default;
                
            }
            frmSymbolSelector symbolForm = new frmSymbolSelector(pLegendClass, layer);

            IStyleGalleryItem styleGalleryItem = null;
            if (symbolForm.ShowDialog() == DialogResult.OK)
            {
                //m_sRen.Symbol = pLegendClass.Symbol;


                //从symbolForm中获取样式

                styleGalleryItem = symbolForm.GetItem();
                if (styleGalleryItem == null)
                {
                    return;
                }

                m_sRen.Symbol = (ISymbol)styleGalleryItem.Item;


                //styleGalleryItem中的符号设置为btnBmp的背景图片
                Bitmap b = symbolForm.Sym2Bitmap(m_sRen.Symbol, 32, 32);
                btnBmp.Image = (Image)b;
                btnBmp.Text = "";
            }
        }


        /// <summary>
        /// 当renderMethodList选中项更改时设置renderMethodTab联动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void renderMethodList_SelectedIndexChanged(object sender, EventArgs e)
        {
            renderMethodTab.SelectedIndex = renderMethodList.SelectedIndex;
            if (renderMethodList.SelectedIndex == 1)
            {
                setComboBoxValues();
            }
        }


        /// <summary>
        /// 当renderMethodTab选中项更改时设置renderMethodList联动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void renderMethodTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            renderMethodList.SelectedIndex = renderMethodTab.SelectedIndex;
            if (renderMethodTab.SelectedIndex == 1)
            {
                setComboBoxValues();
            }
        }



        /// <summary>
        /// 刷新“唯一值渲染”Tab的ListView
        /// </summary>
        /// <param name="sField"></param>
        private void UpdateListView(string sField)
        {
            listView1.LargeImageList = imageList1;
            ListViewItem item;
            listView1.Items.Clear();
            m_UVRen = CreateUVRen(sField);
            int vCount = m_UVRen.ValueCount;
            //m_Symbols = new ISymbol[vCount - 1];
            //m_Labels = new string[vCount - 1];

            m_Symbols = new ISymbol[vCount];
            m_Labels = new string[vCount];
            
            imageList1.Images.Clear();

            for (int i = 0; i < vCount; i++)
            {
                string sValue = m_UVRen.get_Value(i);
                switch (((IFeatureLayer)layer).FeatureClass.ShapeType)
                {
                    case esriGeometryType.esriGeometryPoint:
                        IMarkerSymbol pSym;
                        pSym = m_UVRen.get_Symbol(sValue) as IMarkerSymbol;
                        m_Symbols[i] = pSym as ISymbol;
                        m_Labels[i] = m_UVRen.get_Label(sValue);
                        Bitmap b = sym2Bitmap((ISymbol)pSym, 50, 50);
                        imageList1.Images.Add(b);

                        item = new ListViewItem(sValue);
                        item.SubItems.Add(m_Labels[i]);
                        item.ImageIndex = i;
                        listView1.Items.Add(item);
                        break;
                    case esriGeometryType.esriGeometryPolyline:
                        ILineSymbol pSym2;
                        pSym2 = m_UVRen.get_Symbol(sValue) as ILineSymbol;
                        m_Symbols[i] = pSym2 as ISymbol;
                        m_Labels[i] = m_UVRen.get_Label(sValue);
                        Bitmap b2 = sym2Bitmap((ISymbol)pSym2, 50, 50);
                        imageList1.Images.Add(b2);

                        item = new ListViewItem(sValue);
                        item.SubItems.Add(m_Labels[i]);
                        item.ImageIndex = i;
                        listView1.Items.Add(item);
                        break;
                    case esriGeometryType.esriGeometryPolygon:
                        ILineSymbol pSym3;
                        pSym3 = m_UVRen.get_Symbol(sValue) as ILineSymbol;
                        m_Symbols[i] = pSym3 as ISymbol;
                        m_Labels[i] = m_UVRen.get_Label(sValue);
                        Bitmap b3 = sym2Bitmap((ISymbol)pSym3, 50, 50);
                        imageList1.Images.Add(b3);

                        item = new ListViewItem(sValue);
                        item.SubItems.Add(m_Labels[i]);
                        item.ImageIndex = i;
                        listView1.Items.Add(item);
                        break;


                }
                


            }

        }


        /// <summary>
        /// 根据指定字段生成唯一值渲染对象
        /// </summary>
        /// <param name="sField"></param>
        /// <returns></returns>
        private IUniqueValueRenderer CreateUVRen(string sField)
        {
            int nnClasses = 0;
            System.Collections.IEnumerator pEnum = SortTable((IFeatureLayer)layer, sField);
            pEnum.Reset();
            while (pEnum.MoveNext())
            {
                object myObject = pEnum.Current;
                System.Math.Max(System.Threading.Interlocked.Increment(ref nnClasses), nnClasses - 1);
            }
            IColorRamp colorRamp = new RandomColorRampClass();
            colorRamp.Size = nnClasses;
            bool createRamp = true;
            colorRamp.CreateRamp(out createRamp);
            IEnumColors enumColors = colorRamp.Colors;
            enumColors.Reset();
            ISymbol pSym;
            IUniqueValueRenderer pUVRenderer = new UniqueValueRendererClass();
            pUVRenderer.FieldCount = 1;
            pUVRenderer.set_Field(0, sField);
            System.Collections.IEnumerator pEnum2 = SortTable((IFeatureLayer)layer, sField);
            string value;
            object myObj;
            while (pEnum2.MoveNext())
            {
                myObj = pEnum2.Current;
                value = myObj.ToString();

                // 根据要素类型选择合适的符号类别
                switch (((IFeatureLayer)layer).FeatureClass.ShapeType)
                {
                    case esriGeometryType.esriGeometryPoint:
                        pSym = new SimpleMarkerSymbolClass();
                        ((ISimpleMarkerSymbol)pSym).Size = 8;
                        ((ISimpleMarkerSymbol)pSym).Style = esriSimpleMarkerStyle.esriSMSCircle;
                        ((ISimpleMarkerSymbol)pSym).Color = enumColors.Next();
                        ((ISimpleMarkerSymbol)pSym).Outline = true;
                        ((ISimpleMarkerSymbol)pSym).OutlineSize = 0.4;
                        break;

                    case esriGeometryType.esriGeometryPolyline:
                        pSym = new SimpleLineSymbolClass();
                        ((ISimpleLineSymbol)pSym).Width = 1;
                        ((ISimpleLineSymbol)pSym).Color = enumColors.Next();
                        break;

                    case esriGeometryType.esriGeometryPolygon:
                        pSym = new SimpleFillSymbolClass();
                        ((ISimpleFillSymbol)pSym).Outline = new SimpleLineSymbolClass() { Width = 1, Color = enumColors.Next() };
                        ((ISimpleFillSymbol)pSym).Color = enumColors.Next();
                        break;


                    default:
                        pSym = new SimpleMarkerSymbolClass();
                        break;
                }

                pUVRenderer.AddValue(value, value, pSym);
            }
            return pUVRenderer;
        }



        /// <summary>
        /// 对字段进行排序并取得唯一值
        /// </summary>
        /// <param name="pFeatureLayer"></param>
        /// <param name="sFieldName"></param>
        /// <returns></returns>
        private System.Collections.IEnumerator SortTable(IFeatureLayer pFeatureLayer, string sFieldName)
        {
            ITableSort pTablesort = new TableSortClass();

            pTablesort.Fields = sFieldName;
            pTablesort.set_Ascending(sFieldName, true);
            pTablesort.set_CaseSensitive(sFieldName, false);
            pTablesort.Table = pFeatureLayer as ITable;

            pTablesort.Sort(null);
            ICursor pCursor = pTablesort.Rows;
            IDataStatistics pDataStatistics = new DataStatisticsClass();
            pDataStatistics.Field = sFieldName;
            pDataStatistics.Cursor = pCursor;
            return pDataStatistics.UniqueValues;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateListView(comboBox1.SelectedItem.ToString());
        }



        /// <summary>
        /// 设置显示在字段ComboBox中的值
        /// </summary>
        private void setComboBoxValues()
        {
            IField m_field;
            int fieldType;

            this.m_layerfields = layer as ILayerFields;
            for (int i = 0; i < m_layerfields.FieldCount; i++)
            {
                m_field = m_layerfields.get_Field(i);
                fieldType = (int)m_field.Type;
                if (fieldType == 7 || fieldType == 6)//esriFieldType=7表示esriFieldTypeGeometry,6表示OID
                {
                    continue;//不显示shape、OID字段
                }
                comboBox1.Items.Add(m_field.Name);
            }
            if (comboBox1.Items.Count > 0)//设置默认选择ObjectID
            {
                comboBox1.SelectedIndex = 0;
            }
        }


        /// <summary>
        /// “确定”按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {
            if (m_sRen != null) { 
                pLegendClass.Symbol = m_sRen.Symbol; 
            }
            if (renderMethodTab.SelectedTab == uniqueValueTabPage)
            {
                if (m_UVRen != null)
                {
                    geoFeatureLayer.Renderer = (IFeatureRenderer)m_UVRen;
                }
            }
            IActiveView activeView = m_Hookhelper.ActiveView;
            activeView.Refresh();
            this.Close();

        }


        /// <summary>
        /// “取消”按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            IActiveView activeView = m_Hookhelper.ActiveView;
            activeView.Refresh();
            this.Close();
        }


        /// <summary>
        /// “应用”按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void applyButton_Click(object sender, EventArgs e)
        {
            if (m_sRen != null)
            {
                pLegendClass.Symbol = m_sRen.Symbol;
            }
            if (renderMethodTab.SelectedTab == uniqueValueTabPage)
            {
                if (m_UVRen != null)
                {
                    geoFeatureLayer.Renderer = (IFeatureRenderer)m_UVRen;
                }
            }
            IActiveView activeView = m_Hookhelper.ActiveView;
            activeView.Refresh();
        }


        /// <summary>
        /// 符号转图片
        /// </summary>
        /// <param name="sym"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private Bitmap sym2Bitmap(ISymbol sym, int width, int height)
        {
            Bitmap b = new Bitmap(width + 3, height + 3);
            IDisplayTransformation dispTrans = new DisplayTransformationClass();
            tagRECT r = new tagRECT();
            r.left = 0;
            r.top = 0;
            r.bottom = b.Height;
            r.right = b.Width;
            dispTrans.set_DeviceFrame(r);
            IEnvelope bounds = new EnvelopeClass();
            bounds.PutCoords(0, 0, b.Width, b.Height);
            dispTrans.Bounds = bounds;
            IGeometry geom = makeGeometry(sym, bounds);
            Graphics g = Graphics.FromImage(b);
            IntPtr hDC = g.GetHdc();
            sym.SetupDC(hDC.ToInt32(), dispTrans);
            sym.Draw(geom);
            sym.ResetDC();
            g.ReleaseHdc(hDC);
            return b;
        }


        private IGeometry makeGeometry(ISymbol sym, IEnvelope env)
        {

            if (sym is IMarkerSymbol)           //如果符号是点符号
            {
                return ((IArea)env).Centroid;   //返回范围中心点
            }
            else if (sym is ILineSymbol)        //如果符号为线符号
            {
                object missing = Type.Missing;  //创建一个表示缺失值的对象
                IPointCollection pc = new PolylineClass() as IPointCollection;
                pc.AddPoint(env.LowerLeft, missing, missing);
                pc.AddPoint(env.UpperRight, missing, missing);
                return (IGeometry)pc;           // 创建一个折线，包含范围的 LowerLeft 和 UpperRight 两个点
            }
            else if (sym is IFillSymbol)
            {
                ISegmentCollection sc = new PolygonClass();  //创建一个矩形（多边形），其边界由范围定义
                sc.SetRectangle(env);
                return (IGeometry)sc;
            }
            else
            {
                // todo: throw an exception
                return null;
            }

        }
    }
}

