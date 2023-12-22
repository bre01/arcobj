using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using System.Collections;

namespace EX3
{
    public partial class LoadLayerDlg : Form
    {

        IWorkspaceFactory _workspaceFactory;//创建工作空间工厂
        IFeatureWorkspace _featureWorkspace;
        IFeatureLayer _featureLayer = new FeatureLayerClass();
        IFeatureDataset pFeatureDataset;
        Form1 f = new Form1();
        IMapControl3 _mapControl3;
        IWorkspace _workspace;
        IRasterWorkspaceEx _rasterWorkspace;
        OpenFileDialog openFileDialog;
        int type = 0;

        public LoadLayerDlg(IMapControl3 mapControl)
        {
            _mapControl3 = mapControl;
            InitializeComponent();
        }

        private void LoadLayerDlg_Load(object sender, EventArgs e)
        {

            radioButton2.Checked = true;
            openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "Shapefiles (*.shp)|*.shp|All Files (*.*)|*.*";
            openFileDialog.Multiselect = false;

        }

        private void button1_Click(object sender, EventArgs e)
        {

            _workspaceFactory = new AccessWorkspaceFactoryClass();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

                string FilePath = openFileDialog.FileName;

                _workspace = _workspaceFactory.OpenFromFile(FilePath, 0);
                /*IEnumDataset pEnumDataset =
                _workspace.get_Datasets(ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTAny);*/


                IWorkspaceFactory2 factory = new AccessWorkspaceFactoryClass();
                _rasterWorkspace = (IRasterWorkspaceEx)factory.OpenFromFile(FilePath, 0);


                if (radioButton1.Checked)
                {
                    var geodatasetNames = GeoDatasetNames();
                    addArrayToListBox(geodatasetNames, listBox1);

                }
                else if (radioButton2.Checked)
                {
                    var names = getOutFeatureClassList();
                    addArrayToListBox(names, listBox2);

                }
                /*pEnumDataset.Reset();
                IDataset pDataset = pEnumDataset.Next();
                if (pDataset is IFeatureDataset)
                {
                    pFeatureWorkspace = (IFeatureWorkspace)pWorkspace;
                    //pFeatureWorkspace = 
                    //(IFeatureWorkspace)pAccessWorkspaceFactory.OpenFromFile(path, 0);
                    //pFeatureDataset = pFeatureWorkspace.OpenFeatureDataset(pDataset.Name);
                    IEnumDataset pEnumDataset1 = pDataset.Subsets;
                    pEnumDataset1.Reset();
                    IDataset pDataset1 = pEnumDataset1.Next();
                    if (pDataset1 is IFeatureClass)
                    {
                        pFeatureLayer = new FeatureLayerClass();
                        pFeatureLayer.FeatureClass =
                        pFeatureWorkspace.OpenFeatureClass(pDataset1.Name);
                        pFeatureLayer.Name = pFeatureLayer.FeatureClass.AliasName;
                        _mapControl3.Map.AddLayer(pFeatureLayer);
                        _mapControl3.ActiveView.Refresh();
                    }
                }
                else
                {
                    pFeatureWorkspace = (IFeatureWorkspace)pWorkspace;
                    pFeatureLayer.FeatureClass =
                    pFeatureWorkspace.OpenFeatureClass(pDataset.Name);
                    pFeatureLayer.Name = pFeatureLayer.FeatureClass.AliasName;
                    _mapControl3.Map.AddLayer(pFeatureLayer);
                    _mapControl3.ActiveView.Refresh();
                }*/
            }
        }
        private ArrayList getOutFeatureClassList()
        {
            IEnumDataset enumDataset = _workspace.get_Datasets(ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTAny);
            enumDataset.Reset();
            var dataset = enumDataset.Next();
            ArrayList list = new ArrayList();
            IFeatureClass feature;
            while (dataset != null)
            {
                if (dataset is IFeatureClass)
                {
                    feature = ((IFeatureWorkspace)_workspace).OpenFeatureClass(dataset.Name);
                    list.Add(dataset.Name);

                }
                dataset = enumDataset.Next();

            }
            return list;

        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (_workspace == null)
            {
                return;
            }

            if (radioButton1.Checked)
            {
                type = 0;
                listBox2.Items.Clear();
                var geodatasetNames = GeoDatasetNames();
                addArrayToListBox(geodatasetNames, listBox1);

            }
            else if (radioButton2.Checked)
            {
                type = 0;
                listBox1.Items.Clear();
                var names = getOutFeatureClassList();
                addArrayToListBox(names, listBox2);
            }
            else if (radioButton3.Checked)
            {
                type = 1;
                listBox1.Items.Clear();
                var names = GetRasterNames();
                addArrayToListBox(names, listBox2);
            }
        }
        private ArrayList GetRasterNames()
        {
            IEnumDatasetName enumDatasetName = ((IWorkspace)_rasterWorkspace).get_DatasetNames(esriDatasetType.esriDTRasterDataset);
            ArrayList list = new ArrayList();
            IDatasetName name = enumDatasetName.Next();
            while (name!= null)
            {
                list.Add(name.Name);
                name = enumDatasetName.Next();
            }
            return list;
                
        }
        private ArrayList GeoDatasetNames()
        {
            IEnumDatasetName enumDatasetName = _workspace.DatasetNames[esriDatasetType.esriDTFeatureDataset];
            IDatasetName datasetName = enumDatasetName.Next();
            ArrayList alist = new ArrayList();
            while (datasetName != null)
            {
                alist.Add(datasetName.Name);
                datasetName = enumDatasetName.Next();
            }
            return alist;
        }
        private ArrayList GetFeatureClassListByFeatureDataset(FeatureDataset featureDataset)
        {
            IEnumDataset enumDataset = featureDataset.Subsets;
            IDataset dataset = enumDataset.Next();
            ArrayList list = new ArrayList();
            while (dataset != null)
            {
                if (dataset.Type == esriDatasetType.esriDTFeatureClass)
                {
                    list.Add(dataset.Name);
                    dataset = enumDataset.Next();
                }
            }
            return list;
        }
        private IFeatureDataset getFeatureDatasetByNameFromWorkspace(string featureDatasetName, IWorkspace workspace)
        {
            IFeatureWorkspace featureWorkspace;
            try
            {
                featureWorkspace = (IFeatureWorkspace)workspace;
                IFeatureDataset featureDataset = featureWorkspace.OpenFeatureDataset(featureDatasetName);

                return featureDataset;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        private bool addArrayToListBox(ArrayList list, ListBox box)
        {
            box.Items.Clear();
            try
            {
                for (int i = 0; i < list.Count; i++)
                {
                    box.Items.Add(list[i].ToString());
                }
                return true;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                return false;
            }


        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var index = listBox1.SelectedIndex;
            var selectString = listBox1.SelectedItem.ToString();
            var featureDataset = getFeatureDatasetByNameFromWorkspace(selectString, _workspace);
            var list = GetFeatureClassListByFeatureDataset((FeatureDataset)featureDataset);
            addArrayToListBox(list, listBox2);

        }

        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            var name = listBox2.SelectedItem.ToString();
            if (type == 0)
            {

                IFeatureLayer layer = GetFeatureLayerByFeatureClassName(_workspace, name);
                _mapControl3.AddLayer(layer);
                
            }
            else
            {
                var raster = GetRasterUsingNameFromWorkspace(_rasterWorkspace, name);
                LoadRasterLayer(_mapControl3, raster);
                
            }
        }
        private IFeatureLayer GetFeatureLayerByFeatureClassName(IWorkspace workspace, string featureClassName)
        {
            var layer = new FeatureLayerClass();
            IFeatureClass feature = ((IFeatureWorkspace)workspace).OpenFeatureClass(featureClassName);
            layer.Name = feature.AliasName;
            layer.FeatureClass = feature;
            return layer;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

                string FilePath = openFileDialog.FileName;
                IWorkspaceFactory2 factory = new AccessWorkspaceFactoryClass();
                factory.OpenFromFile(FilePath, 0);
            }
        }
        private IRasterDataset GetRasterUsingNameFromWorkspace(IRasterWorkspaceEx workspace, string datasetName)
        {
            return _rasterWorkspace.OpenRasterDataset(datasetName);
        }
        private void LoadRasterLayer(IMapControl3 map, IRasterDataset data)
        {
            IRasterLayer layer = new RasterLayerClass();
            layer.CreateFromDataset(data);
            map.AddLayer(layer);

        }
    }
}



