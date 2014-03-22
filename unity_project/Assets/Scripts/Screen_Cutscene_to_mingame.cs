using UnityEngine;
using System.Collections;

public class Screen_Cutscene_to_mingame : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void cutscenetominigame () {
		GameController.runtime.screen_cutscene.gameObject.SetActive (false);
		GameController.runtime.screen_minigame.gameObject.SetActive (true);
		CFInput.ctrl = GameController.runtime.screen_minigame.GetComponent<TouchController>();
		}
}
