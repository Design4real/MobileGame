using UnityEngine;
using System.Collections;

public class UIRootScale : MonoBehaviour
{
	private Transform item;
	private Transform item2;
	
	void Start ()
	{
		if (transform.childCount == 0)
		{
			transform.localScale = new Vector3(transform.localScale.x*MenuController.scaleX,
									  				transform.localScale.y*MenuController.scaleY,
									   				transform.localScale.z);
			return;
		}
		
		for (int i = 0; i < transform.childCount; i++)
		{
			item = transform.GetChild(i);
			if (item.GetComponent<UIDraggablePanel>() == null)
			{
				item.transform.localScale = new Vector3(item.transform.localScale.x*MenuController.scaleX,
										  				item.transform.localScale.y*MenuController.scaleY,
										   				item.transform.localScale.z);
				
				item.transform.localPosition = new Vector3(item.transform.localPosition.x*MenuController.scaleX,
										  				item.transform.localPosition.y*MenuController.scaleY,
										   				item.transform.localPosition.z);
			}
			else
			{
				UIPanel panel = item.GetComponent<UIPanel>();
		
//				transform.localPosition = new Vector3 (transform.localPosition.x*MenuController.ScaleX,
//													   transform.localPosition.y*MenuController.ScaleY,
//													   transform.localPosition.z);
				panel.clipRange = new Vector4( panel.clipRange.x * MenuController.scaleX,
											   panel.clipRange.y * MenuController.scaleY,
											   panel.clipRange.z * MenuController.scaleX,
											   panel.clipRange.w * MenuController.scaleY );

				for (int j = 0; j < item.childCount; j++)
				{
					item2 = item.GetChild(j);
					item2.transform.localScale = new Vector3(item2.transform.localScale.x*MenuController.scaleX,
										  				item2.transform.localScale.y*MenuController.scaleY,
										   				item2.transform.localScale.z);
					
					item2.transform.localPosition = new Vector3(item2.transform.localPosition.x*MenuController.scaleX,
										  				item2.transform.localPosition.y*MenuController.scaleY,
										   				item2.transform.localPosition.z);
				}
			}
		}
	}
}
