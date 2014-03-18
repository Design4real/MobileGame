using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightmappedObjectManager : MonoBehaviour
{
    public List<LightmappedObjectPrefabs> LightmappedObjects;

    public LightmappedObjectPrefabs FindByGameObject(GameObject obj)
    {
        LightmappedObjectPrefabs result = null;

        foreach (LightmappedObjectPrefabs index in LightmappedObjects)
        {
            if (index.obj.GetInstanceID() == obj.GetInstanceID())
            {
                result = index;
                break;
            }
        }
        return result;
    }
}
