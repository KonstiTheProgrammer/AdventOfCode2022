using AdventOfCode2022;

var x = File.ReadAllLines("../../../input.txt");

for (int i = 0; i < x.Length; i += 3) {
    var input1 = new Input(x[i].Select(k => k.ToString()).ToList());
    var input2 = new Input(x[i + 1].Select(k => k.ToString()).ToList());

    var list1Level = 0;
    var list2Level = 0;
    while (input1.HasNotEnded()) {
    }
}

class TuboList {
    public List<string> Liste { get; set; }
    
}