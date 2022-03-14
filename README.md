# Runtime-Code-Execution
A project I created to execute C# code dynamically on runtime!
- Preview
![image](https://user-images.githubusercontent.com/69564793/158260271-7e0dae4d-37d2-42b9-b287-d227585210af.png)


# How to use
- Construct the class
```cs 
// dotnet < 5
CodeModule module = new CodeModule();
// dotnet 5 >=
CodeModule module = new();
```
- Define your code
```cs
string _code = @"
using System;
public class Program
{
  public static void Main(string[] args)
  {
    Console.WriteLine(args == null ?? string.Join("" "", args));
  }
}
";
```

- Load and execute your code
```cs
var codeResult = module.Load(_code, out EmitResult result); // returns true if successful
if(!codeResult)
  Console.WriteLine("Failed to load code"); // false, commonly for syntax errors and what not...
else
{
  Console.WriteLine("Successfully compiled code");
  module.Execute(); // you can pass string args here as if it were directly passed into the Main function...
}
```

- Full example
```cs

using System;
using RuntimeCodeExec;
namespace YOUR_PROJECT
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CodeModule module = new CodeModule();
      string _code = @"
      using System;
      public class Program
      {
        public static void Main(string[] args)
        {
          Console.WriteLine(args == null ?? string.Join("" "", args));
        }
      }
      ";

      var codeResult = module.Load(_code, out EmitResult result); // returns true if successful
      if(!codeResult)
        Console.WriteLine("Failed to load code"); // false, commonly for syntax errors and what not...
      else
      {
        Console.WriteLine("Successfully compiled code");
        module.Execute(); // you can pass string args here as if it were directly passed into the Main function...
      }
    }
  }
}
```
