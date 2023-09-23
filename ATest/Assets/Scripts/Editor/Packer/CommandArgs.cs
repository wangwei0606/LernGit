﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandArgs
{
    public Dictionary<string,string> ArgPairs
    {
        get
        {
            return mArgPairs;
        }
    }
    public List<string> Params
    {
        get
        {
            return mParams;
        }
    }
    private List<string> mParams = new List<string>();
    private Dictionary<string, string> mArgPairs = new Dictionary<string, string>();
    public string getParamByName(string key,string defaultVal="")
    {
        if(mArgPairs.ContainsKey(key))
        {
            return mArgPairs[key];
        }
        return defaultVal;
    }
}

public class CommandLine
{
    public static CommandArgs Parse(string[] args)
    {
        char[] kEqual = new char[] { '=' };
        char[] kArgStart = new char[] { '-', '\\' };
        CommandArgs ca = new CommandArgs();
        if(args!=null && args.Length>0)
        {
            int ii = -1;
            string token = NextToken(args, ref ii);
            while(token!=null)
            {
                if(IsArg(token))
                {
                    string arg = token.TrimStart(kArgStart).TrimEnd(kEqual);
                    string value = null;
                    if(arg.Contains("="))
                    {
                        string[] r = arg.Split(kEqual, 2);
                        if(r.Length==2 && r[1]!=string.Empty)
                        {
                            arg = r[0];
                            value = r[1];
                        }
                    }
                    while(value==null)
                    {
                        if(ii>args.Length)
                        {
                            break;
                        }
                        string next = NextToken(args, ref ii);
                        if(next!=null)
                        {
                            if(IsArg(next))
                            {
                                ii--;
                                value = "true";
                            }
                            else if (next!="=")
                            {
                                value = next.TrimStart(kEqual);
                            }
                        }
                    }
                    ca.ArgPairs.Add(arg,value);
                }
                else if (token!= string.Empty)
                {
                    ca.Params.Add(token);
                }
                token = NextToken(args,ref ii);
            }
        }
        return ca;
    }
    private static string NextToken(string[] args,ref int ii)
    {
        ii++;
        while(ii<args.Length)
        {
            string cur = args[ii].Trim();
            if(cur!=string.Empty)
            {
                return cur;
            }
            ii++;
        }
        return null;
    }
    private static bool IsArg(string arg)
    {
        return (arg.StartsWith("-")) || arg.StartsWith("\\");
    }
}
