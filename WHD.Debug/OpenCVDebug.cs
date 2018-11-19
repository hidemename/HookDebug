using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using WHD.Core;
using OpenCvSharp;
using System.Threading;
using System.IO;
using System.Drawing.Imaging;

namespace WHD.Debug
{
    class OpenCVDebug
    {
        public static void Debug()
        {
            Thread.Sleep(3000);

            string sourceBmpPath = AppDomain.CurrentDomain.BaseDirectory + @"Resource\test-fish.png";
            string findImageBmpPath = AppDomain.CurrentDomain.BaseDirectory + @"Resource\BigBait.png";
            //Bitmap sourceBmp = Helper2.ScreenShotAllScreen();
            //sourceBmp.Save(sourceBmpPath);

            //MemoryStream memoryStream = new MemoryStream();
            //sourceBmp.Save(memoryStream, ImageFormat.Bmp);


            Mat templateImage = new Mat(findImageBmpPath, ImreadModes.Color);
            Mat sourceImage = new Mat(sourceBmpPath, ImreadModes.Color);
            // Mat sourceImage = Mat.FromStream(memoryStream, ImreadModes.AnyColor);
            //sourceImage.SaveImage(sourceBmpPath);



            List<Tuple<TemplateMatchModes, bool, double>> list = new List<Tuple<TemplateMatchModes, bool, double>>
            {
                //new Tuple<TemplateMatchModes, bool, double>(TemplateMatchModes.SqDiff, false, 0.01),
                //new Tuple<TemplateMatchModes, bool, double>(TemplateMatchModes.SqDiffNormed, false, 0.01),
                //new Tuple<TemplateMatchModes, bool, double>(TemplateMatchModes.CCorr, true, 0.99),
                //new Tuple<TemplateMatchModes, bool, double>(TemplateMatchModes.CCorrNormed, true, 0.99),
                //new Tuple<TemplateMatchModes, bool, double>(TemplateMatchModes.CCoeff, true, 0.99),
                new Tuple<TemplateMatchModes, bool, double>(TemplateMatchModes.CCoeffNormed, true, 0.99),
            };
            foreach (var item in list)
            {
                double maxOrMin = OpenCV.DebugResults(sourceImage, templateImage, item.Item1,true);
            }
        }
    }
}
