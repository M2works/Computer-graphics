using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrafikaKomputerowa1._2.Models
{
    class Polygon
    {
        List<Edge> edges = new List<Edge>();

        public Polygon() { }
        public Polygon(List<Point> vertices)
        {
            for (int i = 0; i < vertices.Count; i++)
                edges.Add(new Edge(vertices[i], vertices[(i + 1)%vertices.Count]));
        }
        public Polygon(List<Edge> edges)
        {
            this.edges = edges.ToList();
        }

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
        public void drawAllEdges(Bitmap Image)
        {
            for (int i = 0; i < Edges.Count; i++)
            {
                Edge bresenhamEdge = new Edge(
                    new Point(Edges[i][0].X,
                    Edges[i][0].Y),
                    new Point(Edges[i][1].X,
                    Edges[i][1].Y));

                bresenhamEdge.Bresenham(Image, Color.Black);
            }
        }
        private int x_intersect(int x1, int y1, int x2, int y2,
                        int x3, int y3, int x4, int y4)
        {
            int num = (x1 * y2 - y1 * x2) * (x3 - x4) -
                      (x1 - x2) * (x3 * y4 - y3 * x4);
            int den = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
            return num / den;
        }
        int y_intersect(int x1, int y1, int x2, int y2,
                        int x3, int y3, int x4, int y4)
        {
            int num = (x1 * y2 - y1 * x2) * (y3 - y4) -
                      (y1 - y2) * (x3 * y4 - y3 * x4);
            int den = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
            return num / den;
        }
        
        private void clip(ref List<Point> poly_points,
                  int x1, int y1, int x2, int y2)
        {
            List<Point> new_points = new List<Point>();
 
            for (int i = 0; i<poly_points.Count; i++)
            {
                int k = (i + 1) % poly_points.Count;
                int ix = poly_points[i].X, iy = poly_points[i].Y;
                int kx = poly_points[k].X, ky = poly_points[k].Y;
                
                int i_pos = (x2 - x1) * (iy - y1) - (y2 - y1) * (ix - x1);
                int k_pos = (x2 - x1) * (ky - y1) - (y2 - y1) * (kx - x1);
 
                // Case 1 : When both points are inside
                if (i_pos< 0  && k_pos< 0)
                {
                    //Only second point is added
                    new_points.Add(new Point(kx, ky));
                }
 
                // Case 2: When only first point is outside
                else if (i_pos >= 0  && k_pos< 0)
                {
                    // Point of intersection with edge
                    // and the second point is added
                    new_points.Add(new Point(
                                      x_intersect(x1, y1, x2, y2, ix, iy, kx, ky),
                                      y_intersect(x1, y1, x2, y2, ix, iy, kx, ky)));
 
                    new_points.Add(new Point(kx, ky));
                }
 
                // Case 3: When only second point is outside
                else if (i_pos< 0  && k_pos >= 0)
                {
                    //Only point of intersection with edge is added
                    new_points.Add(new Point(
                                      x_intersect(x1,y1, x2, y2, ix, iy, kx, ky),
                                      y_intersect(x1,y1, x2, y2, ix, iy, kx, ky)));
                }
            }

            poly_points = new_points.ToList();
        }
        public List<Point> getVertices()
        {
            List<Point> vertices = new List<Point>();
            for (int i = 0; i < Edges.Count; i++)
                vertices.Add(edges[i][0]);

            return vertices;
        }
        
        public Polygon suthHodgClip(Polygon clipper_polygon)
        {
            List<Point> poly_points = getVertices();
            List<Point> clipper_points = clipper_polygon.getVertices();

                for (int i = 0; i < clipper_points.Count; i++)
                {
                    int k = (i + 1) % clipper_points.Count;

                    clip(ref poly_points, clipper_points[i].X,
                         clipper_points[i].Y, clipper_points[k].X,
                         clipper_points[k].Y);
                }

            return new Polygon(poly_points);
        }
        public int [] findPolygonRectangle()
        {
            int[] rectStat = new int[2];

            rectStat[0] = int.MaxValue;
            rectStat[1] = int.MinValue;

            for(int i=0;i<edges.Count;i++)
                if (edges[i][0].Y<rectStat[0])
                    rectStat[0] = edges[i][0].Y;

            for (int i = 0; i < edges.Count; i++)
                if (edges[i][0].Y - rectStat[0] > rectStat[1])
                    rectStat[1] = edges[i][0].Y - rectStat[0];

            return rectStat;

        }
        public bool fillPolygonWithColor(Color fillColor, Bitmap b)
        {
            return fillPolygonAlgorithm(fillColor, null, b, false);
        }

        public void fillPolygonFromTexture(Color[,] polygonImageColorTable, Bitmap b)
        {
            fillPolygonAlgorithm(Color.Empty, polygonImageColorTable, b, true);
        }

        private bool fillPolygonAlgorithm(Color fillColor, Color [,] polygonImageColorTable, Bitmap b, bool isTexture)
        {
            int[] polygonRect = findPolygonRectangle();
            int yMin = polygonRect[0], yMax = yMin + polygonRect[1];

            if (polygonRect[0] == int.MaxValue || polygonRect[1] == int.MinValue)
                return false;

            List<BucketSortModel>[] ET = new List<BucketSortModel>[polygonRect[1] + 1];
            for (int i = 0; i < polygonRect[1] + 1; i++)
                ET[i] = new List<BucketSortModel>();

            for (int i = 0; i < edges.Count; i++)
            {
                int minY, maxY;
                double currX, diffX;
                if (edges[i][0].Y > edges[i][1].Y)
                {
                    minY = edges[i][1].Y;
                    maxY = edges[i][0].Y;
                    currX = edges[i][1].X;
                }
                else
                {
                    minY = edges[i][0].Y;
                    maxY = edges[i][1].Y;
                    currX = edges[i][0].X;
                }

                if (edges[i][0].Y == edges[i][1].Y)
                    diffX = 0;
                else
                    diffX = 1.0 * (edges[i][0].X - edges[i][1].X) / (edges[i][0].Y - edges[i][1].Y);

                ET[minY - polygonRect[0]].Add(new BucketSortModel(maxY, minY, currX, diffX));
            }

            List<BucketSortModel> AET = new List<BucketSortModel>();

            while (yMin < yMax)
            {
                int numberBeforeRemove = AET.Count;
                AET.RemoveAll(x => { return x.MaxY == yMin; });
                int numberAfterRemove = AET.Count;
                IEnumerable<BucketSortModel> conacatenatedList = AET.Concat(ET[yMin - polygonRect[0]].ToList());
                AET = conacatenatedList.ToList();

                if (numberBeforeRemove != numberAfterRemove)
                    AET.Sort((x, y) => { return (int)(x.XStart - y.XStart); });

                if (yMin != polygonRect[0] || yMin != yMax)
                    for (int i = 0; i < AET.Count - 1; i += 2)
                    {

                        if ((int)AET[i].XStart == (int)AET[i + 1].XStart)
                        {
                            if (AET[i].MinY == AET[i + 1].MinY || AET[i].MaxY == AET[i + 1].MaxY)
                                continue;
                            else
                            {
                                i--;
                                continue;
                            }

                        }

                        Edge bresenhamEdge = new Edge(
                            new Point((int)AET[i].XStart + 1, yMin),
                            new Point((int)AET[i + 1].XStart + 1, yMin));

                        if (isTexture)
                            bresenhamEdge.BresenhamFromTexture(b, polygonImageColorTable);
                        else
                            bresenhamEdge.Bresenham(b, fillColor);

                        if (AET[i].MaxY == yMin)
                            AET.Remove(AET[i]);

                        if (AET[i + 1].MaxY == yMin)
                            AET.Remove(AET[i + 1]);
                    }
                yMin++;

                for (int i = 0; i < AET.Count; i++)
                    AET[i].XStart += AET[i].DiffX;
            }

            return true;
        }
        private Point selectMiddlePoint()
        {
            int Xcoordinate = 0, Ycoordinate = 0;

            for(int i=0;i<edges.Count;i++)
            {
                Xcoordinate += edges[i][0].X;
                Ycoordinate += edges[i][0].Y;
            }

            return new Point(Xcoordinate / edges.Count, Ycoordinate / edges.Count);
        }
        public bool isConvex()
        {
            Point middlePoint = selectMiddlePoint();

            for (int i = 0; i < edges.Count; i++)
            {
                double VectorProduct = (edges[i][1].X - edges[i][0].X) * (middlePoint.Y - edges[i][0].Y) - (middlePoint.X - edges[i][0].X) * (edges[i][1].Y - edges[i][0].Y);
                if (VectorProduct > 0)
                    return false;
                if (VectorProduct == 0)
                {
                    int xMax, xMin, yMax, yMin;
                    if (edges[i][0].X > edges[i][1].X)
                    {
                        xMax = edges[i][0].X;
                        xMin = edges[i][1].X;
                    }
                    else
                    {
                        xMax = edges[i][1].X;
                        xMin = edges[i][0].X;
                    }

                    if (edges[i][0].Y > edges[i][1].Y)
                    {
                        yMax = edges[i][0].Y;
                        yMin = edges[i][1].Y;
                    }
                    else
                    {
                        yMax = edges[i][1].Y;
                        yMin = edges[i][0].Y;
                    }

                    if (middlePoint.X > xMax || middlePoint.X < xMin || middlePoint.Y > yMax || middlePoint.Y < yMin)
                        return false;
                }
            }
            return true;
        }
    }
}
