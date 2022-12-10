using System.Diagnostics;
using System.Drawing;
using AdventOfCode2022;

var inputs = InputReader.CreateInputReader().ReadFilePerLine("input2.txt").GetInputs().ToStringList()
    .Select(k => k.Select(c => new Tree(){ Height = Convert.ToInt32(c.ToString()) }).ToList()).ToList();

var nodes = inputs.ToNeighbors().ToType<Tree>();

var edgeTrees = nodes.Where(k => k.Neighbors.Count < 4).ToList();
var innerTrees = nodes.Where(k => k.Neighbors.Count == 4).ToList();
var levelDirection = new List<EDirection>(){ EDirection.E, EDirection.W, EDirection.S, EDirection.N };

void level1(List<Tree> trees, List<List<Tree>>? list){
    void PrintTrees(){
        int factor = 64;

        StringFormat format = new StringFormat(){
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        //create an empty png and draw the trees on it
        var bitmap = new Bitmap(list.Count * factor, list.First().Count * factor);
        var graphics = Graphics.FromImage(bitmap);
        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        graphics.Clear(Color.White);

        var font = new Font("Arial", factor * 0.66f);
        var x = 0;
        var y = 0;
        list.ForEach(k => {
            k.ForEach(tree => {
                var height = tree.Height;
                if (tree.isVisible){
                    graphics.FillRectangle(Brushes.Green, x, y, factor, factor);
                }
                else{
                    graphics.FillRectangle(Brushes.Red, x, y, factor, factor);
                }

                graphics.DrawString(height.ToString(), font, Brushes.Black, new RectangleF(x, y, factor, factor),
                    format);
                x += factor;
            });
            x = 0;
            y += factor;
        });
        graphics.Flush();
        bitmap.Save("../../../trees.png");
    }


    foreach (var direction in levelDirection){
        var nodesWithStartDirection =
            trees.Where(k => k.Neighbors.All(n => n.Direction != direction.GetOppositeDirection())).ToList();

        var queue = new Queue<QueueElement>();


        nodesWithStartDirection.ForEach(k => queue.Enqueue(new(k, -1)));

        while (queue.Any()){
            var (node, heightOfLastVisibleNode) = queue.Dequeue();
            var neighbor = node.GetNeighbor(direction);
            if (neighbor is null) continue;

            var nextNode = neighbor.To as Tree;

            if (nextNode is null) continue;


            if (node.Height > heightOfLastVisibleNode){
                node.isVisible = true;
                heightOfLastVisibleNode = node.Height;
            }


            queue.Enqueue(new(nextNode, heightOfLastVisibleNode));
        }

        PrintTrees();
    }

    Console.WriteLine(nodes.Count(k => k.isVisible));
}

void Level2(){
    var x = nodes.Select(k => levelDirection.Select(d => GetNumberOfAdjacentTreesSmallerThanThis(d, k, k.Height)));
    var maxNumber = x.Select(k => k.Aggregate((x, i) => (x > 0 ? x : 1) * i));
    Console.WriteLine(maxNumber.Max());

    int GetNumberOfAdjacentTreesSmallerThanThis(EDirection direction, Tree tree, int height){
        var node = tree.GetNeighbor(direction);
        if (node is null) return 0;
        var nextTree = node.To as Tree;
        if (nextTree.Height >= height) return 1;
        return 1 + GetNumberOfAdjacentTreesSmallerThanThis(direction, nextTree, height);
    }
}

Level2();


record QueueElement(Tree tree, int heightOfLastVisibleNode);

class Tree : Node{
    public bool isVisible{ get; set; } = false;
    public int Height{ get; set; }
}