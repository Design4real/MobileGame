#pragma strict

public var offset : float = 0;  // This variable allows you to customise how high your object rides on the water.
public var randomRotationFactor : float = 10; // The higher this value the more random rotation will happen.

private var selectectWaves : Waves; // The water body this object is over will be assigned to this variable

function Update () {

	Check (); // Establish whether your object is over the waterObject.
}


function Check () {
	
	var waves : Waves [] = FindObjectsOfType(Waves) as Waves []; // Finds all wave objects
	
	for (var i = 0; i < waves.length; i++) {
	
		if (waves[i].gameObject.renderer.bounds.IntersectRay(Ray(transform.position, Vector3.up)) || waves[i].gameObject.renderer.bounds.IntersectRay(Ray(transform.position, -Vector3.up))) { // Raycast both up and down
		
			selectectWaves = waves[i]; // we now know what wave object to set the selectedWaves to.
		
			SetHeight (); // Set y axis position so it corresponds to the height of the wave it's floating on.
		
			SetRotation (); // Adds random rotation to your object to make the effect look move realistic.
			
			break;
		
		}
	
	}
}


function SetHeight () {
	
	transform.position.y = selectectWaves.FindHeight(transform.position.x, transform.position.z) + offset + selectectWaves.transform.position.y; // Sets your objects y axis by going into the FindHeight function in the WaveScript which deals with the maths, we then add the offset and the waveObject y position.
}


function SetRotation () {

	transform.Rotate((PerlinVector3(Time.time) - PerlinVector3(Time.time - Time.deltaTime)) * randomRotationFactor); // Rotates the difference between the rotation applied last frame and the rotation which will be applied this frame, it then times it by the rotation variable so the amount of rotation is adjustable.
}


function PerlinVector3 (t : float) {

	var vector : Vector3 = Vector3.zero; // Creaate new Vector3.

	for (var i = 0; i < 3; i++) {
	
		vector[i] = (Mathf.PerlinNoise(t , i * 100) - 0.5) * 2; // Sets the x, y and z to different factors based off a perlin noise function, (the x inputed into the function is always t but the y inputed changes).
	}
	
	return vector; // returns the vector
}