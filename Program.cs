using System.Diagnostics;

namespace aoc01
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine(day01());

            Console.WriteLine(day02());

            string day02()
            {
                var lines = getListOfDay("2");
                string result = "Day 2\n";
                Stopwatch stopwatch = new();

                // Day 2, Part 1
                stopwatch.Start();
                int counter = 0;
                int error = 0;
                foreach (var line in lines)
                {
                    List<int> numbers = line
                        .Split(' ') // Teile den String bei Leerzeichen
                        .Select(int.Parse) // Konvertiere jeden Teil in eine Ganzzahl
                        .ToList(); // Erstelle eine Liste aus den Ergebnissen
                    if (IsListSafe(numbers, error, 0)) counter++;
                }
                stopwatch.Stop();
                result += $"Part 1: {counter}, {stopwatch.ElapsedMilliseconds} ms\n";

                // Day 2, Part 2
                stopwatch.Restart();
                error = 0;
                counter = 0;
                foreach (var line in lines)
                {
                    List<int> numbers = line
                        .Split(' ') // Teile den String bei Leerzeichen
                        .Select(int.Parse) // Konvertiere jeden Teil in eine Ganzzahl
                        .ToList(); // Erstelle eine Liste aus den Ergebnissen
                    if (IsListSafe(numbers, error, 1)) counter++;
                }
                stopwatch.Stop();
                result += $"Part 2: {counter}, {stopwatch.ElapsedMilliseconds} ms\n";

                return result;

            }

            static bool IsListSafe(List<int> numbers, int currentErrors, int maxErrors)
            {
                // 1. Bedingung: Liste prüfen (aufsteigend/absteigend und Nachbar-Differenzen)
                if ((IsAscending(numbers) || IsDescending(numbers)) && NeighboursDiffersNotBetween(numbers, 1, 3)) return true;

                // 2. Fehlerlimit erreicht?
                if (currentErrors >= maxErrors) return false;

                // 3. Rekursion: Versuche, ein Element zu entfernen und erneut zu prüfen
                for (int i = 0; i < numbers.Count; i++)
                {
                    var modifiedList = new List<int>(numbers);
                    modifiedList.RemoveAt(i); // Entferne Element an Position i
                    if (IsListSafe(modifiedList, currentErrors + 1, maxErrors)) return true;
                }

                // 4. Keine Lösung gefunden
                return false;
            }

            static bool IsAscending(List<int> numbers)
            {
                for (int i = 0; i < numbers.Count - 1; i++) if (numbers[i] > numbers[i + 1]) return false;
                return true;
            }

            static bool IsDescending(List<int> numbers)
            {
                for (int i = 0; i < numbers.Count - 1; i++) if (numbers[i] < numbers[i + 1]) return false;
                return true;
            }

            static bool NeighboursDiffersNotBetween(List<int> numbers, int min, int max)
            {
                for (int i = 0; i < numbers.Count - 1; i++)
                {
                    int diff = Math.Abs(numbers[i] - numbers[i + 1]);
                    if (diff < min || diff > max) return false;
                }
                return true;
            }

            static string ListToString(List<int> numbers)
            {
                return string.Join(", ", numbers);
            }


            string day01()
            {
                Stopwatch stopwatch = new();

                // Tag 1 Part 1 Günther
                var lines = getListOfDay("1");
                string result = "Day 1\n";
                stopwatch.Start();

                List<int> listLeft = new List<int>();
                List<int> listRight = new List<int>();

                foreach (string line in lines)
                {
                    listLeft.Add(int.Parse(line.Substring(0, 5)));
                    listRight.Add(int.Parse(line.Substring(line.Length - 5, 5)));
                }

                listLeft.Sort();
                listRight.Sort();

                int s = 0;
                for (int i = 0; i < listLeft.Count; i++)
                {
                    s += Math.Abs(listLeft[i] - listRight[i]);
                }
                stopwatch.Stop();
                result += $"Part 1: {s}, {stopwatch.ElapsedMilliseconds} ms";


                // Tag 1 Part 2 Günther

                stopwatch.Restart();

                s = 0;
                foreach (int i in listLeft)
                {
                    s += (i * cnt(i, listRight));
                }

                stopwatch.Stop();

                result += $"\nPart 2: {s}, {stopwatch.ElapsedMilliseconds} ms";

                return result;

                int cnt(int nr, List<int> list)
                {
                    int r = 0;
                    foreach (int i in list)
                    {
                        if (i == nr) r++;
                    }
                    return r;
                }
            }

            List<string> getListOfDay(string day)
            {
                string fileName = $"TextFile{day}.txt";
                string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string solutionDirectory = Directory.GetParent(projectDirectory).Parent.Parent.Parent.FullName;
                string filePath = Path.Combine(solutionDirectory, fileName);
                return new List<string>(File.ReadAllLines(filePath));

            }
        }


    }
}



