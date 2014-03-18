using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Renderer))]
public class Blend : MonoBehaviour {
  
	public enum OperateON{GPU,CPU};
	public OperateON operateOn = OperateON.GPU;
	
	
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
	
    public delegate void TextureBlend();
    public event TextureBlend blendNow;
    public Texture2D BlendTexture;
    public float level = 0;
    private float last = 0;
    public Brightness.AdjustLightmap options;
    public Brightness.ExecuteON executeTrigger;
    public float Min = -1;
    public float Max = 1;
    private bool IsExecuting = false;
	
	
    public void LoadMaterials()
	{
	
		mats = new List<Material>();
		
		Material[] materials = null;
		
		
		if(useSharedMaterials)
			materials = this.renderer.sharedMaterials;
		else
			materials= this.renderer.materials;
		
		foreach(Material mat in materials)
		{
		
			if(mat.shader.name != "LightmappingEffects/BlendDiffuse")
			{
				mat.shader = Shader.Find("LightmappingEffects/BlendDiffuse");
			}
			mats.Add(mat);
			
		}
	}
	
	// Use this for initialization
	void Start () {

        if (curve == null || curve.keys.Length==0)
            curve = AnimationCurve.Linear(0, 0, 1, 1);

		if(operateOn == OperateON.GPU)
			LoadMaterials();
		
		   if(startOnFirstKey && useCurve && curve!=null)
			currentCurve = curve.keys[0].time;
		
        if (executeTrigger == Brightness.ExecuteON.Start)
        {
			if(!IsExecuting)
			{
				IsExecuting = true;
				
				StartCoroutine(updateValues());
			}
			
			if(useCurve)
				AdjustBlendingCurve(options,BlendTexture);
			else
            AdjustBlending(level, options, BlendTexture);
        }
        else if (executeTrigger == Brightness.ExecuteON.Trigger)
        {
            blendNow += new TextureBlend(Blend_blendNow);
        }
        else if (IsExecuting)
        {
            AdjustBlending(level, options, BlendTexture);
        }
	}
    void FixedUpdate()
    {
        if (executeTrigger == Brightness.ExecuteON.FixedUpdate)
        {
			if(!IsExecuting)
			{
				IsExecuting = true;
				
				StartCoroutine(updateValues());
			}
			
			if(useCurve)
				AdjustBlendingCurve(options,BlendTexture);
			else
            AdjustBlending(level, options, BlendTexture);
        }
    }
    void Awake()
    {
        if (executeTrigger == Brightness.ExecuteON.Awake)
        {
			if(!IsExecuting)
			{
				IsExecuting = true;
				
				StartCoroutine(updateValues());
			}
			
			if(useCurve)
				AdjustBlendingCurve(options,BlendTexture);
			else
            AdjustBlending(level, options, BlendTexture);
        }

    }
    void Blend_blendNow()
    {
        IsExecuting = true;
			StartCoroutine(updateValues());
    }
    public void BlendTextures()
    {
        if (blendNow != null)
            blendNow();
    }
	// Update is called once per frame
	void Update () {
        if (executeTrigger == Brightness.ExecuteON.Update)
        {
			if(!IsExecuting)
			{
				IsExecuting = true;
				
				StartCoroutine(updateValues());
			}
			
			if(useCurve)
				AdjustBlendingCurve(options,BlendTexture);
			else
            AdjustBlending(level, options,BlendTexture);
        }
        else if (IsExecuting)
        {
			if(useCurve)
				AdjustBlendingCurve(options,BlendTexture);
			else
            AdjustBlending(level, options,BlendTexture);
        }
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
    public void AdjustBlending(float val, Brightness.AdjustLightmap options,Texture2D dest)
    {
        if (BlendTexture != null)
        {
            if (last + val > Min && last + val < Max)
            {
                if (this.gameObject.isStatic && this.renderer != null)
                {
                    if (options == Brightness.AdjustLightmap.Both)
                    {
                        BlendNow(LightmapSettings.lightmaps[this.renderer.lightmapIndex].lightmapFar, dest, val);
                        BlendNow(LightmapSettings.lightmaps[this.renderer.lightmapIndex].lightmapNear, dest, val);
                    }
                    else if (options == Brightness.AdjustLightmap.Far)
                    {
                        BlendNow(LightmapSettings.lightmaps[this.renderer.lightmapIndex].lightmapFar, dest, val);

                    }
                    else if (options == Brightness.AdjustLightmap.Near)
                    {
                        BlendNow(LightmapSettings.lightmaps[this.renderer.lightmapIndex].lightmapNear, dest, val);

                    }
                }
                else
                {
                    Debug.Log("Lightmap Manager requires a static gameObject with a renderer to do this job.");
                }
             //   last += level;
            }
            else
            {
                IsExecuting = false;
            }
        }
    }
	 public void AdjustBlendingCurve(Brightness.AdjustLightmap options,Texture2D dest)
    {
      
                if (this.gameObject.isStatic && this.renderer != null)
                {
                    if (options == Brightness.AdjustLightmap.Both)
                    {
                        BlendNow(LightmapSettings.lightmaps[this.renderer.lightmapIndex].lightmapFar, dest, currentCurveVal);
                        BlendNow(LightmapSettings.lightmaps[this.renderer.lightmapIndex].lightmapNear, dest, currentCurveVal);
                    }
                    else if (options == Brightness.AdjustLightmap.Far)
                    {
                        BlendNow(LightmapSettings.lightmaps[this.renderer.lightmapIndex].lightmapFar, dest, currentCurveVal);

                    }
                    else if (options == Brightness.AdjustLightmap.Near)
                    {
                        BlendNow(LightmapSettings.lightmaps[this.renderer.lightmapIndex].lightmapNear, dest, currentCurveVal);

                    }
                }
                else
                {
                    Debug.Log("Lightmap Manager requires a static gameObject with a renderer to do this job.");
                }
             //   last += level;
           
        
    }
    public void BlendNow(Texture2D tex1, Texture2D tex2,float val)
    {

		  if(operateOn == OperateON.CPU)
		{
        if(tex1.width == tex2.width && tex1.height == tex2.height)
        {
        var original = tex1.GetPixels();
        Color[] colors = new Color[original.Length];
        var destination = tex2.GetPixels();
        for (int i = 0; i < original.Length; i++)
        {


            UnityEngine.Color c = original[i];
             UnityEngine.Color c2 = destination[i];
            float r;
            float g;
            float b;
            float a;
            bool rm = false;
            bool gm = false;
            bool bm = false;
            bool am = false;

            if (c.r < c2.r)
            {
                r = c.r + val;
                rm = true;
            }
            else
            {
                r = c.r - val;
                rm = false;
            }
            
            if (c.g < c2.g)
            {
                g = c.g + val;
                gm = true;
            }
            else
            {
                g = c.g - val;
                gm = false;
            } 
            if (c.b < c2.b)
            {
                b = c.b + val;
                bm = true;
            }
            else
            {
                b = c.b - val;
                bm = false;
            }
            if (c.a < c2.a)
            {
                a = c.a + val;
                am = true;
            }
            else
            {
                a = c.a - val;
                am = false;
            }
            if ((r > c2.r && rm == true) || (r < c2.r && rm == false))
            {
               r = c2.r;
            }

            if ((g > c2.g && gm == true) || (g < c2.g && gm == false))
            {

                g = c2.g;
            }

            if ((b > c2.b && bm == true) || (b < c2.b && bm == false))
            {

                b = c2.b;
            }
            if ((a > c2.a && am == true) || (a < c2.a && am == false))
            {

                a = c2.a;
            }

            colors[i] = new UnityEngine.Color(r, g, b, a);
            //        tex.SetPixel(x, y, new UnityEngine.Color(r, g, c.b, c.a));


        }
        tex1.SetPixels(colors);
        tex1.Apply();
        }
        else
        {
            Debug.Log("Lightmap Manager requires the blended textures to have the same size in order to blend them.");
        }
		}
		else
		{
			if(mats == null)
				LoadMaterials();
			
			foreach(Material mat in mats)
			{
				mat.SetFloat("_Adjust",last+val);
				mat.SetTexture("_BlendTex",tex2);
			}
		}
    }
}
