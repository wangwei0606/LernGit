using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LDFSHeader
{
    private long _FileSize = 0;
    private long _HeadPosition = 0;
    public LDFSHeader(long fileSize,long headPosition)
    {
        _FileSize = fileSize;
        _HeadPosition = headPosition;
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

    public long HeadPosition
    {
        get
        {
            return _HeadPosition;
        }
        set
        {
            _HeadPosition = value;
        }
    }


}