string[] filePaths = Directory.GetFiles(@"../../../../ImageGlass/Source/Components/ImageGlass.WebP", "*", SearchOption.AllDirectories);

foreach (var filePath in filePaths)
{
    Console.WriteLine($"Processing: {filePath}, Thread: {Task.CurrentId}");

    Pura.PuraIt(filePath);
    Thread.Sleep(10);
}

Thread.Sleep(500);

foreach (var filePath in filePaths)
{
    Console.WriteLine($"Processing: {filePath}, Thread: {Task.CurrentId}");

    Pura.PuraIt(filePath);
    Thread.Sleep(10);
}
