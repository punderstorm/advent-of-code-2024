using System.Configuration.Assemblies;
using System.Reflection;

Console.WriteLine("Enter the day number to execute");
var day = Console.ReadLine();

Assembly.GetExecutingAssembly().GetType($"day{day}")!.InvokeMember("Run", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, new object [] {});
