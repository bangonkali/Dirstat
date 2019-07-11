namespace Dirstat.Data
{
  public class TypeInfo
  {
    /// <summary>
    /// Extension name of the type without the period.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// OS Description of the particular type.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// The primary key TypeInfoId.
    /// </summary>
    public ulong TypeInfoId { get; set; }
  }
}