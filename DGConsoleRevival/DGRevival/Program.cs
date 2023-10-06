using System;
using System.Collections.Generic;
using System.IO;

namespace DGRevival
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter a txt-file path \nOr a txt-file name if it's in the working directory"); //!!!!!!!!
            var checker = new WordFrequencyChecker();
            string? userInput = Console.ReadLine();

            if (userInput == null)
            {
                Console.WriteLine("No input.");
                return;
            }

            checker.StarterMethod(userInput);
        }
    }
    public class WordFrequencyChecker

    {
        public PreparedWords Words = new();

        public void StarterMethod(string path)
        {
            try
            {
                var filePath = GetPath(path);

                if (filePath == "" || !Path.Exists(filePath))
                {
                    return;
                }

                using (var sr = new StreamReader(new FileStream(filePath, FileMode.Open, FileAccess.Read)))
                {
                    while (!sr.EndOfStream)
                    {
                        var text = sr.ReadLine()?.ToLower();


                        if (text == null || !Words.TryFillWordsList(text))
                        {
                            continue;
                        }

                    }
                }

                if (!Words.TrySortUniqueWords()) //поздно фиксируется ошибка
                {
                    throw new Exception("No valuable characters are found to sort");

                }


                var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "wordsTable.csv");
                using (var sw = new StreamWriter(new FileStream(outputPath, FileMode.OpenOrCreate, FileAccess.Write)))
                {
                    foreach (string line in Words.GetParsedLinesForCsv())
                    {
                        sw.Write(line); //почему ломается кодировка, если записывать результаты файла байконур?
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }


        private string GetPath(string filename)
        {
            // для шортката хак, но вообще здесь логика сборки пути из папки
            filename = GetCheatFileName(filename);
            var dir = Directory.GetCurrentDirectory();
            dir = dir.Remove(45, dir.Length - 45) + "00\\";

            return Path.Combine(dir, filename + ".txt"); // !!!!!!!!!!!!!!!!!
        }

        private static string GetCheatFileName(string path) => path switch
        {
            "x" => "хармс",
            "b" => "байконур",
            "t" => "tool",
            "g" => "годо",

            _ => path
        };
    }
}