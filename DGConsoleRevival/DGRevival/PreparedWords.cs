using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace DGRevival
{
    public class PreparedWords
    {
        private List<string> m_Words = new();

        public Dictionary<string, int> UniqueWords = new Dictionary<string, int>();

        public bool TryFillWordsList(in string text)
        {
            var words = SeparateWords(text);
            if (words == null || words.Length == 0)
            {
                return false;

            }

            m_Words.AddRange(words);
            return true;
        }

        private static string[] SeparateWords(string line)
        {
            var stringBuilder = new StringBuilder();

            foreach (var l in line)
            {
                if ((l == '\n' || l == '\r' || l == '\t')
                    && stringBuilder.Length > 2 && !stringBuilder[stringBuilder.Length - 1].Equals(' '))
                {
                    stringBuilder.Append(' ');
                }

                if (Char.IsLetter(l) || Char.IsDigit(l) // �������� ����� ������� ��� ���� � ��������� ����� 30 000
                                     || l.Equals(' ') || l.Equals('-'))
                {
                    stringBuilder.Append(l);
                }
            }

            if (stringBuilder.Length == 0)
            {
                return null;
            }

            return stringBuilder.ToString().Split(' ');
        }



        public bool TrySortUniqueWords()
        {
            if (m_Words.Count < 1)
            {
                return false;
            }

            foreach (string word in m_Words)
            {
                //��� ����� ��� �� ��� ������� ��������, ������� �� ������ ����� ��� ���������
                if (UniqueWords.ContainsKey(word))
                {
                    UniqueWords[word]++;
                    continue;
                }

                UniqueWords[word] = 1;
            }
            return true;
        }

        public IEnumerable<string> GetParsedLinesForCsv()
        {
            if (UniqueWords.Count == 0)
            {
                yield return "";
            }

            var sorted = UniqueWords.OrderByDescending(i => i.Value);

            foreach (var word in sorted)
            {
                yield return $"{word.Key}; {word.Value}; {(word.Value * 100f / UniqueWords.Count).ToString("0.00")}%\n";
            }
        }
    }
}