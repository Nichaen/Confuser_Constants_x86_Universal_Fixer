
using dnlib.DotNet;
using dnlib.DotNet.Writer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyPredicateKiller
{
    public static class MethodExporter
    {
        public static void ExportMethodsToDll(string filename, List<MethodDef> nativeMethods, ModuleDefMD asmModule)
        {
            ModuleDef moduleDef = (ModuleDef)new ModuleDefUser((UTF8String)filename);
            moduleDef.Kind = ModuleKind.Dll;
            AssemblyDefUser assemblyDefUser = new AssemblyDefUser(new UTF8String("TestAssembly"), new Version(1, 2, 3, 4));
            assemblyDefUser.Modules.Add(moduleDef);
            moduleDef.Types.Add((TypeDef)new TypeDefUser((UTF8String)"Startup", (UTF8String)"MyType", moduleDef.CorLibTypes.Object.TypeDefOrRef));
            TypeDef typeDef = moduleDef.Types.FirstOrDefault<TypeDef>((Func<TypeDef, bool>)(m => m.Name == "MyType"));
            for (int i = 0; i < nativeMethods.Count; i++)
            {
                MethodDef methodDef = asmModule.GlobalType.Methods.FirstOrDefault<MethodDef>((Func<MethodDef, bool>)(m => m.Name == (string)nativeMethods[i].Name + "_IL"));
                MethodDefUser methodDefUser = new MethodDefUser(new UTF8String(Encoding.UTF8.GetBytes((string)nativeMethods[i].Name)), methodDef.MethodSig, methodDef.Attributes);
                methodDefUser.Body = methodDef.Body;
                methodDefUser.DeclaringType = (TypeDef)null;
                methodDefUser.Name = (UTF8String)Convert.ToBase64String(Encoding.UTF8.GetBytes((string)nativeMethods[i].Name));
                typeDef.Methods.Add((MethodDef)methodDefUser);
            }
            ModuleWriterOptions options = new ModuleWriterOptions();
            assemblyDefUser.Write(filename, options);
        }
    }
}
