using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Inspect;

namespace UniEye.Base.MachineInterface
{
    public class CommandResult
    {
        public bool Ack { get; set; } = false;
        public bool Operation { get; set; } = false;
        public bool Success { get; set; } = false;
        public string Message { get; set; } = "";
    }

    public abstract class CommandHandler
    {
        protected MachineIf machineIf;
        public MachineIf MachineIf { set => machineIf = value; }

        public abstract CommandResult ExecuteCommand(string commandString);
        public abstract void OnConnect(bool bConnect, object sender);
    }

    public enum UmxCommand
    {
        ENTER_WAIT,     // 검사 대기 상태로 전환
        EXIT_WAIT,      // 검사 대기 상태를 해제
        BEGIN,          // 검사 시작 ( 다중 단계 검사 )
        TRIG,           // 검사. ( Single Step 일 경우, Trig 명령 만으로 검사 진행
        END,            // 검사 완료 ( 다중 단계 검사 )
        CANCEL,         // 검사 취소
        PAUSE,          // 검사 대기
        LOT_CHANGE,     // 로트 번호 변경
    }

    public class UmxCommandHandler : CommandHandler
    {
        public override CommandResult ExecuteCommand(string commandString)
        {
            var commandResult = new CommandResult();

            if (string.IsNullOrEmpty(commandString) == true)
            {
                return commandResult;
            }

            string[] tokens = commandString.Split(',');

            bool result = Enum.TryParse(tokens[0], out UmxCommand defaultCommand);

            InspectRunner inspectRunner = SystemManager.Instance().InspectRunner;

            switch (defaultCommand)
            {
                case UmxCommand.ENTER_WAIT:
                    inspectRunner.EnterWaitInspection(null);
                    break;
                case UmxCommand.EXIT_WAIT:
                    inspectRunner.ExitWaitInspection();
                    break;
                case UmxCommand.BEGIN:
                    inspectRunner.BeginInspect();
                    break;
                case UmxCommand.TRIG:
                    int triggerIndex = -1;
                    if (tokens.Count() > 1)
                    {
                        triggerIndex = Convert.ToInt32(tokens[1]);
                    }

                    inspectRunner.Inspect(triggerIndex);
                    break;
                case UmxCommand.END:
                    inspectRunner.EndInspect();
                    break;
                case UmxCommand.CANCEL:
                    inspectRunner.CancelInspect();
                    break;
                case UmxCommand.PAUSE:
                    inspectRunner.PauseInspect();
                    break;
                case UmxCommand.LOT_CHANGE:
                    inspectRunner.LotChange();
                    break;
            }

            commandResult.Operation = true;

            return commandResult;
        }

        public override void OnConnect(bool bConnect, object sender)
        {

        }
    }
}
