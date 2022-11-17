using DynMvp.Devices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Unieye.WPF.Base.Helpers;

namespace WPF.UniScanIM.Helpers
{
    public class CameraJsonConverter : JsonCreationConverter<DynMvp.Devices.FrameGrabber.Camera>
    {
        private class Camera
        {
            public string Name { get; set; }
        }

        public override bool CanWrite => true;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var axisHandler = value as DynMvp.Devices.FrameGrabber.Camera;
            var fake = new Camera() { Name = axisHandler.Name };

            serializer.Serialize(writer, fake);
        }

        protected override DynMvp.Devices.FrameGrabber.Camera Create(Type objectType, JObject jObject)
        {
            return DeviceManager.Instance().CameraHandler.cameraList.Find(camera => camera.Name == jObject["Name"].ToString());
        }
    }
}