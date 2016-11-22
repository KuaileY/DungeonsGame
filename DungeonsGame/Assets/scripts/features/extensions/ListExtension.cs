
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

    public static int Find(this List<int> list, int key)
    {
        if (list.Count == 0 || key > list[list.Count - 1] || key < 1)
            return -1;
        int low = 0;
        int high = list.Count;
        int mid = 0;

        while (low <= high)
        {
            mid = (low + high) / 2;
            if (list[mid] == key)
                return mid;
            else if (list[mid] < key)
                low = mid + 1;
            else
                high = mid - 1;
        }
        return low;
    }

}

