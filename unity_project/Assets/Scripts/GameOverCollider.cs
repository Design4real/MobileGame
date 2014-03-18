using UnityEngine;
using System.Collections;

public class GameOverCollider : MonoBehaviour 
{

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Cargo")
		{
			GameController.runtime.LivesCount = 0;
			GameController.runtime.curState = GameController.GameState.loose;
		}
	}
}
