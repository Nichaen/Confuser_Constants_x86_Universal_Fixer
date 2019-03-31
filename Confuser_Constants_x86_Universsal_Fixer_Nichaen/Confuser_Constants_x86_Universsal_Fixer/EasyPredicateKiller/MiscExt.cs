﻿using System;
using ConfuserDeobfuscator.Engine.Routines.Ex.x86;
using SharpDisasm;
using SharpDisasm.Udis86;

namespace EasyPredicateKiller
{
	public static class MiscExt
	{
		public static IX86Operand GetOperand(this Operand argument)
		{
			bool flag = argument.Type == ud_type.UD_OP_IMM;
			IX86Operand result;
			if (flag)
			{
				result = new X86ImmediateOperand((int)argument.Value);
			}
			else
			{
				result = new X86RegisterOperand((X86Register)argument.Base);
			}
			return result;
		}
	}
}
