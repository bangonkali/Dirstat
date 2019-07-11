using System.IO;

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
      ScanChild(_options.Root);
    }

    private void ScanChild(string name)
    {
      // register the directory
      _recordInfo.RegisterDirectory(name);

      foreach (string dir in Directory.EnumerateDirectories(name))
      {
        ScanChild(dir);
      }

      foreach (string file in Directory.EnumerateFiles(name))
      {
        ScanFile(file);
      }
    }

    void ScanFile(string name)
    {
      // add file node
      _recordInfo.RegisterFile(name);

      // update all ancestor aggregates
    }
  }
}