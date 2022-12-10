// See https://aka.ms/new-console-template for more information

using System.Collections;
using System.ComponentModel;
using System.Numerics;
using AdventOfCode2022;
using Raylib_cs;

var queue = new Queue<string>();
var input = File.ReadAllLines("input2.txt").ForEach(k => {
    var split = k.Split(" ");
    for (int j = 0; j < Int32.Parse(split[1]); j++){
        queue.Enqueue(split[0]);
    }
});


var head = new HeadElement();
var tails = Enumerable.Range(1, 9).Select(k => new TailElement(){ Name = k.ToString() }).ToList();

for (var i = 1; i < tails.Count; i++){
    tails[i].Head = tails[i - 1];
}

tails[0].Head = head;
var startingPos = head.Pos;

var visited = new HashSet<Vector2>();
var windowHeight = 1200;

void HandleRaylib(double waitTime){
    Raylib.WaitTime(waitTime);

    var size = 30;
    var middle = windowHeight / size - windowHeight / size / 2;
    var treshhold = middle;

    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.WHITE);
    //draw the tail
    foreach (var pos in visited){
        var x = (pos.X + treshhold) * size;
        var y = windowHeight - (pos.Y + treshhold) * size;
        Raylib.DrawRectangle((int)x, (int)y, size, size, Color.LIGHTGRAY);
    }

    //draw the tail
    foreach (var tail in tails){
        var tailX = (tail.Pos.X + treshhold) * size;
        var tailY = windowHeight - (tail.Pos.Y + treshhold) * size;
        Raylib.DrawRectangle((int)tailX, (int)tailY, size, size, Color.BLUE);
        Raylib.DrawText(tail.Name, (int)tailX, (int)tailY, (int)(size * 0.8), Color.BLACK);
    }

    //draw the head
    var headX = (head.Pos.X + treshhold) * size;
    var headY = windowHeight - (head.Pos.Y + treshhold) * size;
    Raylib.DrawRectangle((int)headX, (int)headY, size, size, Color.RED);


    Raylib.EndDrawing();
}


Raylib.InitWindow(windowHeight, windowHeight, "Advent of Code 2022");

Thread.Sleep(4000);

while (queue.Any()){
    var direction = queue.Dequeue();
    head.Move(direction);
    foreach (var tail in tails){
        tail.CheckIfMove();
    }

    visited.Add(tails[8].Pos);
    HandleRaylib(0.1);
}

Raylib.CloseWindow();
Console.WriteLine(visited.Count);


class HeadElement : RopeElement{
}

class TailElement : RopeElement{
    public RopeElement Head{ get; set; }

    public void CheckIfMove(){
        var pos = Head.Pos - Pos;
        var lengthSquared = pos.LengthSquared();
        if (lengthSquared / 2 > 1){
            var direction = pos.GetDireciton();
            Pos += direction;
        }
    }
}

class RopeElement{
    public string Name{ get; set; }
    public Vector2 Pos{ get; set; } = new Vector2(0, 0);

    public void Move(string direction){
        switch (direction){
            case "U":
                Pos += new Vector2(0, 1);
                break;
            case "D":
                Pos += new Vector2(0, -1);
                break;
            case "L":
                Pos += new Vector2(-1, 0);
                break;
            case "R":
                Pos += new Vector2(1, 0);
                break;
        }
    }
}

public static class Vector2Extensions{
    public static Vector2 GetDireciton(this Vector2 vector){
        var x = vector.X >= 1 ? 1 : vector.X <= -1 ? -1 : 0;
        var y = vector.Y >= 1 ? 1 : vector.Y <= -1 ? -1 : 0;
        return new Vector2(x, y);
    }
}