using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
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
    public partial class FeatureInfo : Form
    {
        IMapControlDefault _mapControl;
        AxMapControl _axMapControl;
        List<IFeatureLayer> _layers;
        IPoint _point;
        IGeometry _bufGeo;
        ISpatialFilter _filter;
        IFeatureLayer _selectedLayer;
        public FeatureInfo(AxMapControl axMap, int x, int y)
        {
            _axMapControl = axMap;
            _mapControl = HookTool.FromAxToMap(axMap);
            _point = _mapControl.ToMapPoint(x, y);
            InitializeComponent();

        }

        private void FeatureInfo_Load(object sender, EventArgs e)
        {
            dataGridView1.ColumnCount = 2;
            dataGridView1.RowHeadersWidth = 60;
            dataGridView1.TopLeftHeaderCell.Value = "序号";
            dataGridView1.Columns[0].HeaderText = "字段";
            dataGridView1.Columns[0].HeaderText = "属性值";
            _layers = LayerTool.GetLayers(_mapControl);
            foreach (IFeatureLayer layer in _layers)
            {
                this.comboBox1.Items.Add(layer.Name);
            }

            _point.SpatialReference = _mapControl.Map.SpatialReference;
            ITopologicalOperator bufferOperator = _point as ITopologicalOperator;
            _bufGeo = bufferOperator.Buffer(_mapControl.ActiveView.Extent.Width / 250);
            _filter = new SpatialFilterClass();
            _filter.Geometry = _bufGeo;
            _filter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;

            try
            {
                this.comboBox1.SelectedIndex = 0;

            }
            catch (Exception exc)
            {
                MessageBox.Show("please add layer first");
                this.Close();
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedLayer = _layers[comboBox1.SelectedIndex];
            var cursor = _selectedLayer.Search(_filter, false);
            var feature = cursor.NextFeature();
            if (feature != null)
            {
                ShowAttribute(feature);
            }
            else
            {
                MessageBox.Show("no feature detected ");
                
            }

        }
        public void ShowAttribute(IFeature feature)
        {
            int num = feature.Fields.FieldCount;
            dataGridView1.RowCount = num;
            int i = 0;
            for (i = 0; i < num; i++)
            {
                dataGridView1.Rows[i].HeaderCell.Value = i.ToString();
                dataGridView1[0, i].Value = feature.Fields.get_Field(i).Name.ToString();
                if (feature.Fields.get_Field(i).Type ==
                esriFieldType.esriFieldTypeGeometry)
                {
                    string type = feature.Shape.GeometryType.ToString();
                    switch (type)
                    {
                        case "esriGeometryPoint":
                            dataGridView1[1, i].Value = "点";
                            break;
                        case "esriGeometryPolyline":
                            dataGridView1[1, i].Value = "线";
                            break;
                        case "esriGeometryPolygon":
                            dataGridView1[1, i].Value = "面";
                            break;
                    }
                }
                else
                {
                    dataGridView1[1, i].Value = feature.Value[i].ToString();
                }
            }
            this.statusStrip1.Text = "查询的要素共有" + feature.Fields.FieldCount.ToString()+ "个字段";
        }
    }
}
