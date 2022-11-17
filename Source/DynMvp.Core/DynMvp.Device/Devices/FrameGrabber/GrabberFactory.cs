using DynMvp.Base;
using System;

namespace DynMvp.Devices.FrameGrabber
{
    public class GrabberFactory
    {
        public static Grabber Create(GrabberInfo grabberInfo)
        {
            LogHelper.Debug(LoggerType.StartUp, "Create Grabber");

            Grabber grabber = null;
            switch (grabberInfo.Type)
            {
                case GrabberType.Pylon:
                    grabber = new GrabberPylon(grabberInfo);
                    break;
                case GrabberType.Pylon2:
                    grabber = new GrabberPylon2(grabberInfo);
                    break;
                case GrabberType.PylonLine:
                    grabber = new GrabberPylonLine(grabberInfo);
                    break;
                case GrabberType.Virtual:
                    grabber = new GrabberVirtual(grabberInfo);
                    break;
                case GrabberType.MultiCam:
                    grabber = new GrabberMultiCam(grabberInfo);
                    break;
                case GrabberType.uEye:
                    grabber = new GrabberUEye(grabberInfo);
                    break;
                case GrabberType.MIL:
                    grabber = new GrabberMil(grabberInfo);
                    break;
                case GrabberType.MILCXP:
                    grabber = new GrabberMilCXP(grabberInfo);
                    break;
                case GrabberType.GenTL:
                    grabber = new GrabberGenTL(grabberInfo);
                    break;
                case GrabberType.Sapera:
                    grabber = new GrabberSapera(grabberInfo);
                    break;
            }

            if (grabber == null)
            {
                ErrorManager.Instance().Report((int)ErrorSection.Grabber, (int)CommonError.FailToCreate, ErrorLevel.Error,
                    ErrorSection.Grabber.ToString(), CommonError.FailToCreate.ToString(), string.Format("Can't create grabber. {0}", grabberInfo.Type.ToString()));
                return null;
            }

            //if (grabber.Initialize() == false)
            //{
            //    ErrorManager.Instance().Report((int)ErrorSection.Grabber, (int)CommonError.FailToInitialize, ErrorLevel.Error,
            //        ErrorSection.Grabber.ToString(), CommonError.FailToInitialize.ToString(), String.Format("Can't initialize grabber. {0}", grabberInfo.Type.ToString()));

            //    grabber = new GrabberVirtual(grabberInfo.Type, grabberInfo.Name);
            //    grabber.UpdateState(DeviceState.Error, "Grabber is invalid.");
            //}
            //else
            //{
            //    grabber.UpdateState(DeviceState.Ready, "Grabber initialization succeeded.");
            //}

            //grabber.GrabberInfo = grabberInfo;

            DeviceManager.Instance().AddDevice(grabber);

            return grabber;
        }
    }
}
