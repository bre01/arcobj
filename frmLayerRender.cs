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
        SimpleRendererClass m_sRen;
        UniqueValueRendererClass m_UVRen;
        ISymbol m_Symbols;
        Label m_Labels;
        ILayer layer;
        ILegendClass pLegendClass;


        public frmLayerRender(ILayer layer, ILegendClass pLegendClass)
        {
            InitializeComponent();
            this.layer = layer;
            this.pLegendClass = pLegendClass;

        }

        private void btnBmp_Click(object sender, EventArgs e)
        {
            frmSymbolSelector symbolForm = new frmSymbolSelector(pLegendClass,layer);
            symbolForm.ShowDialog();

            IStyleGalleryItem styleGalleryItem = null;
            //ISymbol symbol = m_sRen.Symbol;

            m_sRen.Symbol = pLegendClass.Symbol;
            //从symbolForm中获取样式
            styleGalleryItem = symbolForm.GetItem(esriSymbologyStyleClass.esriStyleClassMarkerSymbols,
                m_sRen.Symbol);
            m_sRen.Symbol = (ISymbol)styleGalleryItem.Item;

            pLegendClass.Symbol = m_sRen.Symbol;
            //创建新渲染器
            if (styleGalleryItem == null)
            {
                return;
            }
            m_sRen = new SimpleRendererClass();

            //从styleGalleryItem中设置符号
            ISymbol pSym = (ISymbol)styleGalleryItem.Item;
            IMarkerSymbol pMarkSym = (IMarkerSymbol)pSym;

            Bitmap b = symbolForm.Sym2Bitmap(pSym,(int)pMarkSym.Size,(int)pMarkSym.Size);
            btnBmp.Image = (Image)b;

        }


        /// <summary>
        /// 当renderMethodList选中项更改时设置renderMethodTab联动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void renderMethodList_SelectedIndexChanged(object sender, EventArgs e)
        {
            renderMethodTab.SelectedIndex= renderMethodList.SelectedIndex;
        }


        /// <summary>
        /// 当renderMethodTab选中项更改时设置renderMethodList联动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void renderMethodTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            renderMethodList.SelectedIndex = renderMethodTab.SelectedIndex;
        }


        private void UpdateListView(string sField)
        {
            ListViewItem item;
            listView1.Items.Clear();
            m_UVRen = CreateUVRen(sField);
            int vCount = m_UVRen.ValueCount;
            m_Symbols = new ISymbol[vCount - 1] { };
            m_Labels = new string[vCount - 1] { };
            IMarkerSymbol pSym;
            imageList1.Images.Clear();
            for (int i = 0; i < vCount; i++)
            {
                string sValue = m_UVRen.get_Value(i);
                pSym = m_UVRen.get_Symbol(sValue) as IMarkerSymbol;
                m_Symbols(i) = pSym as ISymbol;
                m_Labels(i) = m_UVRen.get_Label(sValue);
                Bitmap b = Sym2Bitmap((ISymbol)pSym, 50, 50);
                imageList1.Images.Add(b);
                item = new ListViewItem(sValue);
                item.ImageIndex = i;
                listView1.Items.Add(item);
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
                IColorRamp colorRamp = new RandomColorRampClass();

                colorRamp.Size = nnClasses;
                bool createRamp;
                colorRamp.CreateRamp(createRamp);

                IEnumColors enumColors = colorRamp.Colors;
                enumColors.Reset();

                ISimpleMarkerSymbol pSym;
                IUniqueValueRenderer pUVRenderer = new UniqueValueRendererClass();
                pUVRenderer.FieldCount = 1;
                pUVRenderer.set_Field(0, sField);

                System.Collections.IEnumerator pEnum2 = SortTable((IFeatureLayer)layer, sField);

                string value;
                object myObj;
                while (pEnum2.MoveNext())
                {
                    pSym = new SimpleMarkerSymbolClass();
                    pSym.Size = 8;
                    pSym.Style = esriSimpleMarkerStyle.esriSMSCircle;
                    pSym.Color = enumColors.Next();

                    pSym.Outline = true;
                    pSym.OutlineSize = 0.4;

                    myObj = pEnum2.Current;
                    value = myObj.ToString();

                    pUVRenderer.AddValue(value, value, (ISymbol)pSym);
                    return pUVRenderer;
                }
            }
        }


        private System.Collections.IEnumerator SortTable(IFeatureLayer pFeatureLayer,string sFieldName)
        {
            ITableSort pTablesort = new TableSortClass();

            pTablesort.Fields = sFieldName;
            pTablesort.set_Ascending(sFieldName, true);
            pTablesort.set_CaseSensitive(sFieldName, false);
            pTablesort.Table = pFeatureLayer as ITable;

            pTablesort.Sort(null);
            ICursor pCorsor = pTablesort.Rows;
            IDataStatistics pDataStatistics = new DataStatisticsClass();
            pDataStatistics.Field = sFieldName;
            pDataStatistics.Cursor = OnParentCursorChanged();
            return pDataStatistics.UniqueValues;
        }
    }
}
