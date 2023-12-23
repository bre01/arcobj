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
    /// Summary description for ZoomIn.
    /// </summary>
    [Guid("3dc7e83e-8484-463f-aab8-e8d75d3c4940")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("EX3.ZoomIn")]
    public sealed class ZoomIn : BaseTool
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
        //private System.Drawing.Bitmap m_bitmap;
        //private IntPtr m_hBitmap;
        private INewEnvelopeFeedback m_feedBack=null;
        private IPoint m_point=null;
        private Boolean m_isMouseDown=false;
        private IHookHelper m_hookHelper=null;
        private System.Windows.Forms.Cursor m_zoomInCur =null;
        private System.Windows.Forms.Cursor m_moveZoomInCur =null;
        IActiveView pActiveView;
        private IMapControl3 _mapControl;
        //private System.Windows.Forms.Cursor m_moveZoomInCur;

        public ZoomIn()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "Sample_Pan_VBNET/Zoom"; //localizable text
            base.m_caption = "Zoom In";  //localizable text
            base.m_message = "Zooms the Display In By Rectangle or Single Click";  //localizable text 
            base.m_toolTip = "Zoom In";  //localizable text 
            base.m_name = "Sample_Pan/Zoom_Zoom In";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

            try
            {
                //
                // TODO: change bitmap name if necessary
                //
                string bitmapResourceName = this.GetType().Name + ".bmp";
                string[] res = typeof(ZoomIn).Assembly.GetManifestResourceNames();
                if (res.GetLength(0) > 0)
                {
                    base.m_bitmap = new Bitmap(this.GetType(), bitmapResourceName);
                }
                m_hookHelper = new HookHelperClass();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }

        #region Overridden Class Methods
        public override void OnCreate(object hook)
        {
            m_hookHelper.Hook = hook;
            //AxToolbarControl control = (AxToolbarControl)hook;
            
            m_zoomInCur = new System.Windows.Forms.Cursor(typeof(ZoomIn).

                Assembly.GetManifestResourceStream("EX3.ZoomIn.cur"));
            m_moveZoomInCur = new System.Windows.Forms.Cursor(typeof(ZoomIn).
                Assembly.GetManifestResourceStream("EX3.ZoomIn.cur"));
          
            /*var toolbarControl = (IToolbarControl)m_hookHelper;
             _mapControl= (IMapControl3)toolbarControl.Buddy;*/
             /*m_zoomInCur = new System.Windows.Forms.Cursor(GetType().Assembly.GetManifestResourceStream(GetType(), "Ex3.ZoomIn.cur"));
			m_moveZoomInCur = new System.Windows.Forms.Cursor(GetType().Assembly.GetManifestResourceStream(GetType(), "Ex3.MoveZoomIn.cur"));*/
        }
        public override void OnClick()
        {
                            
        }
        public override bool Enabled
        {
            get
            {
                if (m_hookHelper.FocusMap == null) return false;
                return true;
            }
        }
        public override void OnMouseDown(int button, int shift, int x, int y)
		{

            
                m_feedBack = null;
             m_isMouseDown = true;
			if(m_hookHelper.ActiveView == null) return;

			//If the active view is a page layout
			if(m_hookHelper.ActiveView is IPageLayout)
			{
				//Create a point in map coordinates
				IPoint pPoint = (IPoint) m_hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);

				//Get the map if the point is within a data frame
				IMap pMap = m_hookHelper.ActiveView.HitTestMap(pPoint);

				if(pMap == null) return;

				//Set the map to be the page layout's focus map
				if(pMap != m_hookHelper.FocusMap)
				{
					m_hookHelper.ActiveView.FocusMap = pMap;
					m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
				}
			}
			//Create a point in map coordinates
			IActiveView pActiveView = (IActiveView) m_hookHelper.FocusMap;
			m_point = pActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);


		}
        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            if (m_isMouseDown)
            {
                IActiveView pActiveView = (IActiveView) m_hookHelper.FocusMap;
                if (m_feedBack == null)
                {

                    m_feedBack = new NewEnvelopeFeedbackClass();
                    m_feedBack.Display = pActiveView.ScreenDisplay;
                    m_point = pActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
                    m_feedBack.Start(m_point);
                }
                else
                {
                    m_feedBack.MoveTo(pActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y));
                }
            }
        }
        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            if (m_isMouseDown)
            {
                IActiveView pActiveView = (IActiveView) m_hookHelper.FocusMap;
                IEnvelope envelope = null;
                if (m_feedBack != null)
                {
                    envelope = m_feedBack.Stop();
                    if (envelope.Width == 0 || envelope.Height == 0)
                    {
                        m_feedBack = null;
                    }
                }
                if (envelope == null)
                {
                    envelope = pActiveView.Extent;
                    envelope.Expand(0.5, 0.5, true);
                    envelope.CenterAt(m_point);
                }
                pActiveView.Extent = envelope;
                pActiveView.Refresh();
                m_feedBack = null;
                m_isMouseDown = false;
            }
        }
        #endregion
        public override int Cursor
		{
			get
			{
				if(m_isMouseDown)
					return m_moveZoomInCur.Handle.ToInt32();
				else
					return m_zoomInCur.Handle.ToInt32();
			}
		}
    }
}

