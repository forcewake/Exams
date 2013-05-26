using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task08
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputDir;
            //inputDir = args[0];
            inputDir = @"E:\TK";
            string outputDir;
            outputDir = @"E:\TK\TK-copy";
            //outputDir = args[1];

            FileMover mover = new FileMover(inputDir, outputDir, 10);
            mover.Copy();

            Console.WriteLine(mover.FilesCounter);

            Console.ReadLine();
        }
    }
}
