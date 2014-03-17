#pragma strict

public var waveHeight : float = 1; // This variable is multiplied by the height of all waves, this also means that no wave can be higher than this value.
public var waveFrequency : float = 1; // The number of waves.
public var speed : float = 1; // The speed the waves travel.
public var origin : Vector2 = Vector2.zero; // The point the waves flow from or to.
public var calculateUnderTerrain : boolean = false; // If false it doesn't calculate the height under the terrain. Will be false in most cases

private var vertices : Vector3 []; // An array containing the meshs vertex positions.

public var time : float = 0; // The time.

private var terrain : Terrain;

function Start () {

	vertices = GetComponent(MeshFilter).mesh.vertices; // Assign the vertices to the array.
	
	terrain = Terrain.activeTerrain;
}

function Update () {

	time += Time.deltaTime * speed; // Increase time.

	for (var i = 0; i < vertices.Length; i++) {
	
		var worldVertexCoords : Vector3 = transform.TransformPoint(vertices[i]);
	
		vertices[i] = Vector3(vertices[i].x, FindHeight(worldVertexCoords.x, worldVertexCoords.z), vertices[i].z); // This sets the height of each vertex using the FindHeight function
	}
	
	GetComponent(MeshFilter).mesh.vertices = vertices; // Sets the modified vertices back to the mesh.
}


function FindHeight (x : float, y : float) {

	if (terrain != null && !calculateUnderTerrain && (transform.position.y + (waveHeight / 2) < terrain.SampleHeight (Vector3 (x,0,y)))) {
	
		return 0;
		
	}
		
	else {

		var coords : Vector2 = Vector2(x, y); // Create Vector2 variable
		
		// Changes the cartesian coords to polarised coords
	
		var theta : float = Mathf.Atan2(origin.x - coords.x, origin.y - coords.y) * Mathf.Rad2Deg; // Finds Theta (the angle)
	
		var distance : float = Vector2.Distance (Vector2(coords.x, coords.y), origin); // Finds the distance between origin and point
		
		// Adjusts the variables above to fit the perlin noise algorithm and adds the wave Frequency variable to the values
		
		var length : float = (waveFrequency * distance) / 10;
		
		var angle : float = (waveFrequency * theta * (length / 5)) / 36;
		
		// Finds the height by plugging the values above into the PerlinNoise function with the addition of the WaveHeight variable
		
		return (Mathf.PerlinNoise(angle, length + time) * waveHeight) - (waveHeight / 2);
		
	}
}