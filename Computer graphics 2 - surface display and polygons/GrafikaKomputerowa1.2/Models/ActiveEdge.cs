using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrafikaKomputerowa1._2.Models
{
    class ActiveEdge
    {
        int yEnd, xCurrent;
        double diffX;

        public ActiveEdge(int yEnd, int xCurrent, double diffX)
        {
            this.diffX = diffX;
            this.xCurrent = xCurrent;
            this.yEnd = yEnd;
        }
    }
}
