
using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace EasyPredicateKiller
{
    public class X86ILTester
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        public static bool TestNativeWithILMethods(string filenameAsm1, string filenameAsm2)
        {
            IntPtr num = X86ILTester.LoadLibrary(filenameAsm1);
            AssemblyDef assemblyDef = AssemblyDef.Load(filenameAsm1, (ModuleCreationOptions)null);
            MethodInfo[] methods = Assembly.LoadFile(filenameAsm2).GetModules()[0].GetTypes()[0].GetMethods(BindingFlags.Static | BindingFlags.Public);
            foreach (MethodDef methodDef in assemblyDef.Modules[0].GlobalType.Methods.Where<MethodDef>((Func<MethodDef, bool>)(m => m.IsNative)))
            {
                MethodDef method = methodDef;
                if ((int)((IEnumerable<MethodInfo>)methods).FirstOrDefault<MethodInfo>((Func<MethodInfo, bool>)(m => m.Name == Convert.ToBase64String(Encoding.UTF8.GetBytes((string)method.Name)))).Invoke((object)null, new object[1]
                {
          (object) 4919
                }) != ((X86ILTester.PredicateCall)Marshal.GetDelegateForFunctionPointer(new IntPtr((long)num + (long)method.NativeBody.RVA), typeof(X86ILTester.PredicateCall)))(4919))
                    throw new Exception("WRONG CODE!");
            }
            return true;
        }

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        private delegate int PredicateCall(int currentNum);
    }
}
