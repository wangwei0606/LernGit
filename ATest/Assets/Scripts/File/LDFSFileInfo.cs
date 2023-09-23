using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LDFSFileInfo
{
    private string _absFilePath = "";
    private long _Fileposition = 0;
    private long _FileSize = 0;
    public string FilePath
    {
        get
        {
            return _absFilePath;
        }
        set
        {
            _absFilePath = value;
        }
    }
    public long FilePosition
    {
        get
        {
            return _Fileposition;
        }
        set
        {
            _Fileposition = value;
        }
    }
    public long FileSize
    {
        get
        {
            return _FileSize;
        }
        set
        {
            _FileSize = value;
        }
    }
}
