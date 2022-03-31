using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using FackCheckThisBitch.Common;
using SixLabors.ImageSharp;
using Syncfusion.OfficeChart;
using Syncfusion.Presentation;

namespace FactCheckThisBitch.Render
{
    public static class SyncFusionExtensions
    {
        public static IGroupShape GroupShape(this ISlide slide, string groupShapeName) =>
            slide.GroupShapes.FirstOrDefault(s => s.ShapeName == groupShapeName);

        public static void SetShapeTextWithColor(this IShape shape, string text, System.Drawing.Color color)
        {
            shape.TextBody.Paragraphs.Clear();

            //Adds paragraph to the textbody of textbox
            IParagraph paragraph2 = shape.TextBody.AddParagraph();

            //Adds a TextPart to the paragraph
            ITextPart textPartFormatting = paragraph2.AddTextPart();

            //Adds text to the TextPart
            textPartFormatting.Text = text ?? "";

  
            //Retrieves the existing font for modification
            IFont font = textPartFormatting.Font;

            //Sets the font color
            font.Color.SystemColor = color;

        }

        public static void SetShapeTextColor(this IShape shape, System.Drawing.Color color)
        {

            //Adds paragraph to the textbody of textbox
            IParagraph paragraph2 = shape.TextBody.Paragraphs.First();

            //Adds a TextPart to the paragraph
            ITextPart textPartFormatting = paragraph2.TextParts.First();

            //Retrieves the existing font for modification
            IFont font = textPartFormatting.Font;

            //Sets the font color
            font.Color.SystemColor = color;

        }

        public static void RemoveShapeIfTextEmpty(this ISlide slide, string shapeName)
        {
            if (slide == null) return;
            var shape = slide.Shapes.FirstOrDefault(s => s.ShapeName == shapeName) as IShape;
            if (shape == null) return;
            if (shape.TextBody.Text.IsEmpty())
            {
                slide.HideShapeAndRemoveAnimations(shape);
            }
        }

        public static void HideShapeAndRemoveAnimations(this ISlide slide, string shapeName)
        {
            var shape = slide.ShapeByName(shapeName) as IShape;
            if (shape == null) return;
            shape.Left = slide.SlideSize.Width + 100;
            slide.RemoveAnimationsForShape(shape);
        }

        public static void HideShapeAndRemoveAnimations(this ISlide slide, IShape shape)
        {
            shape.Left = slide.SlideSize.Width + 100;
            slide.RemoveAnimationsForShape(shape);
        }

        public static void RemoveAnimationsForShape(this ISlide slide, string shapeName)
        {
            if (slide == null) return;
            var shape = slide.Shapes.FirstOrDefault(s => s.ShapeName == shapeName) as IShape;
            slide.RemoveAnimationsForShape(shape);
        }

        public static void RemoveAnimationsForShape(this ISlide slide, IShape shape)
        {
            if (slide == null || shape==null) return;
            slide.Timeline.MainSequence.RemoveByShape(shape);
        }

        public static IShape UpdateText(this IGroupShape groupShape, string shapeName, string text)
        {
            if (groupShape == null) return null;
            var shape = groupShape.GetShapeFromGroupShape(shapeName);
            shape.TextBody.Text = text ?? "";
            return shape;
        }

        public static IShape GetShapeFromGroupShape(this IGroupShape groupShape, string shapeName)
        {
            if (groupShape == null) return null;
            var shape = groupShape.Shapes.FirstOrDefault(s => s.ShapeName == shapeName) as IShape;
            return shape;
        }

        public static ISlideItem ShapeByName(this ISlide slide,string shapeName)
        {
            return slide.Shapes.FirstOrDefault(_ => _.ShapeName == shapeName);
        }

        /// <summary>
        /// Resize and center this shape inside the slide
        /// </summary>
        public static void CenterShape(this ISlide slide, IShape shape,int width,int height)
        {

            shape.Width = width;
            shape.Height = height;
            shape.Left = slide.SlideSize.Width / 2 - width / 2;
            shape.Top = slide.SlideSize.Height / 2 - height / 2;
        }


        public static IShape UpdateText(this ISlide slide, string textboxName, string text)
        {
            var shape = slide?.Shapes.FirstOrDefault(s => s.ShapeName == textboxName) as IShape;
            if (shape == null) return null;
            shape.TextBody.Text = text ?? "";
            return shape;
        }

        public static void ReplacePicture(this IGroupShape groupShape, string pictureName, string pictureFileName)
        {
            if (groupShape == null) return;
            var picture = groupShape.GetShapeFromGroupShape(pictureName) as IPicture;
            if (picture == null) return;
            using (Stream pictureStream = File.Open(pictureFileName, FileMode.Open))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    pictureStream.CopyTo(memoryStream);
                    picture.ImageData = memoryStream.ToArray();
                }
            }
        }

        public static void ReplacePicture(this ISlide slide, string pictureName, string pictureFileName)
        {
            var picture = slide?.Pictures.FirstOrDefault(p => p.ShapeName == pictureName);
            if (picture == null) return;

            using (Stream pictureStream = File.Open(pictureFileName, FileMode.Open))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    pictureStream.CopyTo(memoryStream);
                    picture.ImageData = memoryStream.ToArray();
                }
            }
        }

        public static void LoadPicture(this IPicture picture, string pictureFileName)
        {
            if (picture == null) return;

            using (Stream pictureStream = File.Open(pictureFileName, FileMode.Open))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    pictureStream.CopyTo(memoryStream);
                    picture.ImageData = memoryStream.ToArray();
                }
            }
        }

        public static double PointsToPixels(this double points)
        {
            return points * 1.3333333333333333;
        }

        public static double PixelsToPoints(this double pixels)
        {
            return pixels / 1.3333333333333333;
        }

        public static double PointsToCm(this double points)
        {
            return points * 28.346;
        }

        public static double CmToPoints(this double cm)
        {
            return cm / 28.346;
        }

        public static double CmToPixels(this double cm)
        {
            return cm * 37.7952755906;
        }

        public static double PixelsToCm(this double pixels)
        {
            return pixels * 0.0264583333;
        }
    }
}