using UnityEngine;
using System.Collections;

public class t_change : MonoBehaviour {
private Texture[] front;
private Object[] fronto;

private Texture[] up;
private Object[] upo;

private Texture[] left;
private Object[] lefto;

private Texture[] right;
private Object[] righto;

private Texture[] back;
private Object[] backo;

private Texture[] down;
private Object[] downo;


string cur_name;	
private float sec=0;
public float day = 48f; // Duration of the day in seconds
float next_change;
	float prev_time=0;
int current_hour=0;	
	float cur_time=0;
int next_hour;
float blend=0;	
	float moon_alpha;
	float light_var;
	void Start () {
	GameObject.Find("rot").transform.eulerAngles = new Vector3(-1f, transform.eulerAngles.y,transform.eulerAngles.z); ;
		this.fronto = Resources.LoadAll("dynamic_sky/front", typeof(Texture)); 
		this.front = new Texture[fronto.Length]; 
		this.righto = Resources.LoadAll("dynamic_sky/right", typeof(Texture)); 
		this.right = new Texture[fronto.Length]; 
		this.lefto = Resources.LoadAll("dynamic_sky/left", typeof(Texture)); 
		this.left = new Texture[fronto.Length]; 
		this.backo = Resources.LoadAll("dynamic_sky/back", typeof(Texture)); 
		this.back = new Texture[fronto.Length];
		this.upo = Resources.LoadAll("dynamic_sky/up", typeof(Texture)); 
		this.up = new Texture[fronto.Length]; 
		this.downo = Resources.LoadAll("dynamic_sky/down", typeof(Texture)); 
		this.down = new Texture[fronto.Length]; 
		//loading all the textures
		for (int i=0; i<=23; i++){
		this.front[i]=(Texture)this.fronto[i];  
			this.left[i]=(Texture)this.lefto[i];  
			this.right[i]=(Texture)this.righto[i];  
			this.up[i]=(Texture)this.upo[i];  
			
			 this.back[i]=(Texture)this.backo[i];   
		}
		this.down[0]=(Texture)this.downo[0];   // - there is only one texture for down in this asset
			change_textures();
	}
	
	// Update is called once per frame
	void Update () {
		if (cur_time>10 && cur_time<22 && light_var<1){
		light_var+=Time.deltaTime*2f;
		} 
		if ((cur_time>22 || cur_time<10) && light_var>0){
			light_var-=Time.deltaTime*2f;
		}
		//
		if (cur_time>22 || cur_time<9 && moon_alpha<1){
		moon_alpha+=Time.deltaTime*2f;
		} 
		if ((cur_time>9&& cur_time<22) && moon_alpha>0){
			moon_alpha-=Time.deltaTime*2f;
		}
		GameObject.Find("rot").transform.Find("Sun").light.intensity=light_var;
		cur_time+=(Time.time-sec)*24f/day;
		GameObject.Find("rot").transform.eulerAngles = new Vector3(-cur_time*15f,transform.rotation.y,transform.rotation.z);
		
			GameObject.Find("rot").transform.Find("Moon").renderer.material.color = new Color(GameObject.Find("rot").transform.Find("Moon").renderer.material.color.r,GameObject.Find("rot").transform.Find("Moon").renderer.material.color.b,GameObject.Find("rot").transform.Find("Moon").renderer.material.color.g,moon_alpha);
		 blend+=Time.time-sec;
	sec=Time.time;
		print ("time_"+ (cur_time-prev_time)+" "+cur_time+" "+current_hour);
		if (cur_time-prev_time>=1f) {
			prev_time++;
			change_textures();
			blend=0;
			
			if (cur_time >= 24f) {
				cur_time=0;
				prev_time=0;
			}
			current_hour=(int)Mathf.Floor(cur_time);
			}
		this.GetComponent<Skybox>().material.SetFloat("_Blend", (cur_time-prev_time));
		
	}
	
	void change_textures(){
		this.GetComponent<Skybox>().material.SetTexture("_FrontTex",front[current_hour]);
		
		this.GetComponent<Skybox>().material.SetTexture("_RightTex",right[current_hour]);
		
		this.GetComponent<Skybox>().material.SetTexture("_LeftTex",left[current_hour]);
		
		this.GetComponent<Skybox>().material.SetTexture("_BackTex",back[current_hour]);
		
		this.GetComponent<Skybox>().material.SetTexture("_UpTex",up[current_hour]);
		
		this.GetComponent<Skybox>().material.SetTexture("_DownTex",down[0]);
		change_2_textures();
	}
	
	void change_2_textures(){
		next_hour=current_hour+1;
		if (next_hour==24){
			next_hour=0;
		}
		this.GetComponent<Skybox>().material.SetTexture("_FrontTex2",front[next_hour]);
		
		this.GetComponent<Skybox>().material.SetTexture("_RightTex2",right[next_hour]);
		
		this.GetComponent<Skybox>().material.SetTexture("_LeftTex2",left[next_hour]);
		
		this.GetComponent<Skybox>().material.SetTexture("_BackTex2",back[next_hour]);
		
		this.GetComponent<Skybox>().material.SetTexture("_UpTex2",up[next_hour]);
		
		this.GetComponent<Skybox>().material.SetTexture("_DownTex2",down[0]);
		
	}
	

}
