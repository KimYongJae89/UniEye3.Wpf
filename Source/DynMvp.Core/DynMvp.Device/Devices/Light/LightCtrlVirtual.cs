using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Devices.Light
{
    public class LightCtrlVirtual : LightCtrl
    {
        private int numChannel;

        public LightCtrlVirtual(LightCtrlType lightCtrlType, string name, int numChannel) : base(lightCtrlType, name)
        {
            this.numChannel = numChannel;
        }

        public override int NumChannel => throw new NotImplementedException();

        public override int GetMaxLightLevel()
        {
            return 255;
        }

        public override bool Initialize(LightCtrlInfo lightCtrlInfo)
        {
            return true;
        }

        public override void TurnOff()
        {

        }

        public override void TurnOff(int deviceIndex)
        {
            throw new NotImplementedException();
        }

        public override void TurnOn()
        {

        }

        public override void TurnOn(LightValue lightValue)
        {

        }

        public override void TurnOn(LightValue lightValue, int deviceIndex)
        {

        }
    }
}
