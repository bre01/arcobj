using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace EX3
{
    /// <summary>
    /// Summary description for FullExtent.
    /// </summary>
    [Guid("d0e7754e-959d-41ee-9bcc-f7a77d1b7649")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("EX3.FullExtent")]
    public sealed class FullExtent : BaseTool
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
        private INewEnvelopeFeedback m_feedBack = null;
        private IPoint m_point = null;
        private Boolean m_isMouseDown = false;
        private IHookHelper m_hookHelper = null;
        private System.Windows.Forms.Cursor m_fullextentCur = null;
        IActiveView pActiveView;

        public FullExtent()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "Sample_FullExtent_VBNET"; //localizable text 
            base.m_caption = "显示全图";  //localizable text 
            base.m_message = "Show a Full Extent";  //localizable text
            base.m_toolTip = "Show a Full Extent";  //localizable text
            base.m_name = "FullExtent ";   //unique id, non-localizable (e.g. "MyCategory_MyTool")
            try
            {
                //
                // TODO: change resource name if necessary
                //
                string bitmapResourceName = this.GetType().Name + ".bmp";
                base.m_bitmap = new Bitmap(this.GetType(), bitmapResourceName);
                base.m_cursor = new System.Windows.Forms.Cursor(this.GetType(), this.GetType().Name + ".cur");
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
            m_hookHelper = new HookHelperClass();
            m_hookHelper.Hook = hook;
            m_fullextentCur = new System.Windows.Forms.Cursor(typeof(ZoomIn).
                Assembly.GetManifestResourceStream("EX3.FullExtent.cur"));

            // TODO:  Add FullExtent.OnCreate implementation
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            try
            {
                // 获取地图对象
                IMapControl3 mapControl = m_hookHelper.Hook as IMapControl3;
                if (mapControl != null)
                {
                    // 获取地图的全局范围
                    IEnvelope envelope = mapControl.ActiveView.FullExtent;

                    // 设置当前显示范围为全局范围
                    mapControl.ActiveView.Extent = envelope;

                    // 刷新地图
                    mapControl.ActiveView.Refresh();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
            }
            IActiveView pActiveView = (IActiveView) m_hookHelper.FocusMap;
			
			//Set the extent to the full extent
			pActiveView.Extent = pActiveView.FullExtent;

			//Refresh the active view
			pActiveView.Refresh();
        }
    }
}

        //public override void OnMouseDown(int Button, int Shift, int X, int Y)
        //{
        //    // TODO:  Add FullExtent.OnMouseDown implementation
        //    if (m_isMouseDown)
        //    {
        //        if (m_feedBack == null)
        //        {
        //            m_feedBack = new NewEnvelopeFeedbackClass();
        //            m_feedBack.Display = pActiveView.ScreenDisplay;
        //            m_point = pActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
        //            m_feedBack.Start(m_point);
        //        }
        //        else
        //        {
        //            m_feedBack.MoveTo(pActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y));
        //        }
        //    }
        //}

        //public override void OnMouseMove(int Button, int Shift, int X, int Y)
        //{
        //    // TODO:  Add FullExtent.OnMouseMove implementation
        //    if (m_isMouseDown)
        //    {
        //        if (m_feedBack == null)
        //        {
        //            m_feedBack = new NewEnvelopeFeedbackClass();
        //            m_feedBack.Display = pActiveView.ScreenDisplay;
        //            m_point = pActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
        //            m_feedBack.Start(m_point);
        //        }
        //        else
        //        {
        //            m_feedBack.MoveTo(pActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y));
        //        }
        //    }
        //}

        //public override void OnMouseUp(int Button, int Shift, int X, int Y)
        //{
        //    // TODO:  Add FullExtent.OnMouseUp implementation
        //    if (m_isMouseDown)
        //    {
        //        IEnvelope envelope = null;
        //        if (m_feedBack != null)
        //        {
        //            envelope = m_feedBack.Stop();
        //            if (envelope.Width == 0 || envelope.Height == 0)
        //            {
        //                m_feedBack = null;
        //            }
        //        }
        //        if (envelope == null)
        //        {
        //            envelope = pActiveView.Extent;
        //            envelope.Expand(0.5, 0.5, true);
        //            envelope.CenterAt(m_point);
        //        }
        //        pActiveView.Extent = envelope;
        //        pActiveView.Refresh();
        //        m_feedBack = null;
        //        m_isMouseDown = false;
        //    }
        //}
        #endregion


