using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_2023
{
    public static class Solutions
    {
        // Day 1
        public static int Day1_1(string path)
        {
            string[] values = File.ReadAllLines(path);
            string[] testCases = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            int output = 0;
            for (int i = 0; i < values.Length; i++)
            {
                IndexValuePair indexValuePair = new IndexValuePair();
                for (int j = 0; j < testCases.Length; j++)
                {
                    int caseIndexFirst = values[i].IndexOf(testCases[j]);
                    if (caseIndexFirst != -1 && caseIndexFirst < indexValuePair.MinIndex)
                    {
                        indexValuePair.MinIndex = caseIndexFirst;
                        indexValuePair.MinValue = testCases[j];
                    }

                    int caseIndexLast = values[i].LastIndexOf(testCases[j]);
                    if (caseIndexLast != -1 && caseIndexLast > indexValuePair.MaxIndex)
                    {
                        indexValuePair.MaxIndex = caseIndexLast;
                        indexValuePair.MaxValue = testCases[j];
                    }
                }

                output += int.Parse(indexValuePair.MinValue + indexValuePair.MaxValue);
            }

            //54877
            return output;
        }
        public static int Day1_2(string path)
        {
            string[] values = File.ReadAllLines(path);
            string[] testCases = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            int output = 0;
            for (int i = 0; i < values.Length; i++)
            {
                IndexValuePair indexValuePair = new IndexValuePair();
                for (int j = 0; j < testCases.Length; j++)
                {
                    int caseIndexFirst = values[i].IndexOf(testCases[j]);
                    if (caseIndexFirst != -1 && indexValuePair.MinIndex > caseIndexFirst)
                    {
                        indexValuePair.MinIndex = caseIndexFirst;
                        indexValuePair.MinValue = WordToNumber(testCases[j]);
                    }

                    int caseIndexLast = values[i].LastIndexOf(testCases[j]);
                    if (caseIndexLast != -1 && indexValuePair.MaxIndex < caseIndexLast)
                    {
                        indexValuePair.MaxIndex = caseIndexLast;
                        indexValuePair.MaxValue = WordToNumber(testCases[j]);
                    }
                }

                output += int.Parse(indexValuePair.MinValue + indexValuePair.MaxValue);
            }
            //54100: solution
            return output;
        }

        private static string WordToNumber(string word)
        {
            return word switch
            {
                "one" => "1",
                "two" => "2",
                "three" => "3",
                "four" => "4",
                "five" => "5",
                "six" => "6",
                "seven" => "7",
                "eight" => "8",
                "nine" => "9",
                _ => word,
            };
        }
        public struct IndexValuePair
        {
            public int MinIndex { get; set; }
            public string MinValue { get; set; }
            public int MaxIndex { get; set; }
            public string MaxValue { get; set; }

            public IndexValuePair()
            {
                MinIndex = int.MaxValue;
                MinValue = "";
                MaxIndex = int.MinValue;
                MaxValue = "";
            }
        }

        // Day 2
        public static int Day2_1(string path)
        {
            Dictionary<string, int> cubeRules = new()
            {
                { "red", 12 },
                { "green", 13 },
                { "blue", 14 }
            };
            string[] values = File.ReadAllLines(path);
            int output = 0;
            foreach (var line in values)
            {
                string[] game = line.Split(':');
                string[] sets = game[1].Split(';');
                bool isGameValid = true;
                foreach (var set in sets)
                {
                    Dictionary<string, int> cubeValuePairs = new()
                    {
                        { "red", 0 },
                        { "green", 0 },
                        { "blue", 0 }
                    };
                    string[] cubes = set.Split(',');
                    foreach (var cube in cubes)
                    {
                        string[] cubeValues = cube.Trim().Split(' ');
                        cubeValuePairs[cubeValues[1]] = int.Parse(cubeValues[0]);
                    }
                    if (cubeValuePairs["red"] > cubeRules["red"] || cubeValuePairs["green"] > cubeRules["green"] || cubeValuePairs["blue"] > cubeRules["blue"])
                    {
                        isGameValid = false;
                        break;
                    }
                }
                if (isGameValid)
                    output += int.Parse(game[0].Split(' ')[1]);
            }
            //1734
            return output;
        }
        public static int Day2_2(string path)
        {
            string[] values = File.ReadAllLines(path);
            int output = 0;
            foreach (var line in values)
            {
                string[] game = line.Split(':');
                string[] sets = game[1].Split(';');
                Dictionary<string, int> cubeValuePairs = new()
                {
                    { "red", 0 },
                    { "green", 0 },
                    { "blue", 0 }
                };
                foreach (var set in sets)
                {
                    string[] cubes = set.Split(',');
                    foreach (var cube in cubes)
                    {
                        string[] cubeValues = cube.Trim().Split(' ');
                        string cubeKey = cubeValues[1];
                        int cubeValue = int.Parse(cubeValues[0]);
                        if (cubeValue > cubeValuePairs[cubeKey])
                            cubeValuePairs[cubeKey] = cubeValue;
                    }
                }
                output += cubeValuePairs["red"] * cubeValuePairs["green"] * cubeValuePairs["blue"];
            }
            //70387
            return output;
        }

        //Day 3
        public static int Day3_1(string path)
        {
            string[] values = File.ReadAllLines(path);
            int output = 0;

            for (int i = 0; i < values.Length; i++)
            {
                string line = values[i];
                NumberInEngine engineNum = new();
                for (int j = 0; j < line.Length; j++)
                {
                    if (int.TryParse(line[j].ToString(), out int num))
                    {
                        if (string.IsNullOrEmpty(engineNum.NumberString))
                        {
                            engineNum = new NumberInEngine(num.ToString(), j, i, 1);
                            continue;
                        }
                        engineNum.NumberString += num;
                        engineNum.Length++;
                        if (j != line.Length - 1)
                            continue;
                    }
                    if (string.IsNullOrEmpty(engineNum.NumberString))
                        continue;
                    if (AdjecentToSymbol(engineNum, values))
                        output += int.Parse(engineNum.NumberString);
                    engineNum = new();
                }
            }
            //560670
            return output;
        }
        //Can be shortened
        private static bool AdjecentToSymbol(NumberInEngine engineNum, string[] lines)
        {
            string line = lines[engineNum.LineIndex];
            if (engineNum.LineIndex != 0)
            {
                for (int k = engineNum.StartIndex - 1; k < engineNum.StartIndex + engineNum.Length + 1; k++)
                {
                    if (k < 0 || k >= line.Length)
                        continue;
                    if (lines[engineNum.LineIndex - 1][k] != '.')
                        return true;
                }
            }
            if (engineNum.LineIndex != lines.Length - 1)
            {
                for (int k = engineNum.StartIndex - 1; k < engineNum.StartIndex + engineNum.Length + 1; k++)
                {
                    if (k < 0 || k >= line.Length)
                        continue;
                    if (lines[engineNum.LineIndex + 1][k] != '.')
                        return true;
                }
                if (engineNum.StartIndex != 0)
                {
                    if (lines[engineNum.LineIndex][engineNum.StartIndex - 1] != '.')
                        return true;
                }
                if (engineNum.StartIndex + engineNum.Length < line.Length)
                {
                    if (lines[engineNum.LineIndex][engineNum.StartIndex + engineNum.Length] != '.')
                        return true;
                }
            }
            return false;
        }
        public static int Day3_2(string path)
        {
            string[] values = File.ReadAllLines(path);
            int output = 0;

            Dictionary<string, int> numberGears = new();

            for (int i = 0; i < values.Length; i++)
            {
                string line = values[i];
                NumberInEngine engineNum = new();
                for (int j = 0; j < line.Length; j++)
                {
                    if (int.TryParse(line[j].ToString(), out int num))
                    {
                        if (string.IsNullOrEmpty(engineNum.NumberString))
                        {
                            engineNum = new NumberInEngine(num.ToString(), j, i, 1);
                            continue;
                        }
                        engineNum.NumberString += num;
                        engineNum.Length++;
                        if (j != line.Length - 1)
                            continue;
                    }
                    if (string.IsNullOrEmpty(engineNum.NumberString))
                        continue;

                    output += AdjecentToGear(engineNum, values, numberGears);
                    engineNum = new();
                }
            }
            //91622824
            return output;
        }
        //Can be shortened
        private static int AdjecentToGear(NumberInEngine engineNum, string[] lines, Dictionary<string, int> numberGears)
        {
            string line = lines[engineNum.LineIndex];
            if (engineNum.LineIndex != 0)
            {
                for (int k = engineNum.StartIndex - 1; k < engineNum.StartIndex + engineNum.Length + 1; k++)
                {
                    if (k < 0 || k >= line.Length)
                        continue;
                    if (lines[engineNum.LineIndex - 1][k] == '*')
                    {
                        string id = $"{engineNum.LineIndex - 1}{k}";
                        int number = int.Parse(engineNum.NumberString);
                        if (!numberGears.TryAdd(id, number))
                            return int.Parse(engineNum.NumberString) * numberGears[id];
                    }
                }
            }
            if (engineNum.LineIndex != lines.Length - 1)
            {
                for (int k = engineNum.StartIndex - 1; k < engineNum.StartIndex + engineNum.Length + 1; k++)
                {
                    if (k < 0 || k >= line.Length)
                        continue;
                    if (lines[engineNum.LineIndex + 1][k] == '*')
                    {
                        string id = $"{engineNum.LineIndex + 1}{k}";
                        int number = int.Parse(engineNum.NumberString);
                        if (!numberGears.TryAdd(id, number))
                            return int.Parse(engineNum.NumberString) * numberGears[id];
                    }
                }
            }
            if (engineNum.StartIndex != 0)
            {
                if (lines[engineNum.LineIndex][engineNum.StartIndex - 1] == '*')
                {
                    string id = $"{engineNum.LineIndex}{engineNum.StartIndex - 1}";
                    int number = int.Parse(engineNum.NumberString);
                    if (!numberGears.TryAdd(id, number))
                        return int.Parse(engineNum.NumberString) * numberGears[id];
                }
            }
            if (engineNum.StartIndex + engineNum.Length < line.Length)
            {
                if (lines[engineNum.LineIndex][engineNum.StartIndex + engineNum.Length] == '*')
                {
                    string id = $"{engineNum.LineIndex}{engineNum.StartIndex + engineNum.Length}";
                    int number = int.Parse(engineNum.NumberString);
                    if (!numberGears.TryAdd(id, number))
                        return int.Parse(engineNum.NumberString) * numberGears[id];
                }
            }
            return 0;
        }
        private struct NumberInEngine
        {
            public string NumberString { get; set; }
            public int StartIndex { get; set; }
            public int LineIndex { get; set; }
            public int Length { get; set; }

            public NumberInEngine(string numberString, int startIndex, int lineIndex, int length)
            {
                NumberString = numberString;
                StartIndex = startIndex;
                LineIndex = lineIndex;
                Length = length;
            }
        }

        //Day 4
    }


}
