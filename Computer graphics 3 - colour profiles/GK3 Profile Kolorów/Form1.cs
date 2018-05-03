using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GK3_Profile_Kolorów
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            initializePictureProfiles();
            countInverseBradfords();
            setDefaultProfile(ref sourceProfile);
            setDefaultProfile(ref targetProfile);
        }

        const int INITIAL_PROFILES = 9;
        const int SRGB_NUMBER = 0;
        
        const int PROFILE_MATRIX_SIZE = 3;
        const int COEFFICIENTS_NUMBER = 3;

        double [] D65LightVector = new double [] { 0.95047, 1.00000, 1.08883 };
        double [] D50LightVector = new double [] { 0.96422, 1.00000, 0.82521 };

        public static PictureProfile newProfile;
        public static string newProfileName;

        PictureProfile defaultProfile = new PictureProfile(2.2, PictureProfile.ReferenceWhite.D65,
                                                    0.64, 0.33,
                                                    0.3, 0.6,
                                                    0.15, 0.06,
                                                    0.3127, 0.3290
                                        );
        PictureProfile sourceProfile, targetProfile;
        List<PictureProfile> profiles = new List<PictureProfile>();

        double[,] Bradford65to50 = new double[,]
        {
            {1.0478112,  0.0228866, -0.0501270 },
            { 0.0295424,  0.9904844, -0.0170491 },
            { -0.0092345,  0.0150436,  0.7521316}
        }, 
            Bradford50to65 = new double[,]
        {
            { 0.9555766, -0.0230393,  0.0631636 },
            { -0.0282895,  1.0099416,  0.0210077 },
            { 0.0122982, -0.0204830,  1.3299098 }
        };

        double[,] inverseBradford65to50, 
                  inverseBradford50to65;

        private void setDefaultProfile( ref PictureProfile profile)
        {
            profile = new PictureProfile(
                defaultProfile.Gamma, defaultProfile.referenceWhite,
                defaultProfile.xR, defaultProfile.yR,
                defaultProfile.xG, defaultProfile.yG,
                defaultProfile.xB, defaultProfile.yB,
                defaultProfile.xW, defaultProfile.yW);
        }
        private void countInverseBradfords()
        {
            inverseBradford50to65 = invertMatrix(Bradford50to65);
            inverseBradford65to50 = invertMatrix(Bradford65to50);
        }

        private void initializePictureProfiles()
        {
            //sRGB
            PictureProfile pp = new PictureProfile(2.2, PictureProfile.ReferenceWhite.D65,
                                                    0.64, 0.33,
                                                    0.3, 0.6,
                                                    0.15, 0.06,
                                                    0.3127, 0.3290
                                        );
            profiles.Add(pp);

            //Adobe RGB
            pp = new PictureProfile(2.2, PictureProfile.ReferenceWhite.D65,
                                                    0.64, 0.33,
                                                    0.21, 0.71,
                                                    0.15, 0.06,
                                                    0.3127, 0.3290
                                        );
            profiles.Add(pp);

            //AppleRGB
            pp = new PictureProfile(1.8, PictureProfile.ReferenceWhite.D65,
                                                    0.625, 0.34,
                                                    0.28, 0.595,
                                                    0.155, 0.07,
                                                    0.3127, 0.3290
                                        );
            profiles.Add(pp);

            //Best RGB
            pp = new PictureProfile(2.2, PictureProfile.ReferenceWhite.D50,
                                                    0.7347, 0.2653,
                                                    0.215, 0.775,
                                                    0.13, 0.035,
                                                    0.3457, 0.3585
                                        );
            profiles.Add(pp);

            //Beta RGB
            pp = new PictureProfile(2.2, PictureProfile.ReferenceWhite.D50,
                                                    0.6888, 0.3112,
                                                    0.1986, 0.7551,
                                                    0.1265, 0.0352,
                                                    0.3457, 0.3585
                                        );
            profiles.Add(pp);

            //Bruce RGB
            pp = new PictureProfile(2.2, PictureProfile.ReferenceWhite.D65,
                                                    0.64, 0.33,
                                                    0.28, 0.65,
                                                    0.15, 0.06,
                                                    0.3127,0.3290
                                        );
            profiles.Add(pp);

            //ColorMatch RGB
            pp = new PictureProfile(1.8, PictureProfile.ReferenceWhite.D50,
                                                    0.63, 0.34,
                                                    0.295, 0.605,
                                                    0.15, 0.075,
                                                    0.3457, 0.3585
                                        );
            profiles.Add(pp);

            //PAL - SECAM RGB
            pp = new PictureProfile(2.2, PictureProfile.ReferenceWhite.D65,
                                                    0.64, 0.33,
                                                    0.29, 0.6,
                                                    0.15, 0.06,
                                                    0.3127, 0.3290
                                        );
            profiles.Add(pp);

            //Wide Gamut RGB
            pp = new PictureProfile(2.2, PictureProfile.ReferenceWhite.D50,
                                                    0.735, 0.265,
                                                    0.115, 0.826,
                                                    0.157, 0.018,
                                                    0.3457, 0.3585
                                        );
            profiles.Add(pp);
        }
        private void loadPhotoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string ext = Path.GetExtension(ofd.FileName);
                if (ext != ".jpg" && ext != ".jpeg" && ext != ".jpe" && ext != ".jfif" && ext != ".png")
                {
                    MessageBox.Show("Incorrect extension!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                sourceProfilePB.Image = new Bitmap(
                                    Image.FromFile(ofd.FileName),
                                    new Size(sourceProfilePB.Width, sourceProfilePB.Height));

                sourceProfileCB.SelectedIndex = SRGB_NUMBER;
                setDefaultProfile(ref sourceProfile);
                enableControlsAfterSettingSourcePicture();
                targetProfileCB.SelectedIndex = 0;
            }
        }
        private void enableControlsAfterSettingSourcePicture()
        {
            enableSourceControls();
            enableTargetControls();
        }

        private void enableSourceControls()
        {
            sourceProfileCB.Enabled = true;
        }
        private void enableTargetControls()
        {
            targetProfileCB.Enabled = true;
        }
        private void convertProfileColor()
        {
            double[,][] XYZ = RGBToXYZ();

            if(sourceProfile.referenceWhite != targetProfile.referenceWhite)
            {
                if (sourceProfile.referenceWhite == PictureProfile.ReferenceWhite.D65)
                    XYZ = convertXYZforDifferentWhiteReferences(XYZ, true);
                else
                    XYZ = convertXYZforDifferentWhiteReferences(XYZ, false);
            }

            double[,][] RGB = XYZToRGB(XYZ);
            setTargetPictureColors(RGB);
        }
        private void setTargetPictureColors(double [,][] Colors)
        {
            Bitmap image = new Bitmap(sourceProfilePB.Image);

            for (int i = 0; i < image.Width; i++)
                for (int j = 0; j < image.Height; j++)
                    for (int k = 0; k < COEFFICIENTS_NUMBER; k++)
                    {
                        image.SetPixel(i, j,Color.FromArgb((int)Colors[i,j][0], (int)Colors[i, j][1], (int)Colors[i, j][2]));
                    }

            targetProfilePB.Image = image;
            targetProfilePB.Invalidate();
        }
        private double [,][] XYZToRGB(double [,][] XYZ)
        {
            Bitmap image = new Bitmap(sourceProfilePB.Image);
            double[,][] RGB = new double[image.Width, image.Height][];

            for (int i = 0; i < image.Width; i++)
                for (int j = 0; j < image.Height; j++)
                    RGB[i, j] = new double[COEFFICIENTS_NUMBER];

            double[,] profileMatrix = targetProfile.computeProfileMatrix(); 

            double[,] invertedProfileMatrix = invertMatrix(profileMatrix); 

            /*double[,] invertedProfileMatrix =  new double[,] 
            { 
                { 3.1338561, - 1.6168667, - 0.4906146 },
                { - 0.9787684,  1.9161415,  0.0334540 },
                { 0.0719453, - 0.2289914,  1.4052427 }
            };*/

            for (int i = 0; i < sourceProfilePB.Image.Width; i++)
                for (int j = 0; j < sourceProfilePB.Image.Height; j++)
                {
                    RGB[i, j] = multiplyMatrixAndVector(invertedProfileMatrix, XYZ[i, j]);

                    for (int k = 0; k < COEFFICIENTS_NUMBER; k++)
                    {
                        RGB[i, j][k] *= 255;

                        if (RGB[i, j][k] < 0)
                            RGB[i, j][k] = 0;

                        if (RGB[i, j][k] > 255)
                            RGB[i, j][k] = 255;
                    }
                }

            return RGB;
        }

        private double [,][] RGBToXYZ()
        {
            Bitmap image = new Bitmap(sourceProfilePB.Image);
            double[,][] XYZ = new double[image.Width,image.Height][];

            for (int i = 0; i < image.Width; i++)
                for (int j = 0; j < image.Height; j++)
                    XYZ[i, j] = new double[COEFFICIENTS_NUMBER];

            double[,] profileMatrix = sourceProfile.computeProfileMatrix();

            /*double[,] profileMatrix =  new double[,] {
                {0.4360747,  0.3850649,  0.1430804 },
                { 0.2225045,  0.7168786,  0.0606169 },
                { 0.0139322,  0.0971045,  0.7141733}
            };*/

            for (int i = 0; i < image.Width; i++)
                for (int j = 0; j < image.Height; j++)
                {
                    for (int k = 0; k < PROFILE_MATRIX_SIZE; k++)
                        XYZ[i, j][k] += ((double)image.GetPixel(i, j).R / 255) * profileMatrix[k, 0];

                    for (int k = 0; k < PROFILE_MATRIX_SIZE; k++)
                        XYZ[i, j][k] += ((double)image.GetPixel(i, j).G / 255) * profileMatrix[k, 1];

                    for (int k = 0; k < PROFILE_MATRIX_SIZE; k++)
                        XYZ[i, j][k] += ((double)image.GetPixel(i, j).B / 255) * profileMatrix[k, 2];
                }

            return XYZ;
        }
        private double[,] invertMatrix(double [,] matrixToInverse)
        {
            double[,] inversedMatrix = new double[PROFILE_MATRIX_SIZE, PROFILE_MATRIX_SIZE];
            double divider;
            

            inversedMatrix[0, 0] = 1;
            inversedMatrix[1, 1] = 1;
            inversedMatrix[2, 2] = 1;

            for (int k = 0; k < PROFILE_MATRIX_SIZE - 1; k++)
            {
                divider = matrixToInverse[PROFILE_MATRIX_SIZE - 1 - k, 0] / matrixToInverse[0, 0];
                for (int i = 0; i < PROFILE_MATRIX_SIZE; i++)
                {
                    matrixToInverse[PROFILE_MATRIX_SIZE - 1 - k, i] -= divider * matrixToInverse[0, i];
                    inversedMatrix[PROFILE_MATRIX_SIZE - 1 - k, i] -= divider * inversedMatrix[0, i];
                }
            }

            divider = matrixToInverse[2, 1] / matrixToInverse[1, 1];
            for (int i = 0; i < PROFILE_MATRIX_SIZE; i++)
            {
                matrixToInverse[PROFILE_MATRIX_SIZE - 1, i] -= divider * matrixToInverse[1, i];
                inversedMatrix[PROFILE_MATRIX_SIZE - 1, i] -= divider * inversedMatrix[1, i];
            }

            for (int i = 0; i < PROFILE_MATRIX_SIZE; i++)
            {
                divider = matrixToInverse[PROFILE_MATRIX_SIZE - i - 1, PROFILE_MATRIX_SIZE - i - 1];
                for (int j = 0; j < i + 1; j++)
                {
                    matrixToInverse[PROFILE_MATRIX_SIZE - i - 1, PROFILE_MATRIX_SIZE - j - 1] /= divider;
                }
                for (int j = 0; j < PROFILE_MATRIX_SIZE; j++)
                {
                    inversedMatrix[PROFILE_MATRIX_SIZE - i - 1, PROFILE_MATRIX_SIZE - j - 1] /= divider;
                }
            }

            double distracter;

            for (int i = 1; i < PROFILE_MATRIX_SIZE; i++)
            {
                distracter = matrixToInverse[PROFILE_MATRIX_SIZE - 1 - i, PROFILE_MATRIX_SIZE - 1];
                matrixToInverse[PROFILE_MATRIX_SIZE - 1 - i, PROFILE_MATRIX_SIZE - 1] = 0;

                for (int j = 0; j < PROFILE_MATRIX_SIZE; j++)
                {
                    inversedMatrix[PROFILE_MATRIX_SIZE - 1 - i, j] -= distracter * inversedMatrix[PROFILE_MATRIX_SIZE - 1, j];
                }
            }

            distracter = matrixToInverse[0, 1];
            matrixToInverse[0, 1] = 0;

            for (int j = 0; j < PROFILE_MATRIX_SIZE; j++)
            {
                inversedMatrix[0, j] -= distracter * inversedMatrix[0, j];
            }

            return inversedMatrix;
        }

        private void generateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sourceProfilePB.Image != null)
                convertProfileColor();
            else
                MessageBox.Show("There is no source photo!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void saveResultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (targetProfilePB.Image != null)
            {
                SaveFileDialog svd = new SaveFileDialog();

                svd.Filter = "Image files (*.jpg, *.bmp, *.jpeg, *.tiff, *.png) | *.jpg; *.bmp; *.jpeg; *.tfif; *.png";

                ImageFormat format = ImageFormat.Jpeg;
                if (svd.ShowDialog() == DialogResult.OK)
                {
                    string ext = System.IO.Path.GetExtension(svd.FileName);
                    switch (ext)
                    {
                        case ".jpg":
                            format = ImageFormat.Jpeg;
                            break;
                        case ".jpeg":
                            format = ImageFormat.Jpeg;
                            break;
                        case ".bmp":
                            format = ImageFormat.Bmp;
                            break;
                        case ".tiff":
                            format = ImageFormat.Tiff;
                            break;
                        case ".png":
                            format = ImageFormat.Png;
                            break;
                    }
                    targetProfilePB.Image.Save(svd.FileName, format);
                }
            }
            else
                MessageBox.Show("There is no result photo!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }        
        private void setSourceProfileInfo(int index)
        {
            sourceRedXTB.Text = profiles[index].xR.ToString();
            sourceRedYTB.Text = profiles[index].yR.ToString();
            sourceWhiteX.Text = profiles[index].xW.ToString();
            sourceWhiteY.Text = profiles[index].yW.ToString();
            sourceGreenXTB.Text = profiles[index].xG.ToString();
            sourceGreenYTB.Text = profiles[index].yG.ToString();
            sourceBlueXTB.Text = profiles[index].xB.ToString();
            sourceBlueYTB.Text = profiles[index].yB.ToString();
            sourceGammaTB.Text = profiles[index].Gamma.ToString();
        }
        private void setTargetProfileInfo(int index)
        {
            targetRedXTB.Text = profiles[index].xR.ToString();
            targetRedYTB.Text = profiles[index].yR.ToString();
            targetWhiteX.Text = profiles[index].xW.ToString();
            targetWhiteY.Text = profiles[index].yW.ToString();
            targetGreenXTB.Text = profiles[index].xG.ToString();
            targetGreenYTB.Text = profiles[index].yG.ToString();
            targetBlueXTB.Text = profiles[index].xB.ToString();
            targetBlueYTB.Text = profiles[index].yB.ToString();
            targetGammaTB.Text = profiles[index].Gamma.ToString();
        }

        private void sourceProfileCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            if (cb == sourceProfileCB)
            {
                setSourceProfileInfo(cb.SelectedIndex);
                sourceProfile = profiles[cb.SelectedIndex];
            }
        }

        private void targetProfileCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            if (cb == targetProfileCB)
            {
                setTargetProfileInfo(cb.SelectedIndex);
                targetProfile = profiles[cb.SelectedIndex];
            }
        }

        private void graynessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sourceProfilePB.Image != null)
            {
                Bitmap sourceImage = new Bitmap(sourceProfilePB.Image);
                Bitmap newImage = new Bitmap(sourceProfilePB.Image);

                for (int i = 0; i < sourceProfilePB.Width; i++)
                    for (int j = 0; j < sourceProfilePB.Height; j++)
                    {
                        int color = sourceImage.GetPixel(i, j).R;
                        newImage.SetPixel(i, j, Color.FromArgb(color, color, color));
                    }

                sourceProfilePB.Image = newImage;
            }

            if (targetProfilePB.Image != null)
            {
                Bitmap targetImage = new Bitmap(targetProfilePB.Image);
                Bitmap newImage = new Bitmap(targetProfilePB.Image);

                for (int i = 0; i < targetProfilePB.Width; i++)
                    for (int j = 0; j < targetProfilePB.Height; j++)
                    {
                        int color = targetImage.GetPixel(i, j).R;
                        newImage.SetPixel(i, j, Color.FromArgb(color, color, color));
                    }

                targetProfilePB.Image = newImage;
            }
        }

        private void newProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewProfileForm npf = new NewProfileForm();
            npf.ShowDialog();
            if(newProfile != null)
            {
                profiles.Add(newProfile);

                sourceProfileCB.Items.Add(newProfileName);

                targetProfileCB.Items.Add(newProfileName);

                newProfile = null;
                newProfileName = null;
            }
        }

        private double[,][] convertXYZforDifferentWhiteReferences(double [,][] XYZ, bool fromD65)
        {
            double[,] matrix, invertedMatrix, adaptationMatrix;

            double[] sourceLightVector, targetLightVector;
            
            if(fromD65)
            {
                matrix = Bradford65to50;
                invertedMatrix = inverseBradford65to50;
                sourceLightVector = D65LightVector;
                targetLightVector = D50LightVector;
            }
            else
            {
                matrix = Bradford50to65;
                invertedMatrix = inverseBradford50to65;
                sourceLightVector = D50LightVector;
                targetLightVector = D65LightVector;
            }

            double[] sourceCoefficients = multiplyMatrixAndVector(matrix, sourceLightVector),
                    targetCoefficients = multiplyMatrixAndVector(matrix, targetLightVector);

            double[,] coefficientsMatrix = new double[,]
            {
                {targetCoefficients[0]/sourceCoefficients[0], 0, 0 },
                {0, targetCoefficients[1]/sourceCoefficients[1], 0 },
                {0, 0, targetCoefficients[2]/sourceCoefficients[2] }
            };

            double[,][] targetXYZ = new double[sourceProfilePB.Image.Width, sourceProfilePB.Image.Height][];

            adaptationMatrix = multiplyMatrices(invertedMatrix, coefficientsMatrix);
            adaptationMatrix = multiplyMatrices(adaptationMatrix, matrix);

            for(int i=0;i< sourceProfilePB.Image.Width;i++)
                for(int j=0;j< sourceProfilePB.Image.Height;j++)
                {
                    targetXYZ[i, j] = multiplyMatrixAndVector(adaptationMatrix, XYZ[i, j]);
                }

            return targetXYZ;
        }
        private double [,] multiplyMatrices(double [,] matrix1, double [,] matrix2)
        {
            double[,] resultMatrix = new double[matrix1.GetLength(0), matrix1.GetLength(1)];

            for (int i = 0; i < resultMatrix.GetLength(0); i++)
                for (int j = 0; j < resultMatrix.GetLength(1); j++)
                    for (int k = 0; k < resultMatrix.GetLength(1); k++)
                        resultMatrix[i, j] += matrix1[i, k] * matrix2[k, j];

            return resultMatrix;
        }

        private double [] multiplyMatrixAndVector(double [,] Matrix, double [] vector)
        {
            double[] resultVector = new double[COEFFICIENTS_NUMBER];

            for(int i=0;i<PROFILE_MATRIX_SIZE;i++)
                for(int j=0;j<PROFILE_MATRIX_SIZE;j++)
                {
                    resultVector[i] += Matrix[i, j] * vector[j];
                }

            return resultVector;
        }
    }
}
