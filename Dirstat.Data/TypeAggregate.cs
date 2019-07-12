namespace Dirstat.Data
{
  public class TypeAggregate
  {
    /// <summary>
    /// The reference TypeInfoId.
    /// </summary>
    public ulong TypeInfoId { get; set; }
    
    /// <summary>
    /// The reference PathNodeId.
    /// </summary>
    public ulong PathNodeId { get; set; }
    
    /// <summary>
    /// Includes descendants. Number of files of this type that are descendant of PathNodeId.
    /// </summary>
    public ulong Count { get; set; }
    
    /// <summary>
    /// Includes descendants. Total size in bytes of this type that are descendants of PathNodeId.
    /// </summary>
    public double Bytes { get; set; }
  }
}