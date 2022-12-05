Console.WriteLine("Hello, World!");

var x = File.ReadAllLines("../../../input1.txt").ToList();

List<TurboStack> Stack = new List<TurboStack>();

var length = x.First().Length;

//take every element until the string is an empty string

var elements = x.TakeWhile(k => k.Length != 0).ToList();
var commands = x.Skip(elements.Count + 1).Select(k =>
    k.Replace("move", "").Replace("from", "")
        .Replace("to", "").Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(k => Int32.Parse(k)).ToList()
).ToList();

for (int i = 0; i < (length + 1); i += 4) {
    var name = elements[^1][(i + 1)..(i + 3)];
    var elem = elements.SkipLast(1).Select(k => new Element(k.Skip(1 + i).Take(1).First())).Where(k => k.Zeichen != 32)
        .Reverse()
        .ToList();
    Stack.Add(new TurboStack(elem, name));
}

Console.WriteLine("su");

foreach (var command in commands) {
    //Stack.ForEach(Console.WriteLine);
    //move 3 elements from stack 1 to stack 2
    var from = command[1];
    var to = command[2];
    var count = command[0];

    var fromStack = Stack.First(k => k.Name == from);
    var toStack = Stack.First(k => k.Name == to);
    //for level 1
    //var elementssus = fromStack.TakeLast(count).Reverse().ToList();
    //for level 2
    var elementssus = fromStack.TakeLast(count).ToList();
    fromStack.RemoveRange(fromStack.Count - count, count);
    toStack.AddRange(elementssus);
    //Console.WriteLine($"move {count} from {from} to {to}");
}

//Stack.ForEach(Console.WriteLine);   

Stack.Select(k => k.Last().Zeichen).ToList().ForEach(Console.Write);

record Element(char Zeichen);

class TurboStack : List<Element> {
    public int Name { get; set; }

    public TurboStack(IEnumerable<Element> collection, string name) : base(collection) {
        Name = Int32.Parse(name);
    }

    public override string ToString() {
        return $"{Name} {string.Join("", this.Select(k => $"[{k.Zeichen}] "))}";
    }
}