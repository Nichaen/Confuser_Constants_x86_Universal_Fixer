using System;
using System.Collections.Generic;

namespace ConfuserDeobfuscator.Engine.Routines.Ex.x86
{
	public abstract class X86Instruction
	{
		public abstract X86OpCode OpCode { get; }

		public IX86Operand[] Operands { get; set; }

		public abstract void Execute(Dictionary<string, int> registers, Stack<int> localStack);
	}
}
