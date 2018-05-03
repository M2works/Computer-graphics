using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Polygon_Editor
{
    public partial class Angle_Form : Form
    {
        public Angle_Form()
        {
            InitializeComponent();
        }

        private void SetDegreeButton_Click(object sender, EventArgs e)
        {
            int value;
            if (int.TryParse(degreeTextBox.Text, out value) && value>0 && value<180)
            {
                Form1.DegreesInAngle = value;
                Close();
            }
            else
            {
                MessageBox.Show("Incorrect value! Should be a positive number < 180.", "Warning!",MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
