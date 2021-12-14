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
    }
}