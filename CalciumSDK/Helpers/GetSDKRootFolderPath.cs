using System;
using System.Collections.Generic;
using System.Text;

namespace CalciumSDK
{
    public static partial class Helpers
    {
        public static string GET_ROOT_SDK_PATH()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar + "CalciumProjects";
        }
    }
}
