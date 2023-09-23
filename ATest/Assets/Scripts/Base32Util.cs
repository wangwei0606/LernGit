using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;

public class Base32Util
{
    public static string key = "";
    public static string qian = "res/";
    public static string hou = ".abc";
    private const string alphabet = "qsabcdefghijklmnoprtuvwxyz012345";//³¤¶È32Î»×Ö·û´®
    public static string EnRes(string org)
    {
        org = org.ToLower();
        string[] patharr = org.Split(Path.AltDirectorySeparatorChar);
        StringBuilder sb = new StringBuilder();
        for(int i=0;i<patharr.Length;i++)
        {
            string tempStr = EnBase32(patharr[i]);
            sb.Append(tempStr);
            if(i<patharr.Length-1)
            {
                sb.Append(Path.AltDirectorySeparatorChar);
            }
        }
        string enstr = sb.ToString();
        return qian + enstr + hou;
    }

    public static string DeRes(string haxi)
    {
        haxi = haxi.Replace(qian, "").Replace(hou, "");
        string[] patharr = haxi.Split(Path.AltDirectorySeparatorChar);
        StringBuilder sb = new StringBuilder();
        for(int i=0;i<patharr.Length;i++)
        {
            string tempStr = DeBase32(patharr[i]);
            sb.Append(tempStr);
            if(i<patharr.Length-1)
            {
                sb.Append(Path.AltDirectorySeparatorChar);
            }
        }
        return sb.ToString();
    }

    public static string EnBase32(string org)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(org);
        string output = "";
        for(int bitIndex=0;bitIndex<bytes.Length*8;bitIndex+=5)
        {
            int dualbyte = bytes[bitIndex / 8] << 8;
            if(bitIndex/8+1<bytes.Length)
            {
                dualbyte |= bytes[bitIndex / 8 + 1];
            }
            dualbyte = 0x1f & (dualbyte >> (16 - bitIndex % 8 - 5));
            output += alphabet[dualbyte];
        }
        return output.Replace("\0", "");
    }

    public static string DeBase32(string base32)
    {
        List<byte> output = new List<byte>();
        char[] bytes = base32.ToCharArray();
        for(int bitIndex=0;bitIndex<base32.Length*5;bitIndex+=8)
        {
            int dualbyte = alphabet.IndexOf(bytes[bitIndex / 5]) << 10;
            if(bitIndex/5+1<bytes.Length)
            {
                dualbyte |= alphabet.IndexOf(bytes[bitIndex / 5 + 1]) << 5;
            }
            if (bitIndex / 5 + 2 < bytes.Length)
            {
                dualbyte |= alphabet.IndexOf(bytes[bitIndex / 5 + 2]);
            }
            dualbyte = 0xff & (dualbyte >> (15 - bitIndex % 5 - 8));
            output.Add((byte)dualbyte);
        }
        byte[] result = output.ToArray();
        return Encoding.UTF8.GetString(result).Replace("\0", "");
    }
}
