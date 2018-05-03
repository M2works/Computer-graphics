using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Polygon_Editor.Models;


namespace Polygon_Editor
{
    public partial class Form1 : Form
    {
        Polygon p = new Polygon();
        const int DIFF_IN_PIXELS_BETWEEN_START_AND_END = 12;
        const int SET_THE_BUTTON_IN_THE_VERTEX = 5;
        const int CURSOR_NEAR_EDGE = 3;
        const int LENGTH_FROM_THE_EDGE = 5;

        static public int DegreesInAngle=-1;

        //tag to save startingPoint of the first edge
        Point firstPoint = Point.Empty;

        //tag to show readiness to move polygon
        bool readyToMove = false;

        bool useBresenhamToDraw = true;

        //first button
        Button firstButton = new Button();

        //selected button
        Button selectedButton = null;
        Tuple<int, int> pairOfEdges = null;
        bool firstVertexSelected = false;

        //last clicked point
        Point lastClicked = Point.Empty;

        //ends of the created edge
        Point startingPoint=Point.Empty, endingPoint=Point.Empty;

        //tag to indicate creating first edge of the polygon
        bool firstClick = true;

        //tags to indicate the level of advance
        bool polygonStarted = false;
        bool polygonFinished = false;

        // tags to use special functionality on selected edges
        bool canSelectEdge = false;
        bool isEdgeSelected = false;
        int edgeToSelect = -1;
        int firstEdgeSelected = -1, secondEdgeSelected = -1;

        // tags to use special functionality on selected vertices
        bool vertexSelected = false;
        bool vertexReadyToMove = false;  

        Bitmap Image = null;
        Graphics grap = null;

        public Form1()
        {
            InitializeComponent();
            Image = new Bitmap(drawingBox.Width, drawingBox.Height);
            drawingBox.Image = Image;
            grap = Graphics.FromImage(Image);
        }

        private void Panel_MD(object sender, MouseEventArgs e)
        {
            if (polygonFinished)
            {
                if (canSelectEdge)
                {
                    if(e.Button == MouseButtons.Left)
                    {
                    isEdgeSelected = true;
                    p.drawOneEdge(edgeToSelect, Image, Color.Yellow, SET_THE_BUTTON_IN_THE_VERTEX, useBresenhamToDraw);
                    canSelectEdge = false;

                    if(vertexSelected)
                    {
                        changeStateOfSmallButton(selectedButton, Color.Black, Color.White);

                        vertexSelected = false;
                        pairOfEdges = null;
                        selectedButton = null;
                        deleteVertexButton.Enabled = false;
                    }

                        if (firstEdgeSelected == -1)
                        {
                            firstEdgeSelected = edgeToSelect;

                            if (p.Edges[edgeToSelect].checkAngleMadeByTheEdge(p.Edges[edgeToSelect][0], p.Edges[edgeToSelect][1]) == 1 &&
                                !p.Edges[(edgeToSelect + 1) % p.Edges.Count].IsVertical &&
                                !p.Edges[(edgeToSelect - 1 + p.Edges.Count) % p.Edges.Count].IsVertical)
                                verticalButton.Enabled = true;

                            if (p.Edges[edgeToSelect].checkAngleMadeByTheEdge(p.Edges[edgeToSelect][0], p.Edges[edgeToSelect][1]) == 0 &&
                                !p.Edges[(edgeToSelect + 1) % p.Edges.Count].IsHorizontal &&
                                !p.Edges[(edgeToSelect - 1 + p.Edges.Count) % p.Edges.Count].IsHorizontal)
                                horizontalButton.Enabled = true;

                            if (p.Edges[firstEdgeSelected].IsAngleSide != true)
                                deleteConstraintsButton.Enabled = true;

                            if (p.Edges[firstEdgeSelected].IsHorizontal == true)
                                horizontalButton.Enabled = false;

                            if (p.Edges[firstEdgeSelected].IsVertical == true)
                                verticalButton.Enabled = false;
                            
                            addVertexButton.Enabled = true;
                        }
                        else
                        {
                            if (secondEdgeSelected == -1)
                            {
                                secondEdgeSelected = edgeToSelect;
                                angleButton.Enabled = true;

                                verticalButton.Enabled = false;
                                horizontalButton.Enabled = false;
                                addVertexButton.Enabled = false;

                                if (p.Edges[firstEdgeSelected].IsAngleSide == true && p.Edges[secondEdgeSelected].IsAngleSide == true)
                                {
                                    if(((firstEdgeSelected+1)%p.Edges.Count==secondEdgeSelected && (int)p.Edges[firstEdgeSelected][1].Tag==5) ||
                                        ((firstEdgeSelected - 1 + p.Edges.Count) % p.Edges.Count == secondEdgeSelected && (int)p.Edges[firstEdgeSelected][0].Tag == 5))
                                        deleteConstraintsButton.Enabled = true;
                                    else
                                        deleteConstraintsButton.Enabled = false;
                                }
                                else
                                    deleteConstraintsButton.Enabled = false;

                                if ((p.Edges[firstEdgeSelected].IsAngleSide && !p.Edges[secondEdgeSelected].IsAngleSide) ||
                                    (!p.Edges[firstEdgeSelected].IsAngleSide && p.Edges[secondEdgeSelected].IsAngleSide) ||
                                    (p.Edges[firstEdgeSelected][1].Tag!=null && (firstEdgeSelected + 1) % p.Edges.Count == secondEdgeSelected && (int)p.Edges[firstEdgeSelected][1].Tag!=5) ||
                                    (p.Edges[firstEdgeSelected][0].Tag!=null && (firstEdgeSelected - 1 + p.Edges.Count) % p.Edges.Count == secondEdgeSelected && (int)p.Edges[firstEdgeSelected][0].Tag != 5))
                                    angleButton.Enabled = false;
                            }


                        }
                    }
                }
                else
                {
                    if(e.Button==MouseButtons.Right)
                    {
                        if(selectedButton!=null)
                            changeStateOfSmallButton(selectedButton, Color.Black, Color.White);

                        selectedButton = null;
                        pairOfEdges = null;
                        vertexSelected = false;
                        firstButton = new Button();
                        firstVertexSelected = false;
                        
                        firstEdgeSelected = -1;
                        secondEdgeSelected = -1;
                        isEdgeSelected = false;

                        disableAllTSButtons();
                    }
                    else
                    {
                        if (lastClicked == Point.Empty)
                        {
                            lastClicked = e.Location;
                            readyToMove = true;
                        }
                    }
                }
                
            }
            else
            {
                if (firstClick)
                {
                    polygonStarted = true;
                    startingPoint = e.Location;
                    endingPoint = e.Location;
                    firstClick = false;
                    
                    drawingBox.Controls.Add(firstButton);
                    setSmallButton(firstButton, startingPoint);
                }
                else
                {
                    endingPoint = e.Location;

                    if (p.Edges.Count >= 2 && DIFF_IN_PIXELS_BETWEEN_START_AND_END*2 > Edge.Length(p.Edges[0][0].Location, endingPoint))
                    {
                        Edge edge = new Edge(p.Edges[p.Edges.Count - 1][1], p.Edges[0][0]);
                        p.Edges.Add(edge);
                        startingPoint = Point.Empty;
                        polygonFinished = true;
                        grap.Clear(Color.White);
                        p.drawAllEdges(Image, SET_THE_BUTTON_IN_THE_VERTEX, useBresenhamToDraw);
                    }
                    else
                    {
                        Button bn2 = new Button();
                        setSmallButton(bn2, endingPoint);

                        Edge ed = new Edge(firstButton, bn2);
                        p.Edges.Add(ed);
                        startingPoint = new Point(endingPoint.X, endingPoint.Y);
                        firstButton = bn2;

                        if(useBresenhamToDraw)
                            Edge.Bresenham(
                                new Point(ed[0].Location.X + SET_THE_BUTTON_IN_THE_VERTEX,
                                ed[0].Location.Y + SET_THE_BUTTON_IN_THE_VERTEX),
                                new Point(ed[1].Location.X + SET_THE_BUTTON_IN_THE_VERTEX,
                                ed[1].Location.Y + SET_THE_BUTTON_IN_THE_VERTEX),
                                Image, Color.Black);
                        else
                            Edge.Xiaolin(
                                new Point(ed[0].Location.X + SET_THE_BUTTON_IN_THE_VERTEX,
                                ed[0].Location.Y + SET_THE_BUTTON_IN_THE_VERTEX),
                                new Point(ed[1].Location.X + SET_THE_BUTTON_IN_THE_VERTEX,
                                ed[1].Location.Y + SET_THE_BUTTON_IN_THE_VERTEX),
                                Image, Color.Black);
                    }
                }
            }
            drawingBox.Invalidate();
        }
        private void Panel_MU(object sender, MouseEventArgs e)
        {
            lastClicked = Point.Empty;
            readyToMove = false;
        }

        private void Panel_MM(object sender, MouseEventArgs e)
        {
            if (polygonStarted)
            {
                if (polygonFinished)
                {
                    if (readyToMove)
                    {
                        if (lastClicked == Point.Empty)
                        {
                            lastClicked = e.Location;
                        }
                        else
                        {
                            int diffX = e.Location.X - lastClicked.X,
                                diffY = e.Location.Y - lastClicked.Y;

                            lastClicked = e.Location;

                            grap.Clear(Color.White);

                            for (int i = 0; i < p.Edges.Count; i++)
                                p.Edges[i][0].Location = new Point(p.Edges[i][0].Location.X + diffX, p.Edges[i][0].Location.Y + diffY);


                            p.drawAllEdges(Image, SET_THE_BUTTON_IN_THE_VERTEX, useBresenhamToDraw);

                            if (firstEdgeSelected != -1)
                            {
                                p.drawOneEdge(firstEdgeSelected, Image, Color.Yellow, SET_THE_BUTTON_IN_THE_VERTEX, useBresenhamToDraw);
                                if (secondEdgeSelected != -1)
                                {
                                    p.drawOneEdge(secondEdgeSelected, Image, Color.Yellow, SET_THE_BUTTON_IN_THE_VERTEX, useBresenhamToDraw);
                                }
                            }
                        }
                    }
                    else
                    {
                        bool noneIsClose = true;
                        for (int i = 0; i < p.Edges.Count; i++)
                        {
                            if ((p.Edges[i][0].Location.X == p.Edges[i][1].Location.X ||
                                p.Edges[i][0].Location.Y == p.Edges[i][1].Location.Y ||
                                p.Edges[i].lenghtToPoint(e.Location) <= CURSOR_NEAR_EDGE)
                                && p.Edges[i].pointNearRectangle(e.Location))
                            {
                                noneIsClose = false;
                                if (secondEdgeSelected == -1 && i != firstEdgeSelected)
                                {
                                    if (firstEdgeSelected != -1)
                                    {
                                        if (i == (firstEdgeSelected - 1 + p.Edges.Count) % p.Edges.Count ||
                                            i == (firstEdgeSelected + 1) % p.Edges.Count)
                                        {
                                            p.drawOneEdge(i, Image, Color.Red, SET_THE_BUTTON_IN_THE_VERTEX, useBresenhamToDraw);

                                            edgeToSelect = i;
                                            canSelectEdge = true;
                                        }
                                    }
                                    else
                                    {
                                        if (!canSelectEdge)
                                        {
                                            p.drawOneEdge(i, Image, Color.Red, SET_THE_BUTTON_IN_THE_VERTEX, useBresenhamToDraw);

                                            edgeToSelect = i;
                                            canSelectEdge = true;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (i != firstEdgeSelected && i != secondEdgeSelected)
                                {
                                    p.drawOneEdge(i, Image, Color.Black, SET_THE_BUTTON_IN_THE_VERTEX, useBresenhamToDraw);
                                }
                            }
                        }
                        if (noneIsClose)
                            canSelectEdge = false;
                    }
                    p.drawAllConstraints(Image, LENGTH_FROM_THE_EDGE, SET_THE_BUTTON_IN_THE_VERTEX);
                }
                else
                {
                    grap.Clear(Color.White);
                    p.drawAllEdges(Image, SET_THE_BUTTON_IN_THE_VERTEX, useBresenhamToDraw);

                    if(useBresenhamToDraw)
                        Edge.Bresenham(endingPoint, e.Location, Image, Color.Black);
                    else
                        Edge.Xiaolin(endingPoint, e.Location, Image, Color.Black);

                }
                drawingBox.Invalidate();
            }
        }
        
        private void setSmallButton(Button bn , Point p)
        {
            drawingBox.Controls.Add(bn);
            bn.Location = new Point(p.X - SET_THE_BUTTON_IN_THE_VERTEX, p.Y - SET_THE_BUTTON_IN_THE_VERTEX);
            bn.Size = new Size(8, 8);
            bn.BackColor = Color.Black;
            bn.ForeColor = Color.Black;
            bn.MouseEnter += smallbutton_enter;
            bn.MouseLeave += smallbutton_leave;
            bn.MouseDown += smallbutton_MouseDown;
            bn.MouseMove += smallbutton_MouseMove;
            bn.MouseUp += smallbutton_MouseUp;
            bn.FlatStyle = FlatStyle.Flat;
            bn.FlatAppearance.BorderSize = 2;
            bn.FlatAppearance.BorderColor = Color.WhiteSmoke;
            bn.BringToFront();
        }
        private void smallbutton_enter(object sender, EventArgs e)
        {
            canSelectEdge = false;

            if(edgeToSelect!=-1 && secondEdgeSelected==-1)
            {
                if(edgeToSelect!=firstEdgeSelected)
                    p.drawOneEdge(edgeToSelect, Image, Color.Black, SET_THE_BUTTON_IN_THE_VERTEX, useBresenhamToDraw);
            }

            Button bn = (Button)sender;
            if(bn!=selectedButton && bn.FlatAppearance.BorderColor!=Color.ForestGreen)
                changeStateOfSmallButton(bn, Color.White, Color.Black);

            if (!polygonFinished)
            {
                grap.Clear(Color.White);
                p.drawAllEdges(Image, SET_THE_BUTTON_IN_THE_VERTEX, useBresenhamToDraw);

                if(useBresenhamToDraw)
                    Edge.Bresenham(endingPoint, new Point(bn.Location.X + SET_THE_BUTTON_IN_THE_VERTEX,
                    bn.Location.Y + SET_THE_BUTTON_IN_THE_VERTEX), Image, Color.Black);
                else
                    Edge.Xiaolin(endingPoint, new Point(bn.Location.X + SET_THE_BUTTON_IN_THE_VERTEX,
                    bn.Location.Y + SET_THE_BUTTON_IN_THE_VERTEX), Image, Color.Black);

            }
            
            drawingBox.Invalidate();
        }
        private void smallbutton_leave(object sender, EventArgs e)
        {
            
            Button bn = (Button)sender;
            if(bn!= selectedButton && bn.FlatAppearance.BorderColor != Color.ForestGreen)
                changeStateOfSmallButton(bn, Color.Black, Color.White);

            for (int i = 0; i < p.Edges.Count; i++)
            {
                if ((p.Edges[i][0].Location.X == p.Edges[i][1].Location.X ||
                    p.Edges[i][0].Location.Y == p.Edges[i][1].Location.Y ||
                    p.Edges[i].lenghtToPoint(Cursor.Position) <= CURSOR_NEAR_EDGE)
                    && p.Edges[i].pointNearRectangle(Cursor.Position))
                {
                    if (!canSelectEdge)
                    {
                        p.drawOneEdge(i, Image, Color.Red, SET_THE_BUTTON_IN_THE_VERTEX, useBresenhamToDraw);

                        edgeToSelect = i;
                        break;
                    }
                }
            }
        }

        private void smallbutton_MouseDown(object sender, MouseEventArgs e)
        {
            if (polygonFinished)
            {
                vertexReadyToMove = true;
                if (isEdgeSelected)
                {
                    isEdgeSelected = false;
                    p.drawOneEdge(firstEdgeSelected, Image, Color.Black, SET_THE_BUTTON_IN_THE_VERTEX, useBresenhamToDraw);

                    if (secondEdgeSelected != -1)
                        p.drawOneEdge(secondEdgeSelected, Image, Color.Black, SET_THE_BUTTON_IN_THE_VERTEX, useBresenhamToDraw);

                    firstEdgeSelected = -1;
                    secondEdgeSelected = -1;
                }

                Button bn = (Button)sender;

                if (bn == selectedButton)
                {
                    if(!(selectedButton.FlatAppearance.BorderColor==Color.ForestGreen))
                        changeStateOfSmallButton(selectedButton, Color.White, Color.Black);
                    else
                        changeStateOfSmallButton(selectedButton, Color.Yellow, Color.ForestGreen);


                    selectedButton = null;
                    vertexSelected = false;

                    deleteVertexButton.Enabled = false;
                }
                else
                {
                    if (selectedButton != null && !(selectedButton.FlatAppearance.BorderColor == Color.ForestGreen))
                        changeStateOfSmallButton(selectedButton, Color.Black, Color.White);
                    else
                    {
                        if(selectedButton != null && selectedButton.FlatAppearance.BorderColor == Color.ForestGreen)
                            changeStateOfSmallButton(selectedButton, Color.Yellow, Color.ForestGreen);
                    }

                    if(!(bn.FlatAppearance.BorderColor == Color.ForestGreen))
                        changeStateOfSmallButton(bn, Color.Red, Color.Yellow);
                    else
                        changeStateOfSmallButton(bn, Color.Red, Color.ForestGreen);

                    vertexSelected = true;
                    selectedButton = bn;

                    disableAllTSButtons();

                    if (p.Edges.Count > 3)
                        deleteVertexButton.Enabled = true;


                    for (int i = 0; i < p.Edges.Count; i++)
                    {
                        if (selectedButton == p.Edges[i][0])
                        {
                            if (i == 0)
                            {
                                pairOfEdges = new Tuple<int, int>(0, p.Edges.Count - 1);
                                firstVertexSelected = true;
                            }
                            else
                            {
                                pairOfEdges = new Tuple<int, int>(i - 1, i);
                                firstVertexSelected = false;
                            }

                            break;
                        }
                    }
                }
                drawingBox.Invalidate();
            }
        }

        private void smallbutton_MouseMove(object sender, MouseEventArgs e)
        {
            if (vertexReadyToMove)
            {
                Button bn = (Button)sender;

                if(bn.Tag!=null)
                    switch((int)bn.Tag)
                        {
                            case 0:
                                bn.Location = new Point(e.X + bn.Left - SET_THE_BUTTON_IN_THE_VERTEX, bn.Top);
                                break;
                            case 1:
                                bn.Location = new Point(bn.Left,e.Y + bn.Top - SET_THE_BUTTON_IN_THE_VERTEX);
                                break;
                            case 2:
                                break;
                            case 3:
                                bn.Location = new Point(e.X + bn.Left - SET_THE_BUTTON_IN_THE_VERTEX, bn.Top);
                            if (firstVertexSelected)
                            {
                                if (p.Edges[p.Edges.Count - 1][0].Tag != null && (int)p.Edges[p.Edges.Count - 1][0].Tag == 5)
                                    setPositionOfSetAngle(p.Edges.Count - 1, 0,
                                        int.Parse(p.Edges[p.Edges.Count - 1][0].Text));

                                if (p.Edges[0][1].Tag != null && (int)p.Edges[0][1].Tag == 5)
                                    setPositionOfSetAngle(0, 1,
                                    int.Parse(p.Edges[0][1].Text));
                            }
                            else
                            {
                                if (p.Edges[pairOfEdges.Item1][0].Tag != null && (int)p.Edges[pairOfEdges.Item1][0].Tag == 5)
                                    setPositionOfSetAngle(pairOfEdges.Item1, (pairOfEdges.Item1 - 1 + p.Edges.Count) % p.Edges.Count,
                                        int.Parse(p.Edges[pairOfEdges.Item1][0].Text));

                                if (p.Edges[pairOfEdges.Item2][1].Tag != null && (int)p.Edges[pairOfEdges.Item2][1].Tag == 5)
                                    setPositionOfSetAngle(pairOfEdges.Item2, (pairOfEdges.Item2 + 1) % p.Edges.Count,
                                    int.Parse(p.Edges[pairOfEdges.Item2][1].Text));
                            }
                            break;
                            case 4:
                                bn.Location = new Point(bn.Left, e.Y + bn.Top - SET_THE_BUTTON_IN_THE_VERTEX);
                            if (firstVertexSelected)
                            {
                                if (p.Edges[p.Edges.Count - 1][0].Tag != null && (int)p.Edges[p.Edges.Count - 1][0].Tag == 5)
                                    setPositionOfSetAngle(p.Edges.Count - 1, 0,
                                        int.Parse(p.Edges[p.Edges.Count - 1][0].Text));

                                if (p.Edges[0][1].Tag != null && (int)p.Edges[0][1].Tag == 5)
                                    setPositionOfSetAngle(0, 1,
                                    int.Parse(p.Edges[0][1].Text));
                            }
                            else
                            { }
                            if (p.Edges[pairOfEdges.Item1][0].Tag!=null && (int)p.Edges[pairOfEdges.Item1][0].Tag == 5)
                                    setPositionOfSetAngle(pairOfEdges.Item1, (pairOfEdges.Item1 - 1 + p.Edges.Count) % p.Edges.Count,
                                        int.Parse(p.Edges[pairOfEdges.Item1][0].Text));

                                if (p.Edges[pairOfEdges.Item2][1].Tag != null && (int)p.Edges[pairOfEdges.Item2][1].Tag == 5)
                                    setPositionOfSetAngle(pairOfEdges.Item2, (pairOfEdges.Item2 + 1) % p.Edges.Count,
                                    int.Parse(p.Edges[pairOfEdges.Item2][1].Text));
                            break;
                            case 6:
                                bn.Location = new Point(e.X + bn.Left - SET_THE_BUTTON_IN_THE_VERTEX, e.Y + bn.Top - SET_THE_BUTTON_IN_THE_VERTEX);

                            if (firstVertexSelected)
                            {
                                if (p.Edges[p.Edges.Count - 1][0].Tag != null && (int)p.Edges[p.Edges.Count - 1][0].Tag == 5)
                                    setPositionOfSetAngle(p.Edges.Count - 1, 0,
                                        int.Parse(p.Edges[p.Edges.Count - 1][0].Text));

                                if (p.Edges[0][1].Tag != null && (int)p.Edges[0][1].Tag == 5)
                                    setPositionOfSetAngle(0, 1,
                                    int.Parse(p.Edges[0][1].Text));
                            }
                            else
                            {
                                if (p.Edges[pairOfEdges.Item1][0].Tag != null && (int)p.Edges[pairOfEdges.Item1][0].Tag == 5)
                                    setPositionOfSetAngle(pairOfEdges.Item1, (pairOfEdges.Item1 - 1 + p.Edges.Count) % p.Edges.Count,
                                        int.Parse(p.Edges[pairOfEdges.Item1][0].Text));

                                if (p.Edges[pairOfEdges.Item2][1].Tag != null && (int)p.Edges[pairOfEdges.Item2][1].Tag == 5)
                                    setPositionOfSetAngle(pairOfEdges.Item2, (pairOfEdges.Item2 + 1) % p.Edges.Count,
                                    int.Parse(p.Edges[pairOfEdges.Item2][1].Text));
                            }
                            break;

                    }
                else
                    bn.Location = new Point(e.X + bn.Left-SET_THE_BUTTON_IN_THE_VERTEX, e.Y + bn.Top-SET_THE_BUTTON_IN_THE_VERTEX);

                grap.Clear(Color.White);
                p.drawAllEdges(Image, SET_THE_BUTTON_IN_THE_VERTEX, useBresenhamToDraw);
                p.drawAllConstraints(Image, LENGTH_FROM_THE_EDGE, SET_THE_BUTTON_IN_THE_VERTEX);
                drawingBox.Invalidate();
            }
        }

        private void smallbutton_MouseUp(object sender, MouseEventArgs e)
        {
            if(polygonFinished)
                vertexReadyToMove = false;
        }

        private void horizontalButton_Click(object sender, EventArgs e)
        {
            p.makeEdgeHorizontal(firstEdgeSelected);

            horizontalTagSwitch(0);
            horizontalTagSwitch(1);

            firstEdgeSelected = -1;
            isEdgeSelected = false;

            grap.Clear(Color.White);
            p.drawAllEdges(Image, SET_THE_BUTTON_IN_THE_VERTEX, useBresenhamToDraw);
            p.drawAllConstraints(Image, LENGTH_FROM_THE_EDGE, SET_THE_BUTTON_IN_THE_VERTEX);

            disableAllTSButtons();
            drawingBox.Invalidate();
        }

        private void verticalButton_Click(object sender, EventArgs e)
        {
            p.makeEdgeVertical(firstEdgeSelected);

            verticalTagSwitch(0);
            verticalTagSwitch(1);

            firstEdgeSelected = -1;
            isEdgeSelected = false;

            grap.Clear(Color.White);
            p.drawAllEdges(Image, SET_THE_BUTTON_IN_THE_VERTEX, useBresenhamToDraw);
            p.drawAllConstraints(Image, LENGTH_FROM_THE_EDGE, SET_THE_BUTTON_IN_THE_VERTEX);

            disableAllTSButtons();
            drawingBox.Invalidate();
        }
        private void verticalTagSwitch(int numberOfButton)
        {
            if (p.Edges[firstEdgeSelected][numberOfButton].Tag != null)
            {
                if ((int)p.Edges[firstEdgeSelected][numberOfButton].Tag == 0)
                    p.Edges[firstEdgeSelected][numberOfButton].Tag = 2;

                if ((int)p.Edges[firstEdgeSelected][numberOfButton].Tag == 6)
                    p.Edges[firstEdgeSelected][numberOfButton].Tag = 4;
            }
            else
                p.Edges[firstEdgeSelected][numberOfButton].Tag = 1;
        }

        private void horizontalTagSwitch(int numberOfButton)
        {
            if (p.Edges[firstEdgeSelected][numberOfButton].Tag != null)
            {
                if ((int)p.Edges[firstEdgeSelected][numberOfButton].Tag == 1)
                    p.Edges[firstEdgeSelected][numberOfButton].Tag = 2;

                if ((int)p.Edges[firstEdgeSelected][numberOfButton].Tag == 6)
                    p.Edges[firstEdgeSelected][numberOfButton].Tag = 3;

            }
            else
                p.Edges[firstEdgeSelected][numberOfButton].Tag = 0;
        }
        private void addVertexButton_Click(object sender, EventArgs e)
        {
            Button btn = new Button();
            Point pt = new Point((p.Edges[firstEdgeSelected][0].Location.X + p.Edges[firstEdgeSelected][1].Location.X) / 2, (p.Edges[firstEdgeSelected][0].Location.Y + p.Edges[firstEdgeSelected][1].Location.Y) / 2);
            setSmallButton(btn, pt);
            Edge firstEdgeToInsert = new Edge(p.Edges[firstEdgeSelected][0], btn),
                secondEdgeToInsert = new Edge(btn, p.Edges[firstEdgeSelected][1]);

            p.deleteCostraintsForSingleEdge(firstEdgeSelected);

            p.Edges.RemoveAt(firstEdgeSelected);
            p.Edges.Insert(firstEdgeSelected, firstEdgeToInsert);
            p.Edges.Insert(firstEdgeSelected + 1, secondEdgeToInsert);

            firstEdgeSelected = -1;

            isEdgeSelected = false;

            disableAllTSButtons();

            grap.Clear(Color.White);
            p.drawAllEdges(Image, SET_THE_BUTTON_IN_THE_VERTEX, useBresenhamToDraw);
            p.drawAllConstraints(Image, LENGTH_FROM_THE_EDGE, SET_THE_BUTTON_IN_THE_VERTEX);
            drawingBox.Invalidate();
        }

        private void deleteConstraintsButton_Click(object sender, EventArgs e)
        {
            if (secondEdgeSelected != -1)
            {
                p.deleteCostraintsForSingleEdge(firstEdgeSelected);
                p.deleteCostraintsForSingleEdge(secondEdgeSelected);
            }
            else
                p.deleteCostraintsForSingleEdge(firstEdgeSelected);

            firstEdgeSelected = -1;
            secondEdgeSelected = -1;
            isEdgeSelected = false;

            grap.Clear(Color.White);
            p.drawAllEdges(Image, SET_THE_BUTTON_IN_THE_VERTEX, useBresenhamToDraw);
            p.drawAllConstraints(Image, LENGTH_FROM_THE_EDGE, SET_THE_BUTTON_IN_THE_VERTEX);

            disableAllTSButtons();
            drawingBox.Invalidate();
        }

        private void deleteVertexButton_Click(object sender, EventArgs e)
        {
            drawingBox.Controls.Remove(selectedButton);

            if (firstVertexSelected)
            {
                switchEdgesAndRemoveConstraints(0, p.Edges.Count - 1);
                firstVertexSelected = false;
            }
            else
                switchEdgesAndRemoveConstraints(pairOfEdges.Item2, pairOfEdges.Item1);

            grap.Clear(Color.White);
            p.drawAllEdges(Image, SET_THE_BUTTON_IN_THE_VERTEX, useBresenhamToDraw);
            p.drawAllConstraints(Image, LENGTH_FROM_THE_EDGE, SET_THE_BUTTON_IN_THE_VERTEX);

            deleteVertexButton.Enabled = false;

            drawingBox.Invalidate();
        }

        private void switchEdgesAndRemoveConstraints(int firstEdge, int secondEdge)
        {
            p.deleteCostraintsForSingleEdge(firstEdge);
            p.deleteCostraintsForSingleEdge(secondEdge);

            p.Edges[firstEdge][0] = p.Edges[secondEdge][0];
            p.Edges.RemoveAt(secondEdge);
        }

        private void angleButton_Click(object sender, EventArgs e)
        {            
            angleButton.Enabled = false;
            Angle_Form af = new Angle_Form();
            af.ShowDialog();

            if (DegreesInAngle != -1)
            {
                setPositionOfSetAngle(firstEdgeSelected, secondEdgeSelected, DegreesInAngle);

                p.Edges[firstEdgeSelected].IsAngleSide = true;
                p.Edges[secondEdgeSelected].IsAngleSide = true;

                firstEdgeSelected = -1;
                secondEdgeSelected = -1;
                DegreesInAngle = -1;
                isEdgeSelected = false;
                
                grap.Clear(Color.White);
                p.drawAllEdges(Image, SET_THE_BUTTON_IN_THE_VERTEX, useBresenhamToDraw);
                p.drawAllConstraints(Image, LENGTH_FROM_THE_EDGE, SET_THE_BUTTON_IN_THE_VERTEX);
                drawingBox.Invalidate();
            }
        }

        private void setTagsForSetAngle(int firstEdgeNumber, int secondEdgeNumber)
        {
            if (p.Edges[(firstEdgeNumber - 1 + p.Edges.Count) % p.Edges.Count][1].Tag != null)
            {
                if ((int)p.Edges[(firstEdgeNumber - 1 + p.Edges.Count) % p.Edges.Count][1].Tag == 1)
                    p.Edges[firstEdgeNumber][0].Tag = 4;
                else
                {
                    if ((int)p.Edges[(firstEdgeNumber - 1 + p.Edges.Count) % p.Edges.Count][1].Tag == 0)
                        p.Edges[firstEdgeNumber][0].Tag = 3;
                }
            }
            else
                p.Edges[firstEdgeNumber][0].Tag = 6;

            if (p.Edges[(secondEdgeNumber + 1) % p.Edges.Count][0].Tag != null)
            {
                if ((int)p.Edges[(secondEdgeNumber + 1) % p.Edges.Count][0].Tag == 1)
                    p.Edges[secondEdgeNumber][1].Tag = 4;
                else
                {
                    if ((int)p.Edges[(secondEdgeNumber + 1) % p.Edges.Count][0].Tag == 0)
                        p.Edges[secondEdgeNumber][1].Tag = 3;
                }
            }
            else
                p.Edges[secondEdgeNumber][1].Tag = 6;
        }
        
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            polygonStarted = false;
            firstClick = true;
            firstPoint = Point.Empty;

            polygonFinished = false;
            p = new Polygon();

            selectedButton = null;
            pairOfEdges = null;
            vertexSelected = false;
            firstButton = new Button();
            firstVertexSelected = false;

            edgeToSelect = -1;
            firstEdgeSelected = -1;
            secondEdgeSelected = -1;
            isEdgeSelected = false;

            lastClicked = Point.Empty;

            grap.Clear(Color.White);
            drawingBox.Controls.Clear();
            drawingBox.Invalidate();

            disableAllTSButtons();
        }        
        private void disableAllTSButtons()
        {
            addVertexButton.Enabled = false;
            deleteConstraintsButton.Enabled = false;
            deleteVertexButton.Enabled = false;
            verticalButton.Enabled = false;
            horizontalButton.Enabled = false;
            angleButton.Enabled = false;
        }

        private void bresenhamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            useBresenhamToDraw = true;
        }

        private void xiaolinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            useBresenhamToDraw = false;
        }

        private void changeStateOfSmallButton(Button btn, Color backColor, Color borderColor)
        {
            btn.BackColor = backColor;
            btn.ForeColor = backColor;
            btn.FlatAppearance.BorderSize = 2;
            btn.FlatAppearance.BorderColor = borderColor;
        }
        private void setPositionOfSetAngle(int firstEdge, int secondEdge, int DegreesToSet)
        {
            bool isSecondAfterFirst = false;
            double lengthBetweenTwoAngleVertices;
            if (secondEdge == (firstEdge + 1) % p.Edges.Count)
            {
                changeStateOfSmallButton(p.Edges[firstEdge][1], Color.Yellow, Color.ForestGreen);
                lengthBetweenTwoAngleVertices = Edge.Length(
                    p.Edges[firstEdge][0].Location,
                    p.Edges[secondEdge][1].Location);
                isSecondAfterFirst = true;
            }
            else
            {
                changeStateOfSmallButton(p.Edges[firstEdge][0], Color.Yellow, Color.ForestGreen);
                lengthBetweenTwoAngleVertices = Edge.Length(
                    p.Edges[firstEdge][1].Location,
                    p.Edges[secondEdge][0].Location);
            }

            double lengthSquared = (lengthBetweenTwoAngleVertices * lengthBetweenTwoAngleVertices) / (2 - 2 * Math.Cos((double)DegreesToSet / 180 * Math.PI));
            double triangleHeight = Math.Sqrt(lengthSquared - (lengthBetweenTwoAngleVertices / 2) * (lengthBetweenTwoAngleVertices / 2));
            double a1, a2, d;
            double middleEdgeX, middleEdgeY, AnglePointX = -1, AnglePointY = -1;

            if (isSecondAfterFirst &&
                (p.Edges[firstEdge][0].Location.X == p.Edges[secondEdge][1].Location.X ||
                p.Edges[firstEdge][0].Location.Y == p.Edges[secondEdge][1].Location.Y))
            {
                if (p.Edges[firstEdge][0].Location.X == p.Edges[secondEdge][1].Location.X)
                {
                    AnglePointX = p.Edges[firstEdge][0].Location.X + triangleHeight;
                    AnglePointY = (p.Edges[firstEdge][0].Location.Y + p.Edges[secondEdge][1].Location.Y) / 2 + SET_THE_BUTTON_IN_THE_VERTEX;
                }
                else
                {
                    AnglePointX = (p.Edges[firstEdge][0].Location.Y + p.Edges[secondEdge][1].Location.Y) / 2 + SET_THE_BUTTON_IN_THE_VERTEX;
                    AnglePointY = p.Edges[firstEdge][0].Location.Y + triangleHeight;
                }
            }
            else
            {
                if (!isSecondAfterFirst &&
                (p.Edges[firstEdge][1].Location.X == p.Edges[secondEdge][0].Location.X ||
                p.Edges[firstEdge][1].Location.Y == p.Edges[secondEdge][0].Location.Y))
                {
                    if (p.Edges[firstEdge][1].Location.X == p.Edges[secondEdge][0].Location.X)
                    {
                        AnglePointX = p.Edges[firstEdge][1].Location.X + triangleHeight;
                        AnglePointY = (p.Edges[firstEdge][1].Location.Y + p.Edges[secondEdge][0].Location.Y) / 2 + SET_THE_BUTTON_IN_THE_VERTEX;
                    }
                    else
                    {
                        AnglePointX = (p.Edges[firstEdge][1].Location.Y + p.Edges[secondEdge][0].Location.Y) / 2 + SET_THE_BUTTON_IN_THE_VERTEX;
                        AnglePointY = p.Edges[firstEdge][1].Location.Y + triangleHeight;
                    }
                }
                else
                {
                    if (isSecondAfterFirst)
                    {
                        middleEdgeX = (p.Edges[firstEdge][0].Location.X + p.Edges[secondEdge][1].Location.X) / 2 + SET_THE_BUTTON_IN_THE_VERTEX;
                        middleEdgeY = (p.Edges[firstEdge][0].Location.Y + p.Edges[secondEdge][1].Location.Y) / 2 + SET_THE_BUTTON_IN_THE_VERTEX;

                        a1 = (double)(p.Edges[firstEdge][0].Location.Y - p.Edges[secondEdge][1].Location.Y) /
                            (double)(p.Edges[secondEdge][1].Location.X - p.Edges[firstEdge][0].Location.X);
                    }
                    else
                    {
                        middleEdgeX = (p.Edges[firstEdge][1].Location.X + p.Edges[secondEdge][0].Location.X) / 2 + SET_THE_BUTTON_IN_THE_VERTEX;
                        middleEdgeY = (p.Edges[firstEdge][1].Location.Y + p.Edges[secondEdge][0].Location.Y) / 2 + SET_THE_BUTTON_IN_THE_VERTEX;

                        a1 = (double)(p.Edges[firstEdge][1].Location.Y - p.Edges[secondEdge][0].Location.Y) /
                            (double)(p.Edges[secondEdge][0].Location.X - p.Edges[firstEdge][1].Location.X);
                    }

                    a2 = 1 / a1;
                    d = middleEdgeY - a2 * middleEdgeX;

                    double a = 1 + a2 * a2,
                            b = -2 * middleEdgeX + 2 * d * a2 - 2 * middleEdgeY * a2,
                            c = middleEdgeX * middleEdgeX + d * d - 2 * middleEdgeY * d + middleEdgeY * middleEdgeY - triangleHeight * triangleHeight;

                    double delta = b * b - 4 * a * c;

                    if (delta < 0)
                        MessageBox.Show("Can't set that angle!");
                    else
                    {
                        AnglePointX = (-b - Math.Sqrt(delta)) / (2 * a);
                        AnglePointY = a2 * AnglePointX + d;
                    }
                }
            }

            if (isSecondAfterFirst)
            {
                p.Edges[firstEdge][1].Location = new Point((int)AnglePointX, (int)AnglePointY);
                setTagsForSetAngle(firstEdge, secondEdge);
                p.Edges[firstEdge][1].Tag = 5;
                p.Edges[firstEdge][1].Text = DegreesToSet.ToString();
            }
            else
            {
                p.Edges[firstEdge][0].Location = new Point((int)AnglePointX, (int)AnglePointY);
                setTagsForSetAngle(secondEdge, firstEdge);
                p.Edges[secondEdge][1].Tag = 5;
                p.Edges[secondEdge][1].Text = DegreesToSet.ToString();
            }
        }
    }
}
