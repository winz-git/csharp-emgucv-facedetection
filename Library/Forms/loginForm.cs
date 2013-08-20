using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace SystemTrayCapture.Library.Forms
{
    public partial class loginForm : Form
    {
        private Capture _capture = null;
        private CascadeClassifier haarCascade;
        

        public loginForm()
        {
            InitializeComponent();

            try
            {
                if (_capture == null)
                {
                    _capture = new Capture();
                    Application.Idle += ProcessFrame;
                }

                
            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }
        }

        private void ProcessFrame(Object sender, EventArgs e)
        {
            using (Image<Bgr, Byte> imgcapture = _capture.QueryFrame())
            {
                using (Image<Gray, byte> grayFrame = imgcapture.Convert<Gray, byte>())
                {
                    grayFrame._EqualizeHist();

                    //var faces = grayFrame.DetectHaarCascade(haarCascade)[0];
                    Rectangle[] faces = haarCascade.DetectMultiScale(grayFrame, 1.1, 10, new Size(20, 20), Size.Empty);

                    foreach (Rectangle face in faces)                    
                    {
                        imgcapture.Draw(face, new Bgr(Color.Green), 2);
                    }


                    imageBox1.Image = imgcapture;
                }
            }
        }

        private void loginForm_Load(object sender, EventArgs e)
        {

            haarCascade = new CascadeClassifier("Library\\Configs\\haarcascade_frontalface_default.xml");
        }

        private void ReleaseData()
        {
            if (_capture != null)
                _capture.Dispose();
        }


    }
}
