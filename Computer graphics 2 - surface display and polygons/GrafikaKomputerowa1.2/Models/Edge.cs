using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrafikaKomputerowa1._2.Models
{
    class Edge
    {
        Point[] vertices = new Point[2];

        public Edge(Point startP, Point endP)
        {
            vertices[0] = startP;
            vertices[1] = endP;
        }
        public Edge(Point [] _vertices)
        {
            vertices = _vertices;
        }

        public Point this[int index]
        {
            get
            {
                return vertices[index];
            }
            set
            {
                vertices[index] = value;
            }
        }

        public double Length()
        {
            return Math.Abs(Math.Sqrt((vertices[0].X - vertices[1].X) * 
                            (vertices[0].X - vertices[1].X) + 
                            (vertices[0].Y - vertices[1].Y) * 
                            (vertices[0].Y - vertices[1].Y)));
        }
        public void BresenhamFromTexture(Bitmap b, Color [,] polygonImageColorTable)
        {
            int deltaX, deltaY, g, h, e;
            int x1 = vertices[0].X, x2 = vertices[1].X, y1 = vertices[0].Y, y2 = vertices[1].Y;
            deltaX = vertices[1].X - vertices[0].X;
            if (deltaX > 0)
                g = 1;
            else
                g = -1;
            deltaX = Math.Abs(deltaX);

            deltaY = vertices[1].Y - vertices[0].Y;
            if (deltaY > 0)
                h = 1;
            else
                h = -1;
            deltaY = Math.Abs(deltaY);

            if (deltaX > deltaY)
            {
                e = -deltaX;
                while (x1 != x2)
                {
                    if (x1 < b.Width && x1 > 0 && y1 < b.Height && y1 > 0)
                        b.SetPixel(x1, y1, polygonImageColorTable[x1,y1]);
                    e += 2 * deltaY;
                    if (e > 0)
                    {
                        y1 += h;
                        e -= 2 * deltaX;
                    }
                    x1 += g;
                }
            }
            else
            {
                e = -deltaY;
                while (y1 != y2)
                {
                    if (x1 < b.Width && x1 > 0 && y1 < b.Height && y1 > 0)
                        b.SetPixel(x1, y1, polygonImageColorTable[x1, y1]);
                    e += 2 * deltaX;
                    if (e > 0)
                    {
                        x1 += g;
                        e -= 2 * deltaY;
                    }
                    y1 += h;
                }
            }
        }
        public void Bresenham(Bitmap b, Color c)
        {
            int deltaX, deltaY, g, h, e;
            int x1 = vertices[0].X, x2 = vertices[1].X, y1 = vertices[0].Y, y2 = vertices[1].Y;
            deltaX = vertices[1].X - vertices[0].X;
            if (deltaX > 0)
                g = 1;
            else
                g = -1;
            deltaX = Math.Abs(deltaX);

            deltaY = vertices[1].Y - vertices[0].Y;
            if (deltaY > 0)
                h = 1;
            else
                h = -1;
            deltaY = Math.Abs(deltaY);

            if (deltaX > deltaY)
            {
                e = -deltaX;
                while (x1 != x2)
                {
                    if (x1 < b.Width && x1 > 0 && y1 < b.Height && y1 > 0)
                        b.SetPixel(x1, y1, c);
                    e += 2 * deltaY;
                    if (e > 0)
                    {
                        y1 += h;
                        e -= 2 * deltaX;
                    }
                    x1 += g;
                }
            }
            else
            {
                e = -deltaY;
                while (y1 != y2)
                {
                    if (x1 < b.Width && x1 > 0 && y1 < b.Height && y1 > 0)
                        b.SetPixel(x1, y1, c);
                    e += 2 * deltaX;
                    if (e > 0)
                    {
                        x1 += g;
                        e -= 2 * deltaY;
                    }
                    y1 += h;
                }
            }
        }
    }
}
