using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LDFSFileCode
{
    public LDFSFileCode()
    {

    }
    public byte[] Encrypto(byte[] Source)
    {
        byte[] buff = new byte[Source.Length];
        for(int i=0;i<Source.Length;i++)
        {
            buff[i] = (byte)(Source[i] + 9);
        }
        return buff;
    }
    public byte[] Decrypto(byte[] Source)
    {
        byte[] buff = new byte[Source.Length];
        for(int i=0;i<Source.Length;i++)
        {
            buff[i] = (byte)(Source[i] - 9);
        }
        return buff;
    }
}
