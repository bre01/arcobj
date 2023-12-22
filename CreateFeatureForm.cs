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
        public CreateFeatureForm()
        {
            InitializeComponent();
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

            var dbFullPath =;
            _ws =;

            var layerName =;
            var validatedFields =;

            var feature= _ws.CreateFeatureClass(layerName,validatedFields,ocDesc.InstanceCLSID,);
            CreateFeatureClassWithSR(_ws,layerName,)

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
            geometryDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPoint;
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
            return featureClass;
        }
    }
}
