using System.Text.RegularExpressions;
using System.Windows.Threading;

namespace ProjectZed
{
    public class LuaHighlighter : KeywordHighlighter
    {
        public override List<TextBlock> HighlightKeywords(string text)
        {
            List<TextBlock> result = new List<TextBlock>();

            List<TextBlock> stringsAndComments = new();

            // Find strings
            int lastIndex = -1;
            foreach (int index in text.AllIndexesOf("\""))
            {
                if (lastIndex == -1)
                {
                    lastIndex = index;
                    continue;
                }

                stringsAndComments.Add(new TextBlock(text.Substring(lastIndex, index + 1 - lastIndex), "#087500", lastIndex));
                lastIndex = -1;
            }
            if (lastIndex != -1)
            {
                stringsAndComments.Add(new TextBlock(text.Substring(lastIndex, text.Length - lastIndex), "#087500", lastIndex));
            }

            // Find comments
            foreach (int index in text.AllIndexesOf("--"))
            {
                stringsAndComments.Add(new TextBlock(text.Substring(index, text.IndexOf("\n", index) + 1 - index), "#a69598", index));
            }

            result.AddRange(stringsAndComments);

            foreach (var keyvalue in m_KeyWordToColor)
            {
                foreach (int index in text.AllIndexesOf(keyvalue.Key))
                {
                    if (index > 0)
                    {
                        if (Char.IsLetterOrDigit(text[index - 1]))
                        {
                            continue;
                        }
                    }
                    else if (index < text.Length - 1)
                    {
                        if (Char.IsLetterOrDigit(text[index + keyvalue.Key.Length]))
                        {
                            continue;
                        }
                    }

                    result.Add(new TextBlock(keyvalue.Key, keyvalue.Value, index));
                }
            }

            if (result.Count == 0) // No keyword to color
            {
                return result;
            }

            result = result.OrderBy(o => o.Index).ToList();
            List<TextBlock> basicText = new();

            // Add non keyword text before the first keyword
            string t0 = text.Substring(0, result[0].Index);
            if (t0 != string.Empty)
            {
                basicText.Add(new TextBlock(t0 + "\n", "#f0f8ff", 0));
            }

            // Add non keyword text in between keywords
            for (int i = 0; i < result.Count - 1; i++)
            {
                int keywordEndIndex = result[i].Index + result[i].Text.Length;
                string t = text.Substring(keywordEndIndex, result[i + 1].Index - keywordEndIndex);

                basicText.Add(new TextBlock(t, "#f0f8ff", keywordEndIndex));
            }

            // Add non keyword text after last keyword
            int keywordEndIndex1 = result[result.Count - 1].Index + result[result.Count - 1].Text.Length;
            string t1 = text.Substring(keywordEndIndex1, text.Length - keywordEndIndex1);
            if (t1 != string.Empty)
            {
                basicText.Add(new TextBlock(t1, "#f0f8ff", keywordEndIndex1));
            }

            result.AddRange(basicText);

            // Changed the original text to the colored one
            result = result.OrderBy(o => o.Index).ToList();

            return result;
        }

        private Dictionary<string, string> m_KeyWordToColor = new Dictionary<string, string>()
        {
            { "and",        "#414dba" },
            { "break",      "#414dba" },
            { "do",         "#414dba" },
            { "else",       "#414dba" },
            { "elseif",     "#414dba" },
            { "end",        "#414dba" },
            { "false",      "#414dba" },
            { "for",        "#414dba" },
            { "function",   "#414dba" },
            { "if",         "#414dba" },
            { "in",         "#414dba" },
            { "local",      "#414dba" },
            { "nil",        "#414dba" },
            { "not",        "#414dba" },
            { "or",         "#414dba" },
            { "repeat",     "#414dba" },
            { "return",     "#414dba" },
            { "then",       "#414dba" },
            { "true",       "#414dba" },
            { "until",      "#414dba" },
            { "while",      "#414dba" }
        };
    }
}
