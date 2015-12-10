using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asmodat.Abbreviate
{
    public static class Enums
    {
        public static string GetEnumDescription<T>(this T value)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : null;
        }

        public static string GetEnumName<T>(this T value)
        {
            return Enums.GetName(value);
        }

        public static string GetName<T>(T value)
        {
            string name;
            try
            {
                name = Enum.GetName(typeof(T), value);
            }
            catch
            {
                name = null;
            }

            return name;
        }

        public static int Count<T>()
        {
            return Enum.GetNames(typeof(T)).Length;
        }

        public static List<T> ToList<T>()//(Enum source)
        {
            List<T> LTypes = new List<T>();
            Type enumList = typeof(T);

            if (!enumList.IsEnum) return LTypes;

            foreach (T value in (T[])Enum.GetValues(enumList))
                LTypes.Add(value);

            return LTypes;
        }

        public static T[] ToArray<T>()
        {
            return Enums.ToList<T>().ToArray();
        }

        public static List<string> ToString<T>()//(Enum source)
        {
            List<string> LNames = new List<string>();
            Type enumList = typeof(T);

            if (!enumList.IsEnum) return LNames;

            foreach (T value in (T[])Enum.GetValues(enumList))
                LNames.Add(value.ToString());

            return LNames;
        }

        public static List<string> ToStringDescription<T>()//(Enum source)
        {
            List<string> LNames = new List<string>();
            Type enumList = typeof(T);

            if (!enumList.IsEnum) return LNames;

            foreach (T value in (T[])Enum.GetValues(enumList))
                LNames.Add(Enums.GetEnumDescription(value));

            return LNames;
        }


        /// <summary>
        /// This method allows to converd one enum to another with the same string name (Case does not matter).
        /// Use example: Enums.ToEnum[Archive.TimeFrame, AsmodatForex.com.efxnow.democharting.chartingservice.TimeFrame](Archive.TimeFrame.FIVE_MINUTE);
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TDestination ToEnum<TSource, TDestination>(TSource value) where TSource  : struct, IConvertible where TDestination  : struct, IConvertible
        {
            Type enumType = typeof(TDestination);

            foreach (string s in Enums.ToString<TDestination>())
            {
                if (value.ToString().ToLower() == s.ToLower())
                    return (TDestination)Enum.Parse(enumType, s);

            }

            throw new Exception("ToEnum exception, no such value could be parsed.");
        }

        public static TDestination ToEnumByDescription<TDestination>(string description) where TDestination : struct, IConvertible
        {
            if(description == null)
                throw new Exception("Description cannot be null.");

            foreach (TDestination _enum in Enums.ToArray<TDestination>())
            {
                if (_enum.GetEnumDescription() == description)
                    return _enum;
            }

            throw new Exception("ToEnumByDescription exception, no enum with specied description exists");
        }


        public static TDestination ToEnum<TDestination>(string value)
        {
            Type enumType = typeof(TDestination);

            return (TDestination)Enum.Parse(enumType, value);
        }

        /// <summary>
        /// This method converts two enum sorurce and destination into one dictionary
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <returns></returns>
        public static Dictionary<TSource, TDestination> ToDictionary<TSource, TDestination>()  where TSource : struct, IConvertible where TDestination : struct, IConvertible
        {
            Dictionary<TSource, TDestination> DictionaryMap = new Dictionary<TSource, TDestination>();

            List<TSource> SourceList = Enums.ToList<TSource>();
            List<TDestination> DestinationList = Enums.ToList<TDestination>();

            foreach (TSource key in SourceList)
            {
                TDestination value;
                try
                {
                    value = Enums.ToEnum<TSource, TDestination>(key);
                }
                catch { continue; }

                if (DictionaryMap.ContainsKey(key)) throw new Exception("There Can't be to exact values, Key cannot be created !");
                DictionaryMap.Add(key, value);
            }


            return DictionaryMap;
        }


        /// <summary>
        /// This is custom Equal method, itcompares string values of two objects.
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public static new bool Equals(object obj1, object obj2)
        {
            string str1 = null;
            string str2 = null;

            if (obj1 != null) str1 = obj1.ToString();
            if (obj2 != null) str2 = obj2.ToString();

            return str1 == str2;
        }
    }
}

/*
public Dictionary<AsmodatForex.com.efxnow.democharting.chartingservice.TimeFrame, ServiceConfiguration.TimeFrame> TimeFrames
        {
            get
            {
                if (_TimeFrames.Count <= 0)
                {
                    var ValueFrames = Enums.ToList<ServiceConfiguration.TimeFrame>();
                    var KeyFrames = Enums.ToList<ServiceConfiguration.TimeFrame>();
                }


                return _TimeFrames;
            }

        }
*/