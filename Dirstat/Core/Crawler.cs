using System.Collections.Concurrent;
using Humanizer;

namespace DirStat.Core;

public class Crawler
{
    public Crawler(string rootPath)
    {
        RootPath = rootPath;
    }

    public string RootPath { get; set; }

    public async Task Crawl()
    {
        var files = new ConcurrentBag<FileInfo>();
        
        Console.WriteLine($"Crawling {RootPath}");

        await Task.Run(() => ParallelCrawl(this.RootPath, files));

        var totalBytes = files.Sum(x => x.Length);
        Console.WriteLine($"Total Number of Files: {files.Count:#,##0}");
        Console.WriteLine($"Total Size in Bytes: {totalBytes:#,##0} ({totalBytes.Bytes().Humanize()})");
    }

    public async Task ParallelCrawl(string path, ConcurrentBag<FileInfo> files)
    {
        // This options instance tells the parallel method to optimize for the number of processors
        var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };

        // Use DirectoryInfo to support both the top directory and subdirectories
        var dirInfo = new DirectoryInfo(path);

        // Create an enumerable of all directories including the root
        var directories = new ConcurrentBag<DirectoryInfo>(dirInfo.EnumerateDirectories("*", SearchOption.AllDirectories)) { dirInfo };

        // Process each directory in parallel
        await Parallel.ForEachAsync(directories, options, async (directory, cancellationToken) =>
        {
            // Enumerate all files in the current directory
            foreach (var file in directory.EnumerateFiles())
            {
                files.Add(file); // Add full path to the thread-safe collection
            }

            // Simulate asynchronous work
            await Task.Yield();
        });
    }
}