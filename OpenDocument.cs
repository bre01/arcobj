using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace EX3
{
    
    /// <summary>
    /// Summary description for OpenDocument.
    /// </summary>
    [Guid("d952a1d4-ecbe-4283-abff-8592c1f8b635")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("EX3.OpenDocument")]
    public sealed class OpenDocument : BaseCommand
    {
        AxMapControl mapControl = null;
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
            ControlsCommands.Register(regKey);

        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            ControlsCommands.Unregister(regKey);

        }

        #endregion
        #endregion
        private IHookHelper m_hookHelper = null;
        public OpenDocument(AxMapControl mainMapCtl)
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "geo-spatial.net"; //localizable text
            base.m_caption = "打开地图文档";  //localizable text
            base.m_message = "Create a new map";  //localizable text 
            base.m_toolTip = "Create a new map";  //localizable text 
            base.m_name = base.m_category + "_" + base.Caption;   //unique id, non-localizable (e.g. "MyCategory_MyCommand")
            try
            {
                //
                // TODO: change bitmap name if necessary
                //
                string bitmapResourceName = this.GetType().Name + ".bmp";
                base.m_bitmap = new Bitmap(this.GetType(), bitmapResourceName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
             mapControl= mainMapCtl;
        }
        #region Overridden Class Methods
        /// <summary>
        /// Occurs when this command is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (m_hookHelper == null)
                m_hookHelper = new HookHelperClass();
            m_hookHelper.Hook = hook;
            // TODO:  Add other initialization code
        }
        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add OpenDocument.OnClick implementation
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Map Documents (*.mxd)|*.mxd";
            dlg.Multiselect = false;
            dlg.Title = "Open Map Document";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string docName = dlg.FileName;
                IMapDocument mapDoc = new MapDocumentClass();
                if (mapDoc.get_IsPresent(docName) && !mapDoc.get_IsPasswordProtected(docName))
                {
                    mapDoc.Open(docName, string.Empty);
                    IMap map = mapDoc.get_Map(0);
                    //  m_controlsSynchronizer.ReplaceMap(map);
                    mapControl.Map = map;
                    mapDoc.Close();
                }
                #endregion
            }
        }
    }
}
