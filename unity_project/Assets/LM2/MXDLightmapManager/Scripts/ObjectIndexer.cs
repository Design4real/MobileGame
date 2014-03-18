using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class ObjectIndexer {

    public int ID;
    public GameObject obj;
    
    public ObjectIndexer()
    {
    }
    public ObjectIndexer(int id, GameObject o)
    {
        ID = id;
        obj = o;
    }
}
