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

}
