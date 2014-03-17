using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Renderer))]
public class Brightness :MonoBehaviour {

    public delegate void ObjectBrightness();
	
	public enum OperateON{GPU,CPU};
	public OperateON operateOn = OperateON.GPU;
	
	public event ObjectBrightness BrightUp;
    public enum AdjustLightmap { Far, Near, Both,off };
    public enum ExecuteON { Start,Awake,Update,FixedUpdate,Trigger };
    public enum ExecuteONLite { Start, Awake,Trigger };
    public float level = 0;
	
	public bool useSharedMaterials = false;
	private List<Material> mats;
	public bool useCurve = true;
	public AnimationCurve curve= new AnimationCurve();
	public bool loopCurve = true;
	public bool startOnFirstKey = true;
	public float timeScale = 1;
	private float currentCurve = 0;
	private float currentCurveVal = 0;
	
	
//	private bool materialsUpdated = false;
	
    private float last = 0;
    public  AdjustLightmap options;
    public ExecuteON executeTrigger;
    public float Min = -1;
    public float Max = 1;
	
    private bool IsExecuting = false;
   
	
	
    public void Bright()
    {
        if (BrightUp != null) 
        BrightUp();
    }
	
	void LoadMaterials()
	{
		mats = new List<Material>();
		
		Material[] materials = null;
		
		
		if(useSharedMaterials)
			materials = this.renderer.sharedMaterials;
		else
			materials= this.renderer.materials;
		
		foreach(Material mat in materials)
		{
			Debug.Log(mat.shader.name);
			if(mat.shader.name != "LightmappingEffects/Diffuse")
			{
				mat.shader = Shader.Find("LightmappingEffects/Diffuse");
			}
			mats.Add(mat);
			
		}
	}
	// Use this for initialization
	void Start () {
		
	if(operateOn == OperateON.GPU)
			LoadMaterials();
		
		
		
       if(startOnFirstKey && useCurve)
			currentCurve = curve.keys[0].time;
		
        if (executeTrigger == ExecuteON.Start)
        {
			if(!IsExecuting)
			{
				IsExecuting = true;
				
				StartCoroutine(updateValues());
			}
			
			if(useCurve)
				AdjustBrightnessCurve(options);
			else
            AdjustBrightness(level, options);
        }
        if (executeTrigger == ExecuteON.Trigger)
        {
            BrightUp += new ObjectBrightness(Brightness_BrightUp);
        }
   }

    void Brightness_BrightUp()
    {
        IsExecuting = true;
		StartCoroutine(updateValues());
    }

    void Awake()
    {
        if (executeTrigger == ExecuteON.Awake)
        {
			if(!IsExecuting)
			{
				IsExecuting = true;
				
				StartCoroutine(updateValues());
			}
			
			if(useCurve)
				AdjustBrightnessCurve(options);
			else
            AdjustBrightness(level, options);
        }

    }
    void Update()
    {
        if (executeTrigger == ExecuteON.Update)
        {
			if(!IsExecuting)
			{
				IsExecuting = true;
				
				StartCoroutine(updateValues());
			}
			
			if(useCurve)
				AdjustBrightnessCurve(options);
			else
            AdjustBrightness(level, options);
        }
        else if (IsExecuting)
        {
			if(useCurve)
				AdjustBrightnessCurve(options);
			else
            AdjustBrightness(level, options);
        }
    }
    void FixedUpdate()
    {
        if (executeTrigger == ExecuteON.FixedUpdate)
        {
			if(!IsExecuting)
			{
				IsExecuting = true;
				
				StartCoroutine(updateValues());
			}
			if(useCurve)
				AdjustBrightnessCurve(options);
			else
            AdjustBrightness(level, options);
        }
    }
    public void AdjustBrightness(float val)
    {
			
        if (last + val > Min && last + val < Max)
        {
            if (this.gameObject.isStatic && this.renderer != null)
            {
                SetBrightness(LightmapSettings.lightmaps[this.renderer.lightmapIndex].lightmapFar, val);
                SetBrightness(LightmapSettings.lightmaps[this.renderer.lightmapIndex].lightmapNear, val);
            }
      //      last += level;
       }
        else
        {
           IsExecuting = false;
       }
     }
    public void AdjustBrightness(float val, AdjustLightmap options)
    {
		
        if (last + val > Min && last + val < Max)
        {
            if (this.gameObject.isStatic && this.renderer != null)
            {
                if (options == AdjustLightmap.Both)
                {
                    SetBrightness(LightmapSettings.lightmaps[this.renderer.lightmapIndex].lightmapFar, val);
                    SetBrightness(LightmapSettings.lightmaps[this.renderer.lightmapIndex].lightmapNear, val);
                }
                else if (options == AdjustLightmap.Far)
                {
                    SetBrightness(LightmapSettings.lightmaps[this.renderer.lightmapIndex].lightmapFar, val);

                }
                else if (options == AdjustLightmap.Near)
                {
                    SetBrightness(LightmapSettings.lightmaps[this.renderer.lightmapIndex].lightmapNear, val);

                }
            }
       //     last += level;
        }
        else
        {
            IsExecuting = false;
        }
		
    }
	 public void AdjustBrightnessCurve( AdjustLightmap options)
    {
       
            if (this.gameObject.isStatic && this.renderer != null)
            {
                if (options == AdjustLightmap.Both)
                {
                    SetBrightness(LightmapSettings.lightmaps[this.renderer.lightmapIndex].lightmapFar, currentCurveVal);
                    SetBrightness(LightmapSettings.lightmaps[this.renderer.lightmapIndex].lightmapNear, currentCurveVal);
                }
                else if (options == AdjustLightmap.Far)
                {
                    SetBrightness(LightmapSettings.lightmaps[this.renderer.lightmapIndex].lightmapFar, currentCurveVal);

                }
                else if (options == AdjustLightmap.Near)
                {
                    SetBrightness(LightmapSettings.lightmaps[this.renderer.lightmapIndex].lightmapNear, currentCurveVal);

                }
            }
		
		
		
       //     last += level;
       
    }
	IEnumerator updateValues()
	{
		
		if(!useCurve)
		{
			while(last + level > Min && last + level < Max)
			{
			last+=level;
			yield return new WaitForSeconds(0.05f);
			}
		}
		else
		{
			while(currentCurve<curve.length || loopCurve)
			{
				if(currentCurve>=curve.length)
				{
					if(startOnFirstKey)
						currentCurve = curve.keys[0].time;
					else
						currentCurve = 0;
				}
			currentCurveVal = curve.Evaluate(currentCurve);
			currentCurve+=0.05f/timeScale;
			yield return new WaitForSeconds(0.05f/timeScale);
			}
		}
	
		
	}
    void SetBrightness(Texture2D tex, float val)
    {
    if(operateOn == OperateON.CPU)
		{
        if (tex != null)
        {
            var original = tex.GetPixels();
            Color[] colors = new Color[original.Length];
            for (int i = 0; i < original.Length; i++)
            {


                UnityEngine.Color c = original[i];

                float r = c.r + val;
                float g = c.g + val;
                float b = c.b + val;

                if (r > 1)
                {

                    r = 1;

                }
                else if (r < 0)
                {
                    r = 0;
                }
                if (g > 1)
                {

                    g = 1;
                }
                else if (g < 0)
                {
                    g = 0;
                }
                if (b > 1)
                {

                    b = 1;
                }
                else if (b < 0)
                {
                    b = 0;
                }

                colors[i] = new UnityEngine.Color(r, g, b, c.a);
                //        tex.SetPixel(x, y, new UnityEngine.Color(r, g, c.b, c.a));


            }
            tex.SetPixels(colors);
            tex.Apply();
        }
        }
		else
		{
			if(mats == null)
				LoadMaterials();
			
			foreach(Material mat in mats)
			{
				mat.SetFloat("_Adjust",last+val);
				
			}
			
		}
	}
	
}
