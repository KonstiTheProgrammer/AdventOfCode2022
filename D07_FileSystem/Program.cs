using System.Runtime.CompilerServices;
using AdventOfCode2022;

var inputs = InputReader.CreateInputReader().ReadFilePerLine("input2.txt").GetInputs();

List<File> allFiles = new List<File>();
List<Directory> allDirectories = new List<Directory>();
Directory rootDirectory = new Directory("/", null);
Directory currentDirectory = rootDirectory;

foreach (var input in inputs) {
    string elem = input.Read();

    if (elem == "$") {
        elem = input.Read();
        if (elem == "cd") {
            elem = input.Read();
            currentDirectory = elem switch {
                ".." => currentDirectory.GetParentDirectory(),
                "/" => rootDirectory,
                _ => currentDirectory.GetDirectory(elem)
            };
        }
        else if (elem == "ls") {
            //Console.WriteLine(currentDirectory.Print());
        }

        continue;
    }

    if (elem == "dir") {
        elem = input.Read();
        currentDirectory.AddDirectory(elem, allDirectories);
        continue;
    }

    if (Int32.TryParse(elem, out var size)) {
        elem = input.Read();
        currentDirectory.AddFile(elem, allFiles, size);
        continue;
    }

    throw new Exception("sus");
}

Console.WriteLine(Stage2());


int Stage1() => allDirectories.Select(k => k.GetTotalSize()).Where(k => k < 100000).Sum();

int Stage2() {
    var freeSpace = 70000000 - rootDirectory.GetTotalSize();
    var list = allDirectories.Select(k => k.GetTotalSize());
    var x = list.OrderBy(k => k).First(k => k + freeSpace >= 30000000);
    return x;
}


class Directory : AElement {
    public List<Directory> Directories { get; set; } = new List<Directory>();
    public List<File> Files { get; set; } = new List<File>();

    public string GetFullDirectoryName() {
        if (Parent == null) return Name;
        return this.Name + Parent.GetFullDirectoryName();
    }

    public Directory(string name, Directory parent) : base(name, parent) {
    }

    public int GetTotalSize() {
        return GetSize() + Directories.Sum(k => k.GetTotalSize());
    }

    public int GetSize() {
        return Files.Sum(k => k.Size);
    }

    public Directory GetDirectory(string elem) {
        return Directories.FirstOrDefault(x => x.Name == elem) ?? this;
    }

    public void AddDirectory(string elem, List<Directory> allDirs) {
        var dir = new Directory(elem, this);
        this.Directories.Add(dir);
        allDirs.Add(dir);
    }

    public void AddFile(string elem, List<File> allFiles, int size) {
        var file = new File(elem, this) { Size = size };
        this.Files.Add(file);
        allFiles.Add(file);
    }

    public Directory GetParentDirectory() {
        return this.Parent ?? this;
    }

    public string Print() {
        return "";
    }
}

class File : AElement {
    public int Size { get; set; }

    public File(string name, Directory parent) : base(name, parent) {
    }
}

abstract class AElement {
    protected AElement(string name, Directory parent) {
        Name = name;
        Parent = parent;
    }

    public Directory? Parent { get; set; }
    public String Name { get; set; }
}