using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class AnimatedFrameData{

    public float frame;
    //string directory;
    public DualLightmaps[] lightmaps;
   
    public AnimatedFrameData()
    {
    }
}

[Serializable]
public class DualLightmaps
{
    public Texture2D far;
    public Texture2D near;
}
