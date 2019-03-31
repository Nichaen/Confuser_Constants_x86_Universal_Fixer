using System;
using System.Collections.Generic;
using EasyPredicateKiller;
using SharpDisasm;

namespace ConfuserDeobfuscator.Engine.Routines.Ex.x86.Instructions
{
	internal class X86IMUL : X86Instruction
	{
		public X86IMUL(Instruction rawInstruction)
		{
			base.Operands = new IX86Operand[3];
			base.Operands[0] = rawInstruction.Operands[0].GetOperand();
			base.Operands[1] = rawInstruction.Operands[1].GetOperand();
			base.Operands[2] = rawInstruction.Operands[2].GetOperand();
		}

		public override X86OpCode OpCode
		{
			get
			{
				return X86OpCode.IMUL;
			}
		}
		public override void Execute(Dictionary<string, int> registers, Stack<int> localStack)
		{
			string source = ((X86RegisterOperand)base.Operands[0]).Register.ToString();
			string target = ((X86RegisterOperand)base.Operands[1]).Register.ToString();
			bool flag = base.Operands[2] is X86ImmediateOperand;
			if (flag)
			{
				registers[source] = registers[target] * ((X86ImmediateOperand)base.Operands[2]).Immediate;
			}
			else
			{
				registers[source] = registers[target] * registers[((X86RegisterOperand)base.Operands[2]).Register.ToString()];
			}
		}
	}
}
