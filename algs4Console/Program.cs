using System;
using System.Linq;
using System.Reflection;

namespace algs4Console
{
    static class Program
    {
        /// <summary>
        /// Possible templates to extract RunMain methods from, usually class name is the name of the algorithm
        /// </summary>
        private static readonly string[] AssemblyTemplates =
        {
            "algs4.algs4.{0}`1[System.Object], algs4" ,
            "algs4.algs4.{0}, algs4",
            "algs4.stdlib.{0}, algs4"
        };

        /// <summary>
        /// The name of the method to extract and run from the desired class
        /// </summary>
        private const string ClassMainMethodName = "RunMain";

        /// <summary>
        /// Main method, runs the RunMethod for the class name
        /// passed in the first argument, and passes the rest of the arguments
        /// </summary>
        /// <param name="args">Main arguments</param>
        static void Main(string[] args)
        {
            // Parse arguments
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: algs4Console.exe className [arg0[ args1[...]]]");
                return;
            }
            string className = args.First();
            string[] actualArgs = args.Skip(1).ToArray();

            // Extract class
            Type classType = AssemblyTemplates.Select(template => Type.GetType(string.Format(template, className))).FirstOrDefault(type => type != null);
            if (classType == null)
            {
                Console.WriteLine("Class with name \"{0}\" was not found!", className);
                return;
            }

            // Extract RunMain method
            MethodInfo mainMethod = classType.GetMethod(ClassMainMethodName, BindingFlags.Public | BindingFlags.Static);
            if (mainMethod == null)
            {
                Console.WriteLine("Class \"{0}\" doesn't have \"RunMain\" method!", className);
                Console.WriteLine("If you think this is a bug, please report it: https://github.com/mina-asham/algs4-CSharp");
                return;
            }

            // Invoke method
            mainMethod.Invoke(null, new object[] { actualArgs });
        }
    }
}
