using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IUnCompressTask
{
    string getZipFileName();
    string getDestFoldName();
    string getFileCrc();
    bool IsZip();
    void OnFinishUnCompress(HttpLoadCode eCode);
    float Process { get; set; }
    void OnUnZipProcess(float process);
}
