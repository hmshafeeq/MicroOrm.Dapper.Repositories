
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;



namespace MicroOrm.Dapper.Repositories.Logger
{
    /// <inheritdoc />
    public static class TinyJsonSerializer
    {
        static readonly Encoding bomlessUtf8 = new UTF8Encoding(false);
        /// <inheritdoc />
        public static string Serialize(object obj)
        {
            using (var ms = new MemoryStream())
            using (var sw = new StreamWriter(ms, bomlessUtf8))
            {
                Serialize(sw, obj);
                sw.Flush();

                return bomlessUtf8.GetString(ms.ToArray());
            }
        }
        /// <inheritdoc />
        public static void Serialize(TextWriter tw, object obj)
        {
            SerializeObject(tw, obj);
        }

        enum JsonType
        {
            @string, number, boolean, @object, array, @null
        }

        static JsonType GetJsonType(object obj)
        {
            if (obj == null) return JsonType.@null;

            switch (Type.GetTypeCode(obj.GetType()))
            {
                case TypeCode.Boolean:
                    return JsonType.boolean;
                case TypeCode.String:
                case TypeCode.Char:
                case TypeCode.DateTime:
                    return JsonType.@string;
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                case TypeCode.SByte:
                case TypeCode.Byte:
                    return JsonType.number;
                case TypeCode.Object:
                    // specialized for wellknown types
                    if (obj is Uri || obj is DateTimeOffset || obj is Guid || obj is StringBuilder)
                    {
                        return JsonType.@string;
                    }
                    else if (obj is IDictionary)
                    {
                        return JsonType.@object;
                    }

                    return (obj is IEnumerable) ? JsonType.array : JsonType.@object;
                case TypeCode.DBNull:
                case TypeCode.Empty:
                default:
                    return JsonType.@null;
            }
        }

        static void SerializeObject(TextWriter tw, object o)
        {
            switch (GetJsonType(o))
            {
                case JsonType.@string:
                    if (o is string)
                    {
                        WriteString(tw, (string)o);
                    }
                    else if (o is DateTime)
                    {
                        var s = ((DateTime)o).ToString("yyyy-MM-dd HH:mm:ss");
                        WriteString(tw, s);
                    }
                    else if (o is DateTimeOffset)
                    {
                        var s = ((DateTimeOffset)o).ToString("yyyy-MM-dd HH:mm:ss");
                        WriteString(tw, s);
                    }
                    else
                    {
                        WriteString(tw, o.ToString());
                    }
                    break;
                case JsonType.number:
                    WriteNumber(tw, o);
                    break;
                case JsonType.boolean:
                    WriteBoolean(tw, (bool)o);
                    break;
                case JsonType.@object:
                    WriteObject(tw, o);
                    break;
                case JsonType.array:
                    WriteArray(tw, (IEnumerable)o);
                    break;
                case JsonType.@null:
                    WriteNull(tw);
                    break;
                default:
                    break;
            }
        }

        static void WriteString(TextWriter tw, string o)
        {
            tw.Write('\"');

            for (int i = 0; i < o.Length; i++)
            {
                var c = o[i];
                switch (c)
                {
                    case '"':
                        tw.Write("\\\"");
                        break;
                    case '\\':
                        tw.Write("\\\\");
                        break;
                    case '\b':
                        tw.Write("\\b");
                        break;
                    case '\f':
                        tw.Write("\\f");
                        break;
                    case '\n':
                        tw.Write("\\n");
                        break;
                    case '\r':
                        tw.Write("\\r");
                        break;
                    case '\t':
                        tw.Write("\\t");
                        break;
                    default:
                        tw.Write(c);
                        break;
                }
            }

            tw.Write('\"');
        }

        static void WriteNumber(TextWriter tw, object o)
        {
            tw.Write(o.ToString());
        }

        static void WriteBoolean(TextWriter tw, bool o)
        {
            tw.Write(o ? "true" : "false");
        }

        static void WriteObject(TextWriter tw, object o)
        {
            tw.Write('{');

            var dict = o as IDictionary;
            if (dict != null)
            {
                // Dictionary
                var isFirst = true;
                foreach (DictionaryEntry item in dict)
                {
                    if (!isFirst) tw.Write(",");
                    else isFirst = false;

                    tw.Write("\r\n\t\"" + item.Key + "\"");
                    tw.Write(":");
                    SerializeObject(tw, item.Value);
                }
            }
            else
            {
                // Object
                var isFirst = true;
                foreach (var item in o.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty))
                {
                    if (!isFirst) tw.Write(",");
                    else isFirst = false;

                    var key = item.Name;
                    var value = item.GetGetMethod().Invoke(o, null); // safe reflection for unity
                    tw.Write("\r\n\t\"" + key + "\"");
                    tw.Write(":");
                    SerializeObject(tw, value);
                }

                isFirst = true;
                foreach (var item in o.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField))
                {
                    if (!isFirst) tw.Write(",");
                    else isFirst = false;

                    var key = item.Name;
                    var value = item.GetValue(o);
                    tw.Write("\r\n\t\"" + key + "\"");
                    tw.Write(":");
                    SerializeObject(tw, value);
                }
            }

            tw.Write("\r\n}");
        }

        static void WriteArray(TextWriter tw, IEnumerable o)
        {
            tw.Write("[");
            var isFirst = true;
            foreach (var item in o)
            {
                if (!isFirst) tw.Write(",");
                else isFirst = false;

                SerializeObject(tw, item);
            }
            tw.Write("]");
        }

        static void WriteNull(TextWriter tw)
        {
            tw.Write("null");
        }
    }
}
