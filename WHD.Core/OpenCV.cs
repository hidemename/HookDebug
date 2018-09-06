using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WHD.Core
{
    public class OpenCV
    {
        /// <summary>
        /// 模板匹配并且返回最佳匹配
        /// </summary>
        /// <param name="sourceImage"></param>
        /// <param name="templateImage"></param>
        /// <param name="model"></param>
        /// <param name="isDebug"></param>
        /// <returns></returns>
        private static Tuple<Mat, Point,double> MyMatchTemplate(Mat sourceImage, Mat templateImage, TemplateMatchModes method, out Mat grayScaleImage, bool isDebug = false)
        {
            Mat matchResultImage;
            Point matchPoint;
            double matchValue;
            //1.构建结果图像resultImg(注意大小和类型)
            //如果原图(待搜索图像)尺寸为W x H, 而模版尺寸为 w x h, 则结果图像尺寸一定是(W-w+1)x(H-h+1)
            //结果图像必须为单通道32位浮点型图像
            int width = sourceImage.Cols - templateImage.Cols + 1;
            int height = sourceImage.Rows - templateImage.Rows + 1;
            if (width < 1 || height < 1)
            {
                throw new ArgumentException("width or height is 0");
            }
            grayScaleImage = new Mat(width, height, MatType.CV_32FC1);

            matchResultImage = new Mat(templateImage.Cols, templateImage.Rows, sourceImage.Type());
            //2.模版匹配
            Cv2.MatchTemplate(sourceImage, templateImage, grayScaleImage, method);
            #region remarks
            //    CV_EXPORTS_W void matchTemplate( InputArray image, InputArray templ,
            //                                     OutputArray result, int method, InputArray mask = noArray() );
            //    image
            //    欲搜索的图像。它应该是单通道、8-比特或32-比特 浮点数图像
            //    templ
            //    搜索模板，不能大于输入图像，且与输入图像具有一样的数据类型
            //    result
            //    比较结果的映射图像。单通道、32-比特浮点数. 如果图像是 W×H 而 templ 是 w×h ，则 result 一定是 (W-w+1）×(H-h+1）.
            //    method
            //    指定匹配方法：
            //    函数 MatchTemplate 与函数 CalcBackProjectPatch 类似。它滑动过整个图像 image, 用指定方法比较 templ 与图像尺寸
            //    为 w×h 的重叠区域，并且将比较结果存到 result 中。 下面是不同的比较方法，可以使用其中的一种 (I 表示图像，T - 模板, R - 结果.
            //    模板与图像重叠区域 x'=0..w-1, y'=0..h-1 之间求和):
            //    CV_TM_SQDIFF   ： 平方差匹配，最好时候为0；
            //    CV_TM_SQDIFF_NORMED ：归一化平方差匹配，最好时候为0；
            //    CV_TM_CCORR      ：相关性匹配法，最差为0；
            //    CV_TM_CCORR_NORMED  ：归一化相关性匹配法，最差为0；
            //    CV_TM_CCOEFF     ：系数匹配法，最好匹配为1；
            //    CV_TM_CCOEFF_NORMED  ：化相关系数匹配法，最好匹配为1；
            #endregion
            //3.正则化(归一化到0-1)
            Cv2.Normalize(grayScaleImage, grayScaleImage, 0, 1, NormTypes.MinMax, -1);

            //4.找出resultImg中的最大值及其位置
            Cv2.MinMaxLoc(grayScaleImage, out double minVal, out double maxVal, out Point minLoc, out Point maxLoc);
            if (method == TemplateMatchModes.SqDiff || method == TemplateMatchModes.SqDiff){
                matchPoint = minLoc;
                matchValue = minVal;
            }
            else{
                matchPoint = maxLoc;
                matchValue = maxVal;
            }
            //切图
            Cv2.GetRectSubPix(sourceImage,
                new Size(templateImage.Width, templateImage.Height),
                new Point2f(matchPoint.X + templateImage.Width / 2, matchPoint.Y + templateImage.Height), matchResultImage);
            if (isDebug)
            {
                Cv2.Rectangle(sourceImage, matchPoint, new Point(matchPoint.X + templateImage.Cols, matchPoint.Y + templateImage.Rows), new Scalar(0, 255, 0), 2, LineTypes.Link8);
                Cv2.PutText(sourceImage, matchValue + "", new Point(matchPoint.X, matchPoint.Y + 100), HersheyFonts.HersheySimplex, 1, new Scalar(0, 255, 255), 2);

                Cv2.PutText(sourceImage, method.ToString() + ":" + matchValue, new Point(0, 100), HersheyFonts.HersheySimplex, 1, new Scalar(255, 0, 255), 2);
                //Cv2.ImShow("result", result);
                //Cv2.ImShow("source", source);
                //Cv2.ImShow("template", template);
                //Cv2.WaitKey(0);

            }
            return new Tuple<Mat, Point, double>(matchResultImage,matchPoint,matchValue);
        }
        /// <summary>
        /// 计算直方图
        /// </summary>
        /// <param name="source"></param>
        /// <param name="template"></param>
        /// <param name="model"></param>
        /// <param name="isDebug"></param>
        /// <returns></returns>
        private static double MyCompareHistHsv(Mat original, Mat copy, HistCompMethods method, bool isDebug = false)
        {
            Mat originalHsv = original.CvtColor(ColorConversionCodes.BGR2HSV);
            Mat copyHsv = copy.CvtColor(ColorConversionCodes.BGR2HSV);

            int h_bins = 50; int s_bins = 32;
            int[] histSize = { h_bins, s_bins };

            Rangef h_ranges = new Rangef(0, 256);
            Rangef s_ranges = new Rangef(0, 180);

            Rangef[] ranges = { h_ranges, s_ranges };

            int[] channels = { 0, 1 };

            Mat originalHist = new Mat();
            Mat copyHist = new Mat();

            Cv2.CalcHist(new Mat[] { originalHsv }, channels, new Mat(), originalHist, 2, histSize, ranges);
            Cv2.Normalize(originalHist, originalHist, 0, 1, NormTypes.MinMax, -1);
            Cv2.CalcHist(new Mat[] { copyHsv }, channels, new Mat(), copyHist, 2, histSize, ranges);
            Cv2.Normalize(copyHist, copyHist, 0, 1, NormTypes.MinMax, -1);



            return Cv2.CompareHist(originalHist, copyHist, method);
        }
        private static Mat[] MyCalcHistBGR(Mat image,out Mat histImage, bool isDebug = false)
        {
            //Mat imageHsv = image.CvtColor(ColorConversionCodes.BGR2HSV);
            Mat[] subImage = image.Split();
            // 设置统计bins(像素框)数目，最大255
            int[] histSize = { 255 };
            /// 设定取值范围 ( R,G,B) )
            Rangef range = new Rangef(0, 255);
            Rangef[] ranges = { range };
            Mat[] subImageHist = new Mat[3] { new Mat(), new Mat(), new Mat() };
            Mat[] resultHist = new Mat[3] { new Mat(), new Mat(), new Mat() };
            int[] channels = {0};

            Cv2.CalcHist(new Mat[] { subImage[0] }, channels, new Mat(), subImageHist[0], 1, histSize, ranges);
            Cv2.CalcHist(new Mat[] { subImage[1] }, channels, new Mat(), subImageHist[1], 1, histSize, ranges);
            Cv2.CalcHist(new Mat[] { subImage[2] }, channels, new Mat(), subImageHist[2], 1, histSize, ranges);


            Cv2.Normalize(subImageHist[0], resultHist[0], 0, 1, NormTypes.MinMax, -1);
            Cv2.Normalize(subImageHist[1], resultHist[1], 0, 1, NormTypes.MinMax, -1);
            Cv2.Normalize(subImageHist[2], resultHist[2], 0, 1, NormTypes.MinMax, -1);

            histImage = null;
            if (isDebug)
            {
                // 创建直方图画布
                int hist_w = 400; int hist_h = 400;
                int bin_w = (int)Math.Round((double)hist_w / histSize[0],MidpointRounding.AwayFromZero);

                histImage = new Mat(new Size(hist_w, hist_h), MatType.CV_8UC3, new Scalar(0, 0, 0));
                Cv2.Normalize(subImageHist[0], subImageHist[0], 0, histImage.Rows, NormTypes.MinMax, -1);
                Cv2.Normalize(subImageHist[1], subImageHist[1], 0, histImage.Rows, NormTypes.MinMax, -1);
                Cv2.Normalize(subImageHist[2], subImageHist[2], 0, histImage.Rows, NormTypes.MinMax, -1);

                /// 在直方图画布上画出直方图
                for (int i = 1; i < histSize[0]; i++)
                {
                    Cv2.Line(histImage, new Point(bin_w * (i - 1), hist_h - (int)Math.Round(subImageHist[0].At<float>(i - 1))),
                                        new Point(bin_w * (i), hist_h - (int)Math.Round(subImageHist[0].At<float>(i))),
                                        Scalar.Blue, 2);
                    Cv2.Line(histImage, new Point(bin_w * (i - 1), hist_h - (int)Math.Round(subImageHist[1].At<float>(i - 1))),
                                        new Point(bin_w * (i), hist_h - (int)Math.Round(subImageHist[1].At<float>(i))),
                                        Scalar.LightSteelBlue, 2);
                    Cv2.Line(histImage, new Point(bin_w * (i - 1), hist_h - (int)Math.Round(subImageHist[2].At<float>(i - 1))),
                                        new Point(bin_w * (i), hist_h - (int)Math.Round(subImageHist[2].At<float>(i))),
                                        Scalar.Red, 2);
                }
            }
            return resultHist;

        }
        private static Mat MyCalcHistGray(Mat image, out Mat histImage, bool isDebug = false)
        {
            Mat[] subImage = image.Split();
            // 设置统计bins(像素框)数目，最大255
            int[] histSize = { 255 };
            /// 设定取值范围 ( Gray) )
            Rangef range = new Rangef(0, 255);
            Rangef[] ranges = { range };
            Mat[] subImageHist = new Mat[1] { new Mat()};
            Mat resultHist = new Mat();
            int[] channels = { 0 };

            Cv2.CalcHist(new Mat[] { subImage[0] }, channels, new Mat(), subImageHist[0], 1, histSize, ranges);


            Cv2.Normalize(subImageHist[0], resultHist, 0, 1, NormTypes.MinMax, -1);

            histImage = null;
            if (isDebug)
            {
                // 创建直方图画布
                int hist_w = 400; int hist_h = 400;
                int bin_w = (int)Math.Round((double)hist_w / histSize[0], MidpointRounding.AwayFromZero);

                histImage = new Mat(new Size(hist_w, hist_h), MatType.CV_8UC3, new Scalar(0, 0, 0));
                Cv2.Normalize(subImageHist[0], subImageHist[0], 0, histImage.Rows, NormTypes.MinMax, -1);

                /// 在直方图画布上画出直方图
                for (int i = 1; i < histSize[0]; i++)
                {
                    Cv2.Line(histImage, new Point(bin_w * (i - 1), hist_h - (int)Math.Round(subImageHist[0].At<float>(i - 1))),
                                        new Point(bin_w * (i), hist_h - (int)Math.Round(subImageHist[0].At<float>(i))),
                                        Scalar.White, 2);
                }
            }
            return resultHist;

        }
        private static double[] MyCompareHistBGR(Mat originalImage, Mat copyImage, HistCompMethods method, bool isDebug = false)
        {
            double[] result;
            Mat[] originalResults = MyCalcHistBGR(originalImage, out Mat originalHistImage, isDebug);
            Mat[] copylResults = MyCalcHistBGR(copyImage, out Mat copyHistImage, isDebug);

            result = new double[originalResults.Count()];
            for (int i = 0; i < originalResults.Count(); i++)
            {
                result[i] = Cv2.CompareHist(originalResults[i], copylResults[i], method);
            }
            return result;
        }
        private static void MyImShow(Mat image, string title = "Debug",  bool isDebug = true)
        {
            if (isDebug)
            {
                Cv2.ImShow(title, image);
            }
        }
        public static double DebugResults(string sourcePath, string templatePath, TemplateMatchModes method = TemplateMatchModes.CCoeffNormed, bool isDebug = false)
        {
            Mat templateImage = new Mat(templatePath, ImreadModes.Color);
            Mat sourceImage = new Mat(sourcePath, ImreadModes.Color);
            return DebugResults(sourceImage, templateImage, method, true);
        }
        public static double DebugResults(Mat sourceImage, Mat templateImage, TemplateMatchModes method = TemplateMatchModes.CCoeffNormed, bool isDebug = false)
        {
            var matchResult = MyMatchTemplate(sourceImage, templateImage, method, out Mat grayScaleImage, true);

            //MyImShow( sourceImage, "sourceImage", isDebug);
            //MyImShow( grayScaleImage, "grayScaleImage", isDebug);
            //MyImShow( sourceHistImage, "sourceHistImage", isDebug);

            //Cv2.WaitKey(0);

            if (isDebug)
            {
                var histCompMethods = new[]{
                HistCompMethods.Bhattacharyya,
                HistCompMethods.Chisqr,
                HistCompMethods.ChisqrAlt,
                HistCompMethods.Correl,
                HistCompMethods.Hellinger,
                HistCompMethods.Intersect,
                HistCompMethods.KLDiv};
                Console.WriteLine("CompareHist:");
                for (int i = 0; i < histCompMethods.Length; i++)
                {
                    double[] compareHistResult = MyCompareHistBGR(templateImage, matchResult.Item1, histCompMethods[i], isDebug);
                    Console.WriteLine($"B:{compareHistResult[0]}\tG:{compareHistResult[1]}\tR:{compareHistResult[2]}");
                    //Cv2.PutText(source, info, new Point(0, 200 + i * 100), HersheyFonts.HersheySimplex, 1, new Scalar(255, 255, 0), 2);
                }
                Cv2.MeanStdDev(grayScaleImage, out Scalar meanResult, out Scalar stddevResult);
                Console.WriteLine("MeanStdDev");
                Console.WriteLine($"meanResult:{meanResult}\tR:{stddevResult}");
            }
            Console.Read();
            return 0;

        }
        public static void SimpleCheckImageLike(System.Drawing.Bitmap sourceBmp, System.Drawing.Bitmap templateBmp, out double avgHistResult, out double meanResult, out double stddevResult)
        {
            MemoryStream sourceStream = new MemoryStream();
            sourceBmp.Save(sourceStream, System.Drawing.Imaging.ImageFormat.Bmp);
            MemoryStream templateStream = new MemoryStream();
            templateBmp.Save(templateStream, System.Drawing.Imaging.ImageFormat.Bmp);


            Mat sourceImage = Mat.FromStream(sourceStream, ImreadModes.AnyColor);
            Mat templateImage = Mat.FromStream(templateStream, ImreadModes.AnyColor);
            

            var matchResult = MyMatchTemplate(sourceImage, templateImage, TemplateMatchModes.CCoeffNormed,out Mat grayScaleImage);

            double[] compareHistResultArray = MyCompareHistBGR(templateImage, matchResult.Item1, HistCompMethods.KLDiv);

            avgHistResult = 0;
            foreach (var item in compareHistResultArray)
            {
                avgHistResult += item;
            }
            avgHistResult /= compareHistResultArray.Count();

            
            Cv2.MeanStdDev(grayScaleImage, out Scalar mean, out Scalar stddev);
            meanResult = mean[0];
            stddevResult = stddev[0];
            return;
        }
        //老代码-模板匹配，简单的寻找拷贝图，效果一般
        private static bool IsMatchTemplateImageCount(string sourcePath,string looingForPath,bool debug = true)
        {
            //Cv2.ImRead()
            //Mat looingFor2 = Cv2.ImDecode(new InputArray()



            Mat looingFor = new Mat(looingForPath, ImreadModes.Color);
            Mat source = new Mat(sourcePath, ImreadModes.Color);

            //1.构建结果图像resultImg(注意大小和类型)
            //如果原图(待搜索图像)尺寸为W x H, 而模版尺寸为 w x h, 则结果图像尺寸一定是(W-w+1)x(H-h+1)
            //结果图像必须为单通道32位浮点型图像
            int width = source.Cols - looingFor.Cols + 1;
            int height = source.Rows - looingFor.Rows + 1;
            if (width < 1 || height < 1)
            {
                return false;
            }
            Mat result = new Mat(width, height, MatType.CV_32FC1);
            //2.模版匹配
            Cv2.MatchTemplate(source, looingFor, result, TemplateMatchModes.CCoeffNormed);
            //    CV_EXPORTS_W void matchTemplate( InputArray image, InputArray templ,
            //                                     OutputArray result, int method, InputArray mask = noArray() );
            //    image
            //    欲搜索的图像。它应该是单通道、8-比特或32-比特 浮点数图像
            //    templ
            //    搜索模板，不能大于输入图像，且与输入图像具有一样的数据类型
            //    result
            //    比较结果的映射图像。单通道、32-比特浮点数. 如果图像是 W×H 而 templ 是 w×h ，则 result 一定是 (W-w+1）×(H-h+1）.
            //    method
            //    指定匹配方法：
            //    函数 MatchTemplate 与函数 CalcBackProjectPatch 类似。它滑动过整个图像 image, 用指定方法比较 templ 与图像尺寸
            //    为 w×h 的重叠区域，并且将比较结果存到 result 中。 下面是不同的比较方法，可以使用其中的一种 (I 表示图像，T - 模板, R - 结果.
            //    模板与图像重叠区域 x'=0..w-1, y'=0..h-1 之间求和):
            //    CV_TM_SQDIFF   ： 平方差匹配，最好时候为0；
            //    CV_TM_SQDIFF_NORMED ：归一化平方差匹配，最好时候为0；
            //    CV_TM_CCORR      ：相关性匹配法，最差为0；
            //    CV_TM_CCORR_NORMED  ：归一化相关性匹配法，最差为0；
            //    CV_TM_CCOEFF     ：系数匹配法，最好匹配为1；
            //    CV_TM_CCOEFF_NORMED  ：化相关系数匹配法，最好匹配为1；

            //3.正则化(归一化到0-1)
            Cv2.Normalize(result, result, 0, 1, NormTypes.MinMax, -1);


            //Cv2.ImShow("result", result);
            //4.找出resultImg中的最大值及其位置
            int tempX = 0;
            int tempY = 0;
            string prob;

            int count = 0;

            //4.1遍历resultImg
            for (int i = 0; i < result.Rows; i++)
            {
                for (int j = 0; j < result.Cols; j++)
                {
                    //4.2获得resultImg中(j,x)位置的匹配值matchValue
                    double matchValue = result.At<float>(i, j);
                    prob = string.Format($"{matchValue}");
                    //4.3给定筛选条件
                    //条件1:概率值大于0.9
                    //条件2:任何选中的点在x方向和y方向上都要比上一个点大5
                    if (matchValue >=1 && Math.Abs(i - tempY) > 5 && Math.Abs(j - tempX) > 5)
                    {
                        //5.给筛选出的点画出边框和文字

                        if (debug)
                        {
                            Cv2.Rectangle(source, new Point(j, i), new Point(j + looingFor.Cols, i + looingFor.Rows), new Scalar(0, 255, 0), 2, LineTypes.Link8);

                            Cv2.PutText(source, prob, new Point(j, i + 100), HersheyFonts.HersheySimplex, 1, new Scalar(0, 255, 255), 2);
                        }
                        tempX = j;
                        tempY = i;
                        count++;
                    }
                }
            }

            if (count==1)
            {
                if (debug)
                {
                    Cv2.ImShow("result", result);
                    Cv2.ImShow("source", source);
                    //Cv2.ImShow("looingFor", looingFor);
                    Cv2.WaitKey(0);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
