using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EX3
{
    public class LayerTool
    {
        public static List<IFeatureLayer> GetLayers(IMapControlDefault mapControl)
        {
            List<IFeatureLayer> layers = new List<IFeatureLayer>();
            for(int i=0; i < mapControl.LayerCount; i++)
            {
                layers.Add(mapControl.Layer[i] as IFeatureLayer);
            }
            return layers;
        }
    }
}
