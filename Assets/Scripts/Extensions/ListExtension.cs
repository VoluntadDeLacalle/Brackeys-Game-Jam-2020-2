using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ListExtensions
{
    public static class ListExtension
    {
        public static T pop_back<T>(this List<T> list)
        {
            T r = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            return r;
        }

        public static void pop_front<T>(this List<T> list)
        {
            list.RemoveAt(0);
        }
    }
}