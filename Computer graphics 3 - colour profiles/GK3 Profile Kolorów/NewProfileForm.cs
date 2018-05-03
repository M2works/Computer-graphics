using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GK3_Profile_Kolorów
{
    public partial class NewProfileForm : Form
    {
        public NewProfileForm()
        {
            InitializeComponent();
            sourceReferenceCB.SelectedIndex = 0;
        }

        private void createProfileButton_Click(object sender, EventArgs e)
        {
            PictureProfile.ReferenceWhite reference;

            PictureProfile pp = computeProfileFromSelectedItems();

            if (sourceReferenceCB.SelectedIndex == 0)
                reference = PictureProfile.ReferenceWhite.D50;
            else
                reference = PictureProfile.ReferenceWhite.D65;

            pp.referenceWhite = reference;

            Form1.newProfile = pp;
            Form1.newProfileName = sourceNameTB.Text;

            Close();
        }
        private PictureProfile computeProfileFromSelectedItems()
        {
            double gamma, XR, YR, XG, YG, YB, XB, XW, YW;

            if (!double.TryParse(sourceBlueXTB.Text, out XB))
                MessageBox.Show("Incorrect data!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (!double.TryParse(sourceBlueYTB.Text, out YB))
                MessageBox.Show("Incorrect data!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (!double.TryParse(sourceGreenXTB.Text, out XG))
                MessageBox.Show("Incorrect data!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (!double.TryParse(sourceGreenYTB.Text, out YG))
                MessageBox.Show("Incorrect data!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (!double.TryParse(sourceRedXTB.Text, out XR))
                MessageBox.Show("Incorrect data!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (!double.TryParse(sourceRedYTB.Text, out YR))
                MessageBox.Show("Incorrect data!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (!double.TryParse(sourceWhiteX.Text, out XW))
                MessageBox.Show("Incorrect data!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (!double.TryParse(sourceWhiteY.Text, out YW))
                MessageBox.Show("Incorrect data!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (!double.TryParse(sourceGammaTB.Text, out gamma))
                MessageBox.Show("Incorrect data!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            return new PictureProfile(gamma, PictureProfile.ReferenceWhite.D50, XR, YR, XG, YG, XB, YB, XW, YW);
        }
    }
}
