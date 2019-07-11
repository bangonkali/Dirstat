namespace Dirstat.Data
{
  public class FileNode
  {
    /// <summary>
    /// The reference TypeInfoId.
    /// </summary>
    public ulong TypeInfoId { get; set; }

    /// <summary>
    /// Filename without extension.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// File bytes.
    /// </summary>
    public double Bytes { get; set; }

    /// <summary>
    /// Depth from root.
    /// </summary>
    public ulong Depth { get; set; }

    /// <summary>
    /// The reference Parent PathNode Id.
    /// </summary>
    public ulong ParentId { get; set; }
  }
}