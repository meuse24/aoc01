using System.Data.SqlTypes;
using System.Diagnostics;

namespace aoc01
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine(SolveDay01());
            Console.WriteLine(SolveDay02());
            Console.WriteLine(SolveDay03());

            string SolveDay03()
            {
                var inputLines = GetListOfDay("3");
                string resultText = "Day 3\n";
                Stopwatch stopwatch = new();

                // Part 1: 
                stopwatch.Start();
                string concatenatedString = string.Join("", inputLines); // Vereine alle Eingabedaten zu einem großen String
                List<string> mulParts = new List<string>(concatenatedString.Split("mul(")); // Teile den String an den Trennzeichen

                long sum = 0;
                foreach (string part in mulParts)
                {
                    int closingBracketIndex = part.IndexOf(')');
                    if (closingBracketIndex != -1)
                    {
                        string substring = part.Substring(0, closingBracketIndex); // Extrahiere Teilstring vor der schließenden Klammer
                        if (IsValidString(substring)) sum += multiNumbers(substring); // Berechne das Produkt und addiere es zur Summe   
                    }
                }
                stopwatch.Stop();
                resultText += $"Part 1: {sum}, {stopwatch.ElapsedMilliseconds} ms\n";

                // Part 2: 
                stopwatch.Restart();
                sum = 0;
                bool enabled = true;
                foreach (string part in mulParts)
                {
                    int closingBracketIndex = part.IndexOf(')');
                    if (closingBracketIndex != -1)
                    {
                        string substring = part.Substring(0, closingBracketIndex); // Extrahiere Teilstring vor der schließenden Klammer
                        if (IsValidString(substring) && enabled) sum += multiNumbers(substring); // Berechne das Produkt und addiere es zur Summe   
                        if (part.Contains("do()")) enabled = true;
                        if (part.Contains("don't()")) enabled = false;
                    }
                }
                stopwatch.Stop();
                resultText += $"Part 2: {sum}, {stopwatch.ElapsedMilliseconds} ms\n";
                return resultText;
            }
            long multiNumbers(string str)
            {
                var numbers = str.Split(","); // Teile den String bei dem Komma
                return int.Parse(numbers[0]) * int.Parse(numbers[1]); // Berechne das Produkt und gib es zurück
            }

            static bool IsValidString(string input)
            {
                // Überprüfen, ob der String mit einer Ziffer beginnt und endet
                if (!char.IsDigit(input[0]) || !char.IsDigit(input[input.Length - 1])) { return false; }

                int commaCount = 0;
                foreach (char character in input)// Zähle die Kommas
                {
                    if (character == ',') commaCount++;
                    else if (!char.IsDigit(character)) return false; // Der String darf nur Ziffern und genau ein Komma enthalten
                }

                // Genau ein Komma muss vorhanden sein
                return commaCount == 1;
            }



            string SolveDay02()
            {
                var inputLines = GetListOfDay("2");
                string resultText = "Day 2\n";
                Stopwatch stopwatch = new();

                // Part 1: Check sequences with no allowed errors
                stopwatch.Start();
                int validSequenceCount = 0;
                int errorCount = 0;
                foreach (var line in inputLines)
                {
                    List<int> sequence = line
                        .Split(' ')
                        .Select(int.Parse)
                        .ToList();
                    if (IsValidSequence(sequence, errorCount, 0)) validSequenceCount++;
                }
                stopwatch.Stop();
                resultText += $"Part 1: {validSequenceCount}, {stopwatch.ElapsedMilliseconds} ms\n";

                // Part 2: Check sequences with one allowed error
                stopwatch.Restart();
                errorCount = 0;
                validSequenceCount = 0;
                foreach (var line in inputLines)
                {
                    List<int> sequence = line
                        .Split(' ')
                        .Select(int.Parse)
                        .ToList();
                    if (IsValidSequence(sequence, errorCount, 1)) validSequenceCount++;
                }
                stopwatch.Stop();
                resultText += $"Part 2: {validSequenceCount}, {stopwatch.ElapsedMilliseconds} ms\n";
                return resultText;
            }

            static bool IsValidSequence(List<int> sequence, int currentErrorCount, int maxAllowedErrors)
            {
                // Check if sequence is monotonic and neighboring differences are valid
                if ((IsMonotonicallyIncreasing(sequence) || IsMonotonicallyDecreasing(sequence))
                    && HasValidNeighborDifferences(sequence, 1, 3)) return true;

                // Check if error limit is reached
                if (currentErrorCount >= maxAllowedErrors) return false;

                // Try removing each element and check if resulting sequence is valid
                for (int index = 0; index < sequence.Count; index++)
                {
                    var modifiedSequence = new List<int>(sequence);
                    modifiedSequence.RemoveAt(index);
                    if (IsValidSequence(modifiedSequence, currentErrorCount + 1, maxAllowedErrors)) return true;
                }

                return false;
            }

            static bool IsMonotonicallyIncreasing(List<int> sequence)
            {
                for (int index = 0; index < sequence.Count - 1; index++)
                {
                    if (sequence[index] > sequence[index + 1]) return false;
                }
                return true;
            }

            static bool IsMonotonicallyDecreasing(List<int> sequence)
            {
                for (int index = 0; index < sequence.Count - 1; index++)
                {
                    if (sequence[index] < sequence[index + 1]) return false;
                }
                return true;
            }

            static bool HasValidNeighborDifferences(List<int> sequence, int minDifference, int maxDifference)
            {
                for (int index = 0; index < sequence.Count - 1; index++)
                {
                    int difference = Math.Abs(sequence[index] - sequence[index + 1]);
                    if (difference < minDifference || difference > maxDifference) return false;
                }
                return true;
            }

            static string ConvertSequenceToString(List<int> sequence)
            {
                return string.Join(", ", sequence);
            }


            string SolveDay01()
            {
                var stopwatch = new Stopwatch();
                // Part 1: Calculate differences between left and right numbers
                var inputLines = GetListOfDay("1");
                var resultText = "Day 1\n";
                stopwatch.Start();

                var leftNumbers = new List<int>();
                var rightNumbers = new List<int>();

                foreach (string line in inputLines)
                {
                    leftNumbers.Add(int.Parse(line.Substring(0, 5)));
                    rightNumbers.Add(int.Parse(line.Substring(line.Length - 5, 5)));
                }

                leftNumbers.Sort();
                rightNumbers.Sort();

                int sumOfDifferences = 0;
                for (int index = 0; index < leftNumbers.Count; index++)
                {
                    sumOfDifferences += Math.Abs(leftNumbers[index] - rightNumbers[index]);
                }
                stopwatch.Stop();
                resultText += $"Part 1: {sumOfDifferences}, {stopwatch.ElapsedMilliseconds} ms";

                // Part 2: Calculate sum of products of matching numbers
                stopwatch.Restart();
                int totalSum = 0;
                foreach (int number in leftNumbers)
                {
                    totalSum += (number * CountOccurrences(number, rightNumbers));
                }
                stopwatch.Stop();
                resultText += $"\nPart 2: {totalSum}, {stopwatch.ElapsedMilliseconds} ms\n";
                return resultText;

                int CountOccurrences(int numberToFind, List<int> numberList)
                {
                    int count = 0;
                    foreach (int currentNumber in numberList)
                    {
                        if (currentNumber == numberToFind) count++;
                    }
                    return count;
                }
            }

            static List<string> GetListOfDay(string day)
            {
                var directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                // Nach oben navigieren bis Projektdatei gefunden
                while (directory != null && !directory.GetFiles("*.csproj").Any()) directory = directory.Parent;
                if (directory == null) throw new DirectoryNotFoundException("Projektverzeichnis nicht gefunden");
                string filePath = Path.Combine(directory.FullName, $"TextFile{day}.txt");
                if (!File.Exists(filePath)) throw new FileNotFoundException($"Datei TextFile{day}.txt nicht gefunden", filePath);
                return File.ReadAllLines(filePath).ToList();
            }
        }
    }
}




