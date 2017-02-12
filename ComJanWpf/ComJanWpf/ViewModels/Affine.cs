using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace ComJan
{
    /// <summary>
    /// 画像のアフィン変換
    /// </summary>
    /// <remarks>
    /// http://opencv.jp/sample/sampling_and_geometricaltransforms.html#affine_1
    /// </remarks>
    class Affine
    {
        public Affine()
        {
        }

        public Bitmap CalcAffine(Bitmap bmp, Point[] srcPoint, Point[] dstPoint)
        {
            // cvGetAffineTransform + cvWarpAffine
            // 画像上の３点対応よりアフィン変換行列を計算し，その行列を用いて画像全体のアフィン変換を行う．

            // (1)画像の読み込み，出力用画像領域の確保を行なう
            //using (IplImage srcImg = new IplImage(filepath, LoadMode.AnyDepth | LoadMode.AnyColor))
            using(IplImage srcImg = BitmapConverter.ToIplImage(bmp))
            using (IplImage dstImg = srcImg.Clone())
            {
                // (2)三角形の回転前と回転後の対応する頂点をそれぞれセットし  
                //    cvGetAffineTransformを用いてアフィン行列を求める  
                CvPoint2D32f[] srcPnt = new CvPoint2D32f[4];
                CvPoint2D32f[] dstPnt = new CvPoint2D32f[4];
                srcPnt[0] = new CvPoint2D32f((double)srcPoint[0].X, (double)srcPoint[0].Y);
                srcPnt[1] = new CvPoint2D32f((double)srcPoint[1].X, (double)srcPoint[1].Y);
                srcPnt[2] = new CvPoint2D32f((double)srcPoint[2].X, (double)srcPoint[2].Y);
                srcPnt[3] = new CvPoint2D32f((double)srcPoint[3].X, (double)srcPoint[3].Y);

                dstPnt[0] = new CvPoint2D32f((double)dstPoint[0].X, (double)dstPoint[0].Y);
                dstPnt[1] = new CvPoint2D32f((double)dstPoint[1].X, (double)dstPoint[1].Y);
                dstPnt[2] = new CvPoint2D32f((double)dstPoint[2].X, (double)dstPoint[2].Y);
                dstPnt[3] = new CvPoint2D32f((double)dstPoint[3].X, (double)dstPoint[3].Y);

                using (CvMat mapMatrix = Cv.GetPerspectiveTransform(srcPnt, dstPnt))
                {
                    // (3)指定されたアフィン行列により，cvWarpAffineを用いて画像を回転させる
                    Cv.WarpPerspective(srcImg, dstImg, mapMatrix, Interpolation.Linear | Interpolation.FillOutliers, CvScalar.ScalarAll(0));

                    //System.Diagnostics.Debug.WriteLine("srcPoint");
                    //for (int i = 0; i < 4; i++)
                    //{
                    //    System.Diagnostics.Debug.WriteLine(srcPoint[i].X.ToString() + "\t" + srcPoint[i].Y.ToString());
                    //}

                    //System.Diagnostics.Debug.WriteLine("");

                    //System.Diagnostics.Debug.WriteLine("dstPoint");
                    //for (int i = 0; i < 4; i++)
                    //{
                    //    System.Diagnostics.Debug.WriteLine(dstPoint[i].X.ToString() + "\t" + dstPoint[i].Y.ToString());
                    //}

                    //System.Diagnostics.Debug.WriteLine("");

                    //for (int r = 0; r < mapMatrix.Rows; r++)
                    //{
                    //    for (int c = 0; c < mapMatrix.Cols; c++)
                    //    {
                    //        System.Diagnostics.Debug.Write(mapMatrix[r][c].ToString() + "\t");
                    //    }
                    //    System.Diagnostics.Debug.WriteLine("");
                    //}

                    // (4)結果を表示する
                    return dstImg.ToBitmap();
                }
            }
        }

        public Bitmap Resize(Bitmap bmp, Point[] srcPoint, Point[] dstPoint)
        {
            using (IplImage srcImg = BitmapConverter.ToIplImage(bmp))
            {
                CvSize size = new CvSize(dstPoint[1].X - dstPoint[0].X + 1, dstPoint[2].Y - dstPoint[1].Y + 1);

                using (IplImage dstImg = new IplImage(size, srcImg.Depth, srcImg.NChannels))
                {
                    Cv.Resize(srcImg, dstImg, Interpolation.Lanczos4);

                    return dstImg.ToBitmap();
                }
            }
        }
    }
}