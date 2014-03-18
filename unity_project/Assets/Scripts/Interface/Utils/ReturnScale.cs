using UnityEngine;
using System.Collections;

public class ReturnScale : MonoBehaviour
{
	public bool isNeedRescale = false;
	public bool isBackground = false;
	public bool isGrid = false;
	public bool isGridX = false;
	private float scaleFactor = 1;
	
	void Start()
	{
	
		transform.localScale = new Vector3( transform.localScale.x / MenuController.scaleX,
						  				    transform.localScale.y / MenuController.scaleY,
						   				    transform.localScale.z );
		
		
		if (isGrid)
		{
			transform.localScale = new Vector3( transform.localScale.x * MenuController.scaleX ,
						  				    	transform.localScale.y * MenuController.scaleX ,
						   				   		transform.localScale.z );
		}
		
		if (isGridX)
		{
			transform.localScale = new Vector3( transform.localScale.x * MenuController.scaleX ,
						  				    	transform.localScale.y * MenuController.scaleY ,
						   				   		transform.localScale.z );
		}
		
		if( isNeedRescale )
		{
			transform.localScale = new Vector3( transform.localScale.x * MenuController.scaleX * scaleFactor,
						  				    	transform.localScale.y * MenuController.scaleX * scaleFactor,
						   				   		transform.localScale.z );
		}
		else if( isBackground )
		{
			scaleFactor = ( MenuController.scaleX > MenuController.scaleY ) ? MenuController.scaleX : MenuController.scaleY;
			transform.localScale = new Vector3( transform.localScale.x * scaleFactor,
							  				    transform.localScale.y * scaleFactor,
							   				    transform.localScale.z );
		}
	}
}