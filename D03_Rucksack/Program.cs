using System.Diagnostics;
using AdventOfCode2022;

var inputs = InputReader.CreateInputReader().ReadFilePerLine("input2.txt").GetInputs().ToStringList();

Console.WriteLine(Stage2());

int Stage2() =>
    inputs.Cluster(3).Select(k =>
    {
        var commonLetters = k[0].Intersect(k[1]).Intersect(k[2]).ToHashSet();
        return commonLetters.Sum(GetPoints);
    }).Sum();

int Stage1() =>
    inputs.Select(k =>
    {
        var length = k.Length / 2;
        var firstHalf = k.Substring(0, length);
        var secondHalf = k.Substring(length, length);
        return firstHalf.Intersect(secondHalf).ToHashSet().Sum(GetPoints);
    }).Sum();


int GetPoints(char s)
{
    //if char is between A and Z
    if (s >= 65 && s <= 90)
        return s - 64 + 26;

    //if char is between a and z
    if (s >= 97 && s <= 122)
        return s - 96;

    return 0;
}