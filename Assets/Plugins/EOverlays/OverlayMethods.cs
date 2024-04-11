#if UNITY_EDITOR
using System.Collections.Generic;
using EOverlays.Editor.Attributes;
using UnityEditor;
using UnityEngine;

namespace EOverlays
{
    public class OverlayMethods
    {
        public static bool f
        {
            get { return true; }
        }

        [EOverlayElement("TEST", enableCondition: nameof(f))]
        public static void Test(List<int> list, int[] array)
        {
        }

        [EOverlayElement("TEST2", enableCondition: nameof(f))]
        public static void Test2(List<int> list, int[] array)
        {
        }

        [EOverlayElement("TEST3", enableCondition: nameof(f))]
        public static void Test3(List<int> list, int[] array)
        {
        }

        [EOverlayElement("TEST4", enableCondition: nameof(f))]
        public static void Test4(List<int> list, int[] array)
        {
        }
    }
}
#endif