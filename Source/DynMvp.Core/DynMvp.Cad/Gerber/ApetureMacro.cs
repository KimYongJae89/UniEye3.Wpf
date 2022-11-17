using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Gerber
{
    public class ApertureMacro
    {
        public string Name { get; set; }
        public List<Instruction> ProgramList { get; } = new List<Instruction>();
        public uint NumOpPush { get; set; } = 0;

        public Instruction AddInstruction(OpCode opCode)
        {
            var instruction = new Instruction();
            instruction.OpCode = opCode;

            if (opCode == OpCode.Push || opCode == OpCode.PushParam)
            {
                NumOpPush++;
            }

            ProgramList.Add(instruction);

            return instruction;
        }

        public static ApertureMacro Parse(BinaryReader reader)
        {
            var apertureMacro = new ApertureMacro();

            int primitive = 0;
            bool found_primitive = false;

            var mathOpStack = new Stack<OpCode>();
            int comma = 0; /* Just read an operator (one of '*+X/) */
            bool negativeValue = false; /* negative numbers succeding , */
            int equate = 0;

            /*
             * Get macroname
             */
            apertureMacro.Name = GerberLoaderHelper.ReadString(reader, '*');

            char ch = (char)reader.ReadByte(); /* skip '*' */

            Instruction instruction;

            while (true)
            {
                ch = (char)reader.ReadByte(); /* skip '*' */
                switch (ch)
                {
                    case '$':
                        if (found_primitive)
                        {
                            instruction = apertureMacro.AddInstruction(OpCode.PushParam);
                            instruction.SetValue(GerberLoaderHelper.ReadInteger(reader));
                            comma = 0;
                        }
                        else
                        {
                            equate = GerberLoaderHelper.ReadInteger(reader);
                        }
                        break;
                    case '*':
                        while (mathOpStack.Count() != 0)
                        {
                            apertureMacro.AddInstruction(mathOpStack.Pop());
                        }
                        /*
                         * Check is due to some gerber files has spurious empty lines.
                         * (EagleCad of course).
                         */
                        if (found_primitive)
                        {
                            if (equate > 0)
                            {
                                instruction = apertureMacro.AddInstruction(OpCode.PopParam);
                                instruction.SetValue(equate);
                            }
                            else
                            {
                                instruction = apertureMacro.AddInstruction(OpCode.Primative);
                                instruction.SetValue(primitive);
                            }
                            equate = 0;
                            primitive = 0;
                            found_primitive = false;
                        }
                        break;
                    case '=':
                        if (equate > 0)
                        {
                            found_primitive = true;
                        }
                        break;
                    case ',':
                        if (!found_primitive)
                        {
                            found_primitive = true;
                            break;
                        }
                        while (mathOpStack.Count() != 0)
                        {
                            apertureMacro.AddInstruction(mathOpStack.Pop());
                        }
                        comma = 1;
                        break;
                    case '+':
                        int opCodeAddOrder = GerberLoaderHelper.GetOpCodeOrder(OpCode.Add);
                        while ((mathOpStack.Count() != 0) &&
                           (GerberLoaderHelper.GetOpCodeOrder(mathOpStack.Peek()) >= opCodeAddOrder))
                        {
                            apertureMacro.AddInstruction(mathOpStack.Pop());
                        }
                        mathOpStack.Push(OpCode.Add);
                        comma = 1;
                        break;
                    case '-':
                        if (comma > 0)
                        {
                            negativeValue = true;
                            comma = 0;
                            break;
                        }
                        int opCodeSubOrder = GerberLoaderHelper.GetOpCodeOrder(OpCode.Sub);

                        while ((mathOpStack.Count() != 0) &&
                          (GerberLoaderHelper.GetOpCodeOrder(mathOpStack.Peek()) >= opCodeSubOrder))
                        {
                            apertureMacro.AddInstruction(mathOpStack.Pop());
                        }
                        mathOpStack.Push(OpCode.Sub);
                        break;
                    case '/':
                        int opCodeDivOrder = GerberLoaderHelper.GetOpCodeOrder(OpCode.Div);
                        while ((mathOpStack.Count() != 0) &&
                           (GerberLoaderHelper.GetOpCodeOrder(mathOpStack.Peek()) >= opCodeDivOrder))
                        {
                            apertureMacro.AddInstruction(mathOpStack.Pop());
                        }
                        mathOpStack.Push(OpCode.Div);
                        comma = 1;
                        break;
                    case 'X':
                    case 'x':
                        int opCodeMulOrder = GerberLoaderHelper.GetOpCodeOrder(OpCode.Mul);
                        while ((mathOpStack.Count() != 0) &&
                           (GerberLoaderHelper.GetOpCodeOrder(mathOpStack.Peek()) >= opCodeMulOrder))
                        {
                            apertureMacro.AddInstruction(mathOpStack.Pop());
                        }
                        mathOpStack.Push(OpCode.Mul);
                        comma = 1;
                        break;
                    case '0':
                        /*
                         * Comments in aperture macros are a definition starting with
                         * zero and ends with a '*'
                         */
                        if (!found_primitive && (primitive == 0))
                        {
                            /* Comment continues 'til next *, just throw it away */
                            GerberLoaderHelper.ReadString(reader, '*');
                            ch = (char)reader.ReadByte();
                            break;
                        }
                        break;
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    case '.':
                        /* 
                         * First number in an aperture macro describes the primitive
                         * as a numerical value
                         */
                        if (!found_primitive)
                        {
                            primitive = (primitive * 10) + (ch - '0');
                            break;
                        }

                        reader.BaseStream.Position--;

                        instruction = apertureMacro.AddInstruction(OpCode.Push);
                        double value = GerberLoaderHelper.ReadDouble(reader);
                        if (negativeValue)
                        {
                            value *= -1;
                        }

                        instruction.SetValue(value);

                        negativeValue = false;
                        comma = 0;
                        break;
                    case '%':
                        reader.BaseStream.Position--;
                        return apertureMacro;
                    default:
                        /* Whitespace */
                        break;
                }
            }
        }

        public void Print()
        {
            Console.WriteLine(string.Format("Macroname {0} :\n", Name));
            foreach (Instruction instruction in ProgramList)
            {
                switch (instruction.OpCode)
                {
                    case OpCode.NOP:
                    case OpCode.Add:
                    case OpCode.Sub:
                    case OpCode.Mul:
                    case OpCode.Div:
                        Console.WriteLine(string.Format("{0} :\n", instruction.OpCode.ToString()));
                        break;
                    case OpCode.Push:
                        Console.WriteLine(string.Format(" {0} {1}\n", instruction.OpCode.ToString(), instruction.GetDouble()));
                        break;
                    case OpCode.PopParam:
                    case OpCode.PushParam:
                    case OpCode.Primative:
                        Console.WriteLine(string.Format(" {0} {1}\n", instruction.OpCode.ToString(), instruction.GetInt()));
                        break;
                    default:
                        Console.WriteLine("  ERROR!\n");
                        break;
                }
            }
        }
    }
}
