class WaterCreator extends EditorWindow {

// Variables shown in expector

var length : int = 100;
var width : int = 100;

var res : float = 1;

var tex : boolean = true;
var mat : int = 0;
var options : String [] = ["Daytime Water", "Nightime Water"];

// Variables in script

private var object : GameObject;

private var lengthVertices : float = 0;
private var widthVertices : float = 0;

public var lengthSegments : float = 10;
public var widthSegments : float = 10;

private var lengthGap : float = 0;
private var widthGap : float = 0;

private var mesh : Mesh;
private var vertices : Vector3 [];
private var triangles : int [];
private var uvs : Vector2 [];

function OnGUI () {
    	
	EditorGUILayout.Space();
    
    length = EditorGUILayout.IntField(GUIContent ("Length", "Length along the x axis."), length);
    width = EditorGUILayout.IntField(GUIContent ("Width", "Length along the z axis."), width);
    
    EditorGUILayout.Space();

    res = EditorGUILayout.Slider(GUIContent ("Resolution of mesh", "The number of vertices relative to the length and width."), res, 0.1,2);
    
    EditorGUILayout.Space();
    
    EditorGUILayout.BeginHorizontal("Label");
    
    	EditorGUILayout.LabelField(GUIContent ("Add basic water material", "Ticking this adds a basic water material to the water."), GUILayout.MaxWidth(150));
    
    	tex = EditorGUILayout.Toggle(tex, GUILayout.MaxWidth(25) );
    	
    	if(tex == true) {
    	
    		EditorGUILayout.LabelField(GUIContent ("Type: ", "Choose which basic water material you want."), GUILayout.MaxWidth(50), GUILayout.MinWidth(50));
    	
    		mat = EditorGUILayout.Popup(mat, options, GUILayout.ExpandWidth(true));
    	}
    
    EditorGUILayout.EndHorizontal();
    
    EditorGUILayout.Space ();
    
    if (GUI.Button(Rect(5,105,Screen.width - 10,20),"Create Water Object")) {
    
    	Create ();
    }
}


function Create () {

	SetupGameObject ("Water Body");
	
	SetupMat ();
	
	SetupMesh(length + " x " + width);
	
	SetupVariables ();
	
	SetupVertices ();
	
	SetupTriangles ();
	
	SetupUvs ();
	
	mesh = TangentSolver(mesh);
	
	this.Close();
}



function SetupGameObject (name : String) {

	object = new GameObject ();
	object.name = name;
	object.AddComponent(MeshFilter);
	object.AddComponent(MeshRenderer);
}


function SetupMesh (name : String) {

	mesh = new Mesh (); mesh.name = name; object.GetComponent(MeshFilter).sharedMesh = mesh; // Create, Name, Assign
}


function SetupMat () {

	if (tex == true) {

		object.AddComponent(WaterSimple);
		if (mat == 0) object.renderer.material = Resources.Load("Daylight Simple Water");
		if (mat == 1) object.renderer.material = Resources.Load("Nighttime Simple Water");
	}
	
	else {
	
		object.renderer.material = new Material(Shader.Find("Diffuse"));
	}
	
	object.AddComponent(Waves);
}

function SetupVariables () {

	lengthSegments = Mathf.Clamp(length * res, 0, 254);
	widthSegments = Mathf.Clamp(width * res, 0, 254);

	lengthGap = length / lengthSegments;
	widthGap = width / widthSegments;

	lengthVertices = lengthSegments + 1;
	widthVertices = widthSegments + 1;
}


function SetupVertices () {

	vertices = new Vector3 [lengthVertices * widthVertices];
	
	var i = 0;

	for (var x = 0; x < lengthVertices; x++) {
		for (var y = 0; y < widthVertices; y++) {
	
			vertices[i] = Vector3(x * lengthGap, 0 ,y * widthGap); i++;
		}
	}
	
	mesh.vertices = vertices;
}


function SetupTriangles () {

	triangles = new int [lengthSegments * widthSegments * 6];
	
	var i = 0;

	for (var x = 0; x < lengthSegments; x++) {
		for (var y = 0; y < widthSegments; y++) {
		
			triangles[i] = (x     * widthVertices) + y + 1;
            triangles[i + 1] = ((x+1) * widthVertices) + y;
            triangles[i + 2]   = (x     * widthVertices) + y;

			triangles[i + 3] = (x     * widthVertices) + y + 1;
            triangles[i + 4] = ((x+1) * widthVertices) + y + 1;
            triangles[i + 5] = ((x+1) * widthVertices) + y;
            
            i += 6;
        }
    }
    
    mesh.triangles = triangles;
}


function SetupUvs () {

	uvs = new Vector2[vertices.Length];

	for (var i = 0 ; i < uvs.Length; i++) {
	
		uvs[i] = Vector2 (vertices[i].x, vertices[i].z);
	}
	
	mesh.uv = uvs;
	mesh.uv2 = uvs;
	
	mesh.RecalculateNormals ();
	mesh.RecalculateBounds ();
}

}








/*

Derived from

Lengyel, Eric. “Computing Tangent Space Basis Vectors for an Arbitrary Mesh”. Terathon Software 3D Graphics Library, 2001.

[url]http://www.terathon.com/code/tangent.html[/url]

*/





    function TangentSolver(theMesh : Mesh)

    {

        vertexCount = theMesh.vertexCount;

        vertices = theMesh.vertices;

        normals = theMesh.normals;

        texcoords = theMesh.uv;

        triangles = theMesh.triangles;

        triangleCount = triangles.length/3;

        tangents = new Vector4[vertexCount];

        tan1 = new Vector3[vertexCount];

        tan2 = new Vector3[vertexCount];

        tri = 0;

        for ( i = 0; i < (triangleCount); i++)

        {

            i1 = triangles[tri];

            i2 = triangles[tri+1];

            i3 = triangles[tri+2];

            

            v1 = vertices[i1];

            v2 = vertices[i2];

            v3 = vertices[i3];

            

            w1 = texcoords[i1];

            w2 = texcoords[i2];

            w3 = texcoords[i3];

            

            x1 = v2.x - v1.x;

            x2 = v3.x - v1.x;

            y1 = v2.y - v1.y;

            y2 = v3.y - v1.y;

            z1 = v2.z - v1.z;

            z2 = v3.z - v1.z;

            

            s1 = w2.x - w1.x;

            s2 = w3.x - w1.x;

            t1 = w2.y - w1.y;

            t2 = w3.y - w1.y;

            

            r = 1.0 / (s1 * t2 - s2 * t1);

            sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);

            tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);

            

            tan1[i1] += sdir;

            tan1[i2] += sdir;

            tan1[i3] += sdir;

            

            tan2[i1] += tdir;

            tan2[i2] += tdir;

            tan2[i3] += tdir;

            

            tri += 3;

        }

        

        for (i = 0; i < (vertexCount); i++)

        {

            n = normals[i];

            t = tan1[i];

            

            // Gram-Schmidt orthogonalize

            Vector3.OrthoNormalize( n, t );

            

            tangents[i].x  = t.x;

            tangents[i].y  = t.y;

            tangents[i].z  = t.z;

        

            // Calculate handedness

            tangents[i].w = ( Vector3.Dot(Vector3.Cross(n, t), tan2[i]) < 0.0 ) ? -1.0 : 1.0;

        }       

        theMesh.tangents = tangents;
        
        return theMesh;

    }
