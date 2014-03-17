using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using System;
#endif
[ExecuteInEditMode]
public class IndexManager : MonoBehaviour {

    public List<ObjectIndexer> SceneObjects;

    public IndexManager()
    {
        SceneObjects = new List<ObjectIndexer>();
    }
    public ObjectIndexer FindByID(int id)
    {
        ObjectIndexer result = null;
 
        foreach (ObjectIndexer index in SceneObjects)
        {
            if (index.ID == id)
            {
                result = index;
                break;
            }        
        }
        return result;
    }
    public ObjectIndexer FindByGameObject(GameObject obj)
    {
        ObjectIndexer result = null;

        foreach (ObjectIndexer index in SceneObjects)
        {
            if (index.obj.GetInstanceID() == obj.GetInstanceID())
            {
                result = index;
                break;
            }
        }
        return result;
    }
    public ArrayList cachedObjects;


}

public class SelectionBakingParam
{
    public UnityEngine.Object[] objs;

}