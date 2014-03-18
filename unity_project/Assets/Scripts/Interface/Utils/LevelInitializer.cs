using UnityEngine;
using System.Collections;

public class LevelInitializer : MonoBehaviour
{
	public Transform menuCamera;
	public Transform transporter;
	public float radius;
	public float deltaY;
	
	float curAngle;
	
	// Use this for initialization
	void Awake()
	{
		if( !GlobalDataKeeper.isGame )
		{
			foreach( Transform child in transform )
			{
				child.gameObject.SetActive( false );
			}
			
			transporter.gameObject.SetActive( true );
			transporter.GetComponent< LevelTransport >().enabled = false;
			foreach( Transform child in transporter.transform )
			{
				if( child.GetComponent< Camera >() != null )
				{
					child.gameObject.SetActive( false );
				}
			}
			menuCamera.gameObject.SetActive( true );
			menuCamera.LookAt( transporter );
			curAngle = 0;
		}
		else
		{
			menuCamera.gameObject.SetActive( false );
			enabled = false;
		}
	}
	
	void Update()
	{
		curAngle += 0.01f;
		Vector3 curPosition = new Vector3( transporter.position.x + Mathf.Sin( curAngle ) * radius, transporter.position.y + deltaY, transporter.position.z + Mathf.Cos( curAngle ) * radius );
		menuCamera.position = curPosition;
		menuCamera.LookAt( transporter );
	}
}
