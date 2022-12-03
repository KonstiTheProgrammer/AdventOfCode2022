using AdventOfCode2022;


var inputs = InputReader.CreateInputReader().ReadFilePerLine("Stage1/input2.txt").GetInputs().ToStringList();

var sus = inputs.ToBoxList("").Select(k => k.ToIntList().Sum()).OrderByDescending(k => k).Take(3).Sum();

Console.WriteLine(sus);