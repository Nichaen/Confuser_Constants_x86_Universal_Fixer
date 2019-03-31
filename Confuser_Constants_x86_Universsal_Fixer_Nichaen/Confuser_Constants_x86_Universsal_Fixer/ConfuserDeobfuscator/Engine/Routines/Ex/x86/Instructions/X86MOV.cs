﻿using System;
using System.Collections.Generic;
using EasyPredicateKiller;
using SharpDisasm;

namespace ConfuserDeobfuscator.Engine.Routines.Ex.x86.Instructions
{
	internal class X86MOV : X86Instruction
	{
		public X86MOV(Instruction rawInstruction)
		{
			base.Operands = new IX86Operand[2];
			base.Operands[0] = rawInstruction.Operands[0].GetOperand();
			base.Operands[1] = rawInstruction.Operands[1].GetOperand();
		}

		public override X86OpCode OpCode
		{
			get
			{
				return X86OpCode.MOV;
			}
		}

		public override void Execute(Dictionary<string, int> registers, Stack<int> localStack)
		{
			bool flag = base.Operands[1] is X86ImmediateOperand;
			if (flag)
			{
				registers[((X86RegisterOperand)base.Operands[0]).Register.ToString()] = (base.Operands[1] as X86ImmediateOperand).Immediate;
			}
			else
			{
				registers[((X86RegisterOperand)base.Operands[0]).Register.ToString()] = registers[(base.Operands[1] as X86RegisterOperand).Register.ToString()];
			}
		}
	}
}
