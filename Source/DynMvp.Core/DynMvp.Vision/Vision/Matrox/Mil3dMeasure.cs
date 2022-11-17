using DynMvp.Base;
using Matrox.MatroxImagingLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DynMvp.Vision.Matrox
{
    public class Mil3dMeasure
    {
        public static Image3D Alignment(Image3D fixedImage, Image3D movingImage)
        {
            var fixedCloud = new MilPointCloud(fixedImage);
            var movingCloud = new MilPointCloud(movingImage);

            Alignment(fixedCloud, movingCloud);

            return movingCloud.Export(fixedImage.Width, fixedImage.Height, 1);
        }

        public static void Alignment(MilPointCloud fixedCloud, MilPointCloud movingCloud)
        {
            // total points counter
            int[] numOfPointCloud = new int[2] { 0, 0 };
            //MIL.M3dmapGetResult(fixedCloud.ObjectId, MIL.M_DEFAULT, MIL.M_NUMBER_OF_3D_POINTS + MIL.M_NO_INVALID_POINT + MIL.M_TYPE_MIL_INT, ref numOfPointCloud[0]);
            //MIL.M3dmapGetResult(movingCloud.ObjectId, MIL.M_DEFAULT, MIL.M_NUMBER_OF_3D_POINTS + MIL.M_NO_INVALID_POINT + MIL.M_TYPE_MIL_INT, ref numOfPointCloud[1]);

            MIL.M3dmapGetResult(fixedCloud.ObjectId, MIL.M_DEFAULT, MIL.M_NUMBER_OF_3D_POINTS + MIL.M_TYPE_MIL_INT, ref numOfPointCloud[0]);
            MIL.M3dmapGetResult(movingCloud.ObjectId, MIL.M_DEFAULT, MIL.M_NUMBER_OF_3D_POINTS + MIL.M_TYPE_MIL_INT, ref numOfPointCloud[1]);

            // Alignment Context
            MIL_ID alignContext = MIL.M_NULL;
            MIL.M3dmapAlloc(MIL.M_DEFAULT_HOST, MIL.M_PAIRWISE_ALIGNMENT_CONTEXT, MIL.M_DEFAULT, ref alignContext);

            // Alignment Result
            MIL_ID alignResult = MIL.M_NULL;
            MIL.M3dmapAllocResult(MIL.M_DEFAULT_HOST, MIL.M_ALIGNMENT_RESULT, MIL.M_DEFAULT, ref alignResult);

            // Parameter
            MIL.M3dmapControl(alignContext, MIL.M_DEFAULT, MIL.M_DECIMATION_STEP_MODEL, 1);
            MIL.M3dmapControl(alignContext, MIL.M_DEFAULT, MIL.M_DECIMATION_STEP_SCENE, 1);
            MIL.M3dmapControl(alignContext, MIL.M_DEFAULT, MIL.M_PREALIGNMENT_MODE, MIL.M_CENTROID);
            MIL.M3dmapControl(alignContext, MIL.M_DEFAULT, MIL.M_MODEL_OVERLAP, 90);
            MIL.M3dmapControl(alignContext, MIL.M_DEFAULT, MIL.M_MAX_ITERATIONS, 100);
            MIL.M3dmapControl(alignContext, MIL.M_DEFAULT, MIL.M_ALIGN_RMS_ERROR_THRESHOLD, 0.001);
            MIL.M3dmapControl(alignContext, MIL.M_DEFAULT, MIL.M_ALIGN_RMS_ERROR_RELATIVE_THRESHOLD, 0.001);

            // Get align param
            long alignState = 0;
            double timeSpend = 0.0;

            MIL.MappTimer(MIL.M_TIMER_RESET);

            MIL.M3dmapAlign(
                alignContext,    // MIL_ID      AlignmentContextId
                fixedCloud.ObjectId,    // MIL_ID      ModelPtCloudContainerId
                MIL.M_ALL,    // MIL_INT     ModelPtCloudIndexOrLabel
                movingCloud.ObjectId,    // MIL_ID      ScenePtCloudContainerId
                MIL.M_ALL,    // MIL_INT     ScenePtCloudIndexOrLabel
                MIL.M_NULL,    // MIL_ID      PreAlignAlignmentResultOrMatrixId
                alignResult,    // MIL_ID      AlignmentResultOrMatrixId
                MIL.M_DEFAULT,    // MIL_INT64   ControlFlag
                ref alignState  // MIL_INT64*  StatusPtr
                );

            MIL.MappTimer(MIL.M_TIMER_READ, ref timeSpend);

            double rmsError = 0;
            double rmsErrorRelative = 0;
            double iteration = 0;
            MIL.M3dmapGetResult(alignResult, MIL.M_DEFAULT, MIL.M_ALIGN_RMS_ERROR + MIL.M_TYPE_MIL_DOUBLE, ref rmsError);
            MIL.M3dmapGetResult(alignResult, MIL.M_DEFAULT, MIL.M_ALIGN_RMS_ERROR_RELATIVE + MIL.M_TYPE_MIL_DOUBLE, ref rmsErrorRelative);
            MIL.M3dmapGetResult(alignResult, MIL.M_DEFAULT, MIL.M_ALIGN_RMS_ERROR_RELATIVE + MIL.M_TYPE_MIL_DOUBLE, ref iteration);

            switch (alignState)
            {
                case MIL.M_NOT_INITIALIZED:
                    Debug.WriteLine("Alignment failed: the alignment result is not initialized.");
                    break;
                case MIL.M_NOT_ENOUGH_POINT_PAIRS:
                    Debug.WriteLine("Alignment failed: point clouds are not overlaping.");
                    break;
                case MIL.M_MAX_ITERATIONS_REACHED:
                    Debug.WriteLine(string.Format("Alignment reached the maximum number of max iterations in {0:.00} ms. Resulting fixture may or may not be valid.", timeSpend * 1000));
                    Debug.WriteLine(string.Format("succeeded in {0:0.00}ms with a final RMS error of {1:0.00}mm.(Relative of {2:0.00}mm) Iteration of {3}", timeSpend * 1000, rmsError, rmsErrorRelative, iteration));
                    break;
                case MIL.M_ALIGN_RMS_ERROR_THRESHOLD_REACHED:
                case MIL.M_ALIGN_RMS_ERROR_RELATIVE_THRESHOLD_REACHED:
                    Debug.WriteLine(string.Format("The alignment of {0} model points with {1} object points.", numOfPointCloud[0], numOfPointCloud[1]));
                    Debug.WriteLine(string.Format("succeeded in {0:0.00}ms with a final RMS error of {1:0.00}mm.(Relative of {2:0.00}mm) Iteration of {3}", timeSpend * 1000, rmsError, rmsErrorRelative, iteration));
                    break;
                default:
                    Debug.WriteLine("Unknown alignment status.");
                    break;
            }

            // move and rotate
            MIL.McalFixture(
                movingCloud.ObjectId,
                MIL.M_NULL,
                MIL.M_MOVE_RELATIVE,
                MIL.M_RESULT_ALIGNMENT_3DMAP,
                alignResult,
                MIL.M_DEFAULT,
                MIL.M_DEFAULT,
                MIL.M_DEFAULT,
                MIL.M_DEFAULT
                );

            MIL.M3dmapFree(alignContext);
            MIL.M3dmapFree(alignResult);
        }
    }
}
