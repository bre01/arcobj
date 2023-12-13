using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
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
    public partial class QueryForm : Form
    {
        IMapControlDefault _mapControl;
        List<IFeatureLayer> _layers;
        IFeatureLayer _selectedLayer;
        IFields _fields;
        esriFieldType _selectedFieldType;
        IField _selectedField;
        public QueryForm(IMapControlDefault mapControl)
        {
            InitializeComponent();
            _mapControl = mapControl;
        }

        private void QueryForm_Load(object sender, EventArgs e)
        {
            _layers = LayerTool.GetLayers(_mapControl);
            foreach(IFeatureLayer layer in _layers)
            {
                this.comboBox1.Items.Add(layer.Name);
            }
            try
            {
                this.comboBox1.SelectedIndex=0;

            }
            catch(Exception exc)
            {
                MessageBox.Show("please add layer first");
                this.Close();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedLayer=_layers[comboBox1.SelectedIndex];
            _fields = _selectedLayer.FeatureClass.Fields;
            for(int i=0; i < _fields.FieldCount; i++)
            {
                IField field = _fields.get_Field(i);
                if (field.Name != "Shape")
                {
                    comboBox2.Items.Add(field.Name);
                }
            }
            comboBox2.SelectedIndex = 0;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBox2.SelectedIndex == 0)
            {
                _selectedField = _fields.get_Field(0);
            }
            else
            {
                _selectedField = _fields.get_Field(comboBox2.SelectedIndex + 1);
            }

            _selectedFieldType = _selectedField.Type;
            if (_selectedFieldType == esriFieldType.esriFieldTypeString)
            {

                List<string> fieldValues =new List<string>();
                int m=_selectedLayer.FeatureClass.FeatureCount(null);
                IFeatureCursor featureCursor = _selectedLayer.FeatureClass.Search(null, false);
                List<IFeature> featureList = new List<IFeature>();
                var currentFeature = featureCursor.NextFeature();
                while (currentFeature != null)
                {
                    featureList.Add(currentFeature);
                    currentFeature = featureCursor.NextFeature();

                }
                for(int i=0;i< featureList.Count ;i++)
                {

                    var feature= featureList[i];
                    var table = feature.Table;

                    var index=table.FindField(_selectedField.Name);
                    var value = feature.get_Value(index);
                    if (fieldValues.Count == 0)
                    {
                        fieldValues.Add(value.ToString());
                    }
                    else
                    {
                        bool contain = false;
                        foreach (string va in fieldValues)
                        {
                            if (value.ToString() == va)
                            {
                                contain = true;
                            }
                        }
                        if (!contain)
                        {
                            fieldValues.Add(value.ToString());
                        }

                    }
                }

                foreach(string value in fieldValues)
                {
                    comboBox4.Items.Add(value);
                }
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            QueryFilter query = new QueryFilter();
            String oper = comboBox3.SelectedItem.ToString();
            if (_selectedFieldType == esriFieldType.esriFieldTypeString)
            {
                if (oper != "=" && oper != "LIKE")
                {
                    MessageBox.Show("Only 'Like' and '=' is applicable for string type field");
                    return;
                }
            }
            else 
            {
                if (oper == "LIKE")
                {
                    MessageBox.Show("can not use like on non String field ");
                    return;
                }
            }
            query.WhereClause = $"\"{_selectedField.Name}\" {oper} '{comboBox4.SelectedItem.ToString()}'";
            var cursor =_selectedLayer.Search(query,false);
            var currentFeature = cursor.NextFeature();

            while (currentFeature != null)
            {
                MessageBox.Show("flash");
                _mapControl.FlashShape(currentFeature.Shape);
            }
        }
    }
}
