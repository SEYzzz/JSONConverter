using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

string inputFile = "content_eng.txt";
string outputFile = "output.json";

try
{
    var jsonDocument = ConvertToJson(inputFile);

    string jsonString = JsonSerializer.Serialize(jsonDocument, new JsonSerializerOptions { WriteIndented = true });

    File.WriteAllText(outputFile, jsonString);

    Console.WriteLine($"Конвертация завершена! Файл сохранён в {outputFile}");
    Console.WriteLine(jsonString);
}
catch (Exception ex)
{
    Console.WriteLine($"Ошибка: {ex.Message}");
}

static Dictionary<string, object> ConvertToJson(string inputFile)
{
    Dictionary<string, object> root = new Dictionary<string, object>();
    Stack<Dictionary<string, object>> stack = new Stack<Dictionary<string, object>>();
    Stack<string> numberStack = new Stack<string>();

    stack.Push(root);
    numberStack.Push("");

    string[] lines = File.ReadAllLines(inputFile);

    for (int i = 0; i < lines.Length; i++)
    {
        string line = lines[i];

        if (IsSectionHeader(line))
        {
            string sectionHeader = line;
            string sectionNumber = ExtractSectionNumber(line);

            while (numberStack.Count > 1 && !sectionNumber.StartsWith(numberStack.Peek()))
            {
                stack.Pop();
                numberStack.Pop();
            }

            var currentLevel = stack.Peek();
            string? content = null;

            if (i + 1 < lines.Length && !IsSectionHeader(lines[i + 1]))
            {
                content = lines[i + 1];
                i++;
            }

            bool hasSubSections = false;
            if (i + 1 < lines.Length && IsSectionHeader(lines[i + 1]))
            {
                string nextSectionNumber = ExtractSectionNumber(lines[i + 1]);
                hasSubSections = nextSectionNumber.StartsWith(sectionNumber + ".");
            }

            if (hasSubSections)
            {
                var nested = new Dictionary<string, object>();
                if (content != null)
                {
                    nested["content"] = content;
                }

                currentLevel[sectionHeader] = nested;
                stack.Push(nested);
                numberStack.Push(sectionNumber + ".");
            }
            else
            {
                currentLevel[sectionHeader] = content ?? "";
            }
        }
    }

    return root;
}

#region Helpers
static bool IsSectionHeader(string line)
{
    return !string.IsNullOrWhiteSpace(line) && char.IsDigit(line[0]);
}

static string ExtractSectionNumber(string line)
{
    int firstSpaceIndex = line.IndexOf(' ');
    if (firstSpaceIndex > 0)
    {
        return line.Substring(0, firstSpaceIndex);
    }
    return line;
}
#endregion