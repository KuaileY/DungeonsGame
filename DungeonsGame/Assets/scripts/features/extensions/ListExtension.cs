
using System;
using System.Collections.Generic;

public static class ListExtension
{
    public static List<T> RandomSortList<T>(this List<T> ListT)
    {
        Random random = new Random();
        List<T> newList = new List<T>();
        foreach (T item in ListT)
        {
            newList.Insert(random.Next(newList.Count + 1), item);
        }
        return newList;
    }
}

