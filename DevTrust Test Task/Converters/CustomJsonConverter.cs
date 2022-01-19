using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace DevTrust_Test_Task.Converters
{
    public static class CustomJsonConverter
    {
        static bool IsPrimitiveType(PropertyInfo property)
        {
            var propertyType = property.PropertyType;
            return propertyType.IsPrimitive || propertyType.IsValueType || (propertyType == typeof(string));
        }

        #region Serializer

        private static string ConvertPropertyToString<T>(PropertyInfo property, T obj)
        {
            string result = "\t" + property.Name + ": ";

            if (IsPrimitiveType(property))
            {
                result += "'" + property.GetValue(obj) + "',\n";
            }
            else
            {
                dynamic propertyType = property.PropertyType;
                var subObj = Convert.ChangeType(property.GetValue(obj), propertyType);

                result += Serialize(subObj);
            }

            return result;
        }

        public static string Serialize<T>(T obj)
        {
            string result = "{\n";
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                result += ConvertPropertyToString(property, obj);
            }

            return result + "}";
        }

        public static string Serialize<T>(IEnumerable<T> objList)
        {
            string result = "{\n";

            foreach (var item in objList)
            {
                result += Serialize(item) + "\n";
            }

            return result + "\n}";
        }

        #endregion

        #region Deserializer

        public static void ConvertDictionaryToObject<T>(Dictionary<string, object> dictionary, T obj)
        {
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var dictionaryValue = dictionary.GetValueOrDefault(property.Name);
                if (IsPrimitiveType(property))
                    property.SetValue(obj, dictionaryValue);
                else
                {
                    dynamic propertyType = property.PropertyType;
                    property.SetValue(obj, Activator.CreateInstance(propertyType));
                    var subObj = Convert.ChangeType(property.GetValue(obj), propertyType);

                    var subDictionaryValue = dictionaryValue as Dictionary<string, object>;

                    ConvertDictionaryToObject(subDictionaryValue, subObj);
                }
            }
        }

        public static T Deserialize<T>(string json) where T: new()
        {
            var dictionary = ConvertJsonToDictionary(json);
            var obj = new T();

            ConvertDictionaryToObject(dictionary, obj);

            return obj;
        }

        public static Dictionary<string, object> ConvertJsonToDictionary(string json)
        {
            int end;
            return ConvertJsonToDictionary(json, 0, out end);
        }
        private static Dictionary<string, object> ConvertJsonToDictionary(string json, int start, out int end)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            bool escbegin = false;
            bool escend = false;
            bool inquotes = false;
            string key = null;
            int cend;
            StringBuilder sb = new StringBuilder();
            Dictionary<string, object> child = null;
            List<object> arraylist = null;
            Regex regex = new Regex(@"\\u([0-9a-z]{4})", RegexOptions.IgnoreCase);
            int autoKey = 0;
            for (int i = start; i < json.Length; i++)
            {
                char c = json[i];
                if (c == '\\') escbegin = !escbegin;
                if (!escbegin)
                {
                    if (c == '\'')
                    {
                        inquotes = !inquotes;
                        if (!inquotes && arraylist != null)
                        {
                            arraylist.Add(DecodeString(regex, sb.ToString()));
                            sb.Length = 0;
                        }
                        continue;
                    }
                    if (!inquotes)
                    {
                        switch (c)
                        {
                            case '{':
                                if (i != start)
                                {
                                    child = ConvertJsonToDictionary(json, i, out cend);
                                    if (arraylist != null) arraylist.Add(child);
                                    else
                                    {
                                        dict.Add(key, child);
                                        key = null;
                                    }
                                    i = cend;
                                }
                                continue;
                            case '}':
                                end = i;
                                if (key != null)
                                {
                                    if (arraylist != null) dict.Add(key, arraylist);
                                    else dict.Add(key, DecodeString(regex, sb.ToString()).Trim());
                                }
                                return dict;
                            case '[':
                                arraylist = new List<object>();
                                continue;
                            case ']':
                                if (key == null)
                                {
                                    key = "array" + autoKey.ToString();
                                    autoKey++;
                                }
                                if (arraylist != null && sb.Length > 0)
                                {
                                    arraylist.Add(sb.ToString());
                                    sb.Length = 0;
                                }
                                dict.Add(key, arraylist);
                                arraylist = null;
                                key = null;
                                continue;
                            case ',':
                                if (arraylist == null && key != null)
                                {
                                    dict.Add(key, DecodeString(regex, sb.ToString()).Trim());
                                    key = null;
                                    sb.Length = 0;
                                }
                                if (arraylist != null && sb.Length > 0)
                                {
                                    arraylist.Add(sb.ToString());
                                    sb.Length = 0;
                                }
                                continue;
                            case ':':
                                key = DecodeString(regex, sb.ToString()).Trim();
                                sb.Length = 0;
                                continue;
                        }
                    }
                }
                sb.Append(c);
                if (escend) escbegin = false;
                if (escbegin) escend = true;
                else escend = false;
            }
            end = json.Length - 1;
            return dict; //theoretically shouldn't ever get here
        }

        private static string DecodeString(Regex regex, string str)
        {
            return Regex.Unescape(regex.Replace(str, match => char.ConvertFromUtf32(Int32.Parse(match.Groups[1].Value, System.Globalization.NumberStyles.HexNumber))));
        }

        #endregion
    }
}
