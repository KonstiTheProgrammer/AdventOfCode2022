using AdventOfCode2022;

var x = InputReader.CreateInputReader().ReadFilePerLine("input2.txt", ",").GetInputs().ToStringsList()
    .Select(k => new Tuple<Elf, Elf>(new Elf(k[0]), new Elf(k[1]))).ToList();


Console.WriteLine(Bsp2());

int Bsp1() => (x.Sum(e => e.Item1.DoesInclude(e.Item2) || e.Item2.DoesInclude(e.Item1) ? 1 : 0));
int Bsp2() => (x.Sum(e => e.Item1.DoesOverlap(e.Item2) || e.Item2.DoesOverlap(e.Item1) ? 1 : 0));


record Elf
{
    public int From { get; set; }
    public int To { get; set; }

    public Elf(string listOfTodo)
    {
        var x = listOfTodo.Split("-");
        From = int.Parse(x[0]);
        To = int.Parse(x[1]);
    }

    public bool DoesInclude(Elf elf)
    {
        return elf.From >= From && elf.To <= To;
    }

    public bool DoesOverlap(Elf elf)
    {
        return elf.From <= To && elf.To >= From;
    }
}