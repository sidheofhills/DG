using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

                string lines = "";

                using (var sr = new StreamReader(new FileStream(filePath, FileMode.Open, FileAccess.Read)))
                {
                    var text = sr.ReadToEnd().ToLower();
                    var words = GetWords(text);
                    lines = GetParsedLinesForCSV(words);
                    //доделать выход в таблицу
                }

                if (lines == "")
                {
                    return;
                }

                var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "wordsTable.csv");
                using (var sw = new StreamWriter(new FileStream(outputPath, FileMode.OpenOrCreate, FileAccess.Write)))
                {
                    sw.Write(lines);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        private string GetParsedLinesForCSV(string[] words)
        {
            if (words.Length > 2)
            {
                return GetParsedLines(words);
            }

            return $"{words[0]}, 1, 100%";
        }

        private string GetParsedLines(string[] words)
        {
            var lines = new StringBuilder();
            var uniqueWords = GetUniqueWords(words);
            foreach (var uniqueWord in uniqueWords)
            {
                string line = $"{uniqueWord.Key.ToString()}, {uniqueWord.Value.ToString()}, {(uniqueWord.Value * 100f / words.Length).ToString()}";

                lines.AppendLine(line);
            }
            return lines.ToString();
        }

        private static Dictionary<string, int> GetUniqueWords(string[] words)
        {
            var uniqueWords = new Dictionary<string, int>();
            foreach (string word in words)
            {
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
            var stringBuilder = new StringBuilder();

            foreach (var t in text)
            {
                if ((t == '\n' || t == '\r' || t == '\t')
                    && stringBuilder.Length > 2 && !stringBuilder[stringBuilder.Length - 1].Equals(' '))
                {
                    stringBuilder.Append(' ');
                }

                if (Char.IsLetter(t) || Char.IsDigit(t) // вероятно нужно условие для цифр с пробелами 30 000
                    || t.Equals(' ') || t.Equals('-'))
                {
                    stringBuilder.Append(t);
                }
            }

            return stringBuilder.ToString().Split(' ');
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

            return Path.Combine(dir, path + ".txt"); // !!!!!!!!!!!!!!!!!
        }
    }
}