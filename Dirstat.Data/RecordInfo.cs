using System;
using System.Collections.Generic;
using System.IO;

namespace Dirstat.Data
{
  public class RecordInfo
  {
    /// <summary>
    /// Path nodes descendants starting from the root.
    /// </summary>
    public List<PathNode> PathNodes { get; set; }

    /// <summary>
    /// File node descendants from the root.
    /// </summary>
    public List<FileNode> FileNodes { get; set; }

    /// <summary>
    /// Aggregate data from descendants of the root.
    /// </summary>
    public List<TypeAggregate> TypeAggregates { get; set; }

    public RecordInfo()
    {
      PathNodes = new List<PathNode>();
      FileNodes = new List<FileNode>();
      TypeAggregates = new List<TypeAggregate>();
    }

    /// <summary>
    /// Registers a directory to the Record.
    /// </summary>
    /// <param name="name"></param>
    public void RegisterDirectory(string name)
    {
      DirectoryInfo di = new DirectoryInfo(name);
      Console.WriteLine($"Dir Parent {di.Parent} Name {di.Name}");
    }

    public void RegisterFile(string name)
    {
      FileInfo fi = new FileInfo(name);
      Console.WriteLine($"File Parent {fi.DirectoryName} Name {fi.Name} Ext {fi.Extension} Size: {fi.Length}");
    }
  }
}