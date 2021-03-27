using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsNameModel
{

    public static Names[] GetNames()
    {
        TextAsset json = Resources.Load<TextAsset>(@"Credits\credits");
        Wrapper<Names> names = JsonUtility.FromJson<Wrapper<Names>>(json.text);
        return names.array; 
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }

    [Serializable]
    public class Names
    {
        public string title;
        public string name;
    }

    public Names[] NameArray;
}
