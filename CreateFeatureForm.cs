using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.DataSourcesGDB;
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
    public partial class CreateFeatureForm : Form
    {
        string _db;
        IFeatureWorkspace _ws;
        AxMapControl _axMapControl;
        IFeatureClass _featureClass;
        public CreateFeatureForm(AxMapControl ax,IFeatureWorkspace ws=null)
        {
            InitializeComponent();
            if(ax==null)
            {
                MessageBox.Show("plase add a map with valid spatial reference");
                this.Close();
            }
            _ws = ws;
            _axMapControl = ax;
            comboBox1.Items.Add("string");
            comboBox1.Items.Add("integer");
            comboBox1.Items.Add("doule");
            comboBox1.Items.Add("date");
            
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            var log = new OpenFileDialog();
            if (log.ShowDialog() == DialogResult.OK)
            {
                _db = log.FileName;
                textBox1.Text = _db;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {


            var layerName =textBox2.Text;
            IWorkspaceFactory workspaceFactory = new AccessWorkspaceFactoryClass();

            _ws = (IFeatureWorkspace)workspaceFactory.OpenFromFile(_db, 0);
                
            var spatialR = _axMapControl.SpatialReference;
            CreateFeatureClassWithSR(layerName,_ws, spatialR);
            RefreshGridView();
        }
        public IFeatureClass CreateFeatureClassWithSR(string featureClassName, IFeatureWorkspace featureWorkspace, ISpatialReference spatialReference)
        {
            IFeatureClassDescription fcDescription = new FeatureClassDescriptionClass();
            IObjectClassDescription ocDescription = (IObjectClassDescription)fcDescription;
            IFields fields = ocDescription.RequiredFields;
            IFieldsEdit fieldsEdit = (IFieldsEdit)fields;
            // 添加 Name 字段.
            IField field = new FieldClass();
            IFieldEdit fieldEdit = (IFieldEdit)field;
            
            fieldEdit.Name_2 = "Name";
            fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            fieldsEdit.AddField(field);
            // 找到 Shape 字段，获取 GeometryDef 以设置空间体系
            int shapeFieldIndex = fields.FindField(fcDescription.ShapeFieldName);
            field = fields.Field[shapeFieldIndex]; //或 get_Field(idx)
            IGeometryDef geometryDef = field.GeometryDef;
            IGeometryDefEdit geometryDefEdit = (IGeometryDefEdit)geometryDef;
            if (radioButton1.Checked)
            {

                geometryDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPoint;
            }
            else if (radioButton2.Checked)
            {
                geometryDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolyline;
            }
            else if (radioButton3.Checked)
            {
                geometryDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon; 
            }

            geometryDefEdit.SpatialReference_2 = spatialReference;
            // In this example, only the required fields from the class description are used as fields
            // for the feature class. If additional fields are added, use IFieldChecker to
            // validate them.
            // Use IFieldChecker to create a validated fields collection.
            IFieldChecker fieldChecker = new FieldCheckerClass();
            IEnumFieldError enumFieldError = null;
            IFields validatedFields = null;
            fieldChecker.ValidateWorkspace = (IWorkspace)featureWorkspace;
            fieldChecker.Validate(fields, out enumFieldError, out validatedFields);
            // 在工作区中创建要素类
            IFeatureClass featureClass = featureWorkspace.CreateFeatureClass(featureClassName,
            validatedFields, ocDescription.InstanceCLSID, ocDescription.ClassExtensionCLSID,
            esriFeatureType.esriFTSimple, fcDescription.ShapeFieldName, "");
            _featureClass = featureClass;
            return featureClass;
            
        }






        public void RefreshGridView()
        {
            dataGridView1.Rows.Clear();
            for (int i=0;i< _featureClass.Fields.FieldCount; i++)
            {
                DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                var field = _featureClass.Fields.get_Field(i);
                row.Cells[0].Value = field.Name;
                row.Cells[1].Value = field.Type;
                row.Cells[2].Value = field.Length;
                dataGridView1.Rows.Add(row);
            }
        }

        private void CreateFeatureForm_Load(object sender, EventArgs e)
        {
            dataGridView1.ColumnCount = 4;
            dataGridView1.RowHeadersWidth = 60;
            dataGridView1.TopLeftHeaderCell.Value = "序号";
            dataGridView1.Columns[0].HeaderText = "";
            dataGridView1.Columns[1].HeaderText = "field name";
            dataGridView1.Columns[2].HeaderText = "field type ";
            dataGridView1.Columns[3].HeaderText = "field length";

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //addFeature();
            var field = new Field();
            IFieldEdit fieldEdit = (IFieldEdit)field;


            fieldEdit.Name_2 = textBox3.Text;
            if (comboBox1.Text == "string")
            {
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            }
            else if (comboBox1.Text =="double")
            {
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
            }
            else if (comboBox1.Text =="integer")
            {
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;
            }
            else if (comboBox1.Text =="date")
            {
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeDate;
            }
            fieldEdit.Length_2 = int.Parse(textBox5.Text);
            _featureClass.AddField(fieldEdit);
            RefreshGridView();               




        }
    }
}
