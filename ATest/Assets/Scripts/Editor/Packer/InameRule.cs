using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface InameRule
{
    int RuleId { get; }
    void SetBundleName(IAssetLibrary Library, string rootPath, string absPath, string abPath);
}
