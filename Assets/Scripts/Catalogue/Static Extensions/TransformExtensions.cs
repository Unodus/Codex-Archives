using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions 
{
  
        //Breadth-first search
        public static Transform FindDeepChild(this Transform aParent, string aName)
        {
            //if you search for "" it will return the object your searching in, so check that string is empty and return nothing
            if (true == string.IsNullOrEmpty(aName))
            {
                return null; //this is intended
            }

            var result = aParent.Find(aName);
            if (result != null)
                return result;
            foreach (Transform child in aParent)
            {
                result = child.FindDeepChild(aName);
                if (result != null)
                    return result;
            }
            return null;
        }

        /*
        //Depth-first search
        public static Transform FindDeepChild(this Transform aParent, string aName)
        {
            foreach(Transform child in aParent)
            {
                if(child.name == aName )
                    return child;
                var result = child.FindDeepChild(aName);
                if (result != null)
                    return result;
            }
            return null;
        }
        */
    
}
