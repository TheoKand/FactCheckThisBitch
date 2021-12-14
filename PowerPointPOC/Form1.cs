using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Syncfusion.Presentation;
using Syncfusion.Presentation.SlideTransition;

namespace PowerPointPOC
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Creates PowerPoint Presentation
            IPresentation pptxDoc = Presentation.Create();
//Adds slide to the PowerPoint
            ISlide slide = pptxDoc.Slides.Add(SlideLayoutType.Blank);
//Adds textbox to the slide
            IShape textboxShape = slide.AddTextBox(0, 0, 500, 500);
//Adds paragraph to the textbody of text box
            IParagraph paragraph = textboxShape.TextBody.AddParagraph();
//Adds a TextPart to the paragraph
            ITextPart textPart = paragraph.AddTextPart();
//Adds text to the TextPart
            textPart.Text = "AdventureWorks Cycles, the fictitious company on which the AdventureWorks sample databases are based, is a large, multinational manufacturing company. The company manufactures and sells metal and composite bicycles to North American, European and Asian commercial markets. While its base operation is located in Washington with 290 employees, several regional sales teams are located throughout their market base.";
//Saves the Presentation
            pptxDoc.Save("Output.pptx");
//Closes the Presentation
            pptxDoc.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var path =
                "C:\\Users\\theok\\source\\repos\\FactCheckThisBitch\\FactCheckThisBitch.Admin.Windows\\data\\template.pptx";
            IPresentation doc = Presentation.Open(path);

            var first = doc.Slides[1];

            var clone = first.Clone();

            clone.SlideTransition.TransitionEffect = TransitionEffect.None;

            doc.Slides.Add(clone);
            doc.Slides.Add(clone);
            doc.Slides.Add(clone);
            doc.Slides.Add(clone);
            
            ////var trans = first.SlideTransition;
            ////var second = doc.Slides[2];

            //var metadataGroupShape = doc.Slides[1].Shapes.First(s => s.ShapeName == "group_metadata") as IGroupShape;
            //var thesis = metadataGroupShape.Shapes.First(s => s.ShapeName == "piece_thesis") as IShape;

            //thesis.TextBody.Text = thesis.TextBody.Text + "changed";

            doc.Save(path);


            doc.Close();


//            IPresentation pptxDoc = Presentation.Open("Sample.pptx");

////Retrieve the first slide from the presentation
//            ISlide slide = pptxDoc.Slides[0];

////Modify the transition effect applied to the slide
//            slide.SlideTransition.TransitionEffect = TransitionEffect.Cover;

////Set the transition subtype
//            slide.SlideTransition.TransitionEffectOption = TransitionEffectOption.Right;

////Save the presentation
//            pptxDoc.Save("Transition.pptx");

////Close the presentation
//            pptxDoc.Close();
        }
    }
}
