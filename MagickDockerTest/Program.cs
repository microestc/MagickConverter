using MagickConverter;
using System;

namespace MagickDockerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new MagickService();
            service.Convert(new TaskSetup("1000", "/app/app/0.pdf", @"/app/app/0.jpeg"));
            Console.WriteLine("Hello World!");
        }
    }
}
