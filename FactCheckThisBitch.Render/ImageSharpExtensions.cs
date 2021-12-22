using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FactCheckThisBitch.Models;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;

namespace FactCheckThisBitch.Render
{
    public static class ImageSharpExtensions
    {
        public static IImageProcessingContext ApplyScalingWaterMark(this IImageProcessingContext processingContext, Font font,
            string text, dynamic boxToDrawTextIn, Color color, float padding, bool wordwrap)
        {
            if (wordwrap)
            {
                return processingContext.ApplyScalingWaterMarkWordWrap(font, text, color, padding);
            }
            else
            {
                return ApplyScalingWaterMarkSimple(processingContext, font, text, boxToDrawTextIn, color, padding);
            }
        }

        public static IImageProcessingContext ApplyScalingWaterMarkSimple(this IImageProcessingContext processingContext,
            Font font, string text, dynamic boxToDrawTextIn, Color color, float padding)
        {
            Size imgSize = processingContext.GetCurrentSize();
            float targetWidth = boxToDrawTextIn.Right - boxToDrawTextIn.Left - (padding * 2);
            float targetHeight = boxToDrawTextIn.Bottom - boxToDrawTextIn.Top - (padding * 2);

            // measure the text size
            FontRectangle size = TextMeasurer.Measure(text, new RendererOptions(font));

            //find out how much we need to scale the text to fill the space (up or down)
            float scalingFactor = Math.Min(targetWidth / size.Width, targetHeight / size.Height);

            //create a new font
            Font scaledFont = new Font(font, scalingFactor * font.Size);

            //var drawInLocation = new PointF(imgSize.Width / 2, imgSize.Height / 2);
            var drawInLocation = new PointF(boxToDrawTextIn.Left + padding, boxToDrawTextIn.Top + padding);

            var textGraphicOptions = new TextGraphicsOptions()
            {
                TextOptions = {HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top}
            };
            return processingContext.DrawText(textGraphicOptions, text, scaledFont, color, drawInLocation);
        }

        public static IImageProcessingContext ApplyScalingWaterMarkWordWrap(this IImageProcessingContext processingContext,
            Font font, string text, Color color, float padding)
        {
            Size imgSize = processingContext.GetCurrentSize();
            float targetWidth = imgSize.Width - (padding * 2);
            float targetHeight = imgSize.Height - (padding * 2);

            float targetMinHeight = imgSize.Height - (padding * 3); // must be with in a margin width of the target height

            // now we are working i 2 dimensions at once and can't just scale because it will cause the text to
            // reflow we need to just try multiple times

            var scaledFont = font;
            FontRectangle s = new FontRectangle(0, 0, float.MaxValue, float.MaxValue);

            float scaleFactor = (scaledFont.Size / 2); // every time we change direction we half this size
            int trapCount = (int) scaledFont.Size * 2;
            if (trapCount < 10)
            {
                trapCount = 10;
            }

            bool isTooSmall = false;

            while ((s.Height > targetHeight || s.Height < targetMinHeight) && trapCount > 0)
            {
                if (s.Height > targetHeight)
                {
                    if (isTooSmall)
                    {
                        scaleFactor = scaleFactor / 2;
                    }

                    scaledFont = new Font(scaledFont, scaledFont.Size - scaleFactor);
                    isTooSmall = false;
                }

                if (s.Height < targetMinHeight)
                {
                    if (!isTooSmall)
                    {
                        scaleFactor = scaleFactor / 2;
                    }

                    scaledFont = new Font(scaledFont, scaledFont.Size + scaleFactor);
                    isTooSmall = true;
                }

                trapCount--;

                s = TextMeasurer.Measure(text, new RendererOptions(scaledFont) {WrappingWidth = targetWidth});
            }

            var center = new PointF(padding, imgSize.Height / 2);
            var textGraphicOptions = new TextGraphicsOptions()
            {
                TextOptions =
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    WrapTextWidth = targetWidth
                }
            };
            return processingContext.DrawText(textGraphicOptions, text, scaledFont, color, center);
        }

        public static string CreateNewImage(this ImageEdit imageEdit, string mediaFolder)
        {
            var originalImage = Path.Combine(mediaFolder, imageEdit.Image);
            var tempPath = Path.GetTempPath();
            var newImage = Path.Combine(tempPath, $"blurred_{imageEdit.Image}.png");

            using (Image image = Image.Load(originalImage))
            {
                using (var clone = image.Clone(p => { p.GaussianBlur(1f); }))
                {
                    foreach (var rect in imageEdit.BlurryAreas)
                    {
                        var imageSharpRect = new Rectangle(rect.Left, rect.Top, rect.Width, rect.Height);
                        clone.Mutate(x => x.Crop(imageSharpRect));
                        var brush = new ImageBrush(clone);
                        image.Mutate(c => c.Fill(brush, imageSharpRect));
                    }
                }

                image.Save(newImage);
            }

            return newImage;
        }
    }
}