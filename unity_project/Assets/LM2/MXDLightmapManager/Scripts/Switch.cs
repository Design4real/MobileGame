using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
[RequireComponent(typeof(Renderer))]
public class Switch : MonoBehaviour {

    public enum TextureSwitchOptions { Index, Range, All };
    
    public delegate void SwitchTexture();

    public event SwitchTexture SwitchNow;
    public Texture2D[] SwitchTexturesFar;
    public Texture2D[] SwitchTexturesNear;
    [HideInInspector]
    public Brightness.AdjustLightmap options;
    [HideInInspector]
    public Brightness.ExecuteONLite executeTrigger;
    [HideInInspector]
    public TextureSwitchOptions switchOptions;
    [HideInInspector]
    public int startIndexFar;
    [HideInInspector]
    public int endIndexFar;
    [HideInInspector]
    public int startIndexNear;
    [HideInInspector]
    public int endIndexNear;
    [HideInInspector]
    public int timestamp;
    private Texture2D originalFar;
    private Texture2D originalNear;
    //private bool IsExecuting = false;
    // Use this for initialization
	void Start () {
        if (executeTrigger == Brightness.ExecuteONLite.Start)
        {
           AdjustSwitching(startIndexFar,startIndexNear);
        }
        else if (executeTrigger == Brightness.ExecuteONLite.Trigger)
        {
            SwitchNow+=new SwitchTexture(Switch_SwitchNow);
        }
	
	}

    void Awake()
    {
        if (executeTrigger == Brightness.ExecuteONLite.Awake)
        {
            AdjustSwitching(startIndexFar, startIndexNear);
        }
    }
void  Switch_SwitchNow()
{
    AdjustSwitching(startIndexFar, startIndexNear);
}
    public void SwitchTextures()
    {
        if (SwitchNow != null)
            SwitchNow();
    }
    public void AdjustSwitching(int indexfar,int indexnear)
    {


        if (this.gameObject.isStatic && this.renderer != null)
        {
            if (options == Brightness.AdjustLightmap.Both)
            {

                SwitchLightmaps(indexfar, indexnear);
            }
            else if (options == Brightness.AdjustLightmap.Far)
            {
                SwitchLightmaps(indexfar, -1);

            }
            else if (options == Brightness.AdjustLightmap.Near)
            {
                SwitchLightmaps(-1, indexnear);

            }
        }
        else
        {
            Debug.Log("Lightmap Manager requires a static gameObject with a renderer to do this job.");
        }
                   
        
    }
    public static void SwitchLightmaps(GameObject obj, Texture2D far, Texture2D near)
    {
        if (obj.renderer != null && obj.isStatic)
        {
            
           SwitchLM.switchmaps(obj.renderer.lightmapIndex, far, near);
        }
        else
        {
            Debug.Log("Lightmap Manager requires a static gameObject with a renderer to do this job.");
        }
        }

    public void SwitchLightmaps(int indexfar, int indexnear)
    {
       
        SwitchLM.switchmaps(indexfar, indexnear, this.renderer.lightmapIndex, SwitchTexturesFar, SwitchTexturesNear);
       
        
        //LightmapData lmapData = new LightmapData();

        //Debug.Log("Swithching textures" + indexfar.ToString() + "," + indexnear.ToString());
        //if (indexfar != -1)
        //    lmapData.lightmapFar = SwitchTexturesFar[indexfar];
        //else
        //    lmapData.lightmapFar = LightmapSettings.lightmaps[this.renderer.lightmapIndex].lightmapFar;
        //if (indexnear != -1)
        //    lmapData.lightmapNear = SwitchTexturesNear[indexnear];
        //else
        //    lmapData.lightmapNear = LightmapSettings.lightmaps[this.renderer.lightmapIndex].lightmapNear;

        //LightmapSettings.lightmaps.SetValue(lmapData, this.renderer.lightmapIndex);
        //LightmapSettings.lightmaps[this.renderer.lightmapIndex] = lmapData;
        
    }

}
