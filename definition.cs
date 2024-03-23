using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace NudgeGen2
{
    public class definition
    {
        // province;red;green;blue;x;x
        public int provinceID;
        public int red;
        public int green;
        public int blue;
        public string provinceName;
        public string definitionX;
        public List<int> pixelX = new List<int>();
        public List<int> pixelY = new List<int>();

        // expected example 8;1;48;192;Dalaskogen;x
        public static definition loadFromLine(string line)
        {
            definition definition = new definition();
            string[] defValues = line.Split(';');
            definition.provinceID = int.Parse(defValues[0]);
            definition.red = int.Parse(defValues[1]);
            definition.green = int.Parse(defValues[2]);
            definition.blue = int.Parse(defValues[3]);
            definition.provinceName = defValues[4];
            definition.definitionX = defValues[5];

            return definition;
        }
    }
}
