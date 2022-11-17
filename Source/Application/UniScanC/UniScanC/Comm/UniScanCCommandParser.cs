using DynMvp.Devices.Comm;
using System;
using UniScanC.Enums;

namespace UniScanC.Comm
{
    public class UniScanCCommandParser
    {
        public CommandInfo Parse(ReceivedPacket packet)
        {
            var CommandInfo = new CommandInfo();
            CommandInfo.Sender = packet.SenderInfo;
            string[] tokens = packet.ReceivedData.Split(',');
            int tokenIndex = 0;
            if (Enum.TryParse<EUniScanCCommand>(tokens[tokenIndex++], out EUniScanCCommand command))
            {
                CommandInfo.Command = command;

                for (int i = tokenIndex; i < tokens.Length; i++)
                {
                    CommandInfo.Parameters.Add(tokens[i]);
                }
            }
            else
            {
                CommandInfo.Command = EUniScanCCommand.Unknown;
            }

            return CommandInfo;
        }
    }
}
