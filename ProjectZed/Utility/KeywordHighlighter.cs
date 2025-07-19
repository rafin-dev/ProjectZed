using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace ProjectZed
{
    public struct TextBlock
    {
        public TextBlock(string text, string color, int index)
        {
            Text = text;
            Color = color;
            Index = index;
        }

        public string Text = string.Empty;
        public string Color = string.Empty;
        public int Index = 0;
    }

    public class KeywordHighlighter
    {
        private static Dictionary<string, Func<KeywordHighlighter>> s_ExtensionToConstructor = new();

        public static void Init()
        {
            s_ExtensionToConstructor[".lua"] = () => { return new LuaHighlighter(); } ;
        }

        public static KeywordHighlighter CreateHighliter(string extension)
        {


            return new KeywordHighlighter();
        }

        public virtual List<TextBlock> HighlightKeywords(string text) { return new List<TextBlock>(); }
    }

    static partial class Extensions
    {
        public static IEnumerable<int> AllIndexesOf(this string str, string value)
        {
            Regex regex = new Regex(value);
            MatchCollection results = regex.Matches(str);
            foreach (Match match in results)
            {
                yield return match.Index;
            }
        }
    }
}
