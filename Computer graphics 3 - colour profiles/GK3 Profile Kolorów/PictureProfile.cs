using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK3_Profile_Kolorów
{
    public class PictureProfile
    {
        public enum ReferenceWhite
        {
            D65,
            D50
        }
        const int PROFILE_MATRIX_SIZE = 3;
        const int COEFFICIENTS_NUMBER = 3;
        public double Gamma { get; set; }
        public double xR { get; set; }
        public double yR { get; set; }
        public double xG { get; set; }
        public double yG { get; set; }
        public double xB { get; set; }
        public double yB { get; set; }
        public double xW { get; set; }
        public double yW { get; set; }
        public ReferenceWhite referenceWhite { get; set; }

        public PictureProfile(double gamma, ReferenceWhite rw,
                                double xR, double yR, 
                                double xG, double yG, 
                                double xB, double yB, 
                                double xW, double yW)
        {
            Gamma = gamma;
            this.xR = xR;
            this.yR = yR;
            this.xG = xG;
            this.yG = yG;
            this.xB = xB;
            this.yB = yB;
            this.xW = xW;
            this.yW = yW;
            referenceWhite = rw;
        }

        public double [,] computeProfileMatrix()
        {
            double [,] profileMatrix = new double[PROFILE_MATRIX_SIZE, PROFILE_MATRIX_SIZE];
            double [] X = new double [COEFFICIENTS_NUMBER],
                    Y = new double [COEFFICIENTS_NUMBER],
                    Z = new double [COEFFICIENTS_NUMBER],
                    W = new double [COEFFICIENTS_NUMBER];

            X = computeVectorX();
            Y = computeVectorY();
            Z = computeVectorZ();
            
            double [] S = computeCoefficientsS(X, Y, Z);

            for (int i = 0; i < PROFILE_MATRIX_SIZE; i++)
            {
                profileMatrix[0, i] = X[i] * S[i];
                profileMatrix[1, i] = Y[i] * S[i];
                profileMatrix[2, i] = Z[i] * S[i];
            }

            return profileMatrix;
        }

        private double [] computeCoefficientsS(double[] X, double[] Y, double[] Z)
        {
            double[] coefficients = new double[COEFFICIENTS_NUMBER];

            double[] whiteBigCoefficients = new double[COEFFICIENTS_NUMBER];

            whiteBigCoefficients[0] = xW / yW;
            whiteBigCoefficients[1] = 1;
            whiteBigCoefficients[2] = (1 - xW - yW) / yW;
            
            double [,] inversedMatrix = new double[PROFILE_MATRIX_SIZE, PROFILE_MATRIX_SIZE];

            inversedMatrix = invertMatrix(X, Y, Z);
            
            for(int i=0;i<PROFILE_MATRIX_SIZE;i++)
                for (int j = 0; j < PROFILE_MATRIX_SIZE; j++)
                    coefficients[i] += inversedMatrix[i, j] * whiteBigCoefficients[j];

            return coefficients;
        }
        private double[,] invertMatrix(double[] X, double[] Y, double[] Z)
        {
            double [,] matrixToInverse = new double[PROFILE_MATRIX_SIZE, PROFILE_MATRIX_SIZE];
            double [,] inversedMatrix = new double[PROFILE_MATRIX_SIZE, PROFILE_MATRIX_SIZE];
            double divider;

            for(int i=0;i<PROFILE_MATRIX_SIZE;i++)
            {
                matrixToInverse[0, i] = X[i];
                matrixToInverse[1, i] = Y[i];
                matrixToInverse[2, i] = Z[i];
            }

            for (int i = 0; i < PROFILE_MATRIX_SIZE; i++)
                inversedMatrix[i, i] = 1;

            for (int k = 0; k < PROFILE_MATRIX_SIZE - 1; k++)
            {
                divider = matrixToInverse[PROFILE_MATRIX_SIZE-1-k, 0] / matrixToInverse[0, 0];
                for (int i = 0; i < PROFILE_MATRIX_SIZE; i++)
                {
                    matrixToInverse[PROFILE_MATRIX_SIZE-1-k, i] -= divider * matrixToInverse[0, i];
                    inversedMatrix[PROFILE_MATRIX_SIZE-1-k, i] -= divider * inversedMatrix[0, i];
                }
            }

            divider = matrixToInverse[2, 1] / matrixToInverse[1, 1];
            for(int i=0;i<PROFILE_MATRIX_SIZE;i++)
            {
                matrixToInverse[PROFILE_MATRIX_SIZE - 1, i] -= divider * matrixToInverse[1, i];
                inversedMatrix[PROFILE_MATRIX_SIZE - 1, i] -= divider * inversedMatrix[1, i];
            }

            for(int i=0;i<PROFILE_MATRIX_SIZE;i++)
            {
                divider = matrixToInverse[PROFILE_MATRIX_SIZE - i - 1, PROFILE_MATRIX_SIZE - i-1];
                for(int j=0;j<i+1;j++)
                {
                    matrixToInverse[PROFILE_MATRIX_SIZE - i - 1, PROFILE_MATRIX_SIZE - j - 1] /= divider;
                }
                for(int j=0;j<PROFILE_MATRIX_SIZE;j++)
                {
                    inversedMatrix[PROFILE_MATRIX_SIZE - i - 1, PROFILE_MATRIX_SIZE - j - 1] /= divider;
                }
            }

            double distracter;

            for(int i=1;i<PROFILE_MATRIX_SIZE;i++)
            {
                distracter = matrixToInverse[PROFILE_MATRIX_SIZE - 1 - i, PROFILE_MATRIX_SIZE - 1];
                matrixToInverse[PROFILE_MATRIX_SIZE - 1 - i, PROFILE_MATRIX_SIZE - 1] = 0;

                for(int j=0;j<PROFILE_MATRIX_SIZE;j++)
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
        private double [] computeVectorX()
        {
            double [] vectorX = new double[COEFFICIENTS_NUMBER];

            vectorX[0] = xR/yR;
            vectorX[1] = xG/yG;
            vectorX[2] = xB/yB;

            return vectorX;
        }
        private double[] computeVectorY()
        {
            double [] vectorY = new double [COEFFICIENTS_NUMBER];

            vectorY[0] = 1;
            vectorY[1] = 1;
            vectorY[2] = 1;

            return vectorY;
        }
        private double[] computeVectorZ()
        {
            double[] vectorZ = new double[COEFFICIENTS_NUMBER];

            vectorZ[0] = (1-xR-yR) / yR;
            vectorZ[1] = (1-xG-yG) / yG;
            vectorZ[2] = (1-xB-yB) / yB;

            return vectorZ;
        }
    }
}
