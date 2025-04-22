using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRACTICE_2_20042025.Models;
using PRACTICE_2_20042025.Workers;
using PRACTICE_2_20042025.GenerateFile;


namespace PRACTICE_2_20042025
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //FileGenerator1 generator = new FileGenerator1();
            //generator.Generate();
            FileAnalyser fileAnalyser = new FileAnalyser();
            await fileAnalyser.ProcessFilesInFolderAsync(@"C:\Users\kiril\Desktop\Test");
        }
    }
}
