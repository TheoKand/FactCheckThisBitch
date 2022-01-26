﻿using System;
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
            slide.GroupShapes.FirstOrDefault(s => s.ShapeName == groupShapeName);

        public static IShape UpdateText(this IGroupShape groupShape, string shapeName, string text)
        {
            if (groupShape == null) return null;
            var shape = groupShape.Shapes.FirstOrDefault(s => s.ShapeName == shapeName) as IShape;
            if (shape == null) return null;
            shape.TextBody.Text = text;
            return shape;
        }

        public static void UpdateText(this ISlide slide, string textboxName, string text)
        {
            var shape = slide?.Shapes.FirstOrDefault(s => s.ShapeName == textboxName) as IShape;
            if (shape == null) return;
            shape.TextBody.Text = text;
        }

        public static void ReplacePicture(this IGroupShape groupShape, string pictureName, string pictureFileName)
        {
            if (groupShape == null) return;
            var picture = groupShape.Shapes.FirstOrDefault(s => s.ShapeName == pictureName) as IPicture;
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