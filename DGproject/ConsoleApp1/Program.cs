using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter a txt-file path \nOr a txt-file name if it's in the working directory"); //!!!!!!!!
            WordFrequencyChecker checker = new WordFrequencyChecker();
            string userInput = Console.ReadLine();

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
        public void StarterMethod(string path)
        {
            try
            {
                var filePath = GetPath(path);

                if (filePath == "")
                {
                    return;
                }

                using (var sr = new StreamReader(new FileStream(filePath, FileMode.Open, FileAccess.Read)))
                {
                    var text = sr.ReadToEnd().ToLower();
                    var words = GetWords(text);
                    string[][] lines = GetParsedLinesForCSV(words);
                    //доделать выход в таблицу

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        private string[][] GetParsedLinesForCSV(string[] words)
        {
            if (words.Length > 2)
            {
                return GetParsedLines(words);
            }

            return new[] { new[] { words[0], "1", "100%" } };
        }

        private string[][] GetParsedLines(string[] words)
        {
            var lineList = new List<string[]>();
            var uniqueWords = GetUniqueWords(words);
            foreach (var uniqueWord in uniqueWords)
            {
                var line = new[] {
                    uniqueWord.Key.ToString(),
                    uniqueWord.Value.ToString(),
                    (uniqueWord.Value * 100f / words.Length).ToString() };

                lineList.Add(line);
            }
            return lineList.ToArray();
        }

        private static Dictionary<string, int> GetUniqueWords(string[] words)
        {
            var uniqueWords = new Dictionary<string, int>();
            foreach (string word in words)
            {
                if (word == "") //по-другому парсить надо, но пусть так для простоты
                {
                    continue;
                }

                //вот здесь мог бы быь сложный механизм, который бы парсил слова без окончаний
                if (uniqueWords.ContainsKey(word))
                {
                    uniqueWords[word]++;
                    continue;
                }

                uniqueWords[word] = 1;
            }
            return uniqueWords;
        }

        private static string[] GetWords(string text)
        {
            var patternN = new Regex("[\n]");
            var newText = patternN.Replace(text, " ");
            var pattern = new Regex("[;,.!?:;\t\r]");
            newText = pattern.Replace(newText, "");
            return newText.Split(' ');
        }

        private string GetPath(string path)
        {
            // для шортката хак, но вообще здесь логика сборки пути из папки
            if (path == " ")
            {
                path = "хармс";
            }
            var dir = Directory.GetCurrentDirectory();
            dir = dir.Remove(45, dir.Length - 45) + "00\\";

            if (!Directory.Exists(dir))
            {
                Console.WriteLine("Path doesn't exist.");
                return ""; //!!!!!!!!!!!!!
            }

            return Path.Combine(dir, path + ".txt"); // !!!!!!!!!!!!!!!!!
        }
    }
}
