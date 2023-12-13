using ESRI.ArcGIS.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EX3
{
    public static class HookTool
    {
        public static IMapControlDefault GetMapControl(IHookHelper helper)
        {
             if (helper==null)
                return null;

            //get the mapControl hook
            object hook = null;
            if (helper.Hook is IToolbarControl2)
            {
                hook = ((IToolbarControl2)helper.Hook).Buddy;
            }
            else
            {
                hook = helper.Hook;
            }
            // TODO: Add OpenCommand.OnClick implementation
            IMapControlDefault mapControl = hook as IMapControlDefault;
            return mapControl;

        }
    }
}
