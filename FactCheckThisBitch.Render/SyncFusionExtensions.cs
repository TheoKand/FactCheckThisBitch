using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using Syncfusion.Presentation;

namespace FactCheckThisBitch.Render
{
    public static class SyncFusionExtensions
    {
        public static IGroupShape GroupShape(this ISlide slide, string groupShapeName) =>
            slide.GroupShapes.First(s => s.ShapeName == groupShapeName);

        public static void UpdateText(this IGroupShape groupShape, string shapeName, string text)
        {
            var shape = groupShape.Shapes.First(s => s.ShapeName == shapeName) as IShape;
            shape.TextBody.Text = text;
        }

        public static void UpdateText(this ISlide slide, string textboxName, string text)
        {
            var shape = slide.Shapes.First(s => s.ShapeName == textboxName) as IShape;
            shape.TextBody.Text = text;
        }

        public static void ReplacePicture(this IGroupShape groupShape,string pictureName, string pictureFileName)
        {
            var picture = groupShape.Shapes.First(s => s.ShapeName == pictureName) as IPicture;
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
            var picture = slide.Pictures.First(p => p.ShapeName == pictureName);

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