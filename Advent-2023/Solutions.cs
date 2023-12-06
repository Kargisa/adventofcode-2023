using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Advent_2023
{
    public static class Solutions
    {
        // Day 1
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

        public static int Day1_1(string path)
        {
            string[] lines = File.ReadAllLines(path);
            string[] testCases = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            int output = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                IndexValuePair indexValuePair = new IndexValuePair();
                for (int j = 0; j < testCases.Length; j++)
                {
                    int caseIndexFirst = lines[i].IndexOf(testCases[j]);
                    if (caseIndexFirst != -1 && caseIndexFirst < indexValuePair.MinIndex)
                    {
                        indexValuePair.MinIndex = caseIndexFirst;
                        indexValuePair.MinValue = testCases[j];
                    }

                    int caseIndexLast = lines[i].LastIndexOf(testCases[j]);
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
            string[] lines = File.ReadAllLines(path);
            string[] testCases = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            int output = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                IndexValuePair indexValuePair = new IndexValuePair();
                for (int j = 0; j < testCases.Length; j++)
                {
                    int caseIndexFirst = lines[i].IndexOf(testCases[j]);
                    if (caseIndexFirst != -1 && indexValuePair.MinIndex > caseIndexFirst)
                    {
                        indexValuePair.MinIndex = caseIndexFirst;
                        indexValuePair.MinValue = WordToNumber(testCases[j]);
                    }

                    int caseIndexLast = lines[i].LastIndexOf(testCases[j]);
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

        // Day 2
        public static int Day2_1(string path)
        {
            ReadOnlyDictionary<string, int> cubeRules = new(new Dictionary<string, int>() {
                { "red", 12 },
                { "green", 13 },
                { "blue", 14 }
            });
            string[] lines = File.ReadAllLines(path);
            int output = 0;
            foreach (var line in lines)
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
            string[] lines = File.ReadAllLines(path);
            int output = 0;
            foreach (var line in lines)
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

        // Day 3
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

        public static int Day3_1(string path)
        {
            string[] lines = File.ReadAllLines(path);
            int output = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
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
                    if (AdjecentToSymbol(engineNum, lines))
                        output += int.Parse(engineNum.NumberString);
                    engineNum = new();
                }
            }

            //560670
            return output;
        }
        private static bool AdjecentToSymbol(NumberInEngine engineNum, string[] lines)
        {
            string line = lines[engineNum.LineIndex];
            // Check top and bottom
            for (int i = -1; i <= 1; i++)
            {
                if (i == 0)
                    continue;

                if ((engineNum.LineIndex == 0 && i != 1) || (engineNum.LineIndex == lines.Length - 1 && i != -1))
                    continue;

                for (int k = engineNum.StartIndex - 1; k < engineNum.StartIndex + engineNum.Length + 1; k++)
                {
                    if (k < 0 || k >= line.Length)
                        continue;
                    if (lines[engineNum.LineIndex + i][k] != '.')
                        return true;
                }

            }

            // Check left
            if (engineNum.StartIndex != 0)
            {
                if (lines[engineNum.LineIndex][engineNum.StartIndex - 1] != '.')
                    return true;
            }

            // Check right
            if (engineNum.StartIndex + engineNum.Length < line.Length)
            {
                if (lines[engineNum.LineIndex][engineNum.StartIndex + engineNum.Length] != '.')
                    return true;
            }
            return false;
        }

        public static int Day3_2(string path)
        {
            string[] lines = File.ReadAllLines(path);
            int output = 0;

            Dictionary<string, int> numberGears = new();

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
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

                    output += AdjecentToGear(engineNum, lines, numberGears);
                    engineNum = new();
                }
            }
            //91622824
            return output;
        }
        private static int AdjecentToGear(NumberInEngine engineNum, string[] lines, Dictionary<string, int> numberGears)
        {
            string line = lines[engineNum.LineIndex];
            //Checks top and bottom
            for (int i = -1; i <= 1; i++)
            {
                if (i == 0)
                    continue;

                if ((engineNum.LineIndex == 0 && i != 1) || (engineNum.LineIndex == lines.Length - 1 && i != -1))
                    continue;

                for (int k = engineNum.StartIndex - 1; k < engineNum.StartIndex + engineNum.Length + 1; k++)
                {
                    if (k < 0 || k >= line.Length)
                        continue;
                    if (lines[engineNum.LineIndex + i][k] == '*')
                    {
                        string id = $"{engineNum.LineIndex + i}{k}";
                        int number = int.Parse(engineNum.NumberString);
                        if (!numberGears.TryAdd(id, number))
                            return number * numberGears[id];
                    }
                }

            }

            //Checks left
            if (engineNum.StartIndex != 0)
            {
                if (lines[engineNum.LineIndex][engineNum.StartIndex - 1] == '*')
                {
                    string id = $"{engineNum.LineIndex}{engineNum.StartIndex - 1}";
                    int number = int.Parse(engineNum.NumberString);
                    if (!numberGears.TryAdd(id, number))
                        return number * numberGears[id];
                }
            }

            //Checks right
            if (engineNum.StartIndex + engineNum.Length < line.Length)
            {
                if (lines[engineNum.LineIndex][engineNum.StartIndex + engineNum.Length] == '*')
                {
                    string id = $"{engineNum.LineIndex}{engineNum.StartIndex + engineNum.Length}";
                    int number = int.Parse(engineNum.NumberString);
                    if (!numberGears.TryAdd(id, number))
                        return number * numberGears[id];
                }
            }
            return 0;
        }

        // Day 4
        public static int Day4_1(string path)
        {
            string[] lines = File.ReadAllLines(path);
            int output = 0;
            foreach (var line in lines)
            {
                int multiplier = 0;
                string[] game = line.Split(':');
                string[] sets = game[1].Split('|');

                string[] winningNumbers = sets[0].Trim().Split(' ');
                winningNumbers = winningNumbers.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                string[] myNumbers = sets[1].Trim().Split(' ');
                myNumbers = myNumbers.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                foreach (var win in winningNumbers)
                {
                    if (myNumbers.Contains(win))
                        multiplier++;
                }
                if (multiplier != 0)
                    output += (int)Math.Pow((double)2, (double)multiplier - 1);
            }
            //32001
            return output;
        }

        public static int Day4_2(string path)
        {
            string[] lines = File.ReadAllLines(path);
            int output = lines.Length * 0;
            output += SolveDay4_2(lines, 0, lines.Length);

            return output;
        }
        private static int SolveDay4_2(string[] lines, int start, int length)
        {
            if (length == 0)
                return 0;
            int output = 0;
            for (int i = start; i < start + length; i++)
            {
                int winNumbers = 0;
                if (i >= lines.Length)
                    break;
                output += 1;
                string line = lines[i];
                string[] game = line.Split(':');
                string[] sets = game[1].Split('|');

                string[] winningNumbers = sets[0].Trim().Split(' ');
                winningNumbers = winningNumbers.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                string[] myNumbers = sets[1].Trim().Split(' ');
                myNumbers = myNumbers.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                foreach (var win in winningNumbers)
                {
                    if (myNumbers.Contains(win))
                    {
                        winNumbers++;
                    }
                }
                output += SolveDay4_2(lines, i + 1, winNumbers);
            }
            //5037841
            return output;
        }

        // Day 5
        public static long Day5_1(string path)
        {
            string[] lines = File.ReadAllLines(path);
            int output = 0;

            long[] seeds = lines[0].Split(':')[1].Trim().Split(' ').Select(s => long.Parse(s)).ToArray();
            long[] destNums = seeds;
            long[] bufferNums = new long[seeds.Length].Select(s => s = -1).ToArray();

            for (int i = 3; i < lines.Length; i++)
            {
                string line = lines[i];
                if (line == "")
                {
                    i++;
                    destNums = bufferNums;
                    bufferNums = new long[seeds.Length].Select(s => s = -1).ToArray();
                    continue;
                }

                string[] lineParts = line.Split(' ');
                string[] numbers = line.Trim().Split(' ');

                for (int j = 0; j < destNums.Length; j++)
                {
                    long num = destNums[j];
                    long start = long.Parse(numbers[1]);
                    long end = long.Parse(numbers[0]);
                    long length = long.Parse(numbers[2]);

                    if (num < start || num >= start + length)
                    {
                        if (bufferNums[j] == -1)
                            bufferNums[j] = num;
                        continue;
                    }

                    long diff = num - start;
                    long newNum = end + diff;
                    bufferNums[j] = newNum;
                }
            }

            destNums = bufferNums;

            //318728750
            return destNums.Min();
        }

        public static long Day5_2(string path)
        {
            string[] lines = File.ReadAllLines(path);
            //lines = new string[]
            //{
            //    "seeds: 79 14 55 13"        ,
            //    "seed-to-soil map:"         ,
            //    "50 98 2"                   ,
            //    "52 50 48"                  ,
            //    ""                        ,
            //    "soil-to-fertilizer map:"   ,
            //    "0 15 37"                   ,
            //    "37 52 2"                   ,
            //    "39 0 15"                   ,
            //    ""                        ,
            //    "fertilizer-to-water map:"  ,
            //    "49 53 8"                   ,
            //    "0 11 42"                   ,
            //    "42 0 7"                    ,
            //    "57 7 4"                    ,
            //    ""                        ,
            //    "water-to-light map:"       ,
            //    "88 18 7"                   ,
            //    "18 25 70"                  ,
            //    ""                        ,
            //    "light-to-temperature map:" ,
            //    "45 77 23"                  ,
            //    "81 45 19"                  ,
            //    "68 64 13"                  ,
            //    ""                        ,
            //    "temperature-to-humidity map:",
            //    "0 69 1"                    ,
            //    "1 0 69"                    ,
            //    ""                        ,
            //    "humidity-to-location map:" ,
            //    "60 56 37"                  ,
            //    "56 93 4"                   ,
            //};
            int output = 0;

            long[] seeds = lines[0].Split(':')[1].Trim().Split(' ').Select(s => long.Parse(s)).ToArray();

            //for (int i = 3; i < lines.Length; i++)
            //{
            //    string line = lines[i];
            //    if (line == "")
            //    {
            //        i++;
            //        destNums = bufferNums;
            //        bufferNums = new();
            //        continue;
            //    }

            //    string[] lineParts = line.Split(' ');
            //    string[] lineNumbers = line.Trim().Split(' ');

            //    for (int j = 0; j < destNums.Count; j += 2)
            //    {
            //        long num = destNums[j];
            //        long numRange = destNums[j + 1];
            //        long endRange = num + numRange - 1;

            //        long start = long.Parse(lineNumbers[1]);
            //        long end = long.Parse(lineNumbers[0]);
            //        long length = long.Parse(lineNumbers[2]);
            //        long endLineNumbers = start + length - 1;

                    

            //        long diff = num - start;

            //        long newNum = end + diff;
            //        bufferNums.Add(newNum);
            //        bufferNums.Add(numRange);
            //    }
            //}

            //destNums = bufferNums;

            //List<long> final = new List<long>();

            //for (int i = 0; i < destNums.Count; i += 2)
            //{
            //    final.Add(destNums[i]);
            //    //Console.WriteLine("-----------");
            //    //Console.WriteLine(destNums[i]);
            //    //Console.WriteLine(destNums[i + 1]);
            //}

            ////
            //return destNums.Min();
            return 0;
        }

        // Day 6
        public static int Day6_1(string path)
        {
            string[] lines = File.ReadAllLines(path);
            int output = 1;
            int[] times = lines[0].Split(':')[1].Trim().Split(' ').Where(s => !string.IsNullOrEmpty(s)).Select(s => int.Parse(s)).ToArray();
            int[] records = lines[1].Split(':')[1].Trim().Split(' ').Where(s => !string.IsNullOrEmpty(s)).Select(s => int.Parse(s)).ToArray();

            for (int i = 0; i < times.Length; i++)
            {
                int time = times[i];
                int possibleTimes = 0;
                for(int j = 1; j <= time; j++)
                {
                    if ((time - j) * j > records[i])
                        possibleTimes++;
                }
                output *= possibleTimes;
            }
            //588588
            return output;
        }

        public static long Day6_2(string path)
        {
            string[] lines = File.ReadAllLines(path);
            long output = 0;
            string[] timesRaw = lines[0].Split(':')[1].Trim().Split(' ').Where(s => !string.IsNullOrEmpty(s)).ToArray();
            string[] recordsRaw = lines[1].Split(':')[1].Trim().Split(' ').Where(s => !string.IsNullOrEmpty(s)).ToArray();

            long time = long.Parse(string.Join("", timesRaw));
            long record = long.Parse(string.Join("", recordsRaw));

            for (int j = 1; j <= time; j++)
            {
                if ((time - j) * j > record)
                    output++;
            }
            //34655848
            return output;
        }

    }


}
