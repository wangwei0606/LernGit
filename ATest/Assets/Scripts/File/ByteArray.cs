
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    public class ByteArray
    {

        private List<byte> _buff;
        private int _position = 0;
        public int Position
        {
            set { _position = value; }
            get { return _position; }
        }

        public int Capacity
        {
            get { return _buff.Count; }
        }

        public int BytesAvailable
        {
            get { return _buff.Count - _position; }
        }

        public byte[] Buffer
        {
            get { return _buff.ToArray(); }
        }

        /// <summary>
        /// 清除
        /// </summary>
        public void Clear()
        {
            _buff = new List<byte>();
            _position = 0;
        }

        /// <summary>
        /// 克隆
        /// </summary>
        /// <returns></returns>
        public ByteArray Clone()
        {
            byte[] bytes = new byte[_buff.Count];
            _buff.CopyTo(bytes);
            ByteArray ba = new ByteArray(bytes);
            return ba;
        }

        public ByteArray()
        {
            _buff = new List<byte>();
        }

        public ByteArray(byte[] buff)
        {
            resetBuff(buff);
        }

        public void resetBuff(byte[] buff)
        {
            if (_buff == null)
                _buff = new List<byte>();
            else
                _buff.Clear();
            _position = 0;
            _buff.AddRange(buff);
        }

        /// <summary>
        /// 读取bool
        /// </summary>
        /// <returns></returns>
        public bool ReadBool()
        {
            return BitConverter.ToBoolean(readBuff(sizeof(bool)), 0);
        }

        /// <summary>
        /// 读取byte
        /// </summary>
        /// <returns></returns>
        public byte ReadByte()
        {
            byte b = _buff[_position];
            _position++;
            return b;
        }

        /// <summary>
        /// 读取ushort
        /// </summary>
        /// <returns></returns>
        public ushort ReadUShort()
        {
            return BitConverter.ToUInt16(readBuff(sizeof(ushort)), 0);
        }

        /// <summary>
        /// 读取一个Vector2
        /// </summary>
        /// <returns></returns>
        public Vector2 ReadVector2()
        {
            Vector2 v = new Vector2();
            v.x = (float)ReadShort() / 100;
            v.y = (float)ReadShort() / 100;
            return v;
        }

        /// <summary>
        /// 读取一个Vector3
        /// </summary>
        /// <returns></returns>
        public Vector3 ReadVector3()
        {
            Vector3 v = new Vector3();
            v.x = (float)ReadShort() / 100;
            v.y = (float)ReadShort() / 100;
            v.z = (float)ReadShort() / 100;
            return v;
        }

        /// <summary>
        /// 读取short
        /// </summary>
        /// <returns></returns>
        public short ReadShort()
        {
            return BitConverter.ToInt16(readBuff(sizeof(short)), 0);
        }

        /// <summary>
        /// 读取int
        /// </summary>
        /// <returns></returns>
        public int ReadInt()
        {
            return BitConverter.ToInt32(readBuff(sizeof(int)), 0);
        }

        /// <summary>
        /// 读取一个UInt类型
        /// </summary>
        /// <returns></returns>
        public uint ReadUInt()
        {
            return BitConverter.ToUInt32(readBuff(sizeof(uint)), 0);
        }

        /// <summary>
        /// 读取long
        /// </summary>
        /// <returns></returns>
        public long ReadLong()
        {
            return BitConverter.ToInt64(readBuff(sizeof(long)), 0);
        }

        /// <summary>
        /// 读取ulong
        /// </summary>
        /// <returns></returns>
        public ulong ReadULong()
        {
            return BitConverter.ToUInt64(readBuff(sizeof(ulong)), 0);
        }

        /// <summary>
        /// 读取float
        /// </summary>
        /// <returns>The float.</returns>
        public float ReadFloat()
        {
            return BitConverter.ToSingle(readBuff(sizeof(float)), 0);
        }

        /// <summary>
        /// 读取一个UTF字符串
        /// </summary>
        /// <returns></returns>
        public string ReadUTFString()
        {
            int len = ReadInt();
            //Debug.Log ("len = " + len + " _position = " + _position + " _buff.Count = " + _buff.Count);
            byte[] buff = _buff.GetRange(_position, len).ToArray();
            _position += buff.Length;
            //Debug.Log("buff.Length = " + buff.Length + " _position = " + _position);
            return Encoding.UTF8.GetString(buff, 0, buff.Length);
        }

        /// <summary>
        /// 读取字节数组
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public byte[] ReadBytes(int length)
        {
            byte[] buff = _buff.GetRange(_position, length).ToArray();
            _position += buff.Length;
            return buff;
        }

        /// <summary>
        /// 读取ByteArray对象
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public ByteArray ReadByteArray(int length)
        {
            byte[] buff = ReadBytes(length);
            return new ByteArray(buff);
        }

        /// <summary>
        /// 写Bool
        /// </summary>
        /// <param name="b"></param>
        public void WriteBool(bool b)
        {
            writeBuff(BitConverter.GetBytes(b));
        }

        /// <summary>
        /// 写byte
        /// </summary>
        /// <param name="b"></param>
        public void WriteByte(byte b)
        {
            if (_position >= _buff.Count)
                _buff.Add(b);
            else
                _buff[_position] = b;
            _position = _buff.Count;
        }

        /// <summary>
        /// 写ushort
        /// </summary>
        /// <param name="us"></param>
        public void WriteUShort(ushort us)
        {
            writeBuff(BitConverter.GetBytes(us));
        }

        /// <summary>
        /// 写Short
        /// </summary>
        /// <param name="s"></param>
        public void WriteShort(short s)
        {
            writeBuff(BitConverter.GetBytes(s));
        }

        /// <summary>
        /// 写Uint
        /// </summary>
        /// <param name="ui"></param>
        public void WriteUInt(uint ui)
        {
            writeBuff(BitConverter.GetBytes(ui));
        }

        /// <summary>
        /// 写Int
        /// </summary>
        /// <param name="i"></param>
        public void WriteInt(int i)
        {
            writeBuff(BitConverter.GetBytes(i));
        }

        /// <summary>
        /// 写ulong
        /// </summary>
        /// <param name="ul"></param>
        public void WriteULong(ulong ul)
        {
            writeBuff(BitConverter.GetBytes(ul));
        }

        /// <summary>
        /// 写long
        /// </summary>
        /// <param name="l"></param>
        public void WriteLong(long l)
        {
            writeBuff(BitConverter.GetBytes(l));
        }

        /// <summary>
        /// 写float
        /// </summary>
        /// <param name="f">F.</param>
        public void WriteFloat(float f)
        {
            writeBuff(BitConverter.GetBytes(f));
        }

        /// <summary>
        /// 写入一个Vector2
        /// </summary>
        /// <param name="v"></param>
        public void WriteVector2(Vector2 v)
        {
            writeBuff(BitConverter.GetBytes((short)(v.x * 100)));
            writeBuff(BitConverter.GetBytes((short)(v.y * 100)));
        }

        /// <summary>
        /// 写入一个Vector3
        /// </summary>
        /// <param name="v"></param>
        public void WriteVector3(Vector3 v)
        {
            writeBuff(BitConverter.GetBytes((short)(v.x * 100)));
            writeBuff(BitConverter.GetBytes((short)(v.y * 100)));
            writeBuff(BitConverter.GetBytes((short)(v.z * 100)));
        }


        /// <summary>
        /// 写UTF
        /// </summary>
        /// <param name="str"></param>
        public void WriteUTF(string str)
        {
            byte[] buff = Encoding.UTF8.GetBytes(str);
            // Debug.Log("----------------------------");
            // Debug.Log("str = " + str);
            // Debug.Log("buff.Length = " + buff.Length);
            WriteInt(buff.Length);
            writeBuff(buff);
            //Debug.Log ("WriteUTF _Position = " + _position);
        }

        /// <summary>
        /// 写字节流
        /// </summary>
        /// <param name="bytes"></param>
        public void WriteBytes(byte[] bytes)
        {
            writeBuff(bytes);
        }

        /// <summary>
        /// 写入ByteArray对象
        /// </summary>
        /// <param name="byteArray"></param>
        public void WriteByteArray(ByteArray byteArray)
        {
            _buff.AddRange(byteArray.Buffer);
            _position = _buff.Count;
        }

        private byte[] readBuff(int length)
        {
            byte[] buff = _buff.GetRange(_position, length).ToArray();
            _position += buff.Length;
            return buff;
        }

        private void writeBuff(byte[] buff)
        {
            //Debug.Log("_po = " + _position + " _buff.Count = " + _buff.Count + " buff = " + buff.Length);
            if (_position >= _buff.Count)
            {
                _buff.AddRange(buff);
                _position = _buff.Count;
            }
            else
            {
                int removeLen = ((buff.Length + _position) > _buff.Count) ? (_buff.Count - _position) : buff.Length;
                //Debug.Log("removeLen = " + removeLen + " _buff.Count = " + _buff.Count + " _position = " + _position + " buff.Length " + buff.Length);
                _buff.RemoveRange(_position, removeLen);
                _buff.InsertRange(_position, buff);
                _position += buff.Length;
            }

        }
    }
