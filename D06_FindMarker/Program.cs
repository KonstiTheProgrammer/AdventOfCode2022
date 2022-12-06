using AdventOfCode2022;

var inputs = InputReader.CreateInputReader().ReadFilePerLine("input.txt").GetInputs().ToStringList()
    .Select(k => new Communication(k));

Console.WriteLine(Stage2());

int Stage1() => inputs.Select(k => k.GetMarkerPosition(4)).Sum();
int Stage2() => inputs.Select(k => k.GetMarkerPosition(14)).Sum();

class Communication {
    public Communication(string stream) {
        Stream = stream;
    }

    public string Stream { get; set; }

    public int GetMarkerPosition(int count) {
        if (Stream.Length <= count) return 0;

        for (var i = 0; i < Stream.Length - count; i++)
            if (Stream.Substring(i, count).GroupBy(k => k).Count() == count)
                return i + count;

        return 0;
    }
}