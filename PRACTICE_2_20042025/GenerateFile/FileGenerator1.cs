using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRACTICE_2_20042025.GenerateFile
{
    internal class FileGenerator1
    {
        public void Generate()
        {
            string folderPath = "C:\\Users\\kiril\\Desktop\\Test"; // Укажи путь к папке
            Directory.CreateDirectory(folderPath);

            string[] sampleWords = new string[]
            {
                "apple", "banana", "cat", "dog", "elephant", "fish", "grape", "house", "ice", "jungle",
                "kite", "lemon", "monkey", "notebook", "orange", "pizza", "queen", "river", "sun", "tree",
                "umbrella", "vase", "water", "xylophone", "yogurt", "zebra", "car", "book", "road", "light",
                "music", "dream", "sky", "cloud", "rain", "storm", "mountain", "valley", "earth", "fire",
                "ocean", "beach", "flower", "garden", "snow", "wind", "star", "moon", "planet", "space"
            };

            Random rand = new Random();

            for (int i = 1; i <= 1000; i++)
            {
                int wordCount = rand.Next(1, 51); // От 1 до 50 слов
                string[] chosenWords = new string[wordCount];

                for (int j = 0; j < wordCount; j++)
                {
                    int index = rand.Next(sampleWords.Length);
                    chosenWords[j] = sampleWords[index];
                }

                string content = string.Join(" ", chosenWords);
                string fileName = Path.Combine(folderPath, $"file_{i}.txt");

                File.WriteAllText(fileName, content);
            }

            //Console.WriteLine("1000 файлов успешно созданы.");
        }
    }
}
