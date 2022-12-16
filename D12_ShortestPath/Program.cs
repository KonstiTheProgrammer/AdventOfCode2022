using AdventOfCode2022;

Console.WriteLine("Hello, World!");

var graph = File.ReadAllLines("../../../input2.txt")
    .Select(k => k.ToCharArray().Select(k => new SusNode(k)).ToArray()).ToArray();

var nodes = graph.ToNeighbors().Cast<SusNode>().ToList();

var startNode = nodes.First(k => k.Value == 'S');
var endNode = nodes.First(k => k.Value == 'E');

startNode.Value = 'a';
endNode.Value = 'z';

nodes.ForEach(n => {
    n.Neighbors.RemoveAll(k => {
        var node = (k.To as SusNode);
        return !(node.Value == n.Value || node.Value == n.Value + 1 || node.Value < n.Value );
    });
});

var x = nodes.Where(k => k.Value == 'a').Select( k=> FindShortestPath(k)).Min();

Console.WriteLine(x);

int FindShortestPath(SusNode startNode) {
    var queue = new Queue<SusNode>();
    queue.Enqueue(startNode);
    var visited = new HashSet<SusNode>();
    var distance = new Dictionary<SusNode, int>();
    distance[startNode] = 0;

    while (queue.Count > 0) {
        var node = queue.Dequeue();
        if (visited.Contains(node)) {
            continue;
        }
        
        if (node == endNode) {
            return distance[endNode];
        }

        visited.Add(node);
        foreach (var neighbor in node.Neighbors) {
            var neighborNode = neighbor.To as SusNode;
            if (!distance.ContainsKey(neighborNode)) {
                distance[neighborNode] = distance[node] + 1;
                queue.Enqueue(neighborNode);
            }
        }
    }
    
    return Int32.MaxValue;
}


Console.WriteLine();

class SusNode : Node {
    public SusNode(char value) {
        Value = value;
    }

    public char Value { get; set; }
}