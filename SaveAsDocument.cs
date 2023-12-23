using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace EX3
{
    /// <summary>
    /// Summary description for SaveAsDocument.
    /// </summary>
    [Guid("92fef8e4-fe37-4d39-85e0-28befd5b5208")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("EX3.SaveAsDocument")]
    public sealed class SaveAsDocument : BaseCommand
    {
        AxMapControl axMapControl = null;
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

        private IHookHelper m_hookHelper;

        public SaveAsDocument(AxMapControl basicControl)
        {
            // 设置组件显示文本和提示信息
            base.m_caption = "另存为地图文档";
            base.m_message = "另存为地图文档";
            base.m_toolTip = "另存为地图文档";

            // 设置组件名称和类别
            base.m_name = "SaveAsDocument";
            base.m_category = "MyCategory";

            // 设置组件图标
            base.m_bitmap = new Bitmap(this.GetType(), "SaveAsDocument.bmp");
            axMapControl = basicControl;
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
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save Map Document";
            saveFileDialog.Filter = "Map Documents(*.mxd)|*.mxd";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                try
                {
                    // 获取当前地图文档
                    IMxdContents mxdContents = axMapControl.Map as IMxdContents;

                    // 创建一个新的MapDocument对象
                    IMapDocument mapDocument = new MapDocumentClass();
                    mapDocument.New(filePath); // 创建一个新的MXD文件

                    // 将当前地图内容复制到新的MXD
                    mapDocument.ReplaceContents(mxdContents);

                    // 保存新的MXD文件
                    mapDocument.Save(true, true);
                    mapDocument.Close();

                    MessageBox.Show("文档保存成功！");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("保存文档时出现错误：" + ex.Message);
                }
         
            #endregion
            }
        }
    }
}


