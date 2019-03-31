
using ConfuserDeobfuscator.Engine.Routines.Ex.x86;
using de4dot.blocks;
using de4dot.blocks.cflow;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.MD;
using dnlib.DotNet.Writer;
using EasyPredicateKiller.x86;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace EasyPredicateKiller
{
    internal class Program
    {
        public static int x86Fixed = 0;
        public static MemoryStream mem = new MemoryStream();
        public static ModuleDefMD module = (ModuleDefMD)null;

        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("No input file specified...");
            }
            else
            {
                Configuration.AssemblyFilename = args[0];
                ModuleDefMD module = ModuleDefMD.Load(Configuration.AssemblyFilename, (ModuleCreationOptions)null);
                TypeDef globalType = module.GlobalType;
                List<MethodDef> methodDefList = new List<MethodDef>();
                foreach (MethodDef method in globalType.Methods.Where<MethodDef>((Func<MethodDef, bool>)(m => m.IsNative)).ToList<MethodDef>())
                {
                    MethodDef ilFromX86Method = X86MethodToILConverter.CreateILFromX86Method(new X86Method(method));
                    method.DeclaringType.Methods.Add(ilFromX86Method);
                    methodDefList.Add(method);
                }
                foreach (MethodDef methodDef1 in methodDefList)
                {
                    MethodDef replacedMethod = methodDef1;
                    IEnumerable<Instruction> allReferences = replacedMethod.FindAllReferences(module);
                    MethodDef methodDef2 = module.GlobalType.Methods.FirstOrDefault<MethodDef>((Func<MethodDef, bool>)(m => m.Name == (string)replacedMethod.Name + "_IL"));
                    foreach (Instruction instruction in allReferences)
                        instruction.Operand = (object)methodDef2;
                    ++Program.x86Fixed;
                }
                foreach (MethodDef methodDef in methodDefList)
                    globalType.Methods.Remove(methodDef);
                module.IsStrongNameSigned = false;
                module.Assembly.PublicKey = (PublicKey)null;
                NativeModuleWriterOptions options1 = new NativeModuleWriterOptions(module);
                options1.MetaDataOptions.Flags |= MetaDataFlags.PreserveAll;
                options1.MetaDataOptions.Flags |= MetaDataFlags.KeepOldMaxStack;
                options1.Cor20HeaderOptions.Flags = new ComImageFlags?(ComImageFlags.ILOnly | ComImageFlags._32BitRequired);
                module.NativeWrite((Stream)Program.mem, options1);
                Program.module = ModuleDefMD.Load((Stream)Program.mem);
                ModuleWriterOptions options2 = new ModuleWriterOptions((ModuleDef)Program.module);
                options1.MetaDataOptions.Flags |= MetaDataFlags.PreserveAll;
                options1.MetaDataOptions.Flags |= MetaDataFlags.KeepOldMaxStack;
                Program.module.Write(args[0].Replace(".exe", "-x86_mode_Fixed.exe"), options2);
                Console.WriteLine("Resolved : " + (object)Program.x86Fixed + " natives ints");
            }
        }
    }
}
