using ESRI.ArcGIS.Carto;
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
    public partial class AttributeForm : Form
    {
        ILayer layer;

        public AttributeForm(ILayer layer)
        {
            InitializeComponent();
            this.layer = layer;
        }


        public void ShowLayerInfo()
        {
            // 显示图层名称
            layerNameBox.Text = layer.Name;

            // 获取几何类型
            if (layer is IFeatureLayer featureLayer)
            {
                IFeatureClass featureClass = featureLayer.FeatureClass;
                if (featureClass != null)
                {
                    // 显示几何类型
                    esriGeometryType geometryType = featureClass.ShapeType;
                    shapeTypeBox.Text = GetGeometryTypeString(geometryType);
                }
            }
        }

        private string GetGeometryTypeString(esriGeometryType geometryType)
        {
            switch (geometryType)
            {
                case esriGeometryType.esriGeometryPoint:
                    return "点";
                case esriGeometryType.esriGeometryPolyline:
                    return "折线";
                case esriGeometryType.esriGeometryPolygon:
                    return "多边形";
                default:
                    return "未知类型";
            }
        }
    }
}
