using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrafikaKomputerowa1._2.Models
{
    class BucketSortModel
    {
        int maxY, minY;
        double diffX, xStart;

        public BucketSortModel(int maxY, int minY, double xStart, double diffX)
        {
            MaxY = maxY;
            MinY = minY;
            XStart = xStart;
            DiffX = diffX;
        }

        public int MinY
        {
            get { return minY; }
            set { minY = value; }
        }
        public int MaxY {
            get { return maxY; }
            set { maxY = value; }
        }
        public double XStart {
            get { return xStart; }
            set { xStart = value; }
        }
        public double DiffX {
            get { return diffX; }
            set { diffX = value; }
        }
    }
}
