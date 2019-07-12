using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dirstat.Data
{
  public class Scanner
  {
    private readonly ScannerOptions _options;
    private RecordInfo _recordInfo;

    private RecordInfo RecordInfo
    {
      get => _recordInfo;
      set => _recordInfo = value;
    }

    public Scanner(ScannerOptions options)
    {
      _options = options;
    }

    public void Scan()
    {
      _recordInfo = new RecordInfo();

      // register the parent folder of the root.
      var di = new DirectoryInfo(_options.Root);
      var parentNode = new PathNode()
      {
        Name = di.FullName,
        PathNodeId = 0,
      };
      _recordInfo.PathNodes.Add(parentNode);

      // start scanning and set parent id as the parent folder
      ScanChild(_options.Root, 0, 0);
    }

    private PathNode ScanChild(string currPath, ulong parentId, ulong depth)
    {
      // string formatting
      string padding = new String(' ', ((int) depth) * 2);

      var currentNode = _recordInfo.RegisterDirectory(currPath, parentId, depth);

      currentNode.ChildFileCount = 0;
      currentNode.ChildDirCount = 0;
      currentNode.DescendantFileCount = 0;
      currentNode.DescendantBytes = 0;
      currentNode.DescendantDirCount = 0;

      // update aggregate types
      var currentTypeAggregates = _recordInfo.TypeAggregates
        .Where(ta => ta.PathNodeId == currentNode.PathNodeId)
        .ToList();

      var childDepth = depth + 1;

      var localPathNodes = new List<PathNode>();
      foreach (var dir in Directory.EnumerateDirectories(currPath))
      {
        var cn = ScanChild(dir, parentId, childDepth);

        currentNode.ChildDirCount += 1;
        currentNode.DescendantDirCount += cn.DescendantDirCount;
        currentNode.DescendantBytes += cn.DescendantBytes;
        currentNode.DescendantFileCount += cn.DescendantFileCount;

        var childTypeAggregates = _recordInfo.TypeAggregates
          .Where(ta => ta.PathNodeId == cn.PathNodeId);

        foreach (var childTypeAggregate in childTypeAggregates)
        {
          // Check if this type aggregate is present on parent. 
          var pta = currentTypeAggregates.FirstOrDefault(x => x.TypeInfoId == childTypeAggregate.TypeInfoId);
          if (pta is null)
          {
            pta = new TypeAggregate()
            {
              Bytes = childTypeAggregate.Bytes,
              Count = childTypeAggregate.Count,
              PathNodeId = currentNode.PathNodeId,
              TypeInfoId = childTypeAggregate.TypeInfoId,
            };
            _recordInfo.TypeAggregates.Add(pta);
          }
          else
          {
            pta.Bytes += childTypeAggregate.Bytes;
            pta.Count += childTypeAggregate.Count;
          }
        }

        localPathNodes.Add(cn);
      }

      var localFileNodes = new List<FileNode>();
      foreach (var filePath in Directory.EnumerateFiles(currPath))
      {
        var fn = ScanFile(filePath, parentId, childDepth);
        currentNode.ChildFileCount += 1;
        currentNode.DescendantFileCount += 1;
        currentNode.DescendantBytes += fn.Bytes;

        localFileNodes.Add(fn);
      }

      Console.WriteLine($"{padding} | {depth:D5} D {currentNode.Name}");

//      foreach (var localPathNode in localPathNodes)
//      {
//        var childPadding = new string(' ', ((int) localPathNode.Depth) * 2);
//        Console.WriteLine($"{childPadding} | {localPathNode.Depth:D5} D {localPathNode.Name}");
//      }

      foreach (var localFileNode in localFileNodes)
      {
        var childPadding = new string(' ', ((int) localFileNode.Depth) * 2);
        Console.WriteLine($"{childPadding} | {localFileNode.Depth:D5} F {localFileNode.Name}");
      }

      return currentNode;
    }

    FileNode ScanFile(string filePath, ulong parentId, ulong depth)
    {
      // string formatting
      // string padding = new String(' ', ((int) depth) * 2);

      // add file node
      var currentFile = _recordInfo.RegisterFile(filePath, parentId, depth);

      // Console.WriteLine($"{padding} | {depth:D5} F {currentFile.Name}");

      // update all ancestor aggregates
      return currentFile;
    }
  }
}