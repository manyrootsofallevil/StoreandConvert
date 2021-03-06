﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertHelper
{
    public class ImageModifier
    {
        string FileName { get; set; }

        public ImageModifier(string fileName)
        {
            FileName = fileName;
        }

        public void WriteMessageToImage(string message, string outputFile)
        {

            Color FillColor = Color.FromArgb(127, 255, 255, 255);
            SolidBrush FillBrush = new SolidBrush(FillColor);
            Rectangle FillRectangle = new Rectangle(0, 0, 227, 50);

            Font TextFont = new Font("Comic Sans MS", 18);
            SolidBrush TextBrush = new SolidBrush(Color.Navy);
            StringFormat TextFormat = new StringFormat();
            TextFormat.Alignment = StringAlignment.Center;
            TextFormat.LineAlignment = StringAlignment.Center;
            System.Drawing.Image GreetingImage = System.Drawing.Image.FromFile(FileName);
            Graphics DrawingSurface = Graphics.FromImage(GreetingImage);
            DrawingSurface.FillRectangle(FillBrush, FillRectangle);
            DrawingSurface.DrawString(message, TextFont, TextBrush, FillRectangle, TextFormat);
            GreetingImage.Save(outputFile, ImageFormat.Jpeg);
        }

        public void WriteMessageToImage(string title, string message, string outputFile)
        {

            Color FillColor = Color.FromArgb(127, 255, 255, 255);
            SolidBrush FillBrush = new SolidBrush(FillColor);
            Rectangle TitleRectangle = new Rectangle(0, 0, 227, 35);
            Rectangle messageRectangle = new Rectangle(0, 35, 227, 35);

            Font TextFont = new Font("Comic Sans MS", 18);
            SolidBrush TextBrush = new SolidBrush(Color.Navy);
            StringFormat TextFormat = new StringFormat();
            TextFormat.Alignment = StringAlignment.Center;
            TextFormat.LineAlignment = StringAlignment.Center;
            System.Drawing.Image GreetingImage = System.Drawing.Image.FromFile(FileName);
            Graphics DrawingSurface = Graphics.FromImage(GreetingImage);
            DrawingSurface.FillRectangle(FillBrush, TitleRectangle);
            DrawingSurface.DrawString(title, TextFont, TextBrush, TitleRectangle, TextFormat);
            DrawingSurface.FillRectangle(FillBrush, messageRectangle);
            DrawingSurface.DrawString(message, TextFont, TextBrush, messageRectangle, TextFormat);
            GreetingImage.Save(outputFile, ImageFormat.Jpeg);
        }

        public override string ToString()
        {
            return FileName;
        }
    }
}
