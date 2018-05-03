using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Polygon_Editor.Models
{
    class Edge
    {
        List<Button> buttons = new List<Button>();
        const int SET_THE_BUTTON_IN_THE_VERTEX = 5;
        const int NEAR_RECTANGLE = 3;

        bool isVertical = false;
        bool isHorizontal = false;
        bool isAngleSide = false;


        public Edge(List<Button> l)
        {
            buttons = l;
        }

        public Edge(Button[] _buttons)
        {
            buttons = _buttons.ToList();
        }

        public Edge(Button bn1, Button bn2)
        {
            buttons.Add(bn1);
            buttons.Add(bn2);
        }

        public Button this[int index]
        {
            get
            {
                return buttons[index];
            }
            set
            {
                buttons[index] = value;
            }
        }
        public List<Button> Buttons
        {
            get
            {
                return buttons;
            }
            set
            {
                buttons = value;
            }
        }

        public bool IsVertical
        {
            get
            {
                return isVertical;
            }
            set
            {
                isVertical = value;
            }
        }
        public bool IsHorizontal
        {
            get
            {
                return isHorizontal;
            }
            set
            {
                isHorizontal = value;
            }
        }
        public bool IsAngleSide
        {
            get
            {
                return isAngleSide;
            }
            set
            {
                isAngleSide = value;
            }
        }

        public double lenghtToPoint(Point p)
        {
            int x1 = Buttons[0].Location.X + SET_THE_BUTTON_IN_THE_VERTEX,
                x2 = Buttons[1].Location.X + SET_THE_BUTTON_IN_THE_VERTEX,
                y1 = Buttons[0].Location.Y + SET_THE_BUTTON_IN_THE_VERTEX,
                y2 = Buttons[1].Location.Y + SET_THE_BUTTON_IN_THE_VERTEX;

            double length =
                Math.Abs((y1 - y2) * p.X + (x2 - x1) * p.Y + x1 * y2 - y1 * x2) /
                Math.Sqrt((y1 - y2) * (y1 - y2) + (x2 - x1) * (x2 - x1));

            return length;
        }
        public bool pointNearRectangle(Point p)
        {

            int xMax, xMin, yMax, yMin;
            if (Buttons[0].Location.X > Buttons[1].Location.X)
            {
                xMax = Buttons[0].Location.X + SET_THE_BUTTON_IN_THE_VERTEX;
                xMin = Buttons[1].Location.X + SET_THE_BUTTON_IN_THE_VERTEX;
            }
            else
            {
                xMax = Buttons[1].Location.X + SET_THE_BUTTON_IN_THE_VERTEX;
                xMin = Buttons[0].Location.X + SET_THE_BUTTON_IN_THE_VERTEX;
            }

            if (Buttons[0].Location.Y > Buttons[1].Location.Y)
            {
                yMax = Buttons[0].Location.Y + SET_THE_BUTTON_IN_THE_VERTEX;
                yMin = Buttons[1].Location.Y + SET_THE_BUTTON_IN_THE_VERTEX;
            }
            else
            {
                yMax = Buttons[1].Location.Y + SET_THE_BUTTON_IN_THE_VERTEX;
                yMin = Buttons[0].Location.Y + SET_THE_BUTTON_IN_THE_VERTEX;
            }

            if (p.X <= xMax + NEAR_RECTANGLE && p.X >= xMin - NEAR_RECTANGLE && p.Y >= yMin - NEAR_RECTANGLE && p.Y <= yMax + NEAR_RECTANGLE)
                return true;
            else
                return false;
        }
        public double checkAngleMadeByTheEdge(Button bn1, Button bn2)
        {
            if (Math.Abs(bn1.Location.X - bn2.Location.X) > Math.Abs(bn1.Location.Y - bn2.Location.Y))
                return 0; 
            else
                return 1; 
        }

        static public double Length(Point startingPoint, Point endingPoint)
        {
            return Math.Abs(Math.Sqrt((startingPoint.X - endingPoint.X) * (startingPoint.X - endingPoint.X) + (startingPoint.Y - endingPoint.Y) * (startingPoint.Y - endingPoint.Y)));
        }

        static public void Bresenham(Point p1, Point p2, Bitmap b, Color c)
        {
            int deltaX, deltaY, g, h, e;
            int x1 = p1.X, x2 = p2.X, y1 = p1.Y, y2 = p2.Y;
            deltaX = p2.X - p1.X;
            if (deltaX > 0)
                g = 1;
            else
                g = -1;
            deltaX = Math.Abs(deltaX);

            deltaY = p2.Y - p1.Y;
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

        static public void plot(Bitmap bitmap, double x, double y, double c, Color _color)
        {
            int alpha = (int)(c * 255);
            if (alpha > 255) alpha = 255;
            if (alpha < 0) alpha = 0;
            Color color = Color.FromArgb(alpha, _color);
            if ((int)x < bitmap.Width && (int)x > 0 && (int)y < bitmap.Height && (int)y > 0)
                bitmap.SetPixel((int)x, (int)y, color);
        }
        static int ipart(double x) { return (int)x; }

        static int round(double x) { return ipart(x + 0.5); }

        static double fpart(double x)
        {
            if (x < 0) return (1 - (x - Math.Floor(x)));
            return (x - Math.Floor(x));
        }

        static double rfpart(double x)
        {
            return 1 - fpart(x);
        }


        static public void Xiaolin(Point p1, Point p2, Bitmap bitmap, Color _color)
        {
            double x0 = p1.X, x1 = p2.X, y0 = p1.Y, y1 = p2.Y;
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            double temp;
            if (steep)
            {
                temp = x0; x0 = y0; y0 = temp;
                temp = x1; x1 = y1; y1 = temp;
            }
            if (x0 > x1)
            {
                temp = x0; x0 = x1; x1 = temp;
                temp = y0; y0 = y1; y1 = temp;
            }

            double dx = x1 - x0;
            double dy = y1 - y0;
            double gradient = dy / dx;

            double xEnd = round(x0);
            double yEnd = y0 + gradient * (xEnd - x0);
            double xGap = rfpart(x0 + 0.5);
            double xPixel1 = xEnd;
            double yPixel1 = ipart(yEnd);

            if (steep)
            {
                plot(bitmap, yPixel1, xPixel1, rfpart(yEnd) * xGap, _color);
                plot(bitmap, yPixel1 + 1, xPixel1, fpart(yEnd) * xGap, _color);
            }
            else {
                plot(bitmap, xPixel1, yPixel1, rfpart(yEnd) * xGap, _color);
                plot(bitmap, xPixel1, yPixel1 + 1, fpart(yEnd) * xGap, _color);
            }
            double intery = yEnd + gradient;

            xEnd = round(x1);
            yEnd = y1 + gradient * (xEnd - x1);
            xGap = fpart(x1 + 0.5);
            double xPixel2 = xEnd;
            double yPixel2 = ipart(yEnd);
            if (steep)
            {
                plot(bitmap, yPixel2, xPixel2, rfpart(yEnd) * xGap, _color);
                plot(bitmap, yPixel2 + 1, xPixel2, fpart(yEnd) * xGap, _color);
            }
            else {
                plot(bitmap, xPixel2, yPixel2, rfpart(yEnd) * xGap, _color);
                plot(bitmap, xPixel2, yPixel2 + 1, fpart(yEnd) * xGap, _color);
            }

            if (steep)
            {
                for (int x = (int)(xPixel1 + 1); x <= xPixel2 - 1; x++)
                {
                    plot(bitmap, ipart(intery), x, rfpart(intery), _color);
                    plot(bitmap, ipart(intery) + 1, x, fpart(intery), _color);
                    intery += gradient;
                }
            }
            else {
                for (int x = (int)(xPixel1 + 1); x <= xPixel2 - 1; x++)
                {
                    plot(bitmap, x, ipart(intery), rfpart(intery), _color);
                    plot(bitmap, x, ipart(intery) + 1, fpart(intery), _color);
                    intery += gradient;
                }
            }

        }
        public Point selectMiddlePointOfTheEdge(int buttonSwitch)
        {
            return new Point((this[0].Location.X + this[1].Location.X) / 2 + buttonSwitch,
                        (this[0].Location.Y + this[1].Location.Y) / 2 + buttonSwitch);
        }
    }
}
