using System.Collections.Generic;
using System.Windows.Media;
using UniEye.Base.Data;

namespace UniScanC.Models
{
    public static class OpStateModel
    {
        public static StatusModel StatusModel(OpState state)
        {
            return StatusModelPair.ContainsKey(state) ? StatusModelPair[state] : null;
        }

        private static Dictionary<OpState, StatusModel> statusModelPair;
        private static Dictionary<OpState, StatusModel> StatusModelPair => statusModelPair ?? (statusModelPair = CreateStatusModel());

        private static Dictionary<OpState, StatusModel> CreateStatusModel()
        {
            var pairs = new Dictionary<OpState, StatusModel>();

            pairs.Add(OpState.Idle, new StatusModel()
            {
                State = OpState.Idle,
                Foreground = Colors.White,
                Background = Colors.DarkGray,
                Text = "IDLE"
            });

            pairs.Add(OpState.Align, new StatusModel()
            {
                State = OpState.Idle,
                Foreground = Colors.White,
                Background = Colors.DodgerBlue,
                Text = "READY"
            });

            pairs.Add(OpState.Wait, new StatusModel()
            {
                State = OpState.Wait,
                Foreground = Colors.White,
                Background = Colors.Orange,
                Text = "WAIT"
            });

            pairs.Add(OpState.Inspect, new StatusModel()
            {
                State = OpState.Inspect,
                Foreground = Colors.White,
                Background = Colors.LimeGreen,
                Text = "INSPECT"
            });

            return pairs;
        }
    }
}
