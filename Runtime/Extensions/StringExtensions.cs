namespace DesertImage.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// string.Format shortcut
        /// </summary>
        /// <param name="text"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static string PasteValues(this string text, params object[] arguments)
        {
            return string.Format(text, arguments);
        }

        public static string FirstCharToUpper(this string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            return input[0].ToString().ToUpper() + input.Substring(1);
        }

        public static string ShortCut(this string text, int maxLength = 147)
        {
            if (text.Length <= maxLength) return text;

            return text.Remove(maxLength, text.Length - maxLength) + "...";
        }

        public static string WrapByTag(this string text, string tag, string parameter = "")
        {
            return $"<{tag} {parameter}>{text}</{tag}>";
        }
        
        public static string BoldTag(this string text)
        {
            return $"<b>{text}</b>";
        }
        
        public static string ColorTag(this string text, string color)
        {
            return $"<color={color}>{text}</color>";
        }
        
        public static string SizeTag(this string text, int size = 16)
        {
            return $"<size={size.ToString()}>{text}</size>";
        }
    }
}