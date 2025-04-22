using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PRACTICE_2_20042025.Models;

namespace PRACTICE_2_20042025.Workers
{
    internal class FileAnalyser
    {
        private readonly Mutex mutexObj = new Mutex();
        private List<FileAnalysis> fileAnalysisResults = new List<FileAnalysis>();
        private int allword = 0;
        private int allchar = 0;

        public async Task ProcessFilesInFolderAsync(string folderPath)
        {
            string[] files;
            try
            {
                files = Directory.GetFiles(folderPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении списка файлов: {ex.Message}");
                return;
            }

            List<Task> tasks = new List<Task>();

            foreach (string file in files)
            {
                tasks.Add(Task.Run(() => ProcessFile(file, fileAnalysisResults))); // запускаем задачи в фон
            }

            await Task.WhenAll(tasks);

            Console.WriteLine();
            Console.Write("Итог:");
            Console.Write($" Слов: {allword}");
            Console.Write($" Символов: {allchar}");
            Console.WriteLine();

            //PrintStatus(fileAnalysisResults);
            //PrintStatus();
        }

        private async Task ProcessFile(string filePath, List<FileAnalysis> fileAnalysisResults)
        {
            string content = string.Empty;
            string fileName = Path.GetFileName(filePath);
            FileAnalysis analysis = new FileAnalysis();

            try
            {
                var reader = new StreamReader(filePath);
                content = await reader.ReadToEndAsync();

                //Основную работу по синхронизации выполняют методы WaitOne() и ReleaseMutex().
                //Метод mutexObj.WaitOne() приостанавливает выполнение потока до тех пор, пока не будет получен мьютекс mutexObj.

                //Изначально мьютекс свободен, поэтому его получает один из потоков.

                //После выполнения всех действий, когда мьютекс больше не нужен, поток освобождает его с помощью метода mutexObj.ReleaseMutex().
                //А мьютекс получает один из ожидающих потоков.

                //Таким образом, когда выполнение дойдет до вызова mutexObj.WaitOne(), поток будет ожидать, пока не освободится мьютекс.
                //И после его получения продолжит выполнять свою работу.

                mutexObj.WaitOne();

                //int charCount = content.Length;
                //int wordCount = content.Split(new[] { ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;

                analysis.FileName = fileName;
                analysis.SymbolNumber = content.Length;
                analysis.WordNumber = content.Split(new[] { ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
                //analysis.WordNumber = wordCount;
                //analysis.SymbolNumber = charCount;

                allword = allword + analysis.WordNumber;
                allchar = allchar + analysis.SymbolNumber;

                fileAnalysisResults.Add(analysis);
                PrintStatus1(analysis);

                mutexObj.ReleaseMutex();
            }
            catch (FileNotFoundException)
            {
                mutexObj.WaitOne();

                Console.WriteLine($"Файл не найден: {fileName}");

                mutexObj.ReleaseMutex();
            }
            catch (IOException ex)
            {
                mutexObj.WaitOne();
                
                Console.WriteLine($"Ошибка ввода-вывода при обработке файла {fileName}: {ex.Message}");
                
                mutexObj.ReleaseMutex();
            }
            catch (Exception ex)
            {
                mutexObj.WaitOne();

                Console.WriteLine($"Неизвестная ошибка при обработке файла {fileName}: {ex.Message}");

                mutexObj.ReleaseMutex();
            }
        }
        private void PrintStatus(List<FileAnalysis> fileAnalysisResults)
        {
            int allword = 0;
            int allsymbol = 0;
            foreach (var analysis in fileAnalysisResults)
            {
                Console.Write($"{analysis.FileName}: ");
                Console.Write($"Слов: {analysis.WordNumber}");
                Console.Write($" Символов: {analysis.SymbolNumber}");
                Console.WriteLine();
                allword = allword + analysis.WordNumber;
                allsymbol = allsymbol + analysis.SymbolNumber;
            }
            Console.Write("Итог: ");
            Console.Write($"Слов: {allword}");
            Console.Write($" Символов: {allsymbol}");
            Console.WriteLine();
        }

        private void PrintStatus1(FileAnalysis analysis)
        {
            Console.Write($"{analysis.FileName}: ");
            Console.Write($"Слов: {analysis.WordNumber}");
            Console.Write($" Символов: {analysis.SymbolNumber}");
            Console.WriteLine();
        }
    }
}
