using System;
using ConfuserDeobfuscator.Engine.Routines.Ex.x86;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace EasyPredicateKiller.x86
{
	public class X86MethodToILConverter
	{
		public static MethodDef CreateILFromX86Method(X86Method methodToConvert)
		{
			TypeSig int32Type = methodToConvert.OriginalMethod.ReturnType;
			MethodDefUser returnMethod = new MethodDefUser(new UTF8String(methodToConvert.OriginalMethod.Name + "_IL"), MethodSig.CreateStatic(int32Type, int32Type), MethodImplAttributes.IL, MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static | MethodAttributes.HideBySig);
			returnMethod.Body = new CilBody();
			returnMethod.Body.MaxStack = 12;
			for (int i = 0; i < 8; i++)
			{
				returnMethod.Body.Variables.Add(new Local(int32Type));
			}
			CilBody returnMethodBody = returnMethod.Body;
			returnMethodBody.Instructions.Add(new Instruction(OpCodes.Ldarg_0));
			foreach (X86Instruction x86Instruction in methodToConvert.Instructions)
			{
				switch (x86Instruction.OpCode)
				{
				case X86OpCode.MOV:
				{
					bool flag = x86Instruction.Operands[0] is X86ImmediateOperand;
					if (flag)
					{
						throw new Exception("Can't mov value to immediate value");
					}
					bool flag2 = x86Instruction.Operands[1] is X86RegisterOperand;
					if (flag2)
					{
						returnMethodBody.Instructions.Add(X86MethodToILConverter.GetLdlocInstructionFromRegister((x86Instruction.Operands[1] as X86RegisterOperand).Register, returnMethodBody));
					}
					else
					{
						bool flag3 = x86Instruction.Operands[1] is X86ImmediateOperand;
						if (flag3)
						{
							returnMethodBody.Instructions.Add(new Instruction(OpCodes.Ldc_I4, (x86Instruction.Operands[1] as X86ImmediateOperand).Immediate));
						}
					}
					returnMethodBody.Instructions.Add(X86MethodToILConverter.GetStlocInstructionFromRegister((x86Instruction.Operands[0] as X86RegisterOperand).Register, returnMethodBody));
					break;
				}
				case X86OpCode.ADD:
				{
					bool flag4 = x86Instruction.Operands[0] is X86ImmediateOperand;
					if (flag4)
					{
						throw new Exception("Can't add value to immediate value");
					}
					returnMethodBody.Instructions.Add(X86MethodToILConverter.GetLdlocInstructionFromRegister((x86Instruction.Operands[0] as X86RegisterOperand).Register, returnMethodBody));
					bool flag5 = x86Instruction.Operands[1] is X86RegisterOperand;
					if (flag5)
					{
						returnMethodBody.Instructions.Add(X86MethodToILConverter.GetLdlocInstructionFromRegister((x86Instruction.Operands[1] as X86RegisterOperand).Register, returnMethodBody));
					}
					else
					{
						bool flag6 = x86Instruction.Operands[1] is X86ImmediateOperand;
						if (flag6)
						{
							returnMethodBody.Instructions.Add(new Instruction(OpCodes.Ldc_I4, (x86Instruction.Operands[1] as X86ImmediateOperand).Immediate));
						}
					}
					returnMethodBody.Instructions.Add(new Instruction(OpCodes.Add));
					returnMethodBody.Instructions.Add(X86MethodToILConverter.GetStlocInstructionFromRegister((x86Instruction.Operands[0] as X86RegisterOperand).Register, returnMethodBody));
					break;
				}
				case X86OpCode.SUB:
				{
					bool flag7 = x86Instruction.Operands[0] is X86ImmediateOperand;
					if (flag7)
					{
						throw new Exception("Can't sub value to immediate value");
					}
					returnMethodBody.Instructions.Add(X86MethodToILConverter.GetLdlocInstructionFromRegister((x86Instruction.Operands[0] as X86RegisterOperand).Register, returnMethodBody));
					bool flag8 = x86Instruction.Operands[1] is X86RegisterOperand;
					if (flag8)
					{
						returnMethodBody.Instructions.Add(X86MethodToILConverter.GetLdlocInstructionFromRegister((x86Instruction.Operands[1] as X86RegisterOperand).Register, returnMethodBody));
					}
					else
					{
						bool flag9 = x86Instruction.Operands[1] is X86ImmediateOperand;
						if (flag9)
						{
							returnMethodBody.Instructions.Add(new Instruction(OpCodes.Ldc_I4, (x86Instruction.Operands[1] as X86ImmediateOperand).Immediate));
						}
					}
					returnMethodBody.Instructions.Add(new Instruction(OpCodes.Sub));
					returnMethodBody.Instructions.Add(X86MethodToILConverter.GetStlocInstructionFromRegister((x86Instruction.Operands[0] as X86RegisterOperand).Register, returnMethodBody));
					break;
				}
				case X86OpCode.IMUL:
				{
					bool flag10 = x86Instruction.Operands[0] is X86ImmediateOperand;
					if (flag10)
					{
						throw new Exception("Can't imul value to immediate value");
					}
					bool flag11 = x86Instruction.Operands[1] is X86RegisterOperand;
					if (flag11)
					{
						returnMethodBody.Instructions.Add(X86MethodToILConverter.GetLdlocInstructionFromRegister((x86Instruction.Operands[1] as X86RegisterOperand).Register, returnMethodBody));
					}
					else
					{
						bool flag12 = x86Instruction.Operands[1] is X86ImmediateOperand;
						if (flag12)
						{
							returnMethodBody.Instructions.Add(new Instruction(OpCodes.Ldc_I4, (x86Instruction.Operands[1] as X86ImmediateOperand).Immediate));
						}
					}
					bool flag13 = x86Instruction.Operands[2] is X86RegisterOperand;
					if (flag13)
					{
						returnMethodBody.Instructions.Add(X86MethodToILConverter.GetLdlocInstructionFromRegister((x86Instruction.Operands[2] as X86RegisterOperand).Register, returnMethodBody));
					}
					else
					{
						bool flag14 = x86Instruction.Operands[2] is X86ImmediateOperand;
						if (flag14)
						{
							returnMethodBody.Instructions.Add(new Instruction(OpCodes.Ldc_I4, (x86Instruction.Operands[2] as X86ImmediateOperand).Immediate));
						}
					}
					returnMethodBody.Instructions.Add(new Instruction(OpCodes.Mul));
					returnMethodBody.Instructions.Add(X86MethodToILConverter.GetStlocInstructionFromRegister((x86Instruction.Operands[0] as X86RegisterOperand).Register, returnMethodBody));
					break;
				}
				case X86OpCode.DIV:
				{
					bool flag15 = x86Instruction.Operands[0] is X86ImmediateOperand;
					if (flag15)
					{
						throw new Exception("Can't div value to immediate value");
					}
					returnMethodBody.Instructions.Add(X86MethodToILConverter.GetLdlocInstructionFromRegister((x86Instruction.Operands[0] as X86RegisterOperand).Register, returnMethodBody));
					bool flag16 = x86Instruction.Operands[1] is X86RegisterOperand;
					if (flag16)
					{
						returnMethodBody.Instructions.Add(X86MethodToILConverter.GetLdlocInstructionFromRegister((x86Instruction.Operands[1] as X86RegisterOperand).Register, returnMethodBody));
					}
					else
					{
						bool flag17 = x86Instruction.Operands[1] is X86ImmediateOperand;
						if (flag17)
						{
							returnMethodBody.Instructions.Add(new Instruction(OpCodes.Ldc_I4, (x86Instruction.Operands[1] as X86ImmediateOperand).Immediate));
						}
					}
					returnMethodBody.Instructions.Add(new Instruction(OpCodes.Div_Un));
					returnMethodBody.Instructions.Add(X86MethodToILConverter.GetStlocInstructionFromRegister((x86Instruction.Operands[0] as X86RegisterOperand).Register, returnMethodBody));
					break;
				}
				case X86OpCode.NEG:
				{
					bool flag18 = x86Instruction.Operands[0] is X86ImmediateOperand;
					if (flag18)
					{
						throw new Exception("Can't neg immediate value");
					}
					returnMethodBody.Instructions.Add(X86MethodToILConverter.GetLdlocInstructionFromRegister((x86Instruction.Operands[0] as X86RegisterOperand).Register, returnMethodBody));
					returnMethodBody.Instructions.Add(new Instruction(OpCodes.Neg));
					returnMethodBody.Instructions.Add(X86MethodToILConverter.GetStlocInstructionFromRegister((x86Instruction.Operands[0] as X86RegisterOperand).Register, returnMethodBody));
					break;
				}
				case X86OpCode.NOT:
				{
					bool flag19 = x86Instruction.Operands[0] is X86ImmediateOperand;
					if (flag19)
					{
						throw new Exception("Can't not immediate value");
					}
					returnMethodBody.Instructions.Add(X86MethodToILConverter.GetLdlocInstructionFromRegister((x86Instruction.Operands[0] as X86RegisterOperand).Register, returnMethodBody));
					returnMethodBody.Instructions.Add(new Instruction(OpCodes.Not));
					returnMethodBody.Instructions.Add(X86MethodToILConverter.GetStlocInstructionFromRegister((x86Instruction.Operands[0] as X86RegisterOperand).Register, returnMethodBody));
					break;
				}
				case X86OpCode.XOR:
				{
					bool flag20 = x86Instruction.Operands[0] is X86ImmediateOperand;
					if (flag20)
					{
						throw new Exception("Can't xor value to immediate value");
					}
					returnMethodBody.Instructions.Add(X86MethodToILConverter.GetLdlocInstructionFromRegister((x86Instruction.Operands[0] as X86RegisterOperand).Register, returnMethodBody));
					bool flag21 = x86Instruction.Operands[1] is X86RegisterOperand;
					if (flag21)
					{
						returnMethodBody.Instructions.Add(X86MethodToILConverter.GetLdlocInstructionFromRegister((x86Instruction.Operands[1] as X86RegisterOperand).Register, returnMethodBody));
					}
					else
					{
						bool flag22 = x86Instruction.Operands[1] is X86ImmediateOperand;
						if (flag22)
						{
							returnMethodBody.Instructions.Add(new Instruction(OpCodes.Ldc_I4, (x86Instruction.Operands[1] as X86ImmediateOperand).Immediate));
						}
					}
					returnMethodBody.Instructions.Add(new Instruction(OpCodes.Xor));
					returnMethodBody.Instructions.Add(X86MethodToILConverter.GetStlocInstructionFromRegister((x86Instruction.Operands[0] as X86RegisterOperand).Register, returnMethodBody));
					break;
				}
				case X86OpCode.POP:
					returnMethodBody.Instructions.Add(X86MethodToILConverter.GetStlocInstructionFromRegister((x86Instruction.Operands[0] as X86RegisterOperand).Register, returnMethodBody));
					break;
				}
			}
			returnMethodBody.Instructions.Add(new Instruction(OpCodes.Ldloc_0));
			returnMethodBody.Instructions.Add(new Instruction(OpCodes.Ret));
			return returnMethod;
		}

		private static Instruction GetLdlocInstructionFromRegister(X86Register register, CilBody body)
		{
			switch (register)
			{
			case X86Register.EAX:
				return new Instruction(OpCodes.Ldloc_0);
			case X86Register.ECX:
				return new Instruction(OpCodes.Ldloc_1);
			case X86Register.EDX:
				return new Instruction(OpCodes.Ldloc_2);
			case X86Register.EBX:
				return new Instruction(OpCodes.Ldloc_3);
			case X86Register.ESI:
				return new Instruction(OpCodes.Ldloc_S, body.Variables[6]);
			}
			throw new Exception("Not implemented");
		}

		private static Instruction GetStlocInstructionFromRegister(X86Register register, CilBody body)
		{
			switch (register)
			{
			case X86Register.EAX:
				return new Instruction(OpCodes.Stloc_0);
			case X86Register.ECX:
				return new Instruction(OpCodes.Stloc_1);
			case X86Register.EDX:
				return new Instruction(OpCodes.Stloc_2);
			case X86Register.EBX:
				return new Instruction(OpCodes.Stloc_3);
			case X86Register.ESI:
				return new Instruction(OpCodes.Stloc_S, body.Variables[6]);
			}
			throw new Exception("Not implemented");
		}
	}
}
