using System.Collections.Generic;
using UnityEngine;
using System;

public class Silo : MonoBehaviour
{
    public int id;
    public SiloContents silo;


    [Serializable]
    public class SiloContents
    {
        public int id;
        public Dictionary<int, int> items;
    }

    void Start()
    {
        silo = new SiloContents
        {
            id = id,
            items = new Dictionary<int, int>()
        };
    }
}