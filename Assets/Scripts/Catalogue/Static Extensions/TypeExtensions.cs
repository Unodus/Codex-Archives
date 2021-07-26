using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class TypeUtilities
{
    /*
       * this gets all the public varibles in the class ie
       * get all the const strings from a class
       * this is handy to validate the string themseves 
       */
    public static List<T> GetAllPublicConstantValues<T>(this Type type)
    {
           return type
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(T))
            .Select(x => (T)x.GetRawConstantValue())
            .ToList();
    }
}