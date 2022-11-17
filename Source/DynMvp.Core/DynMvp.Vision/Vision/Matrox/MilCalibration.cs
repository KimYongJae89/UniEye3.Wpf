using DynMvp.Base;
using DynMvp.Vision;
using Matrox.MatroxImagingLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace DynMvp.Vision.Matrox
{
    public class MilCalibration : Calibration
    {
        private bool calibrated = false;
        private MIL_ID calibrationId = MIL.M_NULL;

        public override void Initialize(int cameraIndex, string datFileName, string gridFileName)
        {
            base.Initialize(cameraIndex, datFileName, gridFileName);

            // Allocate a camera calibration context.
            if (calibrationId == MIL.M_NULL)
            {
                MIL.McalAlloc(MIL.M_DEFAULT_HOST, MIL.M_DEFAULT, MIL.M_DEFAULT, ref calibrationId);
            }
        }

        public override void Dispose()
        {
            if (calibrationId != MIL.M_NULL)
            {
                MIL.McalFree(calibrationId);
                calibrationId = MIL.M_NULL;
                calibrated = false;
            }
        }

        public override bool IsGridCalibrated()
        {
            return calibrated;
        }

        public override CalibrationResult CalibrateGrid(ImageD image, int numRow, int numCol, float rowSpace, float colSpace)
        {
            var greyImage = (MilGreyImage)ImageBuilder.MilImageBuilder.Build(image, ImageType.Grey);

            // Calibrate the camera with the image of the grid and its world description.
            MIL.McalGrid(calibrationId, greyImage.Image, 0, 0, 0, numRow, numCol, rowSpace, colSpace, MIL.M_DEFAULT, MIL.M_DEFAULT);

            MIL_INT calibrationStatus = 0;
            MIL.McalInquire(calibrationId, MIL.M_CALIBRATION_STATUS + MIL.M_TYPE_MIL_INT, ref calibrationStatus);
            calibrated = (calibrationStatus == MIL.M_CALIBRATED);

            UpdatePelSize(image.Width, image.Height);

            return new CalibrationResult() { pelSize = PelSize };
        }

        public override CalibrationResult CalibrateChessboard(ImageD image, int numRow, int numCol, float rowSpace, float colSpace)
        {
            throw new NotImplementedException();
        }

        public override void TransformImage(ImageD image)
        {
            var greyImage = (MilGreyImage)ImageBuilder.MilImageBuilder.Build(image, ImageType.Grey);

            MIL.McalTransformImage(greyImage.Image, greyImage.Image, calibrationId, MIL.M_BILINEAR | MIL.M_OVERSCAN_CLEAR, MIL.M_DEFAULT, MIL.M_DEFAULT);

            ImageD transformImage = MilImageBuilder.ConvertImage(greyImage);

            image.CopyFrom(transformImage);
        }

        public override PointF WorldToPixelGrid(PointF ptWorld)
        {
            if (IsCalibrated() == true)
            {
                double ptPixelX = ptWorld.X;
                double ptPixelY = ptWorld.Y;

                MIL.McalTransformCoordinate(calibrationId, MIL.M_WORLD_TO_PIXEL, ptWorld.X / 1000, ptWorld.Y / 1000, ref ptPixelX, ref ptPixelY);

                return new PointF((float)ptPixelX, (float)ptPixelY);
            }

            return ptWorld;
        }

        public override SizeF WorldToPixelGrid(SizeF ptWorld)
        {
            if (IsCalibrated() == true)
            {
                double ptPixelX = ptWorld.Width;
                double ptPixelY = ptWorld.Width;

                MIL.McalTransformCoordinate(calibrationId, MIL.M_WORLD_TO_PIXEL, ptWorld.Width / 1000, ptWorld.Width / 1000, ref ptPixelX, ref ptPixelY);

                return new SizeF((float)ptPixelX, (float)ptPixelY);
            }

            return ptWorld;
        }

        public override PointF PixelToWorldGrid(PointF ptPixel)
        {
            if (IsCalibrated() == true)
            {
                double ptWorldX = ptPixel.X;
                double ptWorldY = ptPixel.Y;

                MIL.McalTransformCoordinate(calibrationId, MIL.M_PIXEL_TO_WORLD, ptPixel.X, ptPixel.Y, ref ptWorldX, ref ptWorldY);

                return new PointF((float)ptWorldX * 1000, (float)ptWorldY * 1000);
            }

            return ptPixel;
        }

        public override SizeF PixelToWorldGrid(SizeF szPixel)
        {
            if (IsCalibrated() == true)
            {
                double ptWorldX = szPixel.Width;
                double ptWorldY = szPixel.Height;

                MIL.McalTransformCoordinate(calibrationId, MIL.M_PIXEL_TO_WORLD, szPixel.Width, szPixel.Height, ref ptWorldX, ref ptWorldY);

                return new SizeF((float)ptWorldX * 1000, (float)ptWorldY * 1000);
            }

            return szPixel;
        }

        public override void LoadGrid()
        {
            Dispose();

            if (calibrationId == MIL.M_NULL)
            {
                if (File.Exists(gridFileName))
                {
                    MIL.McalRestore(gridFileName, MIL.M_DEFAULT_HOST, MIL.M_DEFAULT, ref calibrationId);
                    calibrated = true;
                }
            }
        }

        public override void SaveGrid()
        {
            if (IsCalibrated())
            {
                MIL.McalSave(gridFileName, calibrationId, MIL.M_DEFAULT);
            }
        }
    }
}
