using UnityEngine;
using System.Collections;
public class ShoreLine : MonoBehaviour {
	Mesh mesh;
	Vector3[] vertices;
	Color[] colors;
	Transform myTransform;
	LayerMask layermask;
	float detectDistance;
	// Use this for initialization
	//Detect the terrain under the water by raycast from every water vertics.
	//If the distance is bigger than detectDistance, set the vertic's color
	void Start () {
		if(!MirrorReflection.instance.enableShoreLine){
			return;
		} 
		
		detectDistance = MirrorReflection.instance.shoreLineDepth;
		layermask = MirrorReflection.instance.shoreLineDetectLayer;
		myTransform = transform;
		mesh = transform.GetComponent<MeshFilter>().mesh;
		vertices = mesh.vertices;
		colors = new Color[vertices.Length];
		RaycastHit hit;
		
		Vector3 offsetUp = new Vector3(0,20,0);
//		int detectlayer =  (1<<8);
		for(int i = 0; i <vertices.Length;i++){
			vertices[i] = myTransform.TransformPoint(vertices[i]);
			colors[i] = Color.white;
			float power = 1;
			if(Physics.Raycast(vertices[i]+offsetUp,Vector3.down,out hit,detectDistance+offsetUp.y,layermask.value)){
				float dis = hit.distance - offsetUp.y;
				if(dis<0){
					dis = 0;
				}
				power = dis/detectDistance;
				
				
				power = Mathf.Clamp01(power);
				power = Mathf.Sqrt(power);
				colors[i] *= power;
			}
		}
		mesh.colors = colors;
		
	}

	
	
	
}
