using System;
using System.IO;

namespace MovieOrganizer
{
    class Program
    {
        static void Main(string[] args)
        {
            new Organizer(GetPath(args));

            Console.WriteLine("");
            Console.WriteLine("Finished. Press any key to exit");
            Console.ReadKey();
        }

        private static string GetPath(string[] args)
        {
            return args.Length == 0 ? Directory.GetCurrentDirectory() : args[0];
        }
    }
}
