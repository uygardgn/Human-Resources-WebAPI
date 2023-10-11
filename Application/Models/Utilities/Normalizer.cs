using System.Text;

namespace Application.Models.Utilities
{
    public static class Normalizer
    {
        public static string convertCharacters(string input)
        {
            input = input.Trim();
            var arr=input.Split(' ');
            input = string.Empty;
            foreach(var item in arr)
            {
                input += item;
            }
            StringBuilder temp = new StringBuilder();

            foreach (char ch in input)
            {
                switch (ch)
                {
                    case 'ç':
                        temp.Append("c");
                        break;
                    case 'Ç':
                        temp.Append("C");
                        break;
                    case 'ğ':
                        temp.Append("g");
                        break;
                    case 'Ğ':
                        temp.Append("G");
                        break;
                    case 'ı':
                        temp.Append("i");
                        break;
                    case 'İ':
                        temp.Append("I");
                        break;
                    case 'ö':
                        temp.Append("o");
                        break;
                    case 'Ö':
                        temp.Append("O");
                        break;
                    case 'ş':
                        temp.Append("s");
                        break;
                    case 'Ş':
                        temp.Append("S");
                        break;
                    case 'ü':
                        temp.Append("u");
                        break;
                    case 'Ü':
                        temp.Append("U");
                        break;
                    default:
                        temp.Append(ch);
                        break;
                }
            }
            return temp.ToString();
        }

        public static string Normalize(string input)
        {
            input = input.ToUpper();

            if (input.Contains("İ"))
            {
                input = input.Replace('İ', 'I');
            }
            return input;
        }
    }
}
