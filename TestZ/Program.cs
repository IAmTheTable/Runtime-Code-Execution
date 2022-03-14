using Microsoft.CodeAnalysis.Emit;

namespace TestZ
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string _code = File.ReadAllText("test.txt");

            CodeModule module = new CodeModule();

            if (module.Load(_code, out EmitResult result))
            {
                Console.WriteLine("Success layer 1");
                module.Execute("Argument 1.", "Argument 2.", "Argument 2.");
            }
            else
            {
                Console.WriteLine("[----Begin-Failure----]");
                foreach (var reason in result.Diagnostics)
                    Console.WriteLine(reason);
                Console.WriteLine("[----END-Failure----]");
            }
        }
    }
}