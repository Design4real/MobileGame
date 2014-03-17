using UnityEngine;
using System.Collections;

public class TempCameraMove : MonoBehaviour {
	public float speed = 200;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
//		transform.Translate(Vector3.forward*speed*Time.deltaTime,Space.Self);
		if(Input.GetKey("w")){
			transform.Translate(Vector3.forward*speed*Time.deltaTime,Space.Self);
		}
		
		if(Input.GetKey("s")){
			transform.Translate(Vector3.back*speed*Time.deltaTime,Space.Self);
		}
		
		if(Input.GetKey("a")){
			transform.Translate(Vector3.left*speed*Time.deltaTime,Space.Self);
		}
		
		if(Input.GetKey("d")){
			transform.Translate(Vector3.right*speed*Time.deltaTime,Space.Self);
		}
		
		
		if(Input.GetKey(KeyCode.UpArrow)){
			transform.Rotate(Vector3.left*30*Time.deltaTime,Space.Self);
		}
		
		if(Input.GetKey(KeyCode.DownArrow)){
			transform.Rotate(Vector3.right*30*Time.deltaTime,Space.Self);
		}
		if(Input.GetKey(KeyCode.LeftArrow)){
			transform.Rotate(Vector3.down*30*Time.deltaTime,Space.World);
			
		}
		if(Input.GetKey(KeyCode.RightArrow)){
			transform.Rotate(Vector3.up*30*Time.deltaTime,Space.World);
		}
		
	}
}
