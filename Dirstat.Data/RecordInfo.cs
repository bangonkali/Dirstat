using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dirstat.Data.Exceptions;

namespace Dirstat.Data
{
  public class RecordInfo
  {
    private ulong _nodeSource = 0;

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

    /// <summary>
    /// Type infos.
    /// </summary>
    public List<TypeInfo> TypeInfos { get; set; }

    /// <summary>
    /// Number of files of this type that are descendant of PathNodeId.
    /// </summary>
    public ulong Count { get; set; }

    /// <summary>
    /// Total size in bytes of this type that are descendants of PathNodeId.
    /// </summary>
    public double Bytes { get; set; }

    public RecordInfo()
    {
      PathNodes = new List<PathNode>();
      FileNodes = new List<FileNode>();
      TypeAggregates = new List<TypeAggregate>();
      TypeInfos = new List<TypeInfo>();
    }

    private PathNode RegisterParent(string name)
    {
      _nodeSource += 1;

      var di = new DirectoryInfo(name);

      var parentNode = new PathNode()
      {
        Name = di.FullName,
        PathNodeId = _nodeSource,
      };
      PathNodes.Add(parentNode);

      Console.WriteLine($"Root Name {_nodeSource} {parentNode.Name} {parentNode.PathNodeId}");

      return parentNode;
    }

    private TypeInfo RegisterTypeInfo(string name)
    {
      _nodeSource += 1;
      var ti = new TypeInfo()
      {
        Name = name,
        Description = "",
        TypeInfoId = _nodeSource,
      };

      TypeInfos.Add(ti);
      return ti;
    }

    /// <summary>
    /// Registers a directory to the Record.
    /// </summary>
    /// <param name="currentPath"></param>
    /// <param name="parentId"></param>
    /// <param name="depth"></param>
    public PathNode RegisterDirectory(string currentPath, ulong parentId, ulong depth)
    {
      // Get directory name
      var di = new DirectoryInfo(currentPath);

      if (di.Parent is null)
        throw new ParentNotFoundException();

      // Check if parent exists
      var parentNode = PathNodes.FirstOrDefault(pns => pns.PathNodeId == parentId) ??
                       RegisterParent(di.Parent.FullName);

      _nodeSource += 1;
      var currentNode = new PathNode()
      {
        Name = di.Name,
        PathNodeId = _nodeSource,
        ParentId = parentNode.PathNodeId,
        Depth = depth,
      };
      PathNodes.Add(currentNode);
      return currentNode;
//      Console.WriteLine($"Dir Current {currentNode.PathNodeId} Parent {currentNode.ParentId} Name {currentNode.Path}");
    }

    /// <summary>
    /// Registers a directory to the record.
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="parentId"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    /// <exception cref="ParentNotFoundException"></exception>
    public FileNode RegisterFile(string filePath, ulong parentId, ulong depth)
    {
      var fi = new FileInfo(filePath);

      var parentNode = PathNodes.FirstOrDefault(pns => pns.PathNodeId == parentId) ??
                       RegisterParent(fi.DirectoryName);

      // check if file type already exists, else create new file type
      var typeInfo = TypeInfos.FirstOrDefault(ti => ti.Name == fi.Extension) ??
                     RegisterTypeInfo(fi.Extension);

      // register the file 
      var fileNode = new FileNode()
      {
        TypeInfoId = typeInfo.TypeInfoId,
        Name = fi.Name,
        Bytes = fi.Length,
        ParentId = parentNode.PathNodeId,
        Depth = depth,
      };
      FileNodes.Add(fileNode);

      // check if typeaggregate is already registered.
      var typeAggregate =
        TypeAggregates.FirstOrDefault(ta =>
          ta.TypeInfoId == typeInfo.TypeInfoId && ta.PathNodeId == parentNode.PathNodeId);

      if (typeAggregate == null)
      {
        typeAggregate = new TypeAggregate()
        {
          TypeInfoId = typeInfo.TypeInfoId,
          PathNodeId = parentNode.PathNodeId,
          Count = 1,
          Bytes = fi.Length
        };
        TypeAggregates.Add(typeAggregate);
      }
      else
      {
        typeAggregate.Count = typeAggregate.Count + 1;
        typeAggregate.Bytes = typeAggregate.Bytes + fi.Length;
      }

      Count = Count + 1;
      Bytes = Bytes + fi.Length;

      return fileNode;
    }
  }
}