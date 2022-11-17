using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Devices.FrameGrabber
{
    public class CameraBufferTag
    {
        public int BufferId { get; set; }
        public ulong FrameId { get; set; }
        public ulong TimeStamp { get; set; }
        /// <summary>
        /// LineScan 카메라의 경우 버퍼 크기와 프레임 크기가 다를 수 있다 -> 그랩하다 중단한 경우.
        /// </summary>
        public Size FrameSize { get; set; } = Size.Empty;

        public CameraBufferTag() { }

        public CameraBufferTag(int bufferId, ulong frameId, ulong timeStamp, Size frameSize)
        {
            BufferId = bufferId;
            FrameId = frameId;
            TimeStamp = timeStamp;
            FrameSize = frameSize;
        }
    }
}
