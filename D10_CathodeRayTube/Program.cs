using System.Drawing;
using AdventOfCode2022;
using Raylib_cs;
using Color = System.Drawing.Color;

var inputs = InputReader.CreateInputReader().ReadFilePerLine("input2.txt").GetInputs().ToStringsList()
    .SelectMany(k => k).ToList();


var screen = new List<List<char>>();
screen.Add(new List<char>());
var y = 0;

int x = 1;

var cycle = 1;

for (var i = 0; i < inputs.Count; i++){
    Draw();

    if (cycle % 40 == 0){
        screen.Add(new List<char>());
        y++;
    }

    if (i > 0 && inputs[i - 1] == "addx"){
        x += int.Parse(inputs[i]);
    }

    cycle++;

    void Draw(){
        if (x + (40 * y) + 1 == i || x + (40 * y) == i || x + (40 * y) - 1 == i){
            screen[y].Add('#');
            return;
        }

        screen[y].Add('.');
    }
}

PrintScreen();

foreach (var line in screen){
    Console.WriteLine(string.Join("", line));
}

void PrintScreen(){
    //make image with bitmap
    var bitmap = new Bitmap(40 + 1, screen.Count + 1);

    //make every element white
    for (var i = 0; i < bitmap.Width; i++){
        for (var j = 0; j < bitmap.Height; j++){
            bitmap.SetPixel(i, j, Color.White);
        }
    }

    for (var i = 0; i < screen.Count; i++){
        for (var j = 0; j < screen[i].Count; j++){
            if (screen[i][j] == '#'){
                bitmap.SetPixel(j + 1, i + 1, Color.Black);
            }
        }
    }

    //save image
    bitmap.Save("../../../screen.png");
}