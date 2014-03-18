using UnityEngine;
using System.Collections;

public class WallCollider : MonoBehaviour 
{

	void OnCollisionEnter(Collision collision)
	{
		if( collision.collider.tag == "Cargo" )
		{
			//GameController.runtime.LivesCount -= 1;
			if (GameController.runtime.hitParticlesPrefab != null)
			{
				foreach (ContactPoint curPoint in collision.contacts)
				{
					Instantiate(GameController.runtime.hitParticlesPrefab, curPoint.point, Quaternion.identity);
				}
			}
		}
	}
}