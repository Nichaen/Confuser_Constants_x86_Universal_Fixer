using System;

namespace ConfuserDeobfuscator.Engine.Routines.Ex.x86
{
	public class X86ImmediateOperand : IX86Operand
	{
		public int Immediate { get; set; }

		public X86ImmediateOperand(int imm)
		{
			this.Immediate = imm;
		}
	}
}
