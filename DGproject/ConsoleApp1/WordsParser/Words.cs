
namespace ConsoleApp1
{
    public class Words
    {

        public string[] GetWords(string text)
        {
            var stringBuilder = new StringBuilder();

            foreach (var t in text)
            {
                if ((t == '\n' || t == '\r' || t == '\t')
                    && stringBuilder.Length > 2 && !stringBuilder[stringBuilder.Length - 1].Equals(' '))
                {
                    stringBuilder.Append(' ');
                }

                if (Char.IsLetter(t) || Char.IsDigit(t) // �������� ����� ������� ��� ���� � ��������� ����� 30 000
                                     || t.Equals(' ') || t.Equals('-'))
                {
                    stringBuilder.Append(t);
                }
            }

            //� ���� ������?
            return stringBuilder.ToString().Split(' ');
        }

    }
}

