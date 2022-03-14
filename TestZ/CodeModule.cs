using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System.Reflection;

namespace RuntimeCodeExec
{
    /// <summary>
    /// A module that bases around dynamically loading C# code in a runtime environment.
    /// </summary>
    public class CodeModule
    {
        /// <summary>
        /// Memory stream that will be used to store the assembly when Loaded.
        /// </summary>
        private MemoryStream _assemblyStream = new MemoryStream();

        /// <summary>
        /// Load code into the module
        /// </summary>
        /// <param name="code">The code you wish to load.</param>
        /// <param name="result">The compile result.</param>
        /// <returns>True if the code is valid, False if the code is not valid.</returns>
        public bool Load(string code, out EmitResult result)
        {
            // flush the stream
            _assemblyStream.Flush();

            // create the syntax tree
            SyntaxTree synTree = CSharpSyntaxTree.ParseText(code);
            // add all references
            List<PortableExecutableReference> references = new List<PortableExecutableReference>();
            foreach (var reference in ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")).Split(Path.PathSeparator))
            {
                references.Add(MetadataReference.CreateFromFile(reference));
            }

            // create the compiler
            var compileResult = CSharpCompilation.Create("DynamicAssemblyTest.dll", new SyntaxTree[] { synTree }, references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, optimizationLevel: OptimizationLevel.Release));
            // copy the assembly to memory
            result = compileResult.Emit(_assemblyStream);

            // return the status
            return result.Success;
        }

        /// <summary>
        /// Execute the module
        /// </summary>
        /// <param name="parameters">Parameters that will be passed to Main</param>
        /// <returns>The result of function execution.</returns>
        public object Execute(params string[] parameters)
        {
            // check the assembly stream to see if theres any data in it...
            if (_assemblyStream.Length < 1)
                return null;

            // get the assembly and invoke the main method, while returning the result of it...
            var customAssembly = Assembly.Load(_assemblyStream.ToArray());
            Console.WriteLine("EXECTUING IN LAYER 1");
            return customAssembly.DefinedTypes.First(x => x.Name == "Program").DeclaredMethods.First(x => x.Name == "Main").Invoke(null, new[] { parameters });
        }


        /// <summary>
        /// Flush the memory stream if needed, automatically flushes on code load.
        /// </summary>
        public void FlushStream() => _assemblyStream.Flush();

        /// <summary>
        /// Get the assembly in bytes.
        /// </summary>
        /// <returns></returns>
        public byte[] GetAssembly() => _assemblyStream.ToArray();
    }
}
