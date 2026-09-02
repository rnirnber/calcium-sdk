using System;
using System.Collections.Generic;
using System.Text;

namespace CalciumSDK.Models
{
    public class Line
    {
        public int x { get; set; } = -1;
        public int y { get; set; } = -1;
        public int height { get; set; }
        public int letter { get; set; }
        public int width { get; set; }

        public Line()
        {

        }
        // zero is horizontal, one is vertical
    }
}
