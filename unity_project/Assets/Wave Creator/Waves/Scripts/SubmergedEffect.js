#pragma strict

private var height : float = 0; // height of the wave above or below this object

public var underwaterFogColor : Color = Color(0.096, 0.336, 0.5, 0.5); // Color of the sea when below the surface
public var underwaterFogDensity : float = 0.55; // Transparency of the water when below the surface (how far you can see)

private var startingSkybox : Material; // The skybox above the surface
private var startingFog : boolean; // The fog above the surface
private var startingFogColor : Color; // The fog color above the surface
private var startingFogDensity : float; // The fog density above the surface
private var startingFlares : LensFlare []; // The flares above the surface

private var selectectWaves : Waves; // The water body this object is over will be assigned to this variable

function Start () {

	startingSkybox = RenderSettings.skybox; // Assign these variables on start
	startingFog = RenderSettings.fog;
	startingFogColor = RenderSettings.fogColor;
	startingFogDensity = RenderSettings.fogDensity;
	
	if (GetComponent(Camera))GetComponent(Camera).nearClipPlane = 0.01;// This gives a realistic effect when the camera is passing through the waves mesh
}

function Update () {

	Check (); // Establish whether your object is over the waterObject.
}


function Check () {

	var waves : Waves [] = FindObjectsOfType(Waves) as Waves []; // Finds all wave objects
	
	for (var i = 0; i < waves.length; i++) {
	
		if (waves[i].gameObject.renderer.bounds.IntersectRay(Ray(transform.position, Vector3.up)) || waves[i].gameObject.renderer.bounds.IntersectRay(Ray(transform.position, -Vector3.up))) { // Raycast up and down
		
			selectectWaves = waves[i]; // we now know what wave object to set the selectedWaves to.
			
			GetHeight (); // finds the height of the wave above or below this object then assigns it to the height variable
		
			ApplyEffect (); // Applies the underwater effect depending on whether the height variable is above or below this object y coordinate
			
			break;
			
		}
		
		AboveWater (); // This function applies the latest above water settings
	}
}


function GetHeight () {
	
	height = selectectWaves.FindHeight(transform.position.x, transform.position.z) + selectectWaves.transform.position.y; // Sets your objects y axis by going into the FindHeight function in the WaveScript which deals with the maths, we then add the offset and the waveObject y position.
}


function ApplyEffect () {

	if (transform.position.y < height) { // check if underwater
		
		if (RenderSettings.skybox != selectectWaves.gameObject.renderer.material) { // Check if underwater settings haven't been applied
		
			startingSkybox = RenderSettings.skybox; // Assigns the latest variables to the above water settings this means you can have an ever-changing skybox above water for example
			startingFog = RenderSettings.fog;
			startingFogColor = RenderSettings.fogColor;
			startingFogDensity = RenderSettings.fogDensity;
			
			RenderSettings.skybox = selectectWaves.gameObject.renderer.material; // Applies the underwater settings
			RenderSettings.fog = true;
			RenderSettings.fogColor = underwaterFogColor;
			RenderSettings.fogDensity = underwaterFogDensity;
			
			startingFlares = FindObjectsOfType(LensFlare) as LensFlare[];
			
			for (var l = 0; l < startingFlares.Length; l++) {
			
				startingFlares[l].brightness = 0;
			}
		}
	}
	
	else {
		
		if (RenderSettings.skybox != startingSkybox) { // Check if above water settings havent been applied
			
			AboveWater ();
		}
	}
}

function AboveWater () {

	RenderSettings.skybox = startingSkybox; // Apply above water settings
	RenderSettings.fog = startingFog;
	RenderSettings.fogColor = startingFogColor;
	RenderSettings.fogDensity = startingFogDensity;
	
	startingFlares = FindObjectsOfType(LensFlare) as LensFlare[];
	
	for (var l = 0; l < startingFlares.Length; l++) {
	
		startingFlares[l].brightness = 1;
	}
}