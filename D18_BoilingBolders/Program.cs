// See https://aka.ms/new-console-template for more information

using System.Numerics;
using Raylib_cs;

var watch = System.Diagnostics.Stopwatch.StartNew();

int MieseCounter(HashSet<Vector3> hashSet){
    var i = 0;
    foreach (var x in hashSet){
        var list = new List<Vector3>();
        list.Add(x + new Vector3(0, 0, -1));
        list.Add(x + new Vector3(0, 0, 1));
        list.Add(x + new Vector3(0, 1, 0));
        list.Add(x + new Vector3(0, -1, 0));
        list.Add(x + new Vector3(1, 0, 0));
        list.Add(x + new Vector3(-1, 0, 0));

        i += list.Sum(k => !hashSet.Contains(k) ? 1 : 0);
    }

    return i;
}

var kussAufdenMund = File.ReadAllLines("../../../input2.txt")
    .Select(k => k.Split(",").Select(k => Convert.ToSingle(k)).ToList()).ToList().Select(
        k => new Vector3(k[0], k[1], k[2])).ToHashSet();
var listOfAllCubes = new HashSet<Vector3>();

var maxOben = kussAufdenMund.Max(k => k.Y) + 2;
var maxUnten = kussAufdenMund.Min(k => k.Y) - 2;
var maxLinks = kussAufdenMund.Min(k => k.X) - 2;
var maxRechts = kussAufdenMund.Max(k => k.X) + 2;
var maxVorne = kussAufdenMund.Max(k => k.Z) + 2;
var maxHinten = kussAufdenMund.Min(k => k.Z) - 2;

Raylib.InitWindow(1900, 1100, "Hello World");
Camera3D camera = new();
camera.position = new Vector3(10.0f, 10.0f, 10.0f); // Camera3D position
camera.target = new Vector3(0.0f, 0.0f, 0.0f); // Camera3D looking at point
camera.up = new Vector3(0.0f, 1.0f, 0.0f); // Camera3D up vector (rotation towards target)
camera.fovy = 80.0f; // Camera3D field-of-view Y


var middle = kussAufdenMund.Aggregate((a, b) => a + b) / kussAufdenMund.Count;
camera.target = middle;

Raylib.SetCameraMode(camera, CameraMode.CAMERA_FREE); // Set a free camera mode
Rlgl.rlDisableBackfaceCulling();


Console.WriteLine(maxOben + " " + maxUnten + " " + maxLinks + " " + maxRechts + " " + maxVorne + " " + maxHinten);


for (int i = (int)maxUnten; i <= maxOben; i++){
    for (int j = (int)maxLinks; j <= maxRechts; j++){
        for (int k = (int)maxHinten; k <= maxVorne; k++){
            listOfAllCubes.Add(new Vector3(j, i, k));
        }
    }
}

var dif = 0f;

var point = new Vector3(maxRechts - 1, maxOben - 1, maxVorne - 1);
var visited = new HashSet<Vector3>(kussAufdenMund);
var queue = new Queue<Vector3>();
queue.Enqueue(point);
visited.Add(point);

while (queue.Any()){
    var x = queue.Dequeue();
    var list = new List<Vector3>();
    list.Add(x + new Vector3(0, 0, -1));
    list.Add(x + new Vector3(0, 0, 1));
    list.Add(x + new Vector3(0, 1, 0));
    list.Add(x + new Vector3(0, -1, 0));
    list.Add(x + new Vector3(1, 0, 0));
    list.Add(x + new Vector3(-1, 0, 0));

    foreach (var k in list.Where(k => !visited.Contains(k) && listOfAllCubes.Contains(k))){
        queue.Enqueue(k);
    }

    visited.Add(x);


    dif = watch.ElapsedMilliseconds / 1000;
    watch.Reset();
    DrawCollection(visited);
}


kussAufdenMund = kussAufdenMund.Union(listOfAllCubes.Except(visited)).ToHashSet();

Console.WriteLine(MieseCounter(kussAufdenMund));


while (!Raylib.WindowShouldClose()){
    Raylib.UpdateCamera(ref camera);
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.RAYWHITE);
    Raylib.BeginMode3D(camera);

    Raylib.DrawGrid(10, 1.0f);

    foreach (var k in kussAufdenMund){
        Raylib.DrawCube(k, 1f, 1f, 1f, Color.RED);
        Raylib.DrawCubeWires(k, 1f, 1f, 1f, Color.BLACK);
        //draw the left side of the cube in blue
    }

    Raylib.EndMode3D();
    Raylib.EndDrawing();
}

Raylib.CloseWindow();


void DrawCollection(ICollection<Vector3> vec){
    Raylib.UpdateCamera(ref camera);
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.RAYWHITE);
    Raylib.DrawText("hats gedauert:" + dif.ToString(), 10, 10, 70, Color.BLACK);

    Raylib.BeginMode3D(camera);

    foreach (var k in vec){
        Raylib.DrawCube(k, 1f, 1f, 1f, Color.RED);
        Raylib.DrawCubeWires(k, 1f, 1f, 1f, Color.BLACK);
    }

    //print the 


    Raylib.EndMode3D();
    Raylib.EndDrawing();
}