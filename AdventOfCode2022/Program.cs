using System.Collections;
using System.Numerics;

Console.WriteLine("Hello World!");

Dictionary<Vector2, bool> sus = new();
Enumerable.Range(0, 10).Select(k => new Vector2(k, k + 1)).ToList().ForEach(k => sus.Add(k, false));

var kuss = new Vector2(0, 1);
if (sus.ContainsKey(kuss)) {
    Console.WriteLine("Found");
}

Console.WriteLine("nix");