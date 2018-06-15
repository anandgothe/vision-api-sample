using Microsoft.ProjectOxford.Vision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisionApiSample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                pictureBox1.ImageLocation = openFileDialog1.FileName;
                var file = System.IO.File.OpenRead(openFileDialog1.FileName);

                // Refer to this blog for all the preceding steps and more info:
                // https://devonblog.com/software-development/artificial-intelligence/censor-pictures-…puter-vision-api/

                // TODO: you have to use your own KEY here.
                var client = new VisionServiceClient("ye2f5cb4ee15341ba8f7c4f0d90ff542c",
                    "https://southeastasia.api.cognitive.microsoft.com/vision/v1.0");

                var visualFeatures = new VisualFeature[] { VisualFeature.Adult, VisualFeature.Categories, VisualFeature.Faces, VisualFeature.ImageType, VisualFeature.Tags };

                var analysisResult = client.AnalyzeImageAsync(file, visualFeatures, new[] { "celebrities" });
                analysisResult.Wait();

                var result = analysisResult.Result;

                // check out these extension methods on AnalysisResultExtension class

                var resultText = $@"Adult/racy content: {result.IsAdultOrRacyContent()}
Clear human face found: {result.HasAFace()} 
Is there a child: {!result.IsYoungerThan(14)},
Celebrity: {result.DetectCelebrity()}";

                label1.Text = resultText;
            }
            catch (AggregateException ex)
            {
                MessageBox.Show(ex.InnerException.Message + " Make sure you are using a valid API KEY ");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }
    }
}
