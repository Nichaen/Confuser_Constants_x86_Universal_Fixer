using System;
using System.Collections.Generic;
using EasyPredicateKiller;
using SharpDisasm;

namespace ConfuserDeobfuscator.Engine.Routines.Ex.x86.Instructions
{
	internal class X86NEG : X86Instruction
	{
		public X86NEG(Instruction rawInstruction)
		{
			base.Operands = new IX86Operand[1];
			base.Operands[0] = rawInstruction.Operands[0].GetOperand();
		}

		public override X86OpCode OpCode
		{
			get
			{
				return X86OpCode.NEG;
			}
		}

		public override void Execute(Dictionary<string, int> registers, Stack<int> localStack)
		{
			registers[((X86RegisterOperand)base.Operands[0]).Register.ToString()] = -registers[((X86RegisterOperand)base.Operands[0]).Register.ToString()];
		}
	}
}
