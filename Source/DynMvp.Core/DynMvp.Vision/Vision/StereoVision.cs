using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DynMvp.Vision
{
    public class SingleVisionCalibData
    {
        public string DeviceSerialNumber { get; set; } = "";

        public int Width { get; set; }

        public int Height { get; set; }

        public double FocalX { get; set; }

        public double FocalY { get; set; }

        public double PrincipleX { get; set; }

        public double PrincipleY { get; set; }

        public double[] DistortionCoef { get; set; }


        public void Save(string calibFile)
        {
            File.WriteAllText(calibFile, JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        public static SingleVisionCalibData FromFile(string calibFile)
        {
            if (File.Exists(calibFile) == true)
            {
                SingleVisionCalibData calibData = JsonConvert.DeserializeObject<SingleVisionCalibData>(File.ReadAllText(calibFile));
                return calibData;
            }
            else
            {
                MessageBox.Show(null, "CalibFile is not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }

    public class StereoVisionCalibData
    {
        public double[,] Rotation { get; set; } = null;

        public double[,] Translation { get; set; } = null;

        public double[,] Affine { get; set; } = null;

        public void Save(string calibFile)
        {
            File.WriteAllText(calibFile, JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        public static StereoVisionCalibData FromFile(string calibFile)
        {
            if (File.Exists(calibFile) == true)
            {
                StereoVisionCalibData calibData = JsonConvert.DeserializeObject<StereoVisionCalibData>(File.ReadAllText(calibFile));
                return calibData;
            }
            else
            {
                MessageBox.Show(null, "CalibFile is not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }

    public class SingleVision
    {
        public SingleVision()
        {
        }


        public int Width { get; private set; } = -1;

        public int Height { get; private set; } = -1;

        public Size Size => new Size(Width, Height);

        public Matrix<double> CameraMatrix { get; private set; } = new Matrix<double>(3, 3);

        public Matrix<double> DistortCoefMatrix { get; private set; } = new Matrix<double>(4, 1);

        public Matrix<double> UndistortedCameraMatrix { get; private set; } = new Matrix<double>(3, 3);


        public void Initialize(VectorOfVectorOfPointF imagePoints, int imageWidth, int imageHeight, Size patternSize, float patternLengthX, float patternLengthY,
            float initFocalX, float initFocalY, float initPrincipleX, float initPrincipleY)
        {
            int imageNumber = imagePoints.Size;
            var objectPoints = new MCvPoint3D32f[imageNumber][];
            for (int i = 0; i < imageNumber; ++i)
            {
                objectPoints[i] = new MCvPoint3D32f[patternSize.Width * patternSize.Height];
                for (int h = 0; h < patternSize.Height; h++)
                {
                    for (int w = 0; w < patternSize.Width; w++)
                    {
                        objectPoints[i][h * patternSize.Width + w] = new MCvPoint3D32f(w * patternLengthX, h * patternLengthY, 0.0f);
                    }
                }
            }

            // Initial
            CameraMatrix[0, 0] = initFocalX;
            CameraMatrix[1, 1] = initFocalY;
            CameraMatrix[0, 2] = initPrincipleX;
            CameraMatrix[1, 2] = initPrincipleY;
            CameraMatrix[2, 2] = 1;
            var DistortCoef = new Matrix<double>(5, 1);
            CvInvoke.CalibrateCamera(objectPoints, imagePoints.ToArrayOfArray(), new Size(imageWidth, imageHeight),
                CameraMatrix, DistortCoef, Emgu.CV.CvEnum.CalibType.UseIntrinsicGuess, new MCvTermCriteria(30, double.Epsilon),
                out Mat[] rotationVectors, out Mat[] translationVectors);
            DistortCoefMatrix[0, 0] = DistortCoef[0, 0];
            DistortCoefMatrix[1, 0] = DistortCoef[1, 0];
            DistortCoefMatrix[2, 0] = DistortCoef[2, 0];
            DistortCoefMatrix[3, 0] = DistortCoef[3, 0];
            Width = imageWidth;
            Height = imageHeight;
            var roi = new Rectangle();
            Mat opt = CvInvoke.GetOptimalNewCameraMatrix(CameraMatrix, DistortCoefMatrix, new Size(Width, Height), 0.0, new Size(Width, Height), ref roi);
            opt.CopyTo(UndistortedCameraMatrix);
        }

        public void Initialize(int width, int height, double fx, double fy, double cx, double cy, double[] distortCoef)
        {
            if (distortCoef.Length != 4)
            {
                throw new Exception("왜곡 계수 4개 필요( k1 k2 p1 p2 )");
            }

            Width = width;
            Height = height;
            CameraMatrix.SetZero();
            CameraMatrix.Data[0, 0] = fx;
            CameraMatrix.Data[1, 1] = fy;
            CameraMatrix.Data[0, 2] = cx;
            CameraMatrix.Data[1, 2] = cy;
            CameraMatrix.Data[2, 2] = 1.0f;
            DistortCoefMatrix.Data[0, 0] = distortCoef[0];
            DistortCoefMatrix.Data[1, 0] = distortCoef[1];
            DistortCoefMatrix.Data[2, 0] = distortCoef[2];
            DistortCoefMatrix.Data[3, 0] = distortCoef[3];
            var roi = new Rectangle();
            Mat opt = CvInvoke.GetOptimalNewCameraMatrix(CameraMatrix, DistortCoefMatrix, new Size(Width, Height), 0.0, new Size(Width, Height), ref roi);
            opt.CopyTo(UndistortedCameraMatrix);
        }

        public string Initialize(string calibFile)
        {
            var calibData = SingleVisionCalibData.FromFile(calibFile);

            if (calibData != null)
            {
                Initialize(calibData.Width, calibData.Height, calibData.FocalX, calibData.FocalY, calibData.PrincipleX, calibData.PrincipleY, calibData.DistortionCoef);
                return calibData.DeviceSerialNumber;
            }

            return "";
        }

        public void SaveCalibData(string calibFile, string deviceSerialNumber)
        {
            if (Width == -1 || Height == -1)
            {
                return;
            }

            var calibData = new SingleVisionCalibData
            {
                DeviceSerialNumber = deviceSerialNumber,
                Width = Width,
                Height = Height,
                FocalX = CameraMatrix.Data[0, 0],
                FocalY = CameraMatrix.Data[1, 1],
                PrincipleX = CameraMatrix.Data[0, 2],
                PrincipleY = CameraMatrix.Data[1, 2],
                DistortionCoef = new double[4] { DistortCoefMatrix.Data[0, 0], DistortCoefMatrix.Data[1, 0], DistortCoefMatrix.Data[2, 0], DistortCoefMatrix.Data[3, 0] }
            };
            calibData.Save(calibFile);
        }

        public PointF UndistortPoint(PointF point)
        {
            var newPoints = new VectorOfPointF();
            CvInvoke.UndistortPoints(new VectorOfPointF(new PointF[1] { point }), newPoints, UndistortedCameraMatrix, DistortCoefMatrix);
            return newPoints[0];
        }

        public VectorOfPointF UndistortPoints(VectorOfPointF points)
        {
            var newPoints = new VectorOfPointF(points.Size);
            CvInvoke.UndistortPoints(points, newPoints, CameraMatrix, DistortCoefMatrix);
            return newPoints;
        }
    }

    public class StereoVision
    {
        public StereoVision(SingleVision leftCamera, SingleVision rightCamera)
        {
            LeftCamera = leftCamera;
            RightCamera = rightCamera;
        }


        public SingleVision LeftCamera { get; private set; }

        public SingleVision RightCamera { get; private set; }

        private Matrix<double> R { get; set; } = new Matrix<double>(3, 3);

        private Matrix<double> T { get; set; } = new Matrix<double>(3, 1);

        private Matrix<double> Affine { get; set; } = new Matrix<double>(3, 3);

        public double[] Translation => new double[3] { T[0, 0], T[1, 0], T[2, 0] };

        public double[] Rotation => RotationMatrixToEulerAngles(R);


        // patternLength 밀리 단위 권장함
        public void Initialize(VectorOfVectorOfPointF leftImagePoints, VectorOfVectorOfPointF rightImagePoints, Size patternSize, float patternLengthX, float patternLengthY)
        {
            if (leftImagePoints.Size != rightImagePoints.Size)
            {
                throw new Exception("양쪽 이미지 수가 다름");
            }

            var objectPoints = new MCvPoint3D32f[leftImagePoints.Size][];
            for (int i = 0; i < leftImagePoints.Size; ++i)
            {
                objectPoints[i] = new MCvPoint3D32f[patternSize.Width * patternSize.Height];
                for (int h = 0; h < patternSize.Height; h++)
                {
                    for (int w = 0; w < patternSize.Width; w++)
                    {
                        objectPoints[i][h * patternSize.Width + w] = new MCvPoint3D32f(w * patternLengthX, h * patternLengthY, 0.0f);
                    }
                }
            }

            var E = new Matrix<double>(3, 3);
            var F = new Matrix<double>(3, 3);
            CvInvoke.StereoCalibrate(objectPoints, leftImagePoints.ToArrayOfArray(), rightImagePoints.ToArrayOfArray(),
                LeftCamera.CameraMatrix.Mat, LeftCamera.DistortCoefMatrix.Mat, RightCamera.CameraMatrix.Mat, RightCamera.DistortCoefMatrix.Mat,
                LeftCamera.Size, R, T, E, F, Emgu.CV.CvEnum.CalibType.FixIntrinsic, new Emgu.CV.Structure.MCvTermCriteria());
        }

        public void Initialize(string calibFile)
        {
            var calibData = StereoVisionCalibData.FromFile(calibFile);

            if (calibData != null)
            {
                R = new Matrix<double>(calibData.Rotation);
                T = new Matrix<double>(calibData.Translation);
                Affine = new Matrix<double>(calibData.Affine);
            }
        }

        public void SaveCalibData(string calibFile)
        {
            var calibData = new StereoVisionCalibData
            {
                Rotation = R.Data,
                Translation = T.Data,
                Affine = Affine.Data,
            };
            calibData.Save(calibFile);
        }

        public MCvPoint3D64f[] ReprojectPoints(PointF[] leftPoints, PointF[] rightPoints)
        {
            if (leftPoints == null || rightPoints == null)
            {
                return null;
            }

            if (leftPoints.Length != rightPoints.Length)
            {
                return null;
            }

            Matrix<double> TR = R;
            TR = TR.ConcateHorizontal(T);
            TR = TR.ConcateVertical(new Matrix<double>(new double[1, 4] { { 0, 0, 0, 1 } }));

            Matrix<double> K1 = LeftCamera.UndistortedCameraMatrix;
            K1 = K1.ConcateHorizontal(new Matrix<double>(new double[3, 1] { { 0 }, { 0 }, { 0 } }));

            Matrix<double> K2 = RightCamera.UndistortedCameraMatrix;
            K2 = K2.ConcateHorizontal(new Matrix<double>(new double[3, 1] { { 0 }, { 0 }, { 0 } })) * TR;

            var points = new MCvPoint3D64f[leftPoints.Length];
            for (int i = 0; i < leftPoints.Length; ++i)
            {
                var A = new Matrix<double>(4, 4);
                for (int c = 0; c < 4; ++c)
                {
                    A[0, c] = K1[0, c] - leftPoints[i].X * K1[2, c];
                    A[1, c] = K1[1, c] - leftPoints[i].Y * K1[2, c];
                }
                for (int c = 0; c < 4; ++c)
                {
                    A[2, c] = K2[0, c] - rightPoints[i].X * K2[2, c];
                    A[3, c] = K2[1, c] - rightPoints[i].Y * K2[2, c];
                }
                var W = new Matrix<double>(4, 1);
                var U = new Matrix<double>(4, 4);
                var V = new Matrix<double>(4, 4);
                CvInvoke.SVDecomp(A, W, U, V, SvdFlag.FullUV);
                Matrix<double> VT = V.Transpose();
                double w = VT[3, 3];
                points[i].X = VT[0, 3] / w;
                points[i].Y = VT[1, 3] / w;
                points[i].Z = VT[2, 3] / w;
            }
            return points;
        }

        public MCvPoint3D64f[] ReprojectPointsWithRT(PointF[] leftPoints, PointF[] rightPoints)
        {
            if (leftPoints == null || rightPoints == null)
            {
                return null;
            }

            if (leftPoints.Length != rightPoints.Length)
            {
                return null;
            }

            Matrix<double> TR = R;
            TR = TR.ConcateHorizontal(T);
            TR = TR.ConcateVertical(new Matrix<double>(new double[1, 4] { { 0, 0, 0, 1 } }));

            Matrix<double> K1 = LeftCamera.UndistortedCameraMatrix;
            K1 = K1.ConcateHorizontal(new Matrix<double>(new double[3, 1] { { 0 }, { 0 }, { 0 } }));

            Matrix<double> K2 = RightCamera.UndistortedCameraMatrix;
            K2 = K2.ConcateHorizontal(new Matrix<double>(new double[3, 1] { { 0 }, { 0 }, { 0 } })) * TR;

            var points = new MCvPoint3D64f[leftPoints.Length];
            for (int i = 0; i < leftPoints.Length; ++i)
            {
                var A = new Matrix<double>(4, 4);
                for (int c = 0; c < 4; ++c)
                {
                    A[0, c] = K1[0, c] - leftPoints[i].X * K1[2, c];
                    A[1, c] = K1[1, c] - leftPoints[i].Y * K1[2, c];
                }
                for (int c = 0; c < 4; ++c)
                {
                    A[2, c] = K2[0, c] - rightPoints[i].X * K2[2, c];
                    A[3, c] = K2[1, c] - rightPoints[i].Y * K2[2, c];
                }
                var W = new Matrix<double>(4, 1);
                var U = new Matrix<double>(4, 4);
                var V = new Matrix<double>(4, 4);
                CvInvoke.SVDecomp(A, W, U, V, SvdFlag.FullUV);
                Matrix<double> VT = V.Transpose();
                double w = VT[3, 3];
                points[i].X = VT[0, 3] / w;
                points[i].Y = VT[1, 3] / w;
                points[i].Z = VT[2, 3] / w;
            }

            ////////////////////////////////////////////////////////////////
            double theta = 15.0 / 180.0 * Math.PI;
            var Rotate = new Matrix<double>(3, 3);

            Rotate[0, 0] = Math.Cos(theta);
            Rotate[0, 1] = 0;
            Rotate[0, 2] = Math.Sin(theta);

            Rotate[1, 0] = 0;
            Rotate[1, 1] = 1;
            Rotate[1, 2] = 0;

            Rotate[2, 0] = -Math.Sin(theta);
            Rotate[2, 1] = 0;
            Rotate[2, 2] = Math.Cos(theta);
            var pt = new Matrix<double>(3, 1);
            var result = new Matrix<double>(3, 1);

            double dX = -202.644;


            for (int k = 0; k < points.Length; k++)
            {
                pt[0, 0] = points[k].X;
                pt[1, 0] = points[k].Y;
                pt[2, 0] = points[k].Z;
                result = Rotate * pt;

                result[0, 0] += dX;

                points[k].X = result[0, 0];
                points[k].Y = result[1, 0];
                points[k].Z = result[2, 0];
            }

            return points;
        }

        public static bool FindPoints(Size patternSize, Mat leftImage, Mat rightImage,
            out VectorOfPointF leftCorners, out VectorOfPointF rightCorners, bool show = true)
        {
            bool leftFind = false;
            bool rightFind = false;

            leftCorners = new VectorOfPointF();
            leftFind = CvInvoke.FindChessboardCorners(leftImage, patternSize, leftCorners,
                Emgu.CV.CvEnum.CalibCbType.AdaptiveThresh | Emgu.CV.CvEnum.CalibCbType.FilterQuads);
            if (leftFind)
            {
                CvInvoke.CornerSubPix(leftImage, leftCorners, new Size(5, 5), new Size(-1, -1), new MCvTermCriteria(30, 0.1));
            }

            rightCorners = new VectorOfPointF();
            rightFind = CvInvoke.FindChessboardCorners(rightImage, patternSize, rightCorners,
                Emgu.CV.CvEnum.CalibCbType.AdaptiveThresh | Emgu.CV.CvEnum.CalibCbType.FilterQuads);
            if (rightFind)
            {
                CvInvoke.CornerSubPix(rightImage, rightCorners, new Size(5, 5), new Size(-1, -1), new MCvTermCriteria(30, 0.1));
            }

            if (show && leftFind && rightFind)
            {
                ShowPoints(patternSize, leftImage, rightImage, leftCorners, rightCorners);
            }

            return leftFind && rightFind;
        }

        private static void ShowPoints(Size patternSize, Mat matLeft, Mat matRight, VectorOfPointF leftCorners, VectorOfPointF rightCorners, bool show = false)
        {
            var matLeftNew = new Mat();
            var matRightNew = new Mat();
            CvInvoke.Resize(matLeft, matLeftNew, new Size(matLeft.Width / 4, matLeft.Height / 4));
            CvInvoke.Resize(matRight, matRightNew, new Size(matRight.Width / 4, matRight.Height / 4));
            CvInvoke.CvtColor(matLeftNew, matLeftNew, ColorConversion.Gray2Bgr);
            CvInvoke.CvtColor(matRightNew, matRightNew, ColorConversion.Gray2Bgr);
            PointF[] resizedLeftPoints = leftCorners.ToArray();
            for (int i = 0; i < resizedLeftPoints.Length; ++i)
            {
                resizedLeftPoints[i].X /= 4;
                resizedLeftPoints[i].Y /= 4;
            }
            PointF[] resizedRightPoints = rightCorners.ToArray();
            for (int i = 0; i < resizedRightPoints.Length; ++i)
            {
                resizedRightPoints[i].X /= 4;
                resizedRightPoints[i].Y /= 4;
            }
            CvInvoke.DrawChessboardCorners(matLeftNew, patternSize, new VectorOfPointF(resizedLeftPoints), true);
            CvInvoke.DrawChessboardCorners(matRightNew, patternSize, new VectorOfPointF(resizedRightPoints), true);
            if (show)
            {
                CvInvoke.Imshow("Left", matLeftNew);
                CvInvoke.Imshow("Right", matRightNew);
            }
            CvInvoke.WaitKey();
        }

        private static bool IsRotationMatrix(Matrix<double> R)
        {
            Matrix<double> Rt = R.Transpose();
            Matrix<double> expected = Rt * R;
            var identity = new Matrix<double>(3, 3, 1);
            identity.SetIdentity();
            return CvInvoke.Norm(identity, expected) < 1e-6;
        }

        private static double[] RotationMatrixToEulerAngles(Matrix<double> R)
        {
            if (!IsRotationMatrix(R))
            {
                return null;
            }

            float sy = (float)Math.Sqrt(R[0, 0] * R[0, 0] + R[1, 0] * R[1, 0]);
            double x, y, z;
            if (sy < 1e-6)
            {
                x = Math.Atan2(-R[1, 2], R[1, 1]);
                y = Math.Atan2(-R[2, 0], sy);
                z = 0;
            }
            else
            {
                x = Math.Atan2(R[2, 1], R[2, 2]);
                y = Math.Atan2(-R[2, 0], sy);
                z = Math.Atan2(R[1, 0], R[0, 0]);
            }
            return new double[] { x, y, z };
        }

        public Matrix<double> MakeAffineMatirx(Size patternSize, VectorOfPointF leftCorners, VectorOfPointF rightCorners)
        {
            var affine = new Mat();

            //affine = CvInvoke.EstimateRigidTransform(leftCorners, rightCorners, true);
            affine = CvInvoke.FindHomography(leftCorners, rightCorners);
            affine.CopyTo(Affine);

            return Affine;
        }

        //우선 패턴매칭은 100% 찾는다는 가정하고..
        public List<PointF> GetCorrespondPoint(PointF[] beforList, PointF[] curList, float radius = 100)
        {
            var retlist = new List<PointF>();

            foreach (PointF beforPT in beforList)
            {
                double dist = 9e10;
                PointF notmatchPt = beforPT; // new PointF(-1000, -1000); 만약 못찾으면, 그전 데이터를 취하도록...
                PointF nearpt = notmatchPt;
                foreach (PointF curPT in curList)
                {
                    double dx = curPT.X - beforPT.X;
                    double dy = curPT.Y - beforPT.Y;
                    double d = Math.Sqrt(dx * dx + dy * dy);
                    if (d < dist && d < radius)
                    {
                        nearpt = curPT;
                    }

                    dist = d;
                }
                retlist.Add(nearpt);
            }
            return retlist;
        }
        // Affine 변환을 실시한 높이에서 왼쪽, 오른쪽 카메라에서 찾은 마커를 매칭시켜 반환
        // 기준위치는 측정기의 중심(왼쪽, 오른쪽 카메라 가운데의 커버표면)에서 측정면까지 675mm임.
        // 측정 범위는 650mm ~ 700mm 
        public List<PointF> GetCorrespondPointAffine(PointF[] refPoints, PointF[] corPoints, float radius = 100)
        {
            var retlist = new List<PointF>();

            var mpoint = new Matrix<double>(3, 1);
            var result = new Matrix<double>(3, 1);

            foreach (PointF refpt in refPoints)
            {
                mpoint[0, 0] = refpt.X;
                mpoint[1, 0] = refpt.Y;
                mpoint[2, 0] = 1.0;
                result = Affine * mpoint;
                result[0, 0] /= result[2, 0];
                result[1, 0] /= result[2, 0];
                result[2, 0] /= result[2, 0];

                double dist = 9e10;
                var notmatchPt = new PointF(0, 0);
                PointF nearpt = notmatchPt;
                foreach (PointF copt in corPoints)
                {
                    double dx = copt.X - result[0, 0];
                    double dy = copt.Y - result[1, 0];
                    double d = Math.Sqrt(dx * dx + dy * dy);

                    if (d < dist && d < radius)
                    {
                        nearpt = copt;
                    }

                    dist = d;
                }
                retlist.Add(nearpt);
            }
            return retlist;
        }

        //Affine 변환 위치가 아닌경우, 전체(왼쪽, 오른쪽) 위치를 추정하여 탐색함 , *높이가 바뀌면 주로 X 포지션이 서로 달라짐..
        public List<PointF> GetCorrespondPointAffine2(PointF[] refPoints, PointF[] corPoints, float radius = 100) //100*0.17mm= 17mm 영역 써치함. 
        {
            var retlist = new List<PointF>();

            var mpoint = new Matrix<double>(3, 1);
            var result = new Matrix<double>(3, 1);

            //1. 변환 결과 X 포지션 평균값 구하기 
            double resultXavg = 0;
            int count = 0;
            foreach (PointF refpt in refPoints)
            {
                mpoint[0, 0] = refpt.X;
                mpoint[1, 0] = refpt.Y;
                mpoint[2, 0] = 1.0;
                result = Affine * mpoint;
                result[0, 0] /= result[2, 0];
                result[1, 0] /= result[2, 0];
                result[2, 0] /= result[2, 0];

                resultXavg += result[0, 0];
                count++;
            }
            resultXavg /= count;


            //2. 대응점 (Right) 마커의 X포지션 평균값 구하기
            double corXavg = 0;
            count = 0;
            foreach (PointF pt in corPoints)
            {
                corXavg += pt.X;
                count++;
            }
            corXavg /= count;

            //3. 검색점 수정(diff 보정) 후
            //   최근방 마커와 매칭시킴
            double diff = corXavg - resultXavg;
            foreach (PointF refpt in refPoints)
            {
                mpoint[0, 0] = refpt.X;
                mpoint[1, 0] = refpt.Y;
                mpoint[2, 0] = 1.0;
                result = Affine * mpoint;
                result[0, 0] /= result[2, 0];
                result[1, 0] /= result[2, 0];
                result[2, 0] /= result[2, 0];

                result[0, 0] += diff; //검색점 수정(diff 보정)

                //최근방 마커 탐색
                double dist = 9e10;
                var notmatchPt = new PointF(0, 0); //반경에서 못찾을경우 이값을 갖게됨.
                PointF nearpt = notmatchPt;
                foreach (PointF copt in corPoints)
                {
                    double dx = copt.X - result[0, 0];
                    double dy = copt.Y - result[1, 0];
                    double d = Math.Sqrt(dx * dx + dy * dy);

                    if (d < dist && d < radius)
                    {
                        nearpt = copt;
                    }

                    dist = d;
                }
                retlist.Add(nearpt);
            }
            return retlist;
        }
    }
}
