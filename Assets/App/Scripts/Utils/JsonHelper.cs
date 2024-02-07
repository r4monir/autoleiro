using System;
using System.Collections.Generic;
using UnityEngine;

public static class JsonHelper
{
    [Serializable]
    private class Wrapper<T>
    {
        public T[] items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    public static string ToJson<T>(List<T> list)
    {
        string dataAsString = "[";
        dataAsString += System.Environment.NewLine;
        for (int i = 0; i < list.Count; i++)
        {
            dataAsString += JsonUtility.ToJson(list[i]);

            // Adds a ',' unless it is the last object
            if (i < (list.Count - 1))
            {
                dataAsString += ",";
            }
            dataAsString += System.Environment.NewLine;
        }
        dataAsString += "]";
        return dataAsString;
    }

    public static T[] FromJson<T>(string jsonArray)
    {
        jsonArray = WrapArray(jsonArray);
        return FromJsonWrapped<T>(jsonArray);
    }

    public static T[] FromJsonWrapped<T>(string jsonObject)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(jsonObject);
        return wrapper.items;
    }

    public static List<T> FromJsonToList<T>(string jsonList)
    {
        List<T> dataAsObjectList = new List<T>();

        if (jsonList != "")
        {
            Wrapper<T> objectInsideJsonString;
            jsonList = "{ \"items\": " + jsonList + "}";
            objectInsideJsonString = JsonUtility.FromJson<Wrapper<T>>(jsonList);
            dataAsObjectList = new List<T>(objectInsideJsonString.items);
        }

        return dataAsObjectList;
    }

    private static string WrapArray(string jsonArray)
    {
        return "{ \"items\": " + jsonArray + "}";
    }

}
