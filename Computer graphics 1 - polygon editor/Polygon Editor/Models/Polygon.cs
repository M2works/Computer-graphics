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
    class Polygon
    {
        List<Edge> edges = new List<Edge>();

        public List<Edge> Edges
        {
            get
            {
                return edges;
            }
            set
            {
                edges = value;
            }
        }
        public void drawAllEdges(Bitmap Image, int selectedLength, bool useBresenhamToDraw)
        {
            for (int i = 0; i < Edges.Count; i++)
            {
                if(useBresenhamToDraw)
                    Edge.Bresenham(
                        new Point(Edges[i][0].Location.X + selectedLength,
                        Edges[i][0].Location.Y + selectedLength),
                        new Point(Edges[i][1].Location.X + selectedLength,
                        Edges[i][1].Location.Y + selectedLength),
                        Image, Color.Black);
                else
                    Edge.Xiaolin(
                        new Point(Edges[i][0].Location.X + selectedLength,
                        Edges[i][0].Location.Y + selectedLength),
                        new Point(Edges[i][1].Location.X + selectedLength,
                        Edges[i][1].Location.Y + selectedLength),
                        Image, Color.Black);

            }            
        }
        public void deleteCostraintsForSingleEdge(int numberOfTheEdge)
        {
            changeButtonTagsFromSingleLine(numberOfTheEdge);
            changeButtonTagsFromDoubleLine(numberOfTheEdge);

            Edges[numberOfTheEdge].IsHorizontal = false;
            Edges[numberOfTheEdge].IsVertical = false;
            Edges[numberOfTheEdge].IsAngleSide = false;
        }
        public void changeButtonTagsFromSingleLine(int numberOfTheEdge)
        {
            if (Edges[numberOfTheEdge].IsHorizontal || Edges[numberOfTheEdge].IsVertical)
            {
                for (int i = 0; i < 2; i++)
                {
                    switch ((int)Edges[numberOfTheEdge][i].Tag)
                    {
                        case 0:
                            Edges[numberOfTheEdge][i].Tag = null;
                            break;
                        case 1:
                            Edges[numberOfTheEdge][i].Tag = null;
                            break;
                        case 2:
                            if (Edges[numberOfTheEdge].IsHorizontal)
                                Edges[numberOfTheEdge][i].Tag = 1;
                            else
                                Edges[numberOfTheEdge][i].Tag = 0;
                            break;
                        case 3:
                            Edges[numberOfTheEdge][i].Tag = 6;
                            break;
                        case 4:
                            Edges[numberOfTheEdge][i].Tag = 6;
                            break;
                    }
                }
            }
        }
        private void changeButtonTagsFromDoubleLine(int numberOfTheEdge)
        { 
            if (Edges[numberOfTheEdge].IsAngleSide)
            {
                if((int)Edges[numberOfTheEdge][0].Tag==5)
                {
                    Edges[numberOfTheEdge][0].Tag = null;
                    Edges[numberOfTheEdge][0].Text = null;
                    changeStateOfButton(Edges[numberOfTheEdge][0], Color.Black, Color.White);

                    Edges[(numberOfTheEdge - 1 + Edges.Count) % Edges.Count].IsAngleSide = false;

                    switch ((int)Edges[(numberOfTheEdge - 1 + Edges.Count) % Edges.Count][0].Tag)
                    {
                        case 3:
                            Edges[(numberOfTheEdge - 1 + Edges.Count) % Edges.Count][1].Tag = 0;
                            break;
                        case 4:
                            Edges[(numberOfTheEdge - 1 + Edges.Count) % Edges.Count][1].Tag = 1;
                            break;
                        case 6:
                            if (!Edges[(numberOfTheEdge - 2 + Edges.Count) % Edges.Count].IsAngleSide)
                                Edges[(numberOfTheEdge - 1 + Edges.Count) % Edges.Count][1].Tag = null;
                            break;
                    }

                    switch ((int)Edges[numberOfTheEdge][1].Tag)
                    {
                        case 3:
                            Edges[numberOfTheEdge][1].Tag = 0;
                            break;
                        case 4:
                            Edges[numberOfTheEdge][1].Tag = 1;
                            break;
                        case 6:
                            if(!Edges[(numberOfTheEdge+1)%Edges.Count].IsAngleSide)
                                Edges[numberOfTheEdge][1].Tag = null;
                            break;
                    }

                }
                else
                {
                    Edges[numberOfTheEdge][1].Tag = null;
                    Edges[numberOfTheEdge][1].Text = null;
                    changeStateOfButton(Edges[numberOfTheEdge][1], Color.Black, Color.White);

                    Edges[(numberOfTheEdge + 1) % Edges.Count].IsAngleSide = false;

                    switch ((int)Edges[(numberOfTheEdge + 1) % Edges.Count][1].Tag)
                    {
                        case 3:
                            Edges[(numberOfTheEdge + 1) % Edges.Count][1].Tag = 0;
                            break;
                        case 4:
                            Edges[(numberOfTheEdge + 1) % Edges.Count][1].Tag = 1;
                            break;
                        case 6:
                            if (!Edges[(numberOfTheEdge + 2) % Edges.Count].IsAngleSide)
                                Edges[(numberOfTheEdge + 1) % Edges.Count][1].Tag = null;
                            break;
                    }

                    switch ((int)Edges[numberOfTheEdge][0].Tag)
                    {
                        case 3:
                            Edges[numberOfTheEdge][0].Tag = 0;
                            break;
                        case 4:
                            Edges[numberOfTheEdge][0].Tag = 1;
                            break;
                        case 6:
                            if (!Edges[(numberOfTheEdge - 1 + Edges.Count) % Edges.Count].IsAngleSide)
                                Edges[numberOfTheEdge][0].Tag = null;
                            break;
                    }
                }
                Edges[numberOfTheEdge].IsAngleSide = false;
            }
        }
        private void changeStateOfButton(Button btn, Color backColor, Color borderColor)
        {
            btn.BackColor = backColor;
            btn.ForeColor = backColor;
            btn.FlatAppearance.BorderSize = 2;
            btn.FlatAppearance.BorderColor = borderColor;
        }
        public void drawAllConstraints(Bitmap Image, int selectedLength, int buttonSwitch)
        {
            for (int i = 0; i < Edges.Count; i++)
            {
                Point middlePoint = Edges[i].selectMiddlePointOfTheEdge(buttonSwitch);
                if (Edges[i].IsHorizontal)
                {
                    Edge.Bresenham(
                        new Point(middlePoint.X - selectedLength,
                        middlePoint.Y + selectedLength),
                        new Point(middlePoint.X + selectedLength,
                        middlePoint.Y + selectedLength),
                        Image, Color.ForestGreen);
                }
                else
                {
                    if(Edges[i].IsVertical)
                    {
                        Edge.Bresenham(
                            new Point(middlePoint.X + selectedLength,
                            middlePoint.Y + selectedLength),
                            new Point(middlePoint.X + selectedLength,
                            middlePoint.Y - selectedLength),
                            Image, Color.ForestGreen);
                    }
                }
            }
        }

        public void makeEdgeHorizontal(int number)
        {
            Edges[number][0].Location = new Point(
                   Edges[number][0].Location.X,
                   Edges[number][1].Location.Y);

            Edges[number].IsHorizontal = true;

        }
        public void makeEdgeVertical(int number)
        {
            Edges[number][0].Location = new Point(
                   Edges[number][1].Location.X,
                   Edges[number][0].Location.Y);

            Edges[number].IsVertical = true;
        }

        public bool isPointInside(Point p)
        {

            for(int i=0;i<edges.Count;i++)
            {
                double VectorProduct = (edges[i][1].Location.X - edges[i][0].Location.X) * (p.Y - edges[i][0].Location.Y) - (p.X - edges[i][0].Location.X) * (edges[i][1].Location.Y - edges[i][0].Location.Y);
                if (VectorProduct > 0)
                    return false;
                if(VectorProduct==0)
                {
                    int xMax, xMin,yMax, yMin;
                    if(edges[i][0].Location.X>edges[i][1].Location.X)
                    {
                        xMax = edges[i][0].Location.X;
                        xMin = edges[i][1].Location.X;
                    }
                    else
                    {
                        xMax = edges[i][1].Location.X;
                        xMin = edges[i][0].Location.X;
                    }

                    if (edges[i][0].Location.Y > edges[i][1].Location.Y)
                    {
                        yMax = edges[i][0].Location.Y;
                        yMin = edges[i][1].Location.Y;
                    }
                    else
                    {
                        yMax = edges[i][1].Location.Y;
                        yMin = edges[i][0].Location.Y;
                    }

                    if (p.X > xMax || p.X < xMin || p.Y > yMax || p.Y < yMin)
                        return false;
                }
            }
            return true;
        }
        public void drawOneEdge(int number, Bitmap Image, Color color, int selectedLength, bool useBresenhamToDraw)
        {
            if(useBresenhamToDraw)
                Edge.Bresenham(
                        new Point(Edges[number][0].Location.X + selectedLength,
                        Edges[number][0].Location.Y + selectedLength),
                        new Point(Edges[number][1].Location.X + selectedLength,
                        Edges[number][1].Location.Y + selectedLength),
                        Image, color);
            else
                Edge.Xiaolin(
                        new Point(Edges[number][0].Location.X + selectedLength,
                        Edges[number][0].Location.Y + selectedLength),
                        new Point(Edges[number][1].Location.X + selectedLength,
                        Edges[number][1].Location.Y + selectedLength),
                        Image, color);

        }
    }
}
