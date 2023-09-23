using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public delegate void LoaderProgress(string id, string url, float progress);
public delegate void LoaderFail(string id, string url, string reason);
public delegate void LoaderObjectCallBack(string id, string url, Asset context);
public delegate void LoaderTextCallBack(string id, string url, string text);
public delegate void LoaderBinCallBack(string id, string url, byte[] datas);
