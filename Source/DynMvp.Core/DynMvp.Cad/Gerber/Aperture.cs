using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Gerber
{
    public class Aperture
    {
        public int ApertureNo { get; set; }
        public ApertureType Type { get; set; }
        public ApertureMacro Macro { get; set; }
        public List<SimplifiedApertureMacro> SamList { get; }
        public double[] Parameter { get; } = new double[Constant.APERTURE_PARAMETERS_MAX];
        public int NumParameter { get; }
        public Unit Unit { get; set; }

        public Aperture()
        {
            SamList = new List<SimplifiedApertureMacro>();
        }

        public void Clear()
        {
            SamList.Clear();
        }

        public void AddSimplifiedApertureMacro(SimplifiedApertureMacro simplifiedApertureMacro)
        {
            SamList.Add(simplifiedApertureMacro);
        }

        public bool AddSimplifiedApertureMacro(double scale)
        {
            bool handled = true;
            int numParameters = 0;
            bool clearOperatorUsed = false;
            ApertureType type = ApertureType.None;

            var valueStack = new Stack<double>();

            double tempValue1;
            double tempValue2;
            foreach (Instruction instruction in Macro.ProgramList)
            {
                switch (instruction.OpCode)
                {
                    case OpCode.NOP:
                        break;
                    case OpCode.Push:
                        valueStack.Push(instruction.GetDouble());
                        break;
                    case OpCode.PushParam:
                        Debug.Assert(Parameter != null);
                        valueStack.Push(Parameter[instruction.GetInt() - 1]);
                        break;
                    case OpCode.PopParam:
                        Debug.Assert(Parameter != null);
                        tempValue1 = valueStack.Pop();
                        Parameter[instruction.GetInt() - 1] = tempValue1;
                        break;
                    case OpCode.Add:
                        tempValue1 = valueStack.Pop();
                        tempValue2 = valueStack.Pop();
                        valueStack.Push(tempValue1 + tempValue2);
                        break;
                    case OpCode.Sub:
                        tempValue1 = valueStack.Pop();
                        tempValue2 = valueStack.Pop();
                        valueStack.Push(tempValue2 - tempValue1);
                        break;
                    case OpCode.Mul:
                        tempValue1 = valueStack.Pop();
                        tempValue2 = valueStack.Pop();
                        valueStack.Push(tempValue2 * tempValue1);
                        break;
                    case OpCode.Div:
                        tempValue1 = valueStack.Pop();
                        tempValue2 = valueStack.Pop();
                        valueStack.Push(tempValue2 / tempValue1);
                        break;
                    case OpCode.Primative:
                        /* 
                         * This handles the exposure thing in the aperture macro
                         * The exposure is always the first element on stack independent
                         * of aperture macro.
                         */
                        switch (instruction.GetInt())
                        {
                            case 1:
                                LogHelper.Debug(LoggerType.Operation, "  Aperture macro circle [1] (");
                                type = ApertureType.MacroCircle;
                                numParameters = 4;
                                break;
                            case 3:
                                break;
                            case 4:
                                LogHelper.Debug(LoggerType.Operation, "  Aperture macro outline [4] (");
                                type = ApertureType.MacroOutline;
                                /*
                                 * Number of parameters are:
                                 * - number of points defined in entry 1 of the stack + 
                                 *   start point. Times two since it is both X and Y.
                                 * - Then three more; exposure,  nuf points and rotation.
                                 */
                                numParameters = ((int)valueStack.ToArray()[1] + 1) * 2 + 3;
                                break;
                            case 5:
                                LogHelper.Debug(LoggerType.Operation, "  Aperture macro polygon [5] (");
                                type = ApertureType.MacroPolygon;
                                numParameters = 6;
                                break;
                            case 6:
                                LogHelper.Debug(LoggerType.Operation, "  Aperture macro moir� [6] (");
                                type = ApertureType.MacroMoire;
                                numParameters = 9;
                                break;
                            case 7:
                                LogHelper.Debug(LoggerType.Operation, "  Aperture macro thermal [7] (");
                                type = ApertureType.MacroThermal;
                                numParameters = 6;
                                break;
                            case 2:
                            case 20:
                                LogHelper.Debug(LoggerType.Operation, "  Aperture macro line 20/2 (");
                                type = ApertureType.MacroLine20;
                                numParameters = 7;
                                break;
                            case 21:
                                LogHelper.Debug(LoggerType.Operation, "  Aperture macro line 21 (");
                                type = ApertureType.MacroLine21;
                                numParameters = 6;
                                break;
                            case 22:
                                LogHelper.Debug(LoggerType.Operation, "  Aperture macro line 22 (");
                                type = ApertureType.MacroLine22;
                                numParameters = 6;
                                break;
                            default:
                                handled = false;
                                break;
                        }

                        if (type != ApertureType.None)
                        {
                            if (numParameters > Constant.APERTURE_PARAMETERS_MAX)
                            {
                                LogHelper.Debug(LoggerType.Operation, "Number of parameters to aperture macro are more than gerbv is able to store.");
                                return false;
                            }

                            /*
                             * Create struct for simplified aperture macro and
                             * start filling in the blanks.
                             */
                            var simplifiedApertureMacro = new SimplifiedApertureMacro();
                            simplifiedApertureMacro.Type = type;
                            simplifiedApertureMacro.CopyParameter(Parameter);

                            double[] samParam = simplifiedApertureMacro.Parameter;

                            int startIdx = 0;
                            int endIdx = 0;
                            bool checkFirst = false;
                            /* convert any mm values to inches */
                            switch (type)
                            {
                                case ApertureType.MacroCircle:
                                    checkFirst = true;
                                    startIdx = 1; endIdx = 4;
                                    break;
                                case ApertureType.MacroOutline:
                                    checkFirst = true;
                                    startIdx = 2; endIdx = numParameters - 1;
                                    break;
                                case ApertureType.MacroPolygon:
                                    checkFirst = true;
                                    startIdx = 2; endIdx = 5;
                                    break;
                                case ApertureType.MacroMoire:
                                    startIdx = 0; endIdx = 8;
                                    break;
                                case ApertureType.MacroThermal:
                                    startIdx = 0; endIdx = 5;
                                    break;
                                case ApertureType.MacroLine20:
                                    checkFirst = true;
                                    startIdx = 1; endIdx = 6;
                                    break;
                                case ApertureType.MacroLine21:
                                case ApertureType.MacroLine22:
                                    checkFirst = true;
                                    startIdx = 1; endIdx = 5;
                                    break;
                                default:
                                    break;
                            }

                            if (checkFirst == true && Math.Abs(samParam[0]) < 0.001)
                            {
                                clearOperatorUsed = true;
                            }

                            for (int j = startIdx; j < endIdx; j++)
                            {
                                samParam[j] /= scale;
                            }

                            /* 
                             * Add this simplified aperture macro to the end of the list
                             * of simplified aperture macros. If first entry, put it
                             * in the top.
                             */
                            SamList.Add(simplifiedApertureMacro);

#if _DEBUG
                            double[] valueArr = valueStack.ToArray();
                            for (i = 0; i < numParameters; i++)
                            {
                                DebugLog("%f, ", valueArr[i]);
                            }
#endif
                            LogHelper.Debug(LoggerType.Operation, ")\n");
                        }

                        /* 
                         * Here we reset the stack pointer. It's not general correct
                         * correct to do this, but since I know how the compiler works
                         * I can do this. The correct way to do this should be to 
                         * subtract number of used elements in each primitive operation.
                         */
                        // s->sp = 0;
                        valueStack.Clear();
                        break;
                    default:
                        break;
                }
            }

            valueStack.Clear();

            /* store a flag to let the renderer know if it should expect any "clear"
               primatives */
            Parameter[0] = Convert.ToDouble(clearOperatorUsed);

            return handled;
        }
    }
}
