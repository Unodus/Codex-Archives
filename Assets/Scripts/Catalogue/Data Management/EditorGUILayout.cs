#if UNITY_EDITOR
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;


using UnityEditor;

namespace Helpers
{

    public class EditorGUILayout
    {
        
        public class EditorGUILayoutInfo
        {
            public int NumberOfSpaces { get; private set; }
            public int SpaceAtBegining { get; private set; }
            public int NameWidth { get; private set; }
            public int InformationWidth { get; private set; }
            public int Height { get; private set; }

            public EditorGUILayoutInfo(int lNumberOfSpaces, int lSpaceAtBegining, int lNameWidth, int lInformationWidth, int lHeight)
            {
                NumberOfSpaces = lNumberOfSpaces;
                SpaceAtBegining = lSpaceAtBegining;
                NameWidth = lNameWidth;
                InformationWidth = lInformationWidth;
                Height = lHeight;
            }
        }

        #region EditorGUILayout

        public static void ColorField(string lName, ref Color lColourVariable, EditorGUILayoutInfo lEditorGUILayoutInfo, bool lUseBeginHorizontal = true)
        {
            Space(lEditorGUILayoutInfo.NumberOfSpaces);
            if (lUseBeginHorizontal == true) UnityEditor.EditorGUILayout.BeginHorizontal();
            UnityEditor.EditorGUILayout.LabelField("", GUILayout.Width(lEditorGUILayoutInfo.SpaceAtBegining), GUILayout.Height(lEditorGUILayoutInfo.Height));
            UnityEditor.EditorGUILayout.LabelField(lName, GUILayout.Width(lEditorGUILayoutInfo.NameWidth), GUILayout.Height(lEditorGUILayoutInfo.Height));
            lColourVariable = UnityEditor.EditorGUILayout.ColorField("", lColourVariable, GUILayout.Width(lEditorGUILayoutInfo.InformationWidth), GUILayout.Height(lEditorGUILayoutInfo.Height));
            if (lUseBeginHorizontal == true) UnityEditor.EditorGUILayout.EndHorizontal();
        }
        public static void IntSlider(string lName, ref int lIntVariable, int lMin, int lMax, EditorGUILayoutInfo lEditorGUILayoutInfo, bool lUseBeginHorizontal = true)
        {
            Space(lEditorGUILayoutInfo.NumberOfSpaces);
            if (lUseBeginHorizontal == true) UnityEditor.EditorGUILayout.BeginHorizontal();
            UnityEditor.EditorGUILayout.LabelField("", GUILayout.Width(lEditorGUILayoutInfo.SpaceAtBegining), GUILayout.Height(lEditorGUILayoutInfo.Height));
            UnityEditor.EditorGUILayout.LabelField(lName, GUILayout.Width(lEditorGUILayoutInfo.NameWidth), GUILayout.Height(lEditorGUILayoutInfo.Height));
            lIntVariable = UnityEditor.EditorGUILayout.IntSlider("", lIntVariable, lMin, lMax, GUILayout.Width(lEditorGUILayoutInfo.InformationWidth), GUILayout.Height(lEditorGUILayoutInfo.Height));
            if (lUseBeginHorizontal == true) UnityEditor.EditorGUILayout.EndHorizontal();
        }
        public static void LabelField(string lName, EditorGUILayoutInfo lEditorGUILayoutInfo, bool lUseBeginHorizontal = true)
        {
            Space(lEditorGUILayoutInfo.NumberOfSpaces);
            if (lUseBeginHorizontal == true) UnityEditor.EditorGUILayout.BeginHorizontal();
            UnityEditor.EditorGUILayout.LabelField("", GUILayout.Width(lEditorGUILayoutInfo.SpaceAtBegining), GUILayout.Height(lEditorGUILayoutInfo.Height));
            UnityEditor.EditorGUILayout.LabelField(lName, GUILayout.Width(lEditorGUILayoutInfo.NameWidth), GUILayout.Height(lEditorGUILayoutInfo.Height));
            if (lUseBeginHorizontal == true) UnityEditor.EditorGUILayout.EndHorizontal();
        }
        public static string TextField(string lText, EditorGUILayoutInfo lEditorGUILayoutInfo, bool lUseBeginHorizontal = true)
        {

            Space(lEditorGUILayoutInfo.NumberOfSpaces);
            if (lUseBeginHorizontal == true) UnityEditor.EditorGUILayout.BeginHorizontal();
            UnityEditor.EditorGUILayout.LabelField("", GUILayout.Width(lEditorGUILayoutInfo.SpaceAtBegining), GUILayout.Height(lEditorGUILayoutInfo.Height));
            lText = UnityEditor.EditorGUILayout.TextField(lText, GUILayout.Width(lEditorGUILayoutInfo.NameWidth), GUILayout.Height(lEditorGUILayoutInfo.Height));
            if (lUseBeginHorizontal == true) UnityEditor.EditorGUILayout.EndHorizontal();
            return lText;
        }
        public static bool Button(string lText, EditorGUILayoutInfo lEditorGUILayoutInfo, bool lUseBeginHorizontal = true)
        {

            Space(lEditorGUILayoutInfo.NumberOfSpaces);
            if (lUseBeginHorizontal == true) UnityEditor.EditorGUILayout.BeginHorizontal();
            UnityEditor.EditorGUILayout.LabelField("", GUILayout.Width(lEditorGUILayoutInfo.SpaceAtBegining), GUILayout.Height(lEditorGUILayoutInfo.Height));
            bool lBool = GUILayout.Button(lText, GUILayout.Width(lEditorGUILayoutInfo.NameWidth), GUILayout.Height(lEditorGUILayoutInfo.Height));
            if (lUseBeginHorizontal == true) UnityEditor.EditorGUILayout.EndHorizontal();
            return lBool;
        }
        public static string DelayedTextField(string lName, string lTextToChange, EditorGUILayoutInfo lEditorGUILayoutInfo, bool lUseBeginHorizontal = true)
        {

            Space(lEditorGUILayoutInfo.NumberOfSpaces);
            if (lUseBeginHorizontal == true) UnityEditor.EditorGUILayout.BeginHorizontal();
            UnityEditor.EditorGUILayout.LabelField("", GUILayout.Width(lEditorGUILayoutInfo.SpaceAtBegining), GUILayout.Height(lEditorGUILayoutInfo.Height));
            UnityEditor.EditorGUILayout.LabelField(lName, GUILayout.Width(lEditorGUILayoutInfo.NameWidth), GUILayout.Height(lEditorGUILayoutInfo.Height));
            lTextToChange = UnityEditor.EditorGUILayout.TextField(lTextToChange, GUILayout.Width(lEditorGUILayoutInfo.InformationWidth), GUILayout.Height(lEditorGUILayoutInfo.Height)); //HACK should be  DelayTextField
            if (lUseBeginHorizontal == true) UnityEditor.EditorGUILayout.EndHorizontal();
            return lTextToChange;
        }
        public static int PopupEnum(string lName, string[] lList, int lIndex, EditorGUILayoutInfo lEditorGUILayoutInfo, bool lUseBeginHorizontal = true)
        {

            Space(lEditorGUILayoutInfo.NumberOfSpaces);
            if (lUseBeginHorizontal == true) UnityEditor.EditorGUILayout.BeginHorizontal();
            UnityEditor.EditorGUILayout.LabelField("", GUILayout.Width(lEditorGUILayoutInfo.SpaceAtBegining), GUILayout.Height(lEditorGUILayoutInfo.Height));
            UnityEditor.EditorGUILayout.LabelField(lName, GUILayout.Width(lEditorGUILayoutInfo.NameWidth), GUILayout.Height(lEditorGUILayoutInfo.Height));
            lIndex = UnityEditor.EditorGUILayout.Popup(lIndex, lList, GUILayout.Width(lEditorGUILayoutInfo.InformationWidth), GUILayout.Height(lEditorGUILayoutInfo.Height));
            if (lUseBeginHorizontal == true) UnityEditor.EditorGUILayout.EndHorizontal();
            return lIndex;
        }
        public static void Popup(string lName, ref List<string> lList, ref int lIndex, EditorGUILayoutInfo lEditorGUILayoutInfo, bool lUseBeginHorizontal = true)
        {

            Space(lEditorGUILayoutInfo.NumberOfSpaces);
            UnityEditor.EditorGUILayout.BeginHorizontal();
            UnityEditor.EditorGUILayout.LabelField("", GUILayout.Width(lEditorGUILayoutInfo.SpaceAtBegining), GUILayout.Height(lEditorGUILayoutInfo.Height));
            UnityEditor.EditorGUILayout.LabelField(lName, GUILayout.Width(lEditorGUILayoutInfo.NameWidth), GUILayout.Height(lEditorGUILayoutInfo.Height));
            lIndex = UnityEditor.EditorGUILayout.Popup(lIndex, lList.ToArray<string>(), GUILayout.Width(lEditorGUILayoutInfo.InformationWidth), GUILayout.Height(lEditorGUILayoutInfo.Height));
            if (lUseBeginHorizontal == true) UnityEditor.EditorGUILayout.EndHorizontal();
        }
        public static void Toggle(string lName, ref bool lBoolVariable, EditorGUILayoutInfo lEditorGUILayoutInfo, bool lUseBeginHorizontal = true)
        {

            Space(lEditorGUILayoutInfo.NumberOfSpaces);
            if (lUseBeginHorizontal == true) UnityEditor.EditorGUILayout.BeginHorizontal();
            UnityEditor.EditorGUILayout.LabelField("", GUILayout.Width(lEditorGUILayoutInfo.SpaceAtBegining), GUILayout.Height(lEditorGUILayoutInfo.Height));
            UnityEditor.EditorGUILayout.LabelField(lName, GUILayout.Width(lEditorGUILayoutInfo.NameWidth), GUILayout.Height(lEditorGUILayoutInfo.Height));
            lBoolVariable = UnityEditor.EditorGUILayout.Toggle("", lBoolVariable, GUILayout.Width(lEditorGUILayoutInfo.InformationWidth), GUILayout.Height(lEditorGUILayoutInfo.Height));
            if (lUseBeginHorizontal == true) UnityEditor.EditorGUILayout.EndHorizontal();
        }
        public static void LabelFieldBlank(int lWidth)
        {
            UnityEditor.EditorGUILayout.LabelField("", GUILayout.Width(lWidth));
        }
        public static void Space(int lSpaceNumber)
        {
            for (int i = 0; i < lSpaceNumber; i++)
            {
                UnityEditor.EditorGUILayout.Space();
            }
        }
        public static List<TEnum> EnumList<TEnum>(string lName, ref int lFlag, EditorGUILayoutInfo lEditorGUILayoutInfo, bool lUseBeginHorizontal = true)
        {
            string[] lStrings = Enum.GetNames(typeof(TEnum));
            TEnum[] lEnums = (TEnum[])Enum.GetValues(typeof(TEnum));
            /// <param name="lEditorGUILayoutInfo"></param>
            Space(lEditorGUILayoutInfo.NumberOfSpaces);
            if (lUseBeginHorizontal == true) UnityEditor.EditorGUILayout.BeginHorizontal();
            UnityEditor.EditorGUILayout.LabelField("", GUILayout.Width(lEditorGUILayoutInfo.SpaceAtBegining), GUILayout.Height(lEditorGUILayoutInfo.Height));
            UnityEditor.EditorGUILayout.LabelField(lName, GUILayout.Width(lEditorGUILayoutInfo.NameWidth), GUILayout.Height(lEditorGUILayoutInfo.Height));
            lFlag = UnityEditor.EditorGUILayout.MaskField("", lFlag, lStrings, GUILayout.Width(lEditorGUILayoutInfo.InformationWidth), GUILayout.Height(lEditorGUILayoutInfo.Height));
            if (lUseBeginHorizontal == true) UnityEditor.EditorGUILayout.EndHorizontal();

            List<TEnum> lSelectedOptions = new List<TEnum>();
            for (int i = 0; i < lStrings.Length; i++)
            {
                if ((lFlag & (1 << i)) == (1 << i)) lSelectedOptions.Add(lEnums[i]);
            }
            return lSelectedOptions;
        }
        public static Dictionary<TEnum, bool> EnumDictionary<TEnum>(string lName, ref int lFlag, EditorGUILayoutInfo lEditorGUILayoutInfo, bool lUseBeginHorizontal = true)
        {
            string[] lStrings = Enum.GetNames(typeof(TEnum));
            TEnum[] lEnums = (TEnum[])Enum.GetValues(typeof(TEnum));

            Space(lEditorGUILayoutInfo.NumberOfSpaces);
            if (lUseBeginHorizontal == true) UnityEditor.EditorGUILayout.BeginHorizontal();
            UnityEditor.EditorGUILayout.LabelField("", GUILayout.Width(lEditorGUILayoutInfo.SpaceAtBegining), GUILayout.Height(lEditorGUILayoutInfo.Height));
            UnityEditor.EditorGUILayout.LabelField(lName, GUILayout.Width(lEditorGUILayoutInfo.NameWidth), GUILayout.Height(lEditorGUILayoutInfo.Height));
            lFlag = UnityEditor.EditorGUILayout.MaskField("", lFlag, lStrings, GUILayout.Width(lEditorGUILayoutInfo.InformationWidth), GUILayout.Height(lEditorGUILayoutInfo.Height));
            if (lUseBeginHorizontal == true) UnityEditor.EditorGUILayout.EndHorizontal();

            Dictionary<TEnum, bool> lSelectedOptions = new Dictionary<TEnum, bool>();
            for (int i = 0; i < lStrings.Length; i++)
            {
                bool lBool = ((lFlag & (1 << i)) == (1 << i));
                lSelectedOptions.Add(lEnums[i], lBool);
            }
            return lSelectedOptions;
        }
        public static Dictionary<TEnum, bool> EnumDictionaryExcept<TEnum, TExcept>(string lName, ref int lFlag, EditorGUILayoutInfo lEditorGUILayoutInfo, bool lUseBeginHorizontal = true)
        {
            /// <summary>
            /// Enums the dictionary.
            /// </summary>

            /// ---------------------------------------------------------------
            /// 		long way of creating a list 
            /// ---------------------------------------------------------------
            TEnum[] lEnums = (TEnum[])Enum.GetValues(typeof(TEnum));
            TExcept[] lExcept = (TExcept[])Enum.GetValues(typeof(TExcept));

            List<String> lList = new List<string>();
            for (int i = 0; i < lEnums.Length; i++)
            {
                bool lValid = true;
                for (int j = 0; j < lExcept.Length; j++)
                {
                    if (lEnums[i].ToString() == lExcept[j].ToString())
                    {
                        lValid = false;
                    }
                }

                if (lValid == true)
                {
                    lList.Add(lEnums[i].ToString());
                }
            }

            string[] lStrings = lList.ToArray();


            Space(lEditorGUILayoutInfo.NumberOfSpaces);
            if (lUseBeginHorizontal == true) UnityEditor.EditorGUILayout.BeginHorizontal();
            UnityEditor.EditorGUILayout.LabelField("", GUILayout.Width(lEditorGUILayoutInfo.SpaceAtBegining), GUILayout.Height(lEditorGUILayoutInfo.Height));
            UnityEditor.EditorGUILayout.LabelField(lName, GUILayout.Width(lEditorGUILayoutInfo.NameWidth), GUILayout.Height(lEditorGUILayoutInfo.Height));
            lFlag = UnityEditor.EditorGUILayout.MaskField("", lFlag, lStrings, GUILayout.Width(lEditorGUILayoutInfo.InformationWidth), GUILayout.Height(lEditorGUILayoutInfo.Height));
            if (lUseBeginHorizontal == true) UnityEditor.EditorGUILayout.EndHorizontal();


            // fill all enums with false     
            Dictionary<TEnum, bool> lSelectedOptions = new Dictionary<TEnum, bool>();

            foreach (TEnum lEnum in lEnums)
            {
                lSelectedOptions.Add(lEnum, false);
            }


            // look through all the strings and make the correct change
            for (int i = 0; i < lStrings.Length; i++)
            {
                TEnum lTemp = (TEnum)Enum.Parse(typeof(TEnum), lStrings[i]);
                bool lBool = ((lFlag & (1 << i)) == (1 << i));
                lSelectedOptions[lTemp] = lBool;
            }
            return lSelectedOptions;
        }
        
        public static int ButtonSpace(int lSpaceAtBeginng, int WidthOfButton, int NumberOfButtons, float lScreenWidth, bool lHasScrollBar)
        {
            return ButtonSpace(lSpaceAtBeginng, WidthOfButton, NumberOfButtons, (int)lScreenWidth, lHasScrollBar);
        }
        public static int ButtonSpace(int lSpaceAtBeginng, int WidthOfButton, int NumberOfButtons, int lScreenWidth, bool lHasScrollBar)
        {
            if (NumberOfButtons != 1)
            {
                int lScrollBarWidth = lHasScrollBar ? 35 : 0;
                int lTotalUsedSpace = (lSpaceAtBeginng * 2) + (WidthOfButton * NumberOfButtons) + lScrollBarWidth;
                int lSpaceLeft = lScreenWidth - lTotalUsedSpace;
                int lNumberOfSpaces = NumberOfButtons - 1;
                return lSpaceLeft / lNumberOfSpaces;
            }
            else
            {
                int lScrollBarWidth = lHasScrollBar ? 17 : 0;
                int lTotalUsedSpace = (lSpaceAtBeginng * 0) + (WidthOfButton * NumberOfButtons) + lScrollBarWidth;
                int lSpaceLeft = lScreenWidth - lTotalUsedSpace;
                return lSpaceLeft / 2;
            }


        }
        public static int ButtonSpaceCentre(int WidthOfButton, float lScreenWidth, bool lHasScrollBar)
        {
            return ButtonSpaceCentre((int)WidthOfButton, (int)lScreenWidth, lHasScrollBar);
        }
        public static int ButtonSpaceCentre(int WidthOfButton, int lScreenWidth, bool lHasScrollBar)
        {
            int lScrollBarWidth = lHasScrollBar ? 35 : 0;
            int lTotalUsedSpace = WidthOfButton + lScrollBarWidth;
            int lSpaceLeft = lScreenWidth - lTotalUsedSpace;
            int lNumberOfSpaces = 2;
            return lSpaceLeft / lNumberOfSpaces;
        }
        public static void LineRepeatField(string lString, GUIStyle lStyle)
        {
            int lRepeatTimes = (int)(300 / lString.Length);

            string lLabel = "";
            for (int i = 0; i < lRepeatTimes; i++)
            {
                lLabel += lString;
            }

            UnityEditor.EditorGUILayout.LabelField(lLabel, lStyle);
        }
        public static void FoldOutStyleWorkAround(FontStyle lSetStyle)
        {
            GUIStyle style = EditorStyles.foldout;
            style.fontStyle = lSetStyle;
        }
        public static bool CentreButton(string lText, int lWidth)
        {
            bool lReturn = false;
            float lBlank = (EditorGUIUtility.currentViewWidth / 2) - (lWidth / 2);
            GUILayout.BeginHorizontal();
            GUILayout.Label("", GUILayout.Width(lBlank));
            lReturn = GUILayout.Button(lText, GUILayout.Width(lWidth));
            GUILayout.EndHorizontal();

            return lReturn;
        }

        #endregion
    }


}
#endif

