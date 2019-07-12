using System;
using System.Collections.Generic;

namespace Dirstat.Data
{
  public class PathNode
  {
    /// <summary>
    /// String path of the node starting from the root.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// The PathNodeId primary key.
    /// </summary>
    public ulong PathNodeId { get; set; }

    /// <summary>
    /// Total number of direct child files under this path.
    /// </summary>
    public ulong ChildFileCount { get; set; }

    /// <summary>
    /// Total number of direct child directories under this path.
    /// </summary>
    public ulong ChildDirCount { get; set; }

    /// <summary>
    /// Total number of descendant files under this path.
    /// </summary>
    public ulong DescendantFileCount { get; set; }

    /// <summary>
    /// Total number of descendant directories under this path.
    /// </summary>
    public ulong DescendantDirCount { get; set; }

    /// <summary>
    /// Total size of all descendant files under this path.
    /// </summary>
    public double DescendantBytes { get; set; }
    
    /// <summary>
    /// Parent PathNode Id.
    /// </summary>
    public ulong ParentId { get; set; }
    
    public ulong Depth { get; set; }
  }
}