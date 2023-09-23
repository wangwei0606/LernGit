
#define JSON_INT_64 
//#define JSON_FLOAT_SINGLE
#define JSON_SUPPORT_SERIALIZE

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.IO;
    using System.Text.RegularExpressions;
    #region 数据接口
    public class DataType
    {
        public const string NUMBER = "number";
        public const string STRING = "string";
        public const string BOOL = "bool";
        public const string ARRAY = "array";
        public const string OBJECT = "object";
        public const string OTHER = "other";
    }
    public class JsonData
    {
        private string _type = "";

        public string Type
        {
            get
            {
                return _type;
            }

        }
        private int _count = 0;

        public int Count
        {
            get { return _count; }
        }
        private object content;
        private Dictionary<object, JsonData> contentDict;
        private List<string> keys = new List<string>();

        public object Content
        {
            get
            {
                return content;
            }
        }

        public JsonData()
        {
            contentDict = new Dictionary<object, JsonData>();
        }

        public void SetType(string t)
        {
            _type = t;
        }

        public void SetValue(object obj)
        {
            content = obj;
            _type = GetTypeByObj(obj);
        }

        public void SetValue(JsonData[] objs)
        {

            _type = DataType.ARRAY;
            _count = 0;
            if (objs != null && objs.Length > 0)
            {
                _count = objs.Length;
                for (int i = 0; i < objs.Length; i++)
                {
                    //contents.Add(objs[i]);
                    SetValue(i, objs[i]);
                }
            }
        }

        public void SetValue(List<JsonData> objs)
        {
            _count = 0;
            _type = DataType.ARRAY;
            if (objs != null && objs.Count > 0)
            {
                _count = objs.Count;
                for (int i = 0; i < objs.Count; i++)
                {
                    SetValue(i, objs[i]);
                }
            }


        }

        public void SetValue(object key, JsonData value)
        {
            if (string.IsNullOrEmpty(_type))
            {
                _type = DataType.OBJECT;
            }
            if (contentDict.ContainsKey(key))
            {
                contentDict[key] = value;
            }
            else
            {
                contentDict.Add(key, value);
            }
            string ks = key.ToString();
            if (keys.IndexOf(ks) < 0)
            {
                keys.Add(ks);
            }
        }



        /// <summary>
        /// 判断传入内容的类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private string GetTypeByObj(object obj)
        {
            Type type = obj.GetType();
            return JsonTools.GetDataType(type);
        }

        /// <summary>
        /// 取某个key的值，返回jsonData
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonData Get(object id)
        {
            if (contentDict.ContainsKey(id))
            {
                return contentDict[id];
            }
            else
            {
                return null;
            }
        }

        public JsonData this[object id]
        {
            get
            {
                if (contentDict.ContainsKey(id))
                {
                    return contentDict[id];
                }
                else
                {
                    return null;
                }
            }
            set
            {

                if (contentDict.ContainsKey(id))
                {
                    contentDict[id] = value;
                }
                else
                {
                    contentDict.Add(id, value);
                }
            }
        }

        /// <summary>
        /// 设置Jsondata某个key的值
        /// </summary>
        /// <param name="id"></param>
        /// <param name="obj"></param>
        private void Set(object id, JsonData obj)
        {
            if (string.IsNullOrEmpty(_type))
            {
                _type = DataType.OBJECT;
            }
            if (contentDict.ContainsKey(id))
            {
                contentDict[id] = obj;
            }
            else
            {
                contentDict.Add(id, obj);
            }
        }

        public object ToValue()
        {
            if (_type == DataType.ARRAY)
            {
                return GetList();
            }
            else if (_type == DataType.BOOL)
            {
                return ToBool();
            }
            else if (_type == DataType.NUMBER)
            {
                return ToFloat();
            }
            else if (_type == DataType.STRING)
            {
                return ToString();
            }
            else
            {
                return ToString();
            }
        }

        /// <summary>
        /// 把值转成string字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (content != null)
            {
                return content.ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 把值转成整型
        /// </summary>
        /// <returns></returns>
        public int ToInt()
        {
            if (content != null)
            {
                return int.Parse(content.ToString());
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 把值转成浮点数值
        /// </summary>
        /// <returns></returns>
        public Double ToFloat()
        {
            if (content != null)
            {
                return Double.Parse(content.ToString());
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 把值转成布尔
        /// </summary>
        /// <returns></returns>
        public bool ToBool()
        {
            if (content != null)
            {
                string b = content.ToString().ToLower();
                if (b == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取该JsonData的所有key
        /// </summary>
        /// <returns></returns>
        public List<string> GetKeys()
        {
            if (_type != DataType.OBJECT)
            {
                return null;
            }
            return keys;
        }

        /// <summary>
        /// 获取该JsonData的数组值
        /// </summary>
        /// <returns></returns>
        public List<JsonData> GetList()
        {
            if (_type != DataType.ARRAY)
            {
                return null;
            }
            List<JsonData> list = new List<JsonData>();
            if (content != null)
            {
                Array arr = content as Array;
                if (arr != null)
                {
                    foreach (System.Object ob in arr)
                    {
                        JsonData js = new JsonData();
                        js.SetValue(ob);
                        list.Add(js);
                    }
                    return list;
                }
            }
            for (int i = 0; i < _count; i++)
            {
                if (contentDict.ContainsKey(i))
                {
                    list.Add(contentDict[i]);
                }
            }
            return list;
        }
    }
    #endregion
    #region 工具类
    public class JsonTools
    {
        static public string GetDataType(Type type)
        {
            if (type == typeof(int) || type == typeof(float) || type == typeof(double) || type == typeof(Single) || type == typeof(long) || type == typeof(byte) || type == typeof(short))
            {
                return DataType.NUMBER;
            }
            else if (type == typeof(string))
            {
                return DataType.STRING;
            }
            else if (type == typeof(bool))
            {
                return DataType.BOOL;
            }
            else if (type.IsGenericType)
            {
                return DataType.ARRAY;
            }
            else if (type.BaseType == typeof(Array))
            {
                return DataType.ARRAY;
            }
            else if (type.IsClass)
            {
                return DataType.OBJECT;
            }
            else
            {
                FieldInfo[] fields = type.GetFields();
                if (fields != null && fields.Length > 0)
                {
                    return DataType.OBJECT;
                }
                else
                {
                    return DataType.OTHER;
                }
            }
        }
    }
    #endregion
    public class Json
    {
        #region 解析器
        sealed class Parser : IDisposable
        {
            const string WORD_BREAK = "{}[],:\"";

            public static bool IsWordBreak(char c)
            {
                return Char.IsWhiteSpace(c) || WORD_BREAK.IndexOf(c) != -1;
            }

            enum TOKEN
            {
                NONE,
                CURLY_OPEN,    // {
                CURLY_CLOSE,   // }
                SQUARED_OPEN,  // [
                SQUARED_CLOSE, // ]
                COLON,         //:
                COMMA,         //,
                STRING,
                NUMBER,
                TRUE,
                FALSE,
                NULL
            };

            StringReader json;

            Parser(string jsonString)
            {
                json = new StringReader(jsonString);
            }
            public void Dispose()
            {
                json.Dispose();
                json = null;
            }
            #region
            public static JsonData ParseJd(string jsonString)
            {
                using (var instance = new Parser(jsonString))
                {
                    return instance.ParseJdValue();
                }
            }
            JsonData ParseJdObject()
            {
                JsonData table = new JsonData();

                // ditch opening brace
                json.Read();

                while (true)
                {
                    switch (NextToken)
                    {
                        case TOKEN.NONE:
                            return null;
                        case TOKEN.COMMA:
                            continue;
                        case TOKEN.CURLY_CLOSE:
                            return table;
                        default:
                            string name = ParseString();
                            if (name == null)
                            {
                                return null;
                            }

                            if (NextToken != TOKEN.COLON)
                            {
                                return null;
                            }
                            // ditch the colon
                            json.Read();

                            // value
                            table.SetValue(name, ParseJdValue());
                            break;
                    }
                }
            }

            JsonData ParseJdArray()
            {
                List<JsonData> array = new List<JsonData>();

                // ditch opening bracket
                json.Read();

                bool parsing = true;
                while (parsing)
                {
                    TOKEN nextToken = NextToken;

                    switch (nextToken)
                    {
                        case TOKEN.NONE:
                            return null;
                        case TOKEN.COMMA:
                            continue;
                        case TOKEN.SQUARED_CLOSE:
                            parsing = false;
                            break;
                        default:
                            JsonData value = ParseJdByToken(nextToken);

                            array.Add(value);
                            break;
                    }
                }
                JsonData jd = new JsonData();
                jd.SetValue(array);
                return jd;
            }

            JsonData ParseJdValue()
            {
                TOKEN nextToken = NextToken;
                return ParseJdByToken(nextToken);
            }

            JsonData ParseJdByToken(TOKEN token)
            {
                switch (token)
                {
                    case TOKEN.STRING:
                        return ParseJdStr();
                    case TOKEN.NUMBER:
                        return ParseJdNum();
                    case TOKEN.CURLY_OPEN:
                        return ParseJdObject();
                    case TOKEN.SQUARED_OPEN:
                        return ParseJdArray();
                    case TOKEN.TRUE:
                        var jd = new JsonData();
                        jd.SetValue(true);
                        return jd;
                    case TOKEN.FALSE:
                        var jd1 = new JsonData();
                        jd1.SetValue(false);
                        return jd1;
                    case TOKEN.NULL:
                    default:
                        var jd2 = new JsonData();
                        jd2.SetValue("null");
                        return jd2;
                }
            }

            JsonData ParseJdStr()
            {
                JsonData jd = new JsonData();
                jd.SetValue(ParseString());
                return jd;
            }

            JsonData ParseJdNum()
            {
                JsonData jd = new JsonData();
                jd.SetValue(ParseNumber());
                return jd;
            }
            #endregion
            #region parese to object
            public static object Parse(string jsonString)
            {
                using (var instance = new Parser(jsonString))
                {
                    return instance.ParseValue();
                }
            }

            Dictionary<string, object> ParseObject()
            {
                Dictionary<string, object> table = new Dictionary<string, object>();

                // ditch opening brace
                json.Read();

                while (true)
                {
                    switch (NextToken)
                    {
                        case TOKEN.NONE:
                            return null;
                        case TOKEN.COMMA:
                            continue;
                        case TOKEN.CURLY_CLOSE:
                            return table;
                        default:
                            string name = ParseString();
                            if (name == null)
                            {
                                return null;
                            }

                            if (NextToken != TOKEN.COLON)
                            {
                                return null;
                            }
                            // ditch the colon
                            json.Read();

                            // value
                            table[name] = ParseValue();
                            break;
                    }
                }
            }

            List<object> ParseArray()
            {
                List<object> array = new List<object>();

                // ditch opening bracket
                json.Read();

                bool parsing = true;
                while (parsing)
                {
                    TOKEN nextToken = NextToken;

                    switch (nextToken)
                    {
                        case TOKEN.NONE:
                            return null;
                        case TOKEN.COMMA:
                            continue;
                        case TOKEN.SQUARED_CLOSE:
                            parsing = false;
                            break;
                        default:
                            object value = ParseByToken(nextToken);

                            array.Add(value);
                            break;
                    }
                }

                return array;
            }

            object ParseValue()
            {
                TOKEN nextToken = NextToken;
                return ParseByToken(nextToken);
            }

            object ParseByToken(TOKEN token)
            {
                switch (token)
                {
                    case TOKEN.STRING:
                        return ParseString();
                    case TOKEN.NUMBER:
                        return ParseNumber();
                    case TOKEN.CURLY_OPEN:
                        return ParseObject();
                    case TOKEN.SQUARED_OPEN:
                        return ParseArray();
                    case TOKEN.TRUE:
                        return true;
                    case TOKEN.FALSE:
                        return false;
                    case TOKEN.NULL:
                        return null;
                    default:
                        return null;
                }
            }

            string ParseString()
            {
                StringBuilder s = new StringBuilder();
                char c;

                // ditch opening quote
                json.Read();

                bool parsing = true;
                while (parsing)
                {

                    if (json.Peek() == -1)
                    {
                        parsing = false;
                        break;
                    }

                    c = NextChar;
                    switch (c)
                    {
                        case '"':
                            parsing = false;
                            break;
                        case '\\':
                            if (json.Peek() == -1)
                            {
                                parsing = false;
                                break;
                            }

                            c = NextChar;
                            switch (c)
                            {
                                case '"':
                                case '\\':
                                case '/':
                                    s.Append(c);
                                    break;
                                case 'b':
                                    s.Append('\b');
                                    break;
                                case 'f':
                                    s.Append('\f');
                                    break;
                                case 'n':
                                    s.Append('\n');
                                    break;
                                case 'r':
                                    s.Append('\r');
                                    break;
                                case 't':
                                    s.Append('\t');
                                    break;
                                case 'u':
                                    var hex = new char[4];

                                    for (int i = 0; i < 4; i++)
                                    {
                                        hex[i] = NextChar;
                                    }

                                    s.Append((char)Convert.ToInt32(new string(hex), 16));
                                    break;
                            }
                            break;
                        default:
                            s.Append(c);
                            break;
                    }
                }
                return s.ToString();
            }

            object ParseNumber()
            {
                string number = NextWord;

                if (number.IndexOf('.') == -1)
                {
#if JSON_INT_64
                    long parsedInt;
                    Int64.TryParse(number, out parsedInt);
#else
                    int parsedInt;
                    Int32.TryParse(number, out parsedInt);
#endif
                    return parsedInt;
                }

#if JSON_FLOAT_SINGLE
                float parsedDouble;
                Single.TryParse(number, out parsedDouble);
#else
                double parsedDouble;
                Double.TryParse(number, out parsedDouble);
#endif
                return parsedDouble;
            }
            #endregion
            void EatWhitespace()
            {
                while (Char.IsWhiteSpace(PeekChar))
                {
                    json.Read();
                    if (json.Peek() == -1)
                    {
                        break;
                    }
                }
            }

            char PeekChar
            {
                get
                {
                    return Convert.ToChar(json.Peek());
                }
            }

            char NextChar
            {
                get
                {
                    return Convert.ToChar(json.Read());
                }
            }

            string NextWord
            {
                get
                {
                    StringBuilder word = new StringBuilder();

                    while (!IsWordBreak(PeekChar))
                    {
                        word.Append(NextChar);
                        if (json.Peek() == -1)
                        {
                            break;
                        }
                    }
                    return word.ToString();
                }
            }

            TOKEN NextToken
            {
                get
                {
                    EatWhitespace();

                    if (json.Peek() == -1)
                    {
                        return TOKEN.NONE;
                    }

                    switch (PeekChar)
                    {
                        case '{':
                            return TOKEN.CURLY_OPEN;
                        case '}':
                            json.Read();
                            return TOKEN.CURLY_CLOSE;
                        case '[':
                            return TOKEN.SQUARED_OPEN;
                        case ']':
                            json.Read();
                            return TOKEN.SQUARED_CLOSE;
                        case ',':
                            json.Read();
                            return TOKEN.COMMA;
                        case '"':
                            return TOKEN.STRING;
                        case ':':
                            return TOKEN.COLON;
                        case '0':
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
                        case '-':
                            return TOKEN.NUMBER;
                    }

                    switch (NextWord)
                    {
                        case "false":
                            return TOKEN.FALSE;
                        case "true":
                            return TOKEN.TRUE;
                        case "null":
                            return TOKEN.NULL;
                    }

                    return TOKEN.NONE;
                }
            }
        }
        #endregion
        #region 序列化
        sealed class Serializer
        {
            StringBuilder builder;

            Serializer()
            {
                builder = new StringBuilder();
            }
            #region object
            public static string Serialize(object obj, bool isUnicode = false)
            {
                var instance = new Serializer();

                instance.SerializeValue(obj, isUnicode);

                return instance.builder.ToString();
            }

            void SerializeValue(object value, bool isUnicode)
            {
                IList asList;
                IDictionary asDict;
                string asStr;
                if (value == null)
                {
                    builder.Append("null");
                }
                else if ((asStr = value as string) != null)
                {
                    SerializeString(asStr, isUnicode);
                }
                else if (value is bool)
                {
                    builder.Append((bool)value ? "true" : "false");
                }
                else if ((asList = value as IList) != null)
                {
                    SerializeArray(asList, isUnicode);
                }
                else if ((asDict = value as IDictionary) != null)
                {
                    SerializeDictionary(asDict, isUnicode);
                }
                else if (value != null && value.GetType().IsClass)
                {
                    SerializeObject(value, isUnicode);
                }
                else if (value is char)
                {
                    SerializeString(new string((char)value, 1), isUnicode);
                }
                else
                {
                    SerializeOther(value, isUnicode);
                }
            }
            void SerializeObject(object obj, bool isUnicode)
            {
                bool first = true;
                builder.Append('{');
                Type type = obj.GetType();
                FieldInfo[] fields = type.GetFields();
                for (int i = 0; i < fields.Length; i++)
                {
                    if (!first)
                    {
                        builder.Append(',');
                    }
                    if (!fields[i].IsPublic)
                    {
                        continue;
                    }
                    if (fields[i].IsStatic)
                    {
                        continue;
                    }
                    SerializeString(fields[i].Name, isUnicode);
                    builder.Append(':');
                    object value = fields[i].GetValue(obj);
                    SerializeValue(value, isUnicode);
                    first = false;
                }
                builder.Append('}');
            }
            void SerializeDictionary(IDictionary obj, bool isUnicode)
            {
                bool first = true;

                builder.Append('{');

                foreach (object e in obj.Keys)
                {
                    if (!first)
                    {
                        builder.Append(',');
                    }

                    SerializeString(e.ToString(), isUnicode);
                    builder.Append(':');

                    SerializeValue(obj[e], isUnicode);

                    first = false;
                }

                builder.Append('}');
            }

            void SerializeArray(IList anArray, bool isUnicode)
            {
                builder.Append('[');

                bool first = true;

                foreach (object obj in anArray)
                {
                    if (!first)
                    {
                        builder.Append(',');
                    }

                    SerializeValue(obj, isUnicode);

                    first = false;
                }

                builder.Append(']');
            }
            void SerializeString(string str, bool isUnicode)
            {
                builder.Append('\"');

                char[] charArray = str.ToCharArray();
                foreach (var c in charArray)
                {
                    switch (c)
                    {
                        case '"':
                            builder.Append("\\\"");
                            break;
                        case '\\':
                            builder.Append("\\\\");
                            break;
                        case '\b':
                            builder.Append("\\b");
                            break;
                        case '\f':
                            builder.Append("\\f");
                            break;
                        case '\n':
                            builder.Append("\\n");
                            break;
                        case '\r':
                            builder.Append("\\r");
                            break;
                        case '\t':
                            builder.Append("\\t");
                            break;
                        default:
                            if (isUnicode)
                            {
                                int codepoint = Convert.ToInt32(c);
                                if ((codepoint >= 32) && (codepoint <= 126))
                                {
                                    builder.Append(c);
                                }
                                else
                                {
                                    builder.Append("\\u");
                                    builder.Append(codepoint.ToString("x4"));
                                }
                            }
                            else
                            {
                                builder.Append(c);
                            }
                            break;
                    }
                }
                builder.Append('\"');
            }
            void SerializeOther(object value, bool isUnicode)
            {
                // NOTE: decimals lose precision during serialization.
                // They always have, I'm just letting you know.
                // Previously floats and doubles lost precision too.
                if (value is float)
                {
                    builder.Append(((float)value).ToString("R"));
                }
                else if (value is int
                  || value is uint
                  || value is long
                  || value is sbyte
                  || value is byte
                  || value is short
                  || value is ushort
                  || value is ulong)
                {
                    builder.Append(value);
                }
                else if (value is double
                  || value is decimal)
                {
                    builder.Append(Convert.ToDouble(value).ToString("R"));
                }
                else
                {
                    SerializeString(value.ToString(), isUnicode);
                }
            }
            #endregion
            #region  序列化jsondata
            public static string SerializeJd(JsonData obj, bool isUnicode = false)
            {
                var instance = new Serializer();
                instance.SerializeJdValue(obj, isUnicode);
                return instance.builder.ToString();
            }
            void SerializeJdValue(JsonData value, bool isUnicode)
            {
                string dataType = value.Type;
                if (dataType == DataType.STRING)
                {
                    SerializeString(value.ToString(), isUnicode);
                }
                else if (dataType == DataType.BOOL)
                {
                    builder.Append(value.ToString().ToLower());
                }
                else if (dataType == DataType.ARRAY)
                {
                    SerializeJdArray(value, isUnicode);
                }
                else if (dataType == DataType.OBJECT)
                {
                    SerializeJdObject(value, isUnicode);
                }
                else
                {
                    if (value.Content != null)
                    {
                        SerializeOther(value.Content, isUnicode);
                    }
                    else
                    {
                        builder.Append("null");
                    }
                }
            }
            void SerializeJdArray(JsonData jd, bool isUnicode)
            {
                builder.Append('[');
                List<JsonData> list = jd.GetList();
                if (list == null)
                {
                    builder.Append(']');
                    return;
                }
                bool first = true;
                for (int i = 0; i < list.Count; i++)
                {
                    if (!first)
                    {
                        builder.Append(',');
                    }
                    SerializeJdValue(list[i], isUnicode);
                    first = false;
                }
                builder.Append(']');
            }
            void SerializeJdObject(JsonData jd, bool isUnicode)
            {
                builder.Append('{');
                List<string> keys = jd.GetKeys();
                if (keys == null)
                {
                    builder.Append('}');
                    return;
                }
                bool first = true;
                for (int i = 0; i < keys.Count; i++)
                {
                    if (!first)
                    {
                        builder.Append(',');
                    }
                    SerializeString(keys[i], isUnicode);
                    builder.Append(':');
                    JsonData value = jd.Get(keys[i]);
                    if (value != null)
                    {
                        SerializeJdValue(value, isUnicode);
                    }
                    else
                    {
                        builder.Append("null");
                    }
                    first = false;
                }
                builder.Append('}');
            }
            #endregion
        }
        #endregion
#if JSON_SUPPORT_SERIALIZE
        /// <summary>
        /// 把对象转成字符串
        /// Converts a IDictionary / IList object or a simple type (string, int, etc.) into a JSON string
        /// </summary>
        /// <param name="json">A Dictionary<string, object> / List<object></param>
        /// <param name="isUnicode">是否需要unicode编码</param>
        /// <returns>A JSON encoded string, or null if object 'json' is not serializable</returns>
        static public string Serialize(object obj, bool isUnicode = false)
        {
            return Serializer.Serialize(obj, isUnicode);
        }
        /// <summary>
        /// 把对象转成字符串
        /// </summary>
        /// <param name="obj">JsonData</param>
        /// <returns></returns>
        static public string ToJson(JsonData obj, bool isUnicode = false)
        {
            return Serializer.SerializeJd(obj, isUnicode);
        }
        ///// <summary>
        ///// 把对象转成字符串
        ///// </summary>
        ///// <param name="obj">JsonData</param>
        ///// <returns></returns>
        //static public string ToJson(object obj, bool isUnicode = false)
        //{
        //    return Serializer.Serialize(obj, isUnicode);
        //}
#endif // JSON_SUPPORT_SERIALIZE
        /// <summary>
        /// 返回一个系统类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static public Object Deserialize(string str)
        {
            try
            {
                return Parser.Parse(str);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// 把字符串转成对象
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static public JsonData ToObject(string str)
        {
            try
            {
                return Parser.ParseJd(str);
            }
            catch (Exception e)
            {
                return null;
            }
        }
        /// <summary>
        /// 根据类型把字符串转成类的实例对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        static public T ToObject<T>(string str) where T : class
        {
            JsonData jd = ToObject(str);
            if (jd != null)
            {
                Type type = typeof(T);
                return (T)GetT(type, jd);
            }
            else
            {
                return null;
            }

        }

        static public object GetT(Type type, JsonData jd)
        {
            string realType = JsonTools.GetDataType(type);
            if (realType == DataType.OBJECT)
            {
                return GetObjT(type, jd);
            }
            else if (realType == DataType.ARRAY)
            {
                if (type.IsGenericType)
                {
                    object orgList = Activator.CreateInstance(type);
                    if (orgList is IList)
                    {
                        Type listType = type.GetMethod("Find").ReturnType;
                        for (int j = 0; j < jd.Count; j++)
                        {
                            object lo = GetT(listType, jd.Get(j));
                            if (lo != null)
                            {
                                MethodInfo m = orgList.GetType().GetMethod("Add");
                                m.Invoke(orgList, new object[] { lo });
                            }
                        }
                    }
                    else if (orgList is IDictionary)
                    {
                        Type[] listType = type.GetGenericArguments();
                        List<string> keys = jd.GetKeys();
                        for (int j = 0; j < jd.Count; j++)
                        {
                            object lo = GetT(listType[1], jd.Get(j));
                            if (lo != null)
                            {
                                MethodInfo m = orgList.GetType().GetMethod("Add");
                                m.Invoke(orgList, new object[] { keys[j], lo });
                            }
                        }
                    }
                    return orgList;

                }
                else if (type.BaseType == typeof(Array))
                {
                    MethodInfo[] ms = type.GetMethods();
                    Type listType = type.GetElementType();
                    object orgList = Array.CreateInstance(listType, jd.Count);
                    MethodInfo sv = type.GetMethod("SetValue", new Type[2] { typeof(object), typeof(int) });
                    for (int j = 0; j < jd.Count; j++)
                    {
                        object lo = GetT(listType, jd.Get(j));
                        if (lo != null)
                        {
                            sv.Invoke(orgList, new object[] { lo, j });
                        }
                    }

                    return orgList;
                }
            }
            else if (realType == DataType.BOOL)
            {
                bool bv = jd.ToString().ToLower() == "true" ? true : false;
                return bv;
            }
            else if (realType == DataType.STRING)
            {
                return jd.ToString();
            }
            else if (realType == DataType.NUMBER)
            {
                string sv = jd.ToString();
                if (type == typeof(int))
                {
                    return int.Parse(sv);
                }
                else if (type == typeof(float))
                {
                    double dsv = Convert.ToDouble(sv);
                    float fsv = Convert.ToSingle(dsv);
                    return fsv;
                }
                else if (type == typeof(double))
                {
                    return double.Parse(sv);
                }
                else if (type == typeof(Single))
                {
                    return Single.Parse(sv);
                }
                else if (type == typeof(long))
                {
                    return long.Parse(sv);
                }
            }

            return null;

        }

        static public object GetObjT(Type type, JsonData jd)
        {
            FieldInfo[] fields = type.GetFields();
            if (fields.Length == 0)
            {
                return null;
            }
            object objT = (object)Activator.CreateInstance(type);

            for (int i = 0; i < fields.Length; i++)
            {
                if (!fields[i].IsPublic)
                {
                    continue;
                }
                if (fields[i].IsStatic)
                {
                    continue;
                }
                string fieldName = fields[i].Name;
                JsonData subJd = jd.Get(fieldName);
                if (subJd == null)
                {
                    continue;
                }
                Type subType = fields[i].FieldType;
                string realType = JsonTools.GetDataType(subType);
                if (realType == DataType.OBJECT)
                {
                    object subObj = GetT(subType, subJd);
                    fields[i].SetValue(objT, subObj);
                }
                else if (realType == DataType.ARRAY)
                {
                    if (subType.IsGenericType)
                    {
                        object orgList = Activator.CreateInstance(subType);
                        if (orgList is IList)
                        {
                            Type listType = subType.GetMethod("Find").ReturnType;
                            for (int j = 0; j < subJd.Count; j++)
                            {
                                object lo = GetT(listType, subJd.Get(j));
                                if (lo != null)
                                {
                                    MethodInfo m = orgList.GetType().GetMethod("Add");
                                    m.Invoke(orgList, new object[] { lo });
                                }
                            }
                        }
                        else if (orgList is IDictionary)
                        {
                            Type[] listType = subType.GetGenericArguments();
                            List<string> keys = subJd.GetKeys();
                            for (int j = 0; j < keys.Count; j++)
                            {
                                object lo = GetT(listType[1], subJd.Get(keys[j]));
                                if (lo != null)
                                {
                                    MethodInfo m = orgList.GetType().GetMethod("Add");
                                    m.Invoke(orgList, new object[] { keys[j], lo });
                                }
                            }
                        }
                        fields[i].SetValue(objT, orgList);

                    }
                    else if (subType.BaseType == typeof(Array))
                    {
                        MethodInfo[] ms = subType.GetMethods();
                        Type listType = subType.GetElementType();
                        object orgList = Array.CreateInstance(listType, subJd.Count);
                        MethodInfo sv = subType.GetMethod("SetValue", new Type[2] { typeof(object), typeof(int) });
                        for (int j = 0; j < subJd.Count; j++)
                        {
                            object lo = GetT(listType, subJd.Get(j));
                            if (lo != null)
                            {
                                sv.Invoke(orgList, new object[] { lo, j });
                            }
                        }

                        fields[i].SetValue(objT, orgList);
                    }
                }
                else if (realType == DataType.BOOL)
                {
                    bool bv = subJd.ToString().ToLower() == "true" ? true : false;
                    fields[i].SetValue(objT, bv);
                }
                else if (realType == DataType.STRING)
                {
                    fields[i].SetValue(objT, subJd.ToString());
                }
                else if (realType == DataType.NUMBER)
                {
                    string sv = subJd.ToString();
                    if (subType == typeof(int))
                    {
                        fields[i].SetValue(objT, int.Parse(sv));
                    }
                    else if (subType == typeof(float))
                    {
                        double dsv = Convert.ToDouble(sv);
                        float fsv = Convert.ToSingle(dsv);
                        fields[i].SetValue(objT, fsv);
                    }
                    else if (subType == typeof(double))
                    {
                        fields[i].SetValue(objT, double.Parse(sv));
                    }
                    else if (subType == typeof(Single))
                    {
                        fields[i].SetValue(objT, Single.Parse(sv));
                    }
                    else if (subType == typeof(long))
                    {
                        fields[i].SetValue(objT, long.Parse(sv));
                    }
                    else if (subType == typeof(byte))
                    {
                        fields[i].SetValue(objT, byte.Parse(sv));
                    }
                    else if (subType == typeof(short))
                    {
                        fields[i].SetValue(objT, short.Parse(sv));
                    }
                }

            }
            return objT;
        }
    }

