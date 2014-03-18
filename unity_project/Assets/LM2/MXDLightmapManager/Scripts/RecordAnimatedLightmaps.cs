using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

[RequireComponent(typeof(Animation))]
[ExecuteInEditMode]
public class RecordAnimatedLightmaps : MonoBehaviour {

    public float sampleValue = 0.0f;
    public AnimationClip clip;
    public GameObject animatedLight;
   // private bool isSaving = false;
   
    public bool baking = false;
    public string description = "";
    public string Label = "";
    public AnimatedFrameData[] animationFrames;
    public int pointer = 0;
	private bool skipUpdate = false;
	// Update is called once per frame
	void Update () {

		if(!skipUpdate)
		{
        if (Application.isPlaying && animation.clip == clip && animation.isPlaying)
        {
            Debug.Log(pointer);
            
            AnimatedFrameData frdata = this.animationFrames[pointer];

            for (int i = 0; i < LightmapSettings.lightmaps.Length; i++)
            {
           
                SwitchLM.switchmaps(i, frdata.lightmaps[i].far, frdata.lightmaps[i].near);
            
            }
          if(pointer<animationFrames.Length-1)
				{
					pointer++;
				skipUpdate = true;
				} else
            pointer = 0;
        }
        else
            pointer = 0;
		}
		else
			skipUpdate = false;

	}

    public void DestroyMe()
    {
     //   Lightmapping.Cancel();
        baking = false;
       
        DestroyImmediate(this);
        
    }
 

   
}
