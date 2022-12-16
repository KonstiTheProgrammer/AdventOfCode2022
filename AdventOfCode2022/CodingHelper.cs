using System.Collections;
using System.IO.Compression;

namespace AdventOfCode2022;

public class InputReader
{
    public static InputReader CreateInputReader()
    {
        return new InputReader();
    }

    private List<Input> Inputs { get; set; } = new();
    private int _currentInputIndex = 0;

    public InputReader ReadZipFile(string zipFile, string splitValue = " ")
    {
        UnzipFile(zipFile);
        ReadFolder(zipFile.Replace(".zip", ""), splitValue);
        return this;
    }

    public InputReader UnzipFile(string zipFile)
    {
        ZipFile.ExtractToDirectory(GetCompletePath(zipFile), GetCompletePath(zipFile.Replace(".zip", "")), true);
        return this;
    }

    //liest folder mit allen dateien ein
    public InputReader ReadFolder(string folderName)
    {
        ReadFolder(folderName, " ");
        return this;
    }

    private InputReader ReadFolder(string folderName, string splitValue)
    {
        foreach (string file in Directory.EnumerateFiles(GetCompletePath(folderName), "*.in"))
        {
            ReadWholeFile(file, splitValue);
        }

        return this;
    }

    public static string GetCompletePath(string fileName)
    {
        if (fileName.Contains("../")) return fileName;
        return "../../../" + fileName;
    }

    //list nur ein file ein mit standart splitValue
    public InputReader ReadWholeFile(string fileName)
    {
        ReadWholeFile(fileName, " ");
        return this;
    }


    //list einen file ein und splittet nach splitValue
    public InputReader ReadWholeFile(string fileName, string splitValue)
    {
        var input = File.ReadAllLines(GetCompletePath(fileName)).Select(k => k.Split(splitValue)).SelectMany(k => k)
            .ToList();
        Inputs.Add(new Input(fileName, input));
        return this;
    }

    //liest file mit standart split value
    public InputReader ReadFilePerLine(string filename)
    {
        ReadFilePerLine(filename, " ");
        return this;
    }

    //liest file mit splitvalue
    public InputReader ReadFilePerLine(string filename, string splitValue)
    {
        var input = File.ReadLines("../../../" + filename);
        foreach (var s in input)
        {
            Inputs.Add(
                new Input(filename, s.Split(splitValue, StringSplitOptions.RemoveEmptyEntries).ToList()));
        }

        return this;
    }

    //wenn man ein file mit mehreren Trennern hat zb ein leerzeichen und ein komma
    public InputReader ReadFilePerLine(string filename, List<string> splitValues)
    {
        if (splitValues.Count == 0) throw new Exception("There has to be at least 1 Split Value");

        var input = File.ReadLines("../../../" + filename).ToList();
        for (int i = 0; i < input.Count; i++)
        {
            foreach (var splitValue in splitValues)
            {
                input[i] = input[i].Replace(splitValue, splitValues[0]);
            }

            Inputs.Add(new Input(filename,
                input[i].Split(splitValues[0], StringSplitOptions.RemoveEmptyEntries).ToList()));
        }

        return this;
    }


    //bekommst alle inputs also alle files
    public List<Input> GetInputs()
    {
        return Inputs;
    }

    //naechstes file sofern es eins gibt
    public Input? GetNextInput()
    {
        if (_currentInputIndex >= Inputs.Count)
        {
            return null;
        }

        return Inputs[_currentInputIndex++];
    }

    //bekommst input mit index
    public Input? GetInputAt(int index)
    {
        if (index >= Inputs.Count)
        {
            return null;
        }

        return Inputs[index];
    }

    //speichert den consolen output in eine datei
    public void InitOutputRedirection()
    {
        FileStream filestream = new FileStream("../../../out.txt", FileMode.Create);
        var streamwriter = new StreamWriter(filestream);
        streamwriter.AutoFlush = true;
        Console.SetOut(streamwriter);
        Console.SetError(streamwriter);
    }
}

public class Input
{
    public List<string> Inputs;
    public int Index;
    public string FileName { get; set; }

    //speichert alle console.writeline outputs in ein file
    public void SetOutput()
    {
        FileStream filestream = new FileStream(
            GetOutputPath("outputs/" + FileName),
            FileMode.Create);
        var streamwriter = new StreamWriter(filestream);
        streamwriter.AutoFlush = true;
        Console.SetOut(streamwriter);
    }

    public Input(string fileName, List<string> inputs)
    {
        FileName = fileName;
        Inputs = inputs;
    }
    
    public Input(List<string> inputs)
    {
        FileName = String.Empty;
        Inputs = inputs;
    }
    
    private string GetOutputPath(string fileName)
    {
        var path = fileName.Replace(".in", "")
            .Split(new string[] { "/", "\\" }, StringSplitOptions.RemoveEmptyEntries);
        path[^2] += "Output";
        Directory.CreateDirectory(InputReader.GetCompletePath(path[^2]));
        return "../../../" + path[^2] + "/" + path[^1] + ".out";
    }

    //gibt das naechste fragment aus
    public string Read()
    {
        return Inputs[Index++];
    }


    public int ReadInt()
    {
        return int.Parse(Read());
    }

    public long ReadLong()
    {
        return long.Parse(Read());
    }

    public double ReadDouble()
    {
        return double.Parse(Read());
    }

    public bool ReadBool()
    {
        return bool.Parse(Read());
    }

    //gibt das naechste fragment aus (mit anzahl an naechsten fragmenten)
    public List<string> Read(int amount)
    {
        var x = Inputs.GetRange(Index, amount);
        Index += amount;
        return x;
    }

    //setzt den index des fragment lesers
    public void SetIndex(int index)
    {
        Index = index;
    }

    //ob file noch nicht fertig gelesen ist
    public bool HasNotEnded() => Index < Inputs.Count;


    //ob file fertig gelesen ist
    public bool HasEnded() => Index >= Inputs.Count;
}

public static class Extension
{
    public static T Pop<T>(this List<T> list)
    {
        var item = list[0];
        list.RemoveAt(0);
        return item;
    }

    public static T Pop<T>(this List<T> list, int index)
    {
        var item = list[index];
        list.RemoveAt(index);
        return item;
    }

    public static List<Node> ToNeighbors(this IEnumerable<IEnumerable<Node>> enumerable)
        => enumerable.Select(k => k.ToArray()).ToArray().ToNeighbors();

    public static List<Node> ToNeighborsFull(this IEnumerable<IEnumerable<Node>> enumerable)
        => enumerable.Select(k => k.ToArray()).ToArray().ToNeighborsFull();

    public static List<T> ToType<T>(this IEnumerable<Node> enumerable) where T : Node =>
        enumerable.Select(k => k as T).ToList();


    public static List<Node> ToNeighbors(this Node[][] array)
    {
        var nodes = new List<Node>();

        for (int y = 0; y < array.Length; y++)
        {
            for (int x = 0; x < array[y].Length; x++)
            {
                var left = (x - 1 >= 0);
                var right = (x + 1 < array[y].Length);
                var top = (y - 1 >= 0);
                var bottom = (y + 1 < array.Length);

                if (left) array[y][x].Neighbors.Add(new Connection(array[y][x], array[y][x - 1], EDirection.W));
                if (right) array[y][x].Neighbors.Add(new Connection(array[y][x], array[y][x + 1], EDirection.E));
                if (top) array[y][x].Neighbors.Add(new Connection(array[y][x], array[y - 1][x], EDirection.N));
                if (bottom) array[y][x].Neighbors.Add(new Connection(array[y][x], array[y + 1][x], EDirection.S));

                nodes.Add(array[y][x]);
            }
        }

        return nodes;
    }

    public static List<Node> ToNeighborsFull(this Node[][] array)
    {
        var nodes = new List<Node>();

        for (int y = 0; y < array.Length; y++)
        {
            for (int x = 0; x < array[y].Length; x++)
            {
                var left = (x - 1 >= 0);
                var right = (x + 1 < array[y].Length);
                var top = (y - 1 >= 0);
                var bottom = (y + 1 < array.Length);

                if (left) array[y][x].Neighbors.Add(new Connection(array[y][x], array[y][x - 1], EDirection.W));
                if (right) array[y][x].Neighbors.Add(new Connection(array[y][x], array[y][x + 1], EDirection.E));
                if (top) array[y][x].Neighbors.Add(new Connection(array[y][x], array[y - 1][x], EDirection.N));
                if (bottom) array[y][x].Neighbors.Add(new Connection(array[y][x], array[y + 1][x], EDirection.S));
                if (bottom && right)
                    array[y][x].Neighbors.Add(new Connection(array[y][x], array[y + 1][x + 1], EDirection.SE));
                if (bottom && left)
                    array[y][x].Neighbors.Add(new Connection(array[y][x], array[y + 1][x - 1], EDirection.SW));
                if (top && right)
                    array[y][x].Neighbors.Add(new Connection(array[y][x], array[y - 1][x + 1], EDirection.NE));
                if (top && left)
                    array[y][x].Neighbors.Add(new Connection(array[y][x], array[y - 1][x - 1], EDirection.NW));

                nodes.Add(array[y][x]);
            }
        }

        return nodes;
    }

    public static int ToInt32(this string s)
    {
        return Convert.ToInt32(s);
    }

    public static long ToInt64(this string s)
    {
        return Convert.ToInt64(s);
    }

    public static bool ToBool(this string s)
    {
        return Convert.ToBoolean(s);
    }

    public static List<T> AddReturn<T>(this List<T> list, T element)
    {
        list.Add(element);
        return list;
    }

    public static List<T> GetFromTo<T>(this List<T> list, int from, int two) => list.GetRange(from, from - two);

    public static string PrintWithComma<T>(this IEnumerable<T> list) =>
        list.Select(k => Convert.ToString(k)).Aggregate((a, b) => a + "," + b)!;
}

public class Node
{
    public List<Connection> Neighbors { get; set; } = new List<Connection>();

    public Connection? GetNeighbor(EDirection direction) => Neighbors.Find(k => k.Direction == direction);


    public List<Node> GetNeighbors() => Neighbors.Select(k => k.To).ToList();
}

public class Connection
{
    public Connection(Node from, Node to, EDirection dir)
    {
        From = from;
        To = to;
        Direction = dir;
    }

    public Node From { get; set; }
    public Node To { get; set; }
    public EDirection Direction { get; set; }
}

public enum EDirection
{
    N,
    S,
    E,
    W,
    NE,
    NW,
    SE,
    SW
}

public static class EDirectionExtensions
{
    public static EDirection GetOppositeDirection(this EDirection direction) =>
        direction switch
        {
            EDirection.N => EDirection.S,
            EDirection.S => EDirection.N,
            EDirection.E => EDirection.W,
            EDirection.W => EDirection.E,
            EDirection.NE => EDirection.SW,
            EDirection.NW => EDirection.SE,
            EDirection.SE => EDirection.NW,
            EDirection.SW => EDirection.NE,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
}

public static class GlobalExtensions
{
    public static List<string> ToStringList(this IEnumerable<Input> list)
    {
        return list.Select(k => String.Join(String.Empty, k.Inputs)).ToList();
    }

    public static List<int> ToIntList(this IEnumerable<string> list)
    {
        return list.Select(k => k.ToInt32()).ToList();
    }

    public static List<List<int>> ToIntList(this IEnumerable<Input> list)
    {
        return list.Select(k => k.Inputs.ToIntList()).ToList();
    }

    public static List<List<string>> ToStringsList(this IEnumerable<Input> list)
    {
        return list.Select(k => k.Inputs).ToList();
    }

    public static List<List<T>> ToBoxList<T>(this IEnumerable<T> list, T Compare)
    {
        var boxed = new List<List<T>>();
        foreach (var item in list)
        {
            if (item.Equals(Compare))
            {
                boxed.Add(new List<T>());
            }
            else
            {
                if (boxed.Count == 0) continue;
                boxed[^1].Add(item);
            }
        }

        return boxed;
    }

    public static List<List<T>> Cluster<T>(this IEnumerable<T> list, int count)
    {
        List<List<T>> boxed = new List<List<T>>();
        int i = 0;

        foreach (var item in list)
        {
            if (i % count == 0)
            {
                boxed.Add(new List<T>());
            }

            boxed[^1].Add(item);
            i++;
        }

        return boxed;
    }

    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> list, Action<T> action)
    {
        foreach (var item in list)
        {
            action(item);
        }

        return list;
    }
}