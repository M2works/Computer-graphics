using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrafikaKomputerowa1._2
{
    using GrafikaKomputerowa1._2.Models;
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            initializeLightVector();
            mainBitmap = new Bitmap(screenPB.Width, screenPB.Height);
            screenPB.Image = mainBitmap;
            grap = Graphics.FromImage(mainBitmap);
            initializeColorTables();
            initializeNormalVectors();
            initializeCoses();
            initializeLightTable();
            normalizeLightTable();
            initializePolygons();            
        }

        #region initFunctions

        private void initializePolygons()
        {
            p = new Polygon(initializePolygon());
            staticPolygon = new Polygon(initializeStaticPolygon());
            setInitializationFlags();
            fillPolygonsFromTextureIfFinished();
        }

        private void setInitializationFlags()
        {
            polygonStarted = true;
            polygonFinished = true;
            staticPolygonStarted = true;
            staticPolygonFinished = true;

            firstClick = false;
        }

        private List<Point> initializePolygon()
        {
            return new List<Point>()
            {
                new Point(10, 10),
                new Point(120,400),
                new Point(370,500),
                new Point(500,50)
            };
        }

        private List<Point> initializeStaticPolygon()
        {
            return new List<Point>()
            {
                new Point(50, 300),
                new Point(100,400),
                new Point(300,450),
                new Point(370,300)
            };
        }

        private void initializeNormalVectors()
        {
            normalVectors = new double[screenPB.Width, screenPB.Height][];
            for (int i = 0; i < normalVectors.GetLength(0); i++)
                for (int j = 0; j < normalVectors.GetLength(1); j++)
                {
                    normalVectors[i, j] = new double[VECTOR_PARTS];
                    normalVectors[i, j][2] = 1;
                }
        }
        private void initializeCoses()
        {
            cos = new double[screenPB.Width, screenPB.Height];
        }
        private void initializeImageColorTable()
        {
            currentImageColorTable = new Color[screenPB.Width, screenPB.Height];
            for (int i = 0; i < currentImageColorTable.GetLength(0); i++)
                for (int j = 0; j < currentImageColorTable.GetLength(1); j++)
                    currentImageColorTable[i, j] = textureImageColorTable[i, j];
        }
        private void initializeColorTables()
        {
            initializeColorTable(polygonImage, setObjectColorPB, ref textureImageColorTable, startingTextureFilePath);
            initializeColorTable(HeightMap, setDisturbancePB, ref  heightMapColorTable, startingHMFilePath);
            initializeColorTable(NormalMap, setNormalVectorPB, ref normalMapColorTable, startingNMFilePath);
            currentImageColorTable = new Color[screenPB.Width, screenPB.Height];
            initializeImageColorTable();
        }

        private void initializeColorTable(Bitmap image, PictureBox pb, ref Color[,] colorTable, string path)
        {
            image = new Bitmap(Image.FromFile(path), new Size(screenPB.Width, screenPB.Height));
            pb.BackgroundImage = new Bitmap(Image.FromFile(path), new Size(setObjectColorPB.Width, setObjectColorPB.Height));
            colorTable = new Color[screenPB.Width, screenPB.Height];
            copyImageToTableImage(colorTable, image);
        }
        private void initializeLightVector()
        {
            lightVector[0] = 0;
            lightVector[1] = 0;
            lightVector[2] = 1;
        }
        private void initializeLightTable()
        {
            lightTable[0] = new double [] { -0.9,0.5, 1 };
            lightTable[1] = new double[] { -0.7, 0.5, 1 };
            lightTable[2] = new double[] { -0.5, 0.5, 1 };
            lightTable[3] = new double[] { -0.3, 0.5, 1 };
            lightTable[4] = new double[] { -0.1, 0.5, 1 };
            lightTable[5] = new double[] { 0.1, 0.5, 1 };
            lightTable[6] = new double[] { 0.3, 0.5, 1 };
            lightTable[7] = new double[] { 0.5, 0.5, 1 };
            lightTable[8] = new double[] { 0.7, 0.5, 1 };
            lightTable[9] = new double[] { 0.9, 0.5, 1 };

        }

        private void normalizeLightTable()
        {
            for (int i = 0; i < LIGHT_ELEMENTS; i++)
            {
                double VectorLength;
                double SubVectorLength = Math.Sqrt(
                    lightTable[i][0] * lightTable[i][0] +
                    lightTable[i][1] * lightTable[i][1]);

                VectorLength = Math.Sqrt(
                    SubVectorLength * SubVectorLength +
                    lightTable[i][2] * lightTable[i][2]);

                for (int j = 0; j < VECTOR_PARTS; j++)
                    lightTable[i][j] /= VectorLength;
            }
        }

        #endregion

        const int VECTOR_PARTS = 3;
        const int CLOSE_TO_THE_VERTEX = 10;
        const int LIGHT_ELEMENTS = 10;

        #region convertingTables

        double [,] cos;

        #endregion

        #region Menu variables

        Color lightColor = Color.White;

        string startingTextureFilePath = "..\\..\\Maps\\Texture3.jpg";
        string startingHMFilePath = "..\\..\\Maps\\HM3.tiff";
        string startingNMFilePath = "..\\..\\Maps\\NM3.jpg";

        bool fixedObjectColor = false;
        bool fixedLightVector = true;
        bool noDisturbance = true;
        bool fixedNormalVector = true;

        Color polygonColor = Color.White;

        Bitmap polygonImage;
        Bitmap HeightMap;
        Bitmap NormalMap;

        Color [,] textureImageColorTable;
        Color [,] heightMapColorTable;
        Color [,] normalMapColorTable;
        Color [,] currentImageColorTable;
        
        double [,][] normalVectors;

        double [] VectorV = new double[VECTOR_PARTS]{ 0,0,1 };

        double[] lightVector = new double[VECTOR_PARTS];
        double lightVectorRadius;

        int tickCounter;

        double[][] lightTable = new double[LIGHT_ELEMENTS][];
        

        #endregion

        #region Screen variables

        Polygon intersection;
        Polygon staticPolygon = new Polygon();
        Polygon p = new Polygon();

        const int DIFF_IN_PIXELS_BETWEEN_START_AND_END = 12;
        const int SET_THE_BUTTON_IN_THE_VERTEX = 5;

        //tag to show readiness to move polygon
        bool readyToMove = false;
                
        //last clicked point
        Point lastClicked = Point.Empty;

        bool vertexSelected = false;
        int numberOfVertexSelected = -1;

        //ends of the created edge
        Point startingPoint = Point.Empty, endingPoint = Point.Empty;

        //tag to indicate creating first edge of the polygon
        bool firstClick = true;

        double Ks=1.0, Kd = 1.0 , _f=1, mFactor=1;

        //tags to indicate the level of advance
        bool polygonStarted = false;
        bool staticPolygonStarted = false;

        bool polygonFinished = false;
        bool staticPolygonFinished = false;
                
        Bitmap mainBitmap;
        Graphics grap;

        #endregion

        #region Menu Buttons' Functions
        private void lightSourceButton_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.Color = lightSourcePB.BackColor;

            if (cd.ShowDialog() == DialogResult.OK)
            {
                lightSourcePB.BackColor = cd.Color;
                lightColor = cd.Color;

                for (int i = 0; i < screenPB.Width; i++)
                    for (int j = 0; j < screenPB.Height; j++)
                        cos[i, j] = countCos(normalVectors[i, j]);

                for (int i = 0; i < screenPB.Width; i++)
                    for (int j = 0; j < screenPB.Height; j++)
                    {
                        double[] colorsPerPixel = new double[VECTOR_PARTS];
                        colorsPerPixel = setColorsToPixels(i, j);
                        currentImageColorTable[i, j] = Color.FromArgb((int)colorsPerPixel[0], (int)colorsPerPixel[1], (int)colorsPerPixel[2]);
                    }

                if(!fixedObjectColor)
                    fillPolygonsFromTextureIfFinished();
            }
        }

        private void fixedObjectColorButton_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.Color = pictfixedObjectColorPB.BackColor;

            if (cd.ShowDialog() == DialogResult.OK)
            {
                pictfixedObjectColorPB.BackColor = cd.Color;
                polygonColor = cd.Color;

                drawPolygonsIfFinished();
            }
        }

        private void drawPolygonsIfFinished()
        {
            if (staticPolygonFinished)
            {
                grap.Clear(Color.White);
                staticPolygon.fillPolygonWithColor(polygonColor, mainBitmap);

                if (polygonFinished)
                {
                    p.fillPolygonWithColor(polygonColor, mainBitmap);
                }
                redrawEdgesOfPolygons();
                screenPB.Invalidate();
            }
        }
        
        private void fixedObjectColorRB_CheckedChanged(object sender, EventArgs e)
        {
            if(fixedObjectColorRB.Checked)
            {
                switchRadioButtonsAndButton(
                    fixedObjectColorRB, setObjectColorRB, fixedObjectColorButton, true);

                fixedObjectColor = true;
                lightTimer.Stop();

                disableButtonsForFixedColor();

                drawPolygonsIfFinished();
            }

        }
        private void disableButtonsForFixedColor()
        {
            fixedLightVectorRB.Enabled = false;
            setLightVectorRB.Enabled = false;

            noDisturbanceRB.Enabled = false;
            setDisturbanceRB.Enabled = false;
            setDisturbanceButton.Enabled = false;

            fixedNormalVectorRB.Enabled = false;
            setNormalVectorRB.Enabled = false;
            setNormalVectorButton.Enabled = false;
        }

        private void setObjectColorRB_CheckedChanged(object sender, EventArgs e)
        {
            if(setObjectColorRB.Checked)
            {
                switchRadioButtonsAndButton(
                    setObjectColorRB, fixedObjectColorRB, fixedObjectColorButton, false);

                fixedObjectColor = false;
                enableSelectedButtonsForTexture();

                checkTimerSetting();

                fillPolygonsFromTextureIfFinished();
            }
        }

        private void fillPolygonsFromTextureIfFinished()
        {
            if (staticPolygonFinished)
            {
                grap.Clear(Color.White);
                staticPolygon.fillPolygonFromTexture(currentImageColorTable, mainBitmap);
                if (polygonFinished)
                {
                    p.fillPolygonFromTexture(currentImageColorTable, mainBitmap);
                }
                redrawEdgesOfPolygons();
                screenPB.Invalidate();
            }
        }

        private void enableSelectedButtonsForTexture()
        {
            fixedLightVectorRB.Enabled = true;
            setLightVectorRB.Enabled = true;

            enableRadioButtonsAndCheckButton(noDisturbanceRB, setDisturbanceRB, setDisturbanceButton);
            enableRadioButtonsAndCheckButton(fixedNormalVectorRB, setNormalVectorRB, setNormalVectorButton);
        }
        private void checkTimerSetting()
        {
            if (!fixedLightVector)
                lightTimer.Start();
        }
        private void enableRadioButtonsAndCheckButton(RadioButton rbToCheck, RadioButton rbToEnable, Button bn)
        {
            rbToCheck.Enabled = true;
            rbToEnable.Enabled = true;

            if (!rbToCheck.Checked)
                bn.Enabled = true;
        }

        private void switchRadioButtonsAndButton(
                    RadioButton RbToCheck, RadioButton RbToUncheck, Button bn, bool enableState)
        {
            RbToCheck.Checked = true;
            RbToUncheck.Checked = false;

            bn.Enabled = enableState;
        }

        private void fixedLightVectorRB_CheckedChanged(object sender, EventArgs e)
        {
            if(fixedLightVectorRB.Checked == true)
            {
                lightTimer.Stop();

                initializeLightVector();
                setLightVectorRB.Checked = false;
                fixedLightVector = true;

                if (fixedNormalVector)
                    setBaseNormalVectors();

                if (noDisturbance)
                    convertImageWithNM();
                else
                    convertImageWithNMAndHM();

                fillPolygonsFromTextureIfFinished();

            }
        }        

        private void setLightVectorRB_CheckedChanged(object sender, EventArgs e)
        {
            if(setLightVectorRB.Checked == true)
            {
                lightTimer.Start();
                fixedLightVectorRB.Checked = false;
                setLightVectorRB.Checked = true;
                fixedLightVector = false;


            }
        }

        private void noDisturbanceRB_CheckedChanged(object sender, EventArgs e)
        {
            if(noDisturbanceRB.Checked)
            {
                switchRadioButtonsAndButton(noDisturbanceRB, setDisturbanceRB, setDisturbanceButton, false);
                noDisturbance = true;

                if(fixedNormalVector)
                    setBaseNormalVectors();

                convertImageWithNM();

                fillPolygonsFromTextureIfFinished();
            }
        }

        private void setDisturbanceRB_CheckedChanged(object sender, EventArgs e)
        {
            if(setDisturbanceRB.Checked)
            {
                switchRadioButtonsAndButton(setDisturbanceRB, noDisturbanceRB, setDisturbanceButton, true);
                noDisturbance = false;

                convertImageWithNMAndHM();
                fillPolygonsFromTextureIfFinished();
            }
        }

        private void setDisturbanceButton_Click(object sender, EventArgs e)
        {
            if(selectPictureToSet(setDisturbancePB, HeightMap, ref heightMapColorTable))
            {
                if (fixedNormalVector)
                    setBaseNormalVectors();
                
                convertImageWithNMAndHM();
                fillPolygonsFromTextureIfFinished();
            }
        }


        private void fixedNormalVectorRB_CheckedChanged(object sender, EventArgs e)
        {
            if(fixedNormalVectorRB.Checked)
            {
                switchRadioButtonsAndButton(fixedNormalVectorRB, setNormalVectorRB, setNormalVectorButton, false);
                fixedNormalVector = true;

                setBaseNormalVectors();

                if(noDisturbance)
                    convertImageWithNM();
                else
                    convertImageWithNMAndHM();

                fillPolygonsFromTextureIfFinished();
            }
        }
        private void setBaseNormalVectors()
        {
            for (int i = 0; i < screenPB.Width; i++)
                for (int j = 0; j < screenPB.Height; j++)
                    initializeNormalVector(i, j);
        }
            private void initializeNormalVector(int i, int j)
        {
            normalVectors[i, j][0] = 0;
            normalVectors[i, j][1] = 0;
            normalVectors[i, j][2] = 1;
        }

        private void setNormalVectorRB_CheckedChanged(object sender, EventArgs e)
        {
            if(setNormalVectorRB.Checked)
            {
                switchRadioButtonsAndButton(setNormalVectorRB, fixedNormalVectorRB, setNormalVectorButton, true);
                fixedNormalVector = false;
                
                if (noDisturbance)
                    convertImageWithNM();
                else
                    convertImageWithNMAndHM();

                fillPolygonsFromTextureIfFinished();
            }
        }
        
        private void setNormalVectorButton_Click(object sender, EventArgs e)
        {
            if(selectPictureToSet(setNormalVectorPB, NormalMap, ref normalMapColorTable))
            {
                if (noDisturbance)
                    convertImageWithNM();
                else
                    convertImageWithNMAndHM();

                fillPolygonsFromTextureIfFinished();
            }
        }

        private bool selectPictureToSet(PictureBox pb, Bitmap image, ref Color [,] colorTable)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.tiff, *.png) | *.jpg; *.jpeg; *.jpe; *.tiff; *.png";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pb.BackgroundImage = new Bitmap(
                                    Image.FromFile(ofd.FileName),
                                    new Size(pb.Width, pb.Height));

                image = new Bitmap(Image.FromFile(ofd.FileName), new Size(screenPB.Width, screenPB.Height));
                initializeColorTable(image, pb, ref colorTable, ofd.FileName);

                return true;
            }
            return false;
        }

        private void setObjectColorButton_Click(object sender, EventArgs e)
        {
            if (selectPictureToSet(setObjectColorPB, polygonImage, ref textureImageColorTable))
            {
                if (noDisturbance)
                    convertImageWithNM();
                else
                    convertImageWithNMAndHM();

                fillPolygonsFromTextureIfFinished();
            }
        }
        private void lightTimer_Tick(object sender, EventArgs e)
        {
            setLightVector(tickCounter++);

            if (noDisturbance)
                convertImageWithNM();
            else
                convertImageWithNMAndHM();

            fillPolygonsFromTextureIfFinished();
        }
        private void setLightVector(int i)
        {
            lightVector[0] = lightTable[i % LIGHT_ELEMENTS][0];
            lightVector[1] = lightTable[i % LIGHT_ELEMENTS][1];
            lightVector[2] = lightTable[i % LIGHT_ELEMENTS][2];
        }
        private void FTrackBar_ValueChanged(object sender, EventArgs e)
        {
            _f = 1 + ((double)FTrackBar.Value/10 - 1);
        }
        private void KsTrackBar_ValueChanged(object sender, EventArgs e)
        {
            Ks = (double)KsTrackBar.Value / 10;
        }

        private void KdTrackBar_Scroll(object sender, EventArgs e)
        {
            Kd = (double)KdTrackBar.Value / 10;
        }

        private void mTrackBar_ValueChanged(object sender, EventArgs e)
        {
            mFactor = mTrackBar.Value;
        }
        #endregion

        #region Screen Functions

        private void copyImageToTableImage(Color [,] ColorTable, Bitmap bitmap)
        {
            for (int i = 0; i < screenPB.Width; i++)
                for (int j = 0; j < screenPB.Height; j++)
                    ColorTable[i, j] = bitmap.GetPixel(i, j);
        }
        private void ScreenPB_MD(object sender, MouseEventArgs e)
        {
            if(staticPolygonFinished)
            {
                if (polygonFinished)
                {
                    if (vertexSelected = mouseCloseToVertex(e.Location)){ }
                    else
                        saveLocationWhenFinished(e.Location);
                }
                else
                    if (firstClick)
                        createFirstVertex(e.Location);
                    else
                    {
                        endingPoint = e.Location;

                        if (canPolygonBeCompleted(p))
                        {
                            finishPolygon();

                        if (fixedObjectColor)
                            staticPolygon.fillPolygonWithColor(polygonColor, mainBitmap);
                        else
                            staticPolygon.fillPolygonFromTexture(currentImageColorTable, mainBitmap);

                            staticPolygon.drawAllEdges(mainBitmap);
                        }
                        else
                        {
                            addNextEdgeToPolygon(p);
                            staticPolygon.drawAllEdges(mainBitmap);
                        }
                    }
            }
            else
            {
                if (firstClick)
                    createFirstStaticVertex(e.Location);
                else
                {
                    endingPoint = e.Location;

                    if (canPolygonBeCompleted(staticPolygon))
                    {
                        finishStaticPolygon();
                        resetAllSetings();
                    }
                    else
                        addNextEdgeToPolygon(staticPolygon);
                }
            }

            screenPB.Invalidate();
        }
        private bool mouseCloseToVertex(Point pt)
        {
            numberOfVertexSelected = isVertexClicked(p, pt);
            if(numberOfVertexSelected != -1)
            {
                return true;
            }
            return false;

        }
        private int isVertexClicked(Polygon polygon, Point p)
        {
            for (int i = 0; i < polygon.Edges.Count; i++)
                if (Math.Abs(Math.Sqrt((polygon.Edges[i][0].X - p.X) * (polygon.Edges[i][0].X - p.X) + (polygon.Edges[i][0].Y - p.Y) * (polygon.Edges[i][0].Y - p.Y))) < (double)CLOSE_TO_THE_VERTEX)
                    return i;
            return -1;
        }
        private void resetAllSetings()
        {
            endingPoint = Point.Empty;
            lastClicked = Point.Empty;
            firstClick = true;
        }
        private void saveLocationWhenFinished(Point pt)
        {
            if (lastClicked == Point.Empty)
            {
                lastClicked = pt;
                readyToMove = true;
            }
        }
        private void createFirstStaticVertex(Point pt)
        {
            staticPolygonStarted = true;
            startingPoint = pt;
            endingPoint = pt;
            firstClick = false;
        }
        private void createFirstVertex(Point pt)
        {
            polygonStarted = true;
            startingPoint = pt;
            endingPoint = pt;
            firstClick = false;
        }
        private void finishPolygon()
        {
            denotePolygonAsFinished();
            completeAndDrawPolygon(p);
        }
        private void completeAndDrawPolygon(Polygon polygon)
        {
            Edge edge = new Edge(polygon.Edges[polygon.Edges.Count - 1][1], polygon.Edges[0][0]);
            polygon.Edges.Add(edge);
            startingPoint = Point.Empty;
            grap.Clear(Color.White);

            if (fixedObjectColor)
                polygon.fillPolygonWithColor(polygonColor, mainBitmap);
            else
                polygon.fillPolygonFromTexture(currentImageColorTable, mainBitmap);
            polygon.drawAllEdges(mainBitmap);
        }
        private void finishStaticPolygon()
        {
            denoteStaticPolygonAsFinished();
            completeAndDrawPolygon(staticPolygon);
        }
        private void denotePolygonAsFinished()
        {
            polygonFinished = true;
        }
        private void denoteStaticPolygonAsFinished()
        {
            staticPolygonFinished = true;
        }
        private void addNextEdgeToPolygon(Polygon p)
        {
            Edge ed = new Edge(startingPoint, endingPoint);
            p.Edges.Add(ed);
            startingPoint = new Point(endingPoint.X, endingPoint.Y);

            Edge bresenhamEdge = new Edge(ed[0], ed[1]);
            bresenhamEdge.Bresenham(mainBitmap, Color.Black);
        }
        private bool canPolygonBeCompleted(Polygon p)
        {
            return p.Edges.Count >= 2 &&
                DIFF_IN_PIXELS_BETWEEN_START_AND_END >
                new Edge(p.Edges[0][0], endingPoint).Length();
        }
        private void ScreenPB_MU(object sender, MouseEventArgs e)
        {
            lastClicked = Point.Empty;
            readyToMove = false;
            vertexSelected = false;
            numberOfVertexSelected = -1;
        }

        private void ScreenPB_MM(object sender, MouseEventArgs e)
        {
            if(staticPolygonStarted)
            {
                if(staticPolygonFinished)
                {
                    if (polygonStarted)
                    {
                        if (polygonFinished)
                        {
                            if(vertexSelected)
                            {
                                switchSelectedVertex(e.Location);
                            }
                            else
                            {
                                if (readyToMove)
                                {
                                    if (lastClicked == Point.Empty)
                                        lastClicked = e.Location;
                                    else
                                    {
                                        switchLocationOfEdges(e.Location);
                                        fillPolygonsWithColor();
                                        p.drawAllEdges(mainBitmap);
                                        staticPolygon.drawAllEdges(mainBitmap);
                                    }
                                }
                            }
                        }
                        else
                        {
                            redrawPolygonWithLastEdgeSwitched(e.Location, p);
                            if (fixedObjectColor)
                                staticPolygon.fillPolygonWithColor(polygonColor, mainBitmap);
                            else
                                staticPolygon.fillPolygonFromTexture(currentImageColorTable, mainBitmap);

                            staticPolygon.drawAllEdges(mainBitmap);
                        }
                    }
                }
                else
                {
                    redrawPolygonWithLastEdgeSwitched(e.Location, staticPolygon);
                }
                screenPB.Invalidate();
            }
        }
        private void switchSelectedVertex(Point pt)
        {
            setNewLocationOfSelectedVertex(pt);

            grap.Clear(Color.White);

            fillPolygonsWithColor();
            redrawEdgesOfPolygons();
        }
        private void setNewLocationOfSelectedVertex(Point pt)
        {
            Point vertexSwitched = new Point(pt.X, pt.Y);
            p.Edges[numberOfVertexSelected][0] = vertexSwitched;
            p.Edges[(numberOfVertexSelected - 1 + p.Edges.Count) % p.Edges.Count][1] = vertexSwitched;
        }
        private void redrawEdgesOfPolygons()
        {
            p.drawAllEdges(mainBitmap);
            staticPolygon.drawAllEdges(mainBitmap);
        }
        private void fillPolygonsWithColor()
        {
            if (fixedObjectColor)
            {
                p.fillPolygonWithColor(polygonColor, mainBitmap);
                staticPolygon.fillPolygonWithColor(polygonColor, mainBitmap);
            }
            else
            {
                p.fillPolygonFromTexture(currentImageColorTable, mainBitmap);
                staticPolygon.fillPolygonFromTexture(currentImageColorTable, mainBitmap);
            }
        }
        private void changeLocationOfSingleEdge(int numberOfEdge, int diffX, int diffY)
        {
            p.Edges[numberOfEdge][0] = new Point(p.Edges[numberOfEdge][0].X + diffX, p.Edges[numberOfEdge][0].Y + diffY);
            p.Edges[numberOfEdge][1] = new Point(p.Edges[numberOfEdge][1].X + diffX, p.Edges[numberOfEdge][1].Y + diffY);
        }
        private void switchLocationOfEdges(Point pt)
        {
            int diffX = pt.X - lastClicked.X,
                diffY = pt.Y - lastClicked.Y;

            lastClicked = pt;

            grap.Clear(Color.White);

            for (int i = 0; i < p.Edges.Count; i++)
                changeLocationOfSingleEdge(i, diffX, diffY);
        }

        private void sutherlandHodgmanButton_Click(object sender, EventArgs e)
        {
            if(polygonFinished)
            {
                if (p.isConvex() || staticPolygon.isConvex())
                {
                    intersection = p.suthHodgClip(staticPolygon);
                    bool isIntersection = intersection.fillPolygonWithColor(Color.Blue, mainBitmap);
                    redrawEdgesOfPolygons();
                    screenPB.Invalidate();
                }
                else
                    MessageBox.Show("None of polygons is convex!\n You can't use Sutherland Hodgman algorithm.", "Info" ,MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        private void redrawPolygonWithLastEdgeSwitched(Point pt, Polygon polygon)
        {
            grap.Clear(Color.White);
            polygon.drawAllEdges(mainBitmap);

            Edge bresenhamEdge = new Edge(endingPoint, pt);

            bresenhamEdge.Bresenham(mainBitmap, Color.Black);
        }

        #endregion

        #region convertFunctions

        private void convertImageWithNMAndHM()
        {

            for (int i = 0; i < screenPB.Width; i++)
                for (int j = 0; j < screenPB.Height; j++)
                {
                    countNormalVectorsWithDistraction(i, j);
                    cos[i, j] = countCos(normalVectors[i, j]);
                }

            setCurrentImageColorTable();
        }
        private double [] setColorsToPixels(int i, int j)
        {
            double[] colorsPerPixel = new double[VECTOR_PARTS];
            double crystalPart;

            colorsPerPixel[0] = ((double)lightColor.R / 255) * (double)textureImageColorTable[i, j].R * cos[i, j];
            /*
            crystalPart = Ks * ((double)lightColor.R / 255);

            for (int k = 0; k < mFactor; k++)
                crystalPart *= countSecondCos(countAndNormalizeRVector(i, j));
            colorsPerPixel[0] += crystalPart;
            */
            colorsPerPixel[1] = ((double)lightColor.G / 255) * (double)textureImageColorTable[i, j].G * cos[i, j];
            /*
            crystalPart = Ks * ((double)lightColor.G / 255);

            for (int k = 0; k < mFactor; k++)
                crystalPart *= countSecondCos(countAndNormalizeRVector(i, j));
            colorsPerPixel[1] += crystalPart;
            */
            colorsPerPixel[2] = ((double)lightColor.B / 255) * (double)textureImageColorTable[i, j].B * cos[i, j];
            /*
            crystalPart = Ks * ((double)lightColor.B / 255);

            for (int k = 0; k < mFactor; k++)
                crystalPart *= countSecondCos(countAndNormalizeRVector(i, j));
            colorsPerPixel[2] += crystalPart;
            */
            return convertPixelColors(colorsPerPixel);
        }

        private double [] countAndNormalizeRVector(int i ,int j)
        {
            double[] VectorR = new double[VECTOR_PARTS];

            if(fixedLightVector)
                for (int k = 0; k < VECTOR_PARTS; k++)
                    VectorR[k] = 2 * crossProduct(i, j, lightVector) * normalVectors[i, j][k] - lightTable[tickCounter % lightTable.Length][k];
            else
                for (int k = 0; k < VECTOR_PARTS; k++)
                    VectorR[k] = 2 * crossProduct(i, j, lightTable[tickCounter % lightTable.Length]) * normalVectors[i, j][k] - lightTable[tickCounter % lightTable.Length][k];

            double vectorRLength = countVectorRLength(VectorR);

            for (int k = 0; k < VECTOR_PARTS; k++)
                VectorR[k] /= vectorRLength;

            return VectorR;
        }
        private double crossProduct(int i, int j, double [] lightVector)
        {
            double crossProduct=0;
            for (int k = 0; k < VECTOR_PARTS; k++)
                crossProduct += normalVectors[i, j][k] * lightTable[tickCounter % lightTable.Length][k];
            return crossProduct;
        }

        private double countVectorRLength(double [] vectorR)
        {
            double SubVectorLength = Math.Sqrt(
                vectorR[0] * vectorR[0] +
                vectorR[1] * vectorR[1]);

            double VectorLength = Math.Sqrt(
                SubVectorLength * SubVectorLength +
                vectorR[2] * vectorR[2]);

            return VectorLength;
        }

        private void convertImageWithNM()
        {

            for (int i = 0; i < screenPB.Width; i++)
                for (int j = 0; j < screenPB.Height; j++)
                {
                    countNormalVectorsWithoutDistraction(i, j);
                    cos[i, j] = countCos(normalVectors[i, j]);
                }

            setCurrentImageColorTable();
        }

        private void convertImageWithHM()
        {
            for (int i = 0; i < screenPB.Width; i++)
                for (int j = 0; j < screenPB.Height; j++)
                {
                    countNormalVectorsFromDistraction(i, j);
                    cos[i, j] = countCos(normalVectors[i, j]);
                }

            setCurrentImageColorTable();
        }
        private void setCurrentImageColorTable()
        {
            for (int i = 0; i < screenPB.Width; i++)
                for (int j = 0; j < screenPB.Height; j++)
                {
                    double[] colorsPerPixel = new double[VECTOR_PARTS];
                    colorsPerPixel = setColorsToPixels(i, j);
                    currentImageColorTable[i, j] = Color.FromArgb((int)colorsPerPixel[0], (int)colorsPerPixel[1], (int)colorsPerPixel[2]);
                }
        }

        private double [] convertPixelColors(double [] colorsPerPixel)
        {
            for (int k = 0; k < VECTOR_PARTS; k++)
            {
                if (colorsPerPixel[k] < 0)
                    colorsPerPixel[k] = 0;
                if (colorsPerPixel[k] > 255)
                    colorsPerPixel[k] = 255;
            }

            return colorsPerPixel;
        }
        private void countNormalVectorsFromDistraction(int i ,int j)
        {
            countAndAddDistraction(i, j);
            normalizeVectors(i, j);
        }
        private void countNormalVectorsWithDistraction(int i, int j)
        {
            if(!fixedNormalVector)
                refactorNormalVectors(i, j);

            countAndAddDistraction(i, j);
            normalizeVectors(i, j);
        }
        private void countNormalVectorsWithoutDistraction(int i, int j)
        {
            if (!fixedNormalVector)
            {
                refactorNormalVectors(i, j);
                normalizeVectors(i, j);
            }
        }
        private void normalizeVectors(int i, int j)
        {
            double VectorLength = countVectorLength(i, j);

            for (int k = 0; k < VECTOR_PARTS; k++)
                normalVectors[i, j][k] /= VectorLength;
        }
        private void refactorNormalVectors(int i, int j)
        {
            convertNormalVectorsToInterval(i, j);

            for (int k = 0; k < 3; k++)
                normalVectors[i, j][k] /= normalVectors[i, j][2];
        }
        private void countAndAddDistraction(int i, int j)
        {
            double[] distraction = new double[VECTOR_PARTS];
            distraction = countDistraction(i, j);
            addDistractionToNormalVectors(i, j, distraction);
        }
        private double countVectorLength(int i, int j)
        {
            double SubVectorLength = Math.Sqrt(
                normalVectors[i, j][0] * normalVectors[i, j][0] +
                normalVectors[i, j][1] * normalVectors[i, j][1]);

            double VectorLength = Math.Sqrt(
                SubVectorLength * SubVectorLength +
                normalVectors[i, j][2] * normalVectors[i, j][2]);

            return VectorLength;
        }
        private void convertNormalVectorsToInterval(int i, int j)
        {
            normalVectors[i, j][0] = (((double)normalMapColorTable[i, j].R / 255) - 0.5) * 2;
            normalVectors[i, j][1] = (((double)normalMapColorTable[i, j].G / 255) - 0.5) * 2;
            normalVectors[i, j][2] = (double)normalMapColorTable[i, j].B / 255;
        }
        
        private void addDistractionToNormalVectors(int i, int j, double [] distraction)
        {
            normalVectors[i, j][0] += distraction[0];
            normalVectors[i, j][1] += distraction[1];
            normalVectors[i, j][2] += distraction[2];
        }

        private double [] countDistraction(int i, int j)
        {
            double[] distraction = new double[VECTOR_PARTS];
            double dhx, dhy;

            if (i != screenPB.Width - 1)
                dhx = heightMapColorTable[i + 1, j].R - heightMapColorTable[i, j].R;
            else
                dhx = heightMapColorTable[i, j].R;

            if (j != screenPB.Height - 1)
                dhy = heightMapColorTable[i, j + 1].R - heightMapColorTable[i, j].R;
            else
                dhy = heightMapColorTable[i, j].R;

            distraction[0] = dhx;
            distraction[1] = dhy;
            distraction[2] = -normalVectors[i, j][0] * dhx - normalVectors[i, j][1] * dhy;

            for (int k = 0; k < VECTOR_PARTS; k++)
                distraction[k] *= _f;

            return distraction;
        }
        
        private double countCos(double [] vector)
        {
            return vector[0] * lightVector[0] +
                vector[1] * lightVector[1] + 
                vector[2] * lightVector[2];
        }
        private double countSecondCos(double[] vector)
        {
            return vector[0] * VectorV[0] +
                vector[1] * VectorV[1] +
                vector[2] * VectorV[2];
        }

        #endregion
    }
}
