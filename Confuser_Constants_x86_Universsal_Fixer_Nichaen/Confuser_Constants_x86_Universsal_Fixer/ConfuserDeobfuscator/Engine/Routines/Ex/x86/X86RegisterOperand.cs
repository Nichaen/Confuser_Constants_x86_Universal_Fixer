using System;

namespace ConfuserDeobfuscator.Engine.Routines.Ex.x86
{
	public class X86RegisterOperand : IX86Operand
	{
		public X86Register Register { get; set; }

		public X86RegisterOperand(X86Register reg)
		{
			this.Register = reg;
		}
	}
}
