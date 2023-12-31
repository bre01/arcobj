using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace EX3
{
    /// <summary>
    /// Summary description for EditTool.
    /// </summary>
    [Guid("376ca7f2-f258-4148-8731-bf638f2ef211")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("EX3.EditTool")]
    public sealed class EditTool : BaseTool
    {
        #region COM Registration Function(s)
        [ComRegisterFunction()]
        [ComVisible(false)]
        static void RegisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryRegistration(registerType);

            //
            // TODO: Add any COM registration code here
            //
        }

        [ComUnregisterFunction()]
        [ComVisible(false)]
        static void UnregisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryUnregistration(registerType);

            //
            // TODO: Add any COM unregistration code here
            //
        }

        #region ArcGIS Component Category Registrar generated code
        /// <summary>
        /// Required method for ArcGIS Component Category registration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryRegistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Register(regKey);
            ControlsCommands.Register(regKey);
        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Unregister(regKey);
            ControlsCommands.Unregister(regKey);
        }

        #endregion
        #endregion

        private IHookHelper m_hookHelper = null;
         AxMapControl _ax;
        public EditTool(IWorkspaceEdit edit,IFeatureLayer layer,AxMapControl ax)
        {
            _ax = ax;
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text 
            base.m_caption = "Add Feature";  //localizable text 
            base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
            base.m_toolTip = "";  //localizable text
            base.m_name = "";   //unique id, non-localizable (e.g. "MyCategory_MyTool")
            try
            {
                //
                // TODO: change resource name if necessary
                //
                string bitmapResourceName = GetType().Name + ".bmp";
                base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
                base.m_cursor = new System.Windows.Forms.Cursor(GetType(), GetType().Name + ".cur");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }

        #region Overridden Class Methods

        /// <summary>
        /// Occurs when this tool is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            try
            {
                m_hookHelper = new HookHelperClass();
                m_hookHelper.Hook = hook;
                if (m_hookHelper.ActiveView == null)
                {
                    m_hookHelper = null;
                }
            }
            catch
            {
                m_hookHelper = null;
            }

            if (m_hookHelper == null)
                base.m_enabled = false;
            else
                base.m_enabled = true;

            // TODO:  Add other initialization code
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add EditTool.OnClick implementation
            
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            var editingLayer = EditEnvSingleton.EditingLayer;
            // TODO:  Add EditTool.OnMouseDown implementation
                EditEnvSingleton.EditSpan.StartEditOperation();
                EditEnvSingleton.EditSpan.StartEditing(true);
            var feature = editingLayer.FeatureClass.CreateFeature();
            int indexOfGeometry = editingLayer.FeatureClass.Fields.FindField("geometry");
            int indexOfName = editingLayer.FeatureClass.Fields.FindField("Name");
            feature.set_Value(indexOfName, "testFeature");
            esriGeometryType type = editingLayer.FeatureClass.ShapeType;
            if(type==esriGeometryType.esriGeometryPoint)
            {
                IPoint newPt = _ax.ToMapPoint(X, Y);
                feature.Shape = newPt;
                feature.Store();
            }
            else if(type==esriGeometryType.esriGeometryPolyline)
            {
                IPolyline line = (IPolyline) _ax.TrackLine();
                feature.Shape = line;
                feature.Store();
            }

            else if(type==esriGeometryType.esriGeometryPolygon)
            {
                IPolygon line = (IPolygon) _ax.TrackPolygon();
                feature.Shape = line;
                feature.Store();
            }
            _ax.ActiveView.Refresh();

        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add EditTool.OnMouseMove implementation
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add EditTool.OnMouseUp implementation
        }
        #endregion
    }
}
