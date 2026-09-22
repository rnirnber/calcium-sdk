using System;
using System.Collections.Generic;
using System.Text;

namespace CalciumSDK
{
    public static partial class Helpers
    {
        public static string GetDocsFolder()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Documents");
        }
    }
}
