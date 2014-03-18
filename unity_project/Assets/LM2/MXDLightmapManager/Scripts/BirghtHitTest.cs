using UnityEngine;
using System.Collections;

public class BirghtHitTest : MonoBehaviour {

  public  Brightness bright;
	// Use this for initialization
	void Start () {
	
	}

    void OnMouseDown()
    {
        bright.Bright();

    }
	// Update is called once per frame
	void Update () {
	
	}
}
