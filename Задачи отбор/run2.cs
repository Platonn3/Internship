using System;
using System.Collections.Generic;
using System.Linq;


class run2
{
    // Константы для символов ключей и дверей
    static readonly char[] keys_char = Enumerable.Range('a', 26).Select(i => (char)i).ToArray();
    static readonly char[] doors_char = keys_char.Select(char.ToUpper).ToArray();
    static readonly HashSet<char> keySet = new(keys_char);
    static readonly HashSet<char> doorSet = new(doors_char);
    private static int width;
    private static int height;
    private static List<(int x, int y)> robots = new();
    private static HashSet<(int x, int y)> doors = new();
    private static HashSet<(int x, int y)> keys = new();
    private static HashSet<(int x, int y)> walls = new();
    
    // Метод для чтения входных данных
    static List<List<char>> GetInput()
    {
        var data = new List<List<char>>();
        string line;
        var y = 0;
        while ((line = Console.ReadLine()) != null && line != "")
        {
            data.Add(line.ToCharArray().ToList());
            for (var x = 0; x < line.Length; x++)
            {
                var c = line[x];
                if(c == '@')
                    robots.Add((x, y));
                else if(c == '#')
                    walls.Add((x, y));
                else if (keySet.Contains(c))
                    keys.Add((x, y));
                else if (doorSet.Contains(c))
                    doors.Add((x, y));
            }

            y++;
        }
        height = data.Count;
        width = data[0].Count;
        return data;
    }
    
    static bool IsInside(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }
    
    static bool IsWall(int x, int y)
    {
        return walls.Contains((x, y));
    }
    
    static int Solve(List<List<char>> data)
    {
        var directions = new[] { (-1, 0), (0, -1), (1, 0), (0, 1) };
        var visited = new HashSet<(string, int mask)>();
        var allKeysMask = (1 << keys.Count) - 1;
        var q = new PriorityQueue<((int x, int y)[], int len, int mask), int>();
        q.Enqueue((robots.ToArray(), 0, 0), 0);
        while (q.Count > 0)
        {
            var current = q.Dequeue();
            if(current.mask == allKeysMask)
                return current.len;
            var currentPos = current.Item1;
            for (var i = 0; i < currentPos.Length; i++)
            {
                foreach (var dir in directions)
                {
                    var newX = currentPos[i].x + dir.Item1;
                    var newY = currentPos[i].y + dir.Item2;
                    if(!IsInside(newX, newY) || IsWall(newX, newY))
                        continue;
                    var steps = current.len;
                    var mask = current.mask;
                    var c = data[newY][newX];
                    if (doorSet.Contains(c))
                    {
                        var keyBit = char.ToLower(c) - 'a';
                        if ((mask & (1 << keyBit)) == 0)
                            continue;
                    }
                    if (keySet.Contains(c))
                    {
                        var keyBit = c - 'a';
                        mask |= 1 << keyBit;
                    }

                    var newPos = currentPos.ToArray();
                    newPos[i] = (newX, newY);
                    var posKey = string.Join(",", newPos.Select(p => $"{p.x}-{p.y}"));

                    if (visited.Add((posKey, mask)))
                        q.Enqueue((newPos, steps + 1, mask), steps + 1);
                }
            }
        }
        return -1;
    }
    
    static void Main()
    {
        var data = GetInput();
        int result = Solve(data);
        
        if (result == -1)
        {
            Console.WriteLine("No solution found");
        }
        else
        {
            Console.WriteLine(result);
        }
    }
}