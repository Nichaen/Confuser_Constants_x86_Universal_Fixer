
using ConfuserDeobfuscator.Engine.Routines.Ex.x86.Instructions;
using dnlib.DotNet;
using EasyPredicateKiller;
using SharpDisasm;
using System.Collections.Generic;
using System.Linq;

namespace ConfuserDeobfuscator.Engine.Routines.Ex.x86
{
    public sealed class X86Method
    {
        public Stack<int> LocalStack = new Stack<int>();
        public Dictionary<string, int> Registers = new Dictionary<string, int>()
    {
      {
        "EAX",
        0
      },
      {
        "EBX",
        0
      },
      {
        "ECX",
        0
      },
      {
        "EDX",
        0
      },
      {
        "ESP",
        0
      },
      {
        "EBP",
        0
      },
      {
        "ESI",
        0
      },
      {
        "EDI",
        0
      }
    };
        public List<X86Instruction> Instructions;
        public MethodDef OriginalMethod;

        public X86Method(MethodDef method)
        {
            this.Instructions = new List<X86Instruction>();
            this.ParseInstructions(method);
            this.OriginalMethod = method;
        }

        private void ParseInstructions(MethodDef method)
        {
            List<Instruction> instructionList = new List<Instruction>();
            foreach (Instruction rawInstruction in new Disassembler(method.ReadBodyFromRva(), ArchitectureMode.x86_32, 0UL, false, Vendor.Any).Disassemble().ToList<Instruction>())
            {
                bool flag = false;
                string str = rawInstruction.ToString() + " ";
                switch (str.Remove(str.IndexOf(" ")))
                {
                    case "add":
                        this.Instructions.Add((X86Instruction)new X86ADD(rawInstruction));
                        break;
                    case "div":
                        this.Instructions.Add((X86Instruction)new X86DIV(rawInstruction));
                        break;
                    case "imul":
                        this.Instructions.Add((X86Instruction)new X86IMUL(rawInstruction));
                        break;
                    case "mov":
                        this.Instructions.Add((X86Instruction)new X86MOV(rawInstruction));
                        break;
                    case "neg":
                        this.Instructions.Add((X86Instruction)new X86NEG(rawInstruction));
                        break;
                    case "not":
                        this.Instructions.Add((X86Instruction)new X86NOT(rawInstruction));
                        break;
                    case "pop":
                        this.Instructions.Add((X86Instruction)new X86POP(rawInstruction));
                        break;
                    case "ret":
                        while (this.Instructions[this.Instructions.Count - 1].OpCode == X86OpCode.POP)
                            this.Instructions.RemoveAt(this.Instructions.Count - 1);
                        flag = true;
                        break;
                    case "sub":
                        this.Instructions.Add((X86Instruction)new X86SUB(rawInstruction));
                        break;
                    case "xor":
                        this.Instructions.Add((X86Instruction)new X86XOR(rawInstruction));
                        break;
                }
                if (flag)
                    break;
            }
        }

        public int Execute(params int[] @params)
        {
            foreach (int num in @params)
                this.LocalStack.Push(num);
            foreach (X86Instruction instruction in this.Instructions)
                instruction.Execute(this.Registers, this.LocalStack);
            return this.Registers["EAX"];
        }
    }
}
