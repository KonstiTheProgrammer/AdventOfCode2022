using System.Net.Http.Headers;
using System.Numerics;
using Microsoft.Win32.SafeHandles;
using Raylib_cs;

Console.WriteLine("Hello, World!");


var input = File.ReadAllLines("../../../input.txt").Select(k => k.Split(" -> ").Select(s => {
    var spliti = s.Split(",").Select(k => float.Parse(k)).ToList();
    return new Vector2(spliti[0], spliti[1]);
}).ToList()).ToList();

Dictionary<Vector2, Tile> map = new();
// Dictionary<float, Vector2> highest = new();
// Dictionary<float, Vector2> lowest = new();
Init();
var maxY = map.Keys.Max(k => k.Y);
var maxX = map.Keys.Max(k => k.X);
var minX = map.Keys.Min(k => k.X);
var boxSize = (maxX - minX - 1) > (maxY) ? (int)(1000 / (maxX - minX + 2)) : (int)(1000 / (maxY - 0 + 2));
InitRaylib();

HandleRaylib();


while (true) {
    Drop();

    void Drop() {
        var hightestFreeTile = new Vector2(500, 0);
        Place(hightestFreeTile);


        bool Place(Vector2 vec) {
            vec += new Vector2(0, 1);
            Console.WriteLine($"{vec}: yez gehts los");
            if (vec.Y >= maxY) return false;

            while (!map.ContainsKey(vec) || vec.Y < maxY) {
                PlaceBlock(vec, Color.YELLOW);
                Raylib.WaitTime(1);
                vec += new Vector2(0, 1);
                //Console.WriteLine(vec);
            }

            Console.WriteLine("done");
            //Thread.Sleep(1000);

            if (vec.Y > maxY) throw new Exception("sus");

            var x = map[vec];

            var left = vec + new Vector2(-1, 0);
            if (Place(left)) return true;

            var right = vec + new Vector2(1, 0);
            if (Place(right)) return true;

            var sus = vec + new Vector2(0, -1);
            if (sus.Y >= 0) {
                map[sus] = new Sand(sus);
                Console.WriteLine($"Block placed at {sus}");
                return true;
            }

            return false;
        }
    }
}

Console.WriteLine();

void Init() {
    input.ForEach(k => {
        var range = new List<Vector2>();

        for (var i = 0; i < k.Count - 1; i++) {
            var point1 = k[i];
            var point2 = k[i + 1];
            var list = GetPointsTo(point1, point2).ToHashSet();
            range.AddRange(list);
        }

        range.ForEach(v => { map.TryAdd(v, new Stone(v)); });
    });
}

IEnumerable<Vector2> GetPointsTo(Vector2 vector1, Vector2 point2) {
    //get all points between the two points
    var points = new List<Vector2>();
    var x1 = vector1.X;
    var y1 = vector1.Y;
    var x2 = point2.X;
    var y2 = point2.Y;
    var dx = Math.Abs(x2 - x1);
    var dy = Math.Abs(y2 - y1);
    var sx = x1 < x2 ? 1 : -1;
    var sy = y1 < y2 ? 1 : -1;
    var err = dx - dy;

    while (true) {
        points.Add(new Vector2(x1, y1));
        if (x1 == x2 && y1 == y2) break;
        var e2 = 2 * err;
        if (e2 > -dy) {
            err -= dy;
            x1 += sx;
        }

        if (e2 < dx) {
            err += dx;
            y1 += sy;
        }
    }

    return points;
}

void HandleRaylib() {
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.WHITE);
    
    foreach (var tile in map) {
        var posX = tile.Key.X - minX + 1;
        var posY = tile.Key.Y;
        Raylib.DrawRectangle((int)posX * boxSize, (int)posY * boxSize, boxSize, boxSize,
            tile.Value.Type == EType.Stone ? Color.BLACK : Color.YELLOW);
    }
    
    Raylib.EndDrawing();
}

void PlaceBlock(Vector2 vec, Color color) {
    Raylib.BeginDrawing();
    var posX = vec.X - minX + 1;
    var posY = vec.Y;
    Raylib.DrawRectangle((int)posX * boxSize, (int)posY * boxSize, boxSize, boxSize,
        color);
    Raylib.EndDrawing();
}

void InitRaylib() {
    Raylib.InitWindow(1000, 1000, "Advent of Code");
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.WHITE);
    Raylib.EndDrawing();
}

abstract class Tile {
    public Vector2 Position { get; set; }
    public abstract EType Type { get; set; }

    protected Tile(Vector2 position) {
        Position = position;
    }
}

class Sand : Tile {
    public override EType Type { get; set; } = EType.Sand;

    public Sand(Vector2 position) : base(position) {
    }
}

class Stone : Tile {
    public override EType Type { get; set; } = EType.Stone;

    public Stone(Vector2 position) : base(position) {
    }
}

enum EType {
    Sand,
    Stone
}