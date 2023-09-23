using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IDevelopNameRule
{
    int RuleId
    {
        get;
    }

    void setBundleName(string rootPath, string absPath, string abPath);
    void clearBundleName(string rootPath, string absPath);
}