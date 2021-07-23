using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;


namespace Helpers
{
    public class Enums
    {

        public static bool IsEnumInEnumItem<T1, T2>(T2 lT2Item)
        {
            int lT2Number = EnumToInt<T2>(lT2Item);

            T1[] lEnums = (T1[])System.Enum.GetValues(typeof(T1));
            foreach (T1 lT1Item in lEnums)
            {
                int lT1Number = EnumToInt<T1>(lT1Item);

                if (lT1Number.Equals(lT2Number))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Enums to int.
        /// </summary>
        public static int EnumToInt<T>(T lEnum)
        {
            System.Enum lEnumTest = System.Enum.Parse(typeof(T), lEnum.ToString()) as System.Enum;
            return Convert.ToInt32(lEnumTest);
        }

        /// <summary>
        /// Ints to enum.
        /// </summary>
        public static T IntToEnum<T>(int lInt, out bool lValid)
        {
            if (System.Enum.IsDefined(typeof(T), lInt) == true)
            {
                lValid = true;
                return (T)System.Enum.Parse(typeof(T), lInt.ToString(), true);
            }
            T[] lTemp = (T[])System.Enum.GetValues(typeof(T));
            lValid = false;
            return lTemp[0];
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool CompareEnumList<T>(List<T> lList1, List<T> lList2)
        {
            if (lList1.Count != lList2.Count)
            {
                return false;
            }

            for (int i = 0; i < lList1.Count; i++)
            {
                if (lList1[i].Equals(lList2[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static T ClampIncrement<T>(T lIndex) where T : struct, IConvertible
        {
            int lEnumInt = lIndex.ToInt32(CultureInfo.InvariantCulture.NumberFormat);
            lEnumInt++;
            int lEnumCount = Enum.GetNames(typeof(T)).Length;
            if (lEnumInt > lEnumCount - 1)
            {
                lEnumInt = lEnumCount - 1;
            }
            return (T)(object)lEnumInt;
        }

        public static T ClampDecrement<T>(T lIndex) where T : struct, IConvertible
        {
            int lEnumInt = lIndex.ToInt32(CultureInfo.InvariantCulture.NumberFormat);
            lEnumInt--;
            int lEnumCount = Enum.GetNames(typeof(T)).Length;
            if (lEnumInt < 0)
            {
                lEnumInt = 0;
            }
            return (T)(object)lEnumInt;
        }

        public static T LoopIncrement<T>(T lIndex) where T : struct, IConvertible
        {
            int lEnumInt = lIndex.ToInt32(CultureInfo.InvariantCulture.NumberFormat);
            lEnumInt++;
            int lEnumCount = Enum.GetNames(typeof(T)).Length;
            if (lEnumInt > lEnumCount - 1)
            {
                lEnumInt = 0;
            }
            return (T)(object)lEnumInt;
        }

        public static T LoopDecrement<T>(T lIndex) where T : struct, IConvertible
        {
            int lEnumInt = lIndex.ToInt32(CultureInfo.InvariantCulture.NumberFormat);
            lEnumInt--;
            int lEnumCount = Enum.GetNames(typeof(T)).Length;
            if (lEnumInt < 0)
            {
                lEnumInt = lEnumCount - 1;
            }
            return (T)(object)lEnumInt;
        }

    }
}