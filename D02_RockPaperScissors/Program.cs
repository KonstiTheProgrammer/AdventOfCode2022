using AdventOfCode2022;

var inputs = InputReader.CreateInputReader().ReadFilePerLine("input2.txt").GetInputs().ToStringsList();

var result = inputs.Select(k =>
    CalculateWin(k[0], k[1])
).Sum();

Console.WriteLine(result);

static int CalculateWin(string a, string b) {
    a = GetEqual(a);
    b = GetEqual(b);

    if (b == "R") {
        if (a == "R") return (GetPoints("S"));
        if (a == "P") return (GetPoints("R"));
        if (a == "S") return (GetPoints("P"));
    }

    if (b == "P") {
        if (a == "R") return (GetPoints("R")) + 3;
        if (a == "P") return (GetPoints("P")) + 3;
        if (a == "S") return (GetPoints("S")) + 3;
    }

    if (b == "S") {
        if (a == "R") return (GetPoints("P")) + 6;
        if (a == "P") return (GetPoints("S")) + 6;
        if (a == "S") return (GetPoints("R")) + 6;
    }

    return 0;

    string GetEqual(string input) =>
        input switch {
            "X" => "R",
            "Y" => "P",
            "Z" => "S",
            "A" => "R",
            "B" => "P",
            "C" => "S",
        };

    int GetPoints(string input) =>
        input switch {
            "R" => 1,
            "P" => 2,
            "S" => 3
        };
}