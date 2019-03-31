
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.IO;
using dnlib.PE;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace EasyPredicateKiller
{
    public static class MethodDefExt
    {
        private static IntPtr moduleHandle = IntPtr.Zero;

        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        public static ModuleDefMD OriginalMD { get; set; }

        public static byte[] ReadBodyFromRva(this MethodDef method)
        {
            if (MethodDefExt.OriginalMD == null)
                MethodDefExt.OriginalMD = AssemblyDef.Load(Configuration.AssemblyFilename, (ModuleCreationOptions)null).ManifestModule as ModuleDefMD;
            IImageStream fullStream = MethodDefExt.OriginalMD.MetaData.PEImage.CreateFullStream();
            FileOffset fileOffset = MethodDefExt.OriginalMD.MetaData.PEImage.ToFileOffset(method.RVA);
            long num = MethodDefExt.OriginalMD.MetaData.PEImage.ToFileOffset((RVA)MethodDefExt.OriginalMD.TablesStream.ReadMethodRow(method.Rid + 1U).RVA) - fileOffset;
            byte[] buffer = new byte[100];
            fullStream.Position = (long)(fileOffset + 20L);
            fullStream.Read(buffer, 0, buffer.Length);
            return buffer;
        }

        public static IEnumerable<Instruction> FindAllReferences(this MethodDef mDef, ModuleDefMD module)
        {
            List<Instruction> instructionList = new List<Instruction>();
            List<MethodDef> methodDefList = new List<MethodDef>();
            for (int index = 1; index < 65536; ++index)
            {
                MethodDef methodDef = module.ResolveMethod((uint)index);
                if (methodDef != null && methodDef.HasBody)
                    methodDefList.Add(methodDef);
            }
            foreach (MethodDef methodDef in methodDefList)
            {
                if (methodDef.HasBody)
                {
                    for (int index = 0; index < methodDef.Body.Instructions.Count; ++index)
                    {
                        if (methodDef.Body.Instructions[index].OpCode == OpCodes.Call)
                        {
                            MethodDef operand1 = methodDef.Body.Instructions[index].Operand as MethodDef;
                            MDToken mdToken;
                            int num1;
                            if (operand1 != null)
                            {
                                mdToken = operand1.MDToken;
                                int int32_1 = mdToken.ToInt32();
                                mdToken = mDef.MDToken;
                                int int32_2 = mdToken.ToInt32();
                                num1 = int32_1 == int32_2 ? 1 : 0;
                            }
                            else
                                num1 = 0;
                            if (num1 != 0)
                                instructionList.Add(methodDef.Body.Instructions[index]);
                            MethodSpec operand2 = methodDef.Body.Instructions[index].Operand as MethodSpec;
                            int num2;
                            if (operand2 != null)
                            {
                                mdToken = operand2.MDToken;
                                int int32_1 = mdToken.ToInt32();
                                mdToken = mDef.MDToken;
                                int int32_2 = mdToken.ToInt32();
                                num2 = int32_1 == int32_2 ? 1 : 0;
                            }
                            else
                                num2 = 0;
                            if (num2 != 0)
                                instructionList.Add(methodDef.Body.Instructions[index]);
                        }
                    }
                }
            }
            return (IEnumerable<Instruction>)instructionList;
        }
    }
}
