using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System;
using System.Reflection;

public class LightmapManager : EditorWindow
{
   
	public enum LoadType
	{
		All,
		Probes,
		Lightmaps
	};
	public LoadType loadType;
	string Label = "lightmap";
	string description = "";
	bool groupEnabled;
//    bool myBool = true;
//    float myFloat = 1.23f;
	bool baking = false;
	public int toolbarInt = 0;
	public int ADVtoolbarInt = 0;
	public bool skipFrame = false;
	Vector2 scrollLoads;
	Vector2 scrollLoads1;
	Vector2 scrollLoads2;
	Vector2 scrollLoads3;
	Vector2 scrollLoads4;
	bool selection;
//    double adjust = 0;
	Texture2D tex;
	string path = "";
	int resize = 512;
	float brightness = 0;
	Color tint;
	bool draw = true;
	bool flood0 = false;
	bool flood1 = false;
	bool flood2 = false;
	bool flood3 = false;
	bool flood4 = false;
	public float RecordRate = 0.05f;
	//float sampleVal = 0;
	private GUIStyle style;
	bool showPreview = false;
	AnimationClip clip;
	// int AnimationFPS = 25;
	GameObject animatedLight;
	int previewIndex = 0;
	bool autoPreview = false;
	string animationDescription = "Description";
	string animationName = "Animated Lightmaps";
	bool SaveAsOnePrefab = false;
	bool SaveInSpecialDirectory = true;
	string PrefabDescription = "Saved Lightmapped Object description";
	string prefabName = "LightmappedObject";
	UnityEngine.Object[] selectedObjs;
	UnityEngine.Object manual = null;
	bool enterPreview = false;
	
	//  int w1;
	//  int h1;
    
	 GUIContent[] mainContent = null;
	GUIContent[] supportContent = null;
	string LMPath;
	public string[] toolbarStrings = new string[] { "Save\\Load","Settings","Object","Advanced","Support"};
	public string[] AdvancedStrings = new string[] { "Lightmapped Objects","Animated Lightmaps" };
 
	/*    [MenuItem("Lightmap Manager/InstallPrerequests")]
    static void InstallPre()
    {
#if UNITY_EDITOR
        if (EditorUtility.DisplayDialog("Installing prerequests", "Prerequests much be installed, click OK to install them then try again", "OK"))
            AssetDatabase.ImportPackage(Application.dataPath + "/MXDLightmapManager/Prerequests.unitypackage", true);
#endif
    } */
	[MenuItem("Lightmap Manager/Manage")] 
	static void ShowWindow ()
	{
         
		EditorWindow window;
      
		window = (EditorWindow)EditorWindow.GetWindow<LightmapManager>("Lightmap");
        window.position = new Rect(100, 100, 400, 300);
		window.autoRepaintOnSceneChange = true;
		window.Show ();
        
	}

	void OnSelectionChange ()
	{
		Repaint (); 
        
	}

	void OnGUI ()
	{
		
		  if (LMPath == null)
            {
                string[] locations = System.IO.Directory.GetDirectories(Application.dataPath, "MXDLightmapManager", SearchOption.AllDirectories);
                if (locations != null && locations.Length > 0)
                    LMPath = locations[0].Remove(0, Application.dataPath.Length - 6).Replace('\\', '/');
                else
                    LMPath = "Assets/MXDLightmapManager";
            }
		if(manual==null)	
		{
			manual =  AssetDatabase.LoadAssetAtPath(LMPath+"/Manual.pdf", typeof(UnityEngine.Object));
			
		}
		 if (mainContent == null)
            {
                mainContent = new GUIContent[5];
                Texture2D tex = AssetDatabase.LoadAssetAtPath(LMPath+"/Icons/1.png", typeof(Texture2D)) as Texture2D;
                GUIContent c0 = new GUIContent("", tex, "Save and load lightmaps");
                mainContent[0] = c0;
                Texture2D tex1 = AssetDatabase.LoadAssetAtPath(LMPath+"/Icons/2.png", typeof(Texture2D)) as Texture2D;
                GUIContent c1 = new GUIContent("", tex1, "lightmap settings");
                mainContent[1] = c1;
                Texture2D tex2 = AssetDatabase.LoadAssetAtPath(LMPath+"/Icons/3.png", typeof(Texture2D)) as Texture2D;
                GUIContent c2 = new GUIContent("", tex2, "Per object lightmap effects");
                mainContent[2] = c2;
                Texture2D tex3 = AssetDatabase.LoadAssetAtPath(LMPath+"/Icons/4.png", typeof(Texture2D)) as Texture2D;
                GUIContent c3 = new GUIContent("", tex3, "Advanced features");
                mainContent[3] = c3;
                Texture2D tex4 = AssetDatabase.LoadAssetAtPath(LMPath+"/Icons/5.png", typeof(Texture2D)) as Texture2D;
                GUIContent c4 = new GUIContent("", tex4, "Support");
                mainContent[4] = c4;

            }
		   if (supportContent == null)
            {
                supportContent = new GUIContent[7];
                Texture2D tex = AssetDatabase.LoadAssetAtPath(LMPath+"/Icons/51.png", typeof(Texture2D)) as Texture2D;
                GUIContent c0 = new GUIContent("", tex, "Mixed Dimensions");
                supportContent[0] = c0;
                Texture2D tex1 = AssetDatabase.LoadAssetAtPath(LMPath+"/Icons/52.png", typeof(Texture2D)) as Texture2D;
                GUIContent c1 = new GUIContent("", tex1, "Manual");
                supportContent[1] = c1;
                Texture2D tex2 = AssetDatabase.LoadAssetAtPath(LMPath+"/Icons/53.png", typeof(Texture2D)) as Texture2D;
                GUIContent c2 = new GUIContent("", tex2, "Tutorials");
                supportContent[2] = c2;
                Texture2D tex3 = AssetDatabase.LoadAssetAtPath(LMPath+"/Icons/54.png", typeof(Texture2D)) as Texture2D;
                GUIContent c3 = new GUIContent("", tex3, "Support");
                supportContent[3] = c3;
                Texture2D tex4 = AssetDatabase.LoadAssetAtPath(LMPath+"/Icons/55.png", typeof(Texture2D)) as Texture2D;
                GUIContent c4 = new GUIContent("", tex4, "Feedback");
                supportContent[4] = c4;
                Texture2D tex5 = AssetDatabase.LoadAssetAtPath(LMPath+"/Icons/56.png", typeof(Texture2D)) as Texture2D;
                GUIContent c5 = new GUIContent("", tex5, "Other tools");
                supportContent[5] = c5;
                Texture2D tex6 = AssetDatabase.LoadAssetAtPath(LMPath+"/Icons/57.png", typeof(Texture2D)) as Texture2D;
                GUIContent c6 = new GUIContent("", tex6, "Contact Us");
                supportContent[6] = c6;


            }
		if (style == null) {
           
			style = new GUIStyle ();
			style.normal.textColor = Color.yellow;
			style.onActive.textColor = Color.yellow;
			style.onFocused.textColor = Color.yellow;
			style.onHover.textColor = Color.yellow;
			style.onNormal.textColor = Color.yellow;
			style.hover.textColor = Color.yellow;
			style.focused.textColor = Color.yellow;
			style.active.textColor = Color.yellow;
		}
		toolbarInt = GUILayout.SelectionGrid(toolbarInt, mainContent,5,GUILayout.Height(50));
		toolbarInt = GUILayout.Toolbar (toolbarInt, toolbarStrings);
		var scenepath = EditorApplication.currentScene;
		string[] pathelements = scenepath.Split (new char[] { '.' });
		string fullscenepath = pathelements [0].Replace ("Assets/", string.Empty);
		pathelements = pathelements [0].Split (new char[] { '/' });
		// string scene = pathelements[pathelements.Length - 1];
        
		if (Lightmapping.isRunning)
			baking = true;
		else
			baking = false;

		if (path == "")
			path = Application.dataPath;
		/*
        if (toolbarInt == 0)
        {
            EditorApplication.ExecuteMenuItem("Window/Lightmapping");
            toolbarInt = 1;
            

        }
         else*/
       
		if (toolbarInt == 0) {
			GUILayout.Space(5);
			scrollLoads = GUILayout.BeginScrollView (scrollLoads);
			showPreview = EditorGUILayout.Toggle ("Preview Image", showPreview);
			DirectoryInfo dirinfo = new DirectoryInfo (path + "/Lightmaps");

			if (dirinfo.Exists) {
				if (dirinfo.GetDirectories ().Length == 0)
					GUILayout.Label ("No lightmaps is saved in the current selected folder");
				int k = 0;
				foreach (DirectoryInfo dir in dirinfo.GetDirectories()) {
					if (dir.Name != "TEMPMXD") {
						GUILayout.BeginHorizontal ();

						if (showPreview) {
							GUILayout.Space (120);
							if (File.Exists (dir.FullName + "/Screenshot.png")) {
								Texture2D tex = (Texture2D)AssetDatabase.LoadAssetAtPath ("Assets/Lightmaps/" + dir.Name + "/Screenshot.png", typeof(Texture2D));

								if (tex != null)
									EditorGUI.DrawPreviewTexture (new Rect (10, 30 + k * 100, 100, 100), tex);
								else
									EditorGUI.DrawPreviewTexture (new Rect (10, 30 + k * 100, 100, 100), EditorGUIUtility.whiteTexture);
								// EditorGUILayout.ObjectField(AssetDatabase.LoadAssetAtPath("Assets/Lightmaps/" + dir.Name + "/Screenshot.png", typeof(Texture2D)), typeof(Texture2D),GUILayout.Width(100),GUILayout.Height(100));

							} else
								EditorGUI.DrawPreviewTexture (new Rect (10, 30 + k * 100, 100, 100), EditorGUIUtility.whiteTexture);
						}
						k++;
						GUILayout.BeginVertical ();
						GUILayout.BeginHorizontal ();
						GUILayout.Label (dir.Name, GUILayout.MinWidth (150));
						GUILayout.Label (dir.CreationTime.ToShortDateString () + " " + dir.CreationTime.ToShortTimeString (), GUILayout.Width (120));
						if (GUILayout.Button ("Load", GUILayout.Width (50))) {

                            AssetDatabase.Refresh();
							LightmapSettings.lightmaps = new LightmapData[0];
							object obj = null;
							Directory.Delete(path + "/" + fullscenepath + "/",true);
							Directory.CreateDirectory(path + "/" + fullscenepath + "/");
						
							foreach (FileInfo file in dir.GetFiles()) {
								if (loadType == LoadType.All || loadType == LoadType.Lightmaps) {
									if (file.Name == "Lightmap.xml") {
										Stream ms = file.Open (FileMode.Open, FileAccess.Read, FileShare.Read);
										System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer (typeof(LightmappedScene));
										obj = x.Deserialize (ms);
										ms.Close ();
										//    BinaryFormatter formatter = new BinaryFormatter();
										//    BinaryReader reader = new BinaryReader(file.Open(FileMode.Open, FileAccess.Read, FileShare.Read));
										//    obj = formatter.Deserialize(reader.BaseStream);
										//    reader.BaseStream.Flush();
										//    reader.BaseStream.Close();
										//    reader.Close();


                                    }
                                    else if (file.Name != "Description.txt" && file.Name != "Screenshot.png" && (file.Name.EndsWith(".exr") || file.Name.EndsWith(".asset")) && !file.Name.EndsWith(".meta"))
                                    {
										//  Debug.Log(file.Name);
										if(LightmapSettings.lightmapsMode == LightmapsMode.Directional)
										{
											if(file.Name.Contains("Color") || file.Name.Contains("Scale"))
												file.CopyTo (path + "/" + fullscenepath + "/" + file.Name, true);
											
											if (loadType == LoadType.All || loadType == LoadType.Probes)
											{
												if(file.Name.Contains("LightProbes"))
														file.CopyTo (path + "/" + fullscenepath + "/" + file.Name, true);
													
											}
										}
										else if(LightmapSettings.lightmapsMode == LightmapsMode.Single)
										{
											if(file.Name.Contains("Far"))
												file.CopyTo (path + "/" + fullscenepath + "/" + file.Name, true);
											
											if (loadType == LoadType.All || loadType == LoadType.Probes)
											{
												if(file.Name.Contains("LightProbes"))
														file.CopyTo (path + "/" + fullscenepath + "/" + file.Name, true);
													
											}
										}
										else if(LightmapSettings.lightmapsMode == LightmapsMode.Dual)
										{
											if(file.Name.Contains("Far") || file.Name.Contains("Near"))
												file.CopyTo (path + "/" + fullscenepath + "/" + file.Name, true);
											
											if (loadType == LoadType.All || loadType == LoadType.Probes)
											{
												if(file.Name.Contains("LightProbes"))
														file.CopyTo (path + "/" + fullscenepath + "/" + file.Name, true);
													
											}
										}
									}
									//  Debug.Log("Assets" + "/" + fullscenepath + "/" + file.Name);
									//  AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
                                    if (file.Name.EndsWith(".exr") && !file.Name.EndsWith(".meta"))
                                    {
                                     
                                        AssetDatabase.Refresh();
										TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath ("Assets" + "/" + fullscenepath + "/" + file.Name);
										importer.textureType = TextureImporterType.Lightmap;
										importer.isReadable = true;
										importer.lightmap = true;
										importer.textureFormat = TextureImporterFormat.AutomaticTruecolor;
										AssetDatabase.ImportAsset ("Assets" + "/" + fullscenepath + "/" + file.Name, ImportAssetOptions.ForceSynchronousImport);
									}
								} else if (loadType == LoadType.All || loadType == LoadType.Probes) {
									if (file.Name == "LightProbes.asset") {
										AssetDatabase.ImportAsset ("Assets" + "/" + fullscenepath + "/" + file.Name, ImportAssetOptions.ForceSynchronousImport);
									
										LightmapSettings.lightProbes = (LightProbes)AssetDatabase.LoadAssetAtPath ("Assets" + "/" + fullscenepath + "/" + file.Name, typeof(LightProbes));
									}
								}
							}

							if (obj != null) {
								LightmappedScene sceneobj = (LightmappedScene)obj;
								if (sceneobj != null && sceneobj.Lightmaps != null) {
									//  Debug.Log(sceneobj.Lightmaps.Count);
									// ResizeArray arr = new ResizeArray();

									// var lightmaps = LightmapSettings.lightmaps;
									//  System.Array.Resize<LightmapData>(ref lightmaps, sceneobj.Lightmaps.Count);

									//  LightmapSettings.lightmaps = lightmaps;
									//LightmapSettings.lightmaps = new LightmapData[sceneobj.Lightmaps.Count];

									foreach (var lobj in sceneobj.Lightmaps) {
										//  Debug.Log(sceneobj.Lightmaps.Count);
										LightmapData d = new LightmapData ();
										// if (lobj.Value != null)
										// d.lightmapFar =(Texture2D) Resources.LoadAssetAtPath(dir.FullName + "\\" + lobj.Value.far, typeof(Texture2D));
										if(LightmapSettings.lightmapsMode == LightmapsMode.Dual)
										{
										d.lightmapFar = (Texture2D)AssetDatabase.LoadAssetAtPath ("Assets\\" + fullscenepath + "\\" + lobj.Value.far, typeof(Texture2D));
										d.lightmapNear = (Texture2D)Resources.LoadAssetAtPath ("Assets\\" + fullscenepath + "\\" + lobj.Value.near, typeof(Texture2D));
										}
										else if(LightmapSettings.lightmapsMode == LightmapsMode.Single)
										{
										d.lightmapFar = (Texture2D)AssetDatabase.LoadAssetAtPath ("Assets\\" + fullscenepath + "\\" + lobj.Value.far, typeof(Texture2D));
										
										}
										else if(LightmapSettings.lightmapsMode == LightmapsMode.Directional)
										{
										d.lightmapFar = (Texture2D)AssetDatabase.LoadAssetAtPath ("Assets\\" + fullscenepath + "\\" + lobj.Value.far, typeof(Texture2D));
										d.lightmapNear = (Texture2D)Resources.LoadAssetAtPath ("Assets\\" + fullscenepath + "\\" + lobj.Value.near, typeof(Texture2D));
										}
										//Debug.Log(lobj);
										if (lobj.Key < LightmapSettings.lightmaps.Length)
										{
											LightmapSettings.lightmaps [lobj.Key].lightmapFar = d.lightmapFar;
											LightmapSettings.lightmaps [lobj.Key].lightmapNear = d.lightmapNear;
										}else {

											ResizeArray.Add (lobj.Key, d);
										}
										// LightmapSettings.lightmaps[lobj.Key] = d;
										//  Texture2D far = new Texture2D(1024, 1024);
										//  Texture2D near = new Texture2D(1024, 1024);
										//  far = (Texture2D)AssetDatabase.LoadAssetAtPath(dir.FullName + "\\" + lobj.Value.far, typeof(Texture2D));
										//  LightmapSettings.lightmaps[lobj.Key].lightmapFar = far;
										//  near = (Texture2D)AssetDatabase.LoadAssetAtPath(dir.FullName + "\\" + lobj.Value.near, typeof(Texture2D));
										//  LightmapSettings.lightmaps[lobj.Key].lightmapNear = near;
										// }
										// Debug.Log(LightmapSettings.lightmaps.Length);
									}
									//Debug.Log(LightmapSettings.lightmaps.Length);
								}

								if (EditorUtility.DisplayDialog ("Confirm", "Do you want to load data for selection only?", "Yes", "No")) {

									IndexManager manager = (IndexManager)GameObject.FindObjectOfType (typeof(IndexManager));

									if (manager != null) {

										foreach (var key in sceneobj.LightmappedObjects.Keys) {
											ObjectIndexer indexer = manager.FindByID (int.Parse (key.ToString ()));
											if (indexer != null && indexer.obj != null) {
												if (Selection.Contains (indexer.obj.GetInstanceID ())) {
													GameObject LMobj = (GameObject)EditorUtility.InstanceIDToObject (indexer.obj.GetInstanceID ());
													if (LMobj != null && LMobj.renderer != null && LMobj.isStatic) {
														LMobj.renderer.lightmapIndex = sceneobj.LightmappedObjects [key].mapindex;
														LMobj.renderer.lightmapTilingOffset = new Vector4 (sceneobj.LightmappedObjects [key].tilex, sceneobj.LightmappedObjects [key].tiley, sceneobj.LightmappedObjects [key].offsetx, sceneobj.LightmappedObjects [key].offsety);
													}
												}
											}
										}
									}
								} else {
									IndexManager manager = (IndexManager)GameObject.FindObjectOfType (typeof(IndexManager));

									if (manager != null) {

										foreach (var key in sceneobj.LightmappedObjects.Keys) {
											ObjectIndexer indexer = manager.FindByID (int.Parse (key.ToString ()));
											if (indexer != null && indexer.obj != null) {

												GameObject LMobj = (GameObject)EditorUtility.InstanceIDToObject (indexer.obj.GetInstanceID ());
												if (LMobj != null && LMobj.renderer != null && LMobj.isStatic) {
													LMobj.renderer.lightmapIndex = sceneobj.LightmappedObjects [key].mapindex;
													LMobj.renderer.lightmapTilingOffset = new Vector4 (sceneobj.LightmappedObjects [key].tilex, sceneobj.LightmappedObjects [key].tiley, sceneobj.LightmappedObjects [key].offsetx, sceneobj.LightmappedObjects [key].offsety);
												}

											}
										}
									}

								}
								//UnityEngine.Object[] activeGOs;
								//if (selection)
								//    activeGOs = Selection.GetFiltered(typeof(MeshRenderer), SelectionMode.Editable | SelectionMode.Deep);
								//else
								//    activeGOs = GameObject.FindObjectsOfType(typeof(MeshRenderer));

								//foreach (MeshRenderer activeGO in activeGOs)
								//{
								//    if (sceneobj.LightmappedObjects.ContainsKey(activeGO.gameObject.GetInstanceID()))
								//    {
								//        activeGO.gameObject.isStatic = true;
								//        LightmappedObject lobj = sceneobj.LightmappedObjects[activeGO.gameObject.GetInstanceID()];
								//        if (lobj != null)
								//        {
								//            activeGO.lightmapIndex = lobj.mapindex;
								//            activeGO.lightmapTilingOffset = new Vector4(lobj.tilex, lobj.tiley, lobj.offsetx, lobj.offsety);
								//        }

								//    }

								//}
							}
							//  AssetDatabase.Refresh();
						}
						if (GUILayout.Button ("Delete", GUILayout.Width (50))) {
							if (EditorUtility.DisplayDialog ("Delete", "Are you sure you want to delete?", "Yes", "No")) {
								dir.Delete (true);
								AssetDatabase.Refresh ();
							}
						}
						GUILayout.EndHorizontal ();
						if (File.Exists (dir.FullName + "/Description.txt"))
							GUILayout.TextArea (File.ReadAllText (dir.FullName + "/Description.txt"), GUILayout.Height (80));
						GUILayout.EndVertical ();
						GUILayout.EndHorizontal ();
					}
				}
			}

			
			GUILayout.Space (10);
			loadType = (LoadType)EditorGUILayout.EnumPopup ("Loading Type", loadType);
			LightmapSettings.lightmapsMode = (LightmapsMode) EditorGUILayout.EnumPopup("Load Lightmaps Mode",LightmapSettings.lightmapsMode);
			GUILayout.Label ("Lightmap save\\Open Settings", EditorStyles.boldLabel);
			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Path", EditorStyles.boldLabel);
                
			// GUILayout.TextField(path);
           

			if (GUILayout.Button ("Save\\Open")) {
				path = EditorUtility.SaveFolderPanel ("Save\\Open lightmaps to Directory", path, path);
			}

			GUILayout.EndHorizontal (); 
			selection = GUILayout.Toggle (selection, "Preserve tiling and offset for Selection Only");
		
			Label = EditorGUILayout.TextField ("Name", Label);
			GUILayout.Label ("TEMPMXD is reserved for temporary saving so don't use it to avoid losing data.");
			
			EditorGUILayout.LabelField ("Description", "");
			description = GUILayout.TextArea (description, 150);
			if (GUILayout.Button ("Save Lightmaps")) {
				SaveCurrentLightmapping ();
				AssetDatabase.Refresh ();
			}
            GUILayout.EndScrollView();
		} else if (toolbarInt == 1) {
            # region Settings implementation



			scrollLoads1 = GUILayout.BeginScrollView (scrollLoads1, GUILayout.Height (Screen.height - 100));
			EditorGUILayout.Space ();
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.PrefixLabel ("Mode");
			LightmapSettings.lightmapsMode = (LightmapsMode)EditorGUILayout.EnumPopup (LightmapSettings.lightmapsMode);
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.Space ();
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.PrefixLabel ("Quality");
			LightmapEditorSettings.quality = (LightmapBakeQuality)EditorGUILayout.EnumPopup (LightmapEditorSettings.quality);
			EditorGUILayout.EndHorizontal ();
           
			EditorGUILayout.Space ();
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.PrefixLabel ("Bounces");
			LightmapEditorSettings.bounces = EditorGUILayout.IntPopup (LightmapEditorSettings.bounces, new string[] { "0", "1", "2", "3", "4" }, new int[] { 0, 1, 2, 3, 4 });
			EditorGUILayout.EndHorizontal ();
			if (LightmapEditorSettings.bounces > 0) {
				EditorGUILayout.Space ();
				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.PrefixLabel ("SkyLight Color");
				LightmapEditorSettings.skyLightColor = EditorGUILayout.ColorField (LightmapEditorSettings.skyLightColor);
				EditorGUILayout.EndHorizontal ();

				EditorGUILayout.Space ();
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label ("SkyLight Intensity");
				LightmapEditorSettings.skyLightIntensity = EditorGUILayout.FloatField (LightmapEditorSettings.skyLightIntensity);
				EditorGUILayout.EndHorizontal ();

				EditorGUILayout.Space ();
				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.PrefixLabel ("Bounce Boost");
				LightmapEditorSettings.bounceBoost = EditorGUILayout.Slider (LightmapEditorSettings.bounceBoost, 0, 4);
				EditorGUILayout.EndHorizontal ();

				EditorGUILayout.Space ();
				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.PrefixLabel ("Bounce Intensity");
				LightmapEditorSettings.bounceIntensity = EditorGUILayout.Slider (LightmapEditorSettings.bounceIntensity, 0, 5);
				EditorGUILayout.EndHorizontal ();

				EditorGUILayout.Space ();
				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.PrefixLabel ("Final Gather Rays");
				LightmapEditorSettings.finalGatherRays = EditorGUILayout.IntField (LightmapEditorSettings.finalGatherRays);
				EditorGUILayout.EndHorizontal ();

				EditorGUILayout.Space ();
				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.PrefixLabel ("Contrast Threshold");
				LightmapEditorSettings.finalGatherContrastThreshold = EditorGUILayout.Slider (LightmapEditorSettings.finalGatherContrastThreshold, 0, 0.5f);
				EditorGUILayout.EndHorizontal ();

				EditorGUILayout.Space ();
				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.PrefixLabel ("Interpolation");
				LightmapEditorSettings.finalGatherGradientThreshold = EditorGUILayout.Slider (LightmapEditorSettings.finalGatherGradientThreshold, 0, 1);
				EditorGUILayout.EndHorizontal ();

				EditorGUILayout.Space ();
				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.PrefixLabel ("Interpolation Points");
				LightmapEditorSettings.finalGatherInterpolationPoints = EditorGUILayout.IntSlider (LightmapEditorSettings.finalGatherInterpolationPoints, 15, 30);
				EditorGUILayout.EndHorizontal ();
			}
			EditorGUILayout.Space ();
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.PrefixLabel ("Ambient Occlusion");
			LightmapEditorSettings.aoAmount = EditorGUILayout.Slider (LightmapEditorSettings.aoAmount, 0, 1);
			EditorGUILayout.EndHorizontal ();

			if (LightmapEditorSettings.aoAmount > 0) {

				EditorGUILayout.Space ();
				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.PrefixLabel ("Max Distance");
				LightmapEditorSettings.aoMaxDistance = EditorGUILayout.FloatField (LightmapEditorSettings.aoMaxDistance);
				EditorGUILayout.EndHorizontal ();

				EditorGUILayout.Space ();
				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.PrefixLabel ("Contrast");
				LightmapEditorSettings.aoContrast = EditorGUILayout.Slider (LightmapEditorSettings.aoContrast, 0, 2);
				EditorGUILayout.EndHorizontal ();

			}

			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.PrefixLabel ("Lock Atlas");
			LightmapEditorSettings.lockAtlas = EditorGUILayout.Toggle (LightmapEditorSettings.lockAtlas);
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.Space ();
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.PrefixLabel ("Resolution");
			LightmapEditorSettings.resolution = EditorGUILayout.FloatField (LightmapEditorSettings.resolution);
			EditorGUILayout.LabelField ("Texels per world unity", "");
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.Space ();
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.PrefixLabel ("Texture Compression");
			LightmapEditorSettings.textureCompression = EditorGUILayout.Toggle (LightmapEditorSettings.textureCompression);
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.Space ();
			GUILayout.TextArea ("Set the following field to the targeted size of your baked lightmap.\n reducing the size will increase the number of lightmaps and eventually different objects will have different lightmap, increasing the size will reduce the number of lightmaps and eventually different objects will have single lightmap");
			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label ("Max Atlas Width\\Height");
			LightmapEditorSettings.maxAtlasWidth = EditorGUILayout.IntPopup (LightmapEditorSettings.maxAtlasWidth, new string[] { "32", "64", "128", "256", "512","1024","2048","4096" }, new int[] { 32, 64, 128, 256, 512,1024,2048,4096});

			LightmapEditorSettings.maxAtlasHeight = LightmapEditorSettings.maxAtlasWidth;
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.Space ();

			// EditorGUILayout.BeginHorizontal();
			// EditorGUILayout.LabelField("Last Used Resolution:", LightmapEditorSettings.lastUsedResolution.ToString());
			// EditorGUILayout.EndHorizontal();
         
           
			GUILayout.EndScrollView ();
        
             
            #endregion
		} else if (toolbarInt == 2) {
			if (draw) {
				scrollLoads2 = GUILayout.BeginScrollView (scrollLoads2, GUILayout.Height (Screen.height - 100));
				if (Selection.activeGameObject != null && Selection.activeGameObject.renderer != null) {
					int index = Selection.activeGameObject.renderer.lightmapIndex;
					if (index >= 0 && index < LightmapSettings.lightmaps.Length) {
						GUILayout.BeginHorizontal ();
						GUILayout.BeginVertical ();
						GUILayout.Label ("Near");
						LightmapSettings.lightmaps [index].lightmapNear = (Texture2D)EditorGUILayout.ObjectField (LightmapSettings.lightmaps [index].lightmapNear, typeof(Texture2D), false, GUILayout.Height (100));
						GUILayout.EndVertical ();


						GUILayout.BeginVertical ();
						GUILayout.Label ("Far");
						LightmapSettings.lightmaps [index].lightmapFar = (Texture2D)EditorGUILayout.ObjectField (LightmapSettings.lightmaps [index].lightmapFar, typeof(Texture2D), false, GUILayout.Height (100));
						GUILayout.EndVertical ();
						GUILayout.EndHorizontal ();
						flood0 = EditorGUILayout.Foldout (flood0, "Resize");
						if (flood0) {
							resize = EditorGUILayout.IntPopup (resize, new string[] { "32", "64", "128", "256", "512", "1024", "2048", "4096" }, new int[] { 32, 64, 128, 256, 512, 1024, 2048, 4096 });
							/*
                                   GUILayout.Label("Width");
                                   w1 = EditorGUILayout.IntField(w1);
                                   GUILayout.Label("Height");
                                   h1 = EditorGUILayout.IntField(h1);
                                   GUILayout.Space(10);

                                  */

							GUILayout.BeginHorizontal ();
							if (GUILayout.Button ("Resize Selected")) {

								TextureImporterSettings settings = new TextureImporterSettings ();
								settings.maxTextureSize = resize;
								// int original = LightmapSettings.lightmaps[index].lightmapNear.width;

								TextureImporter importerN;
								TextureImporter importerF;

								if (LightmapSettings.lightmaps [index].lightmapNear != null) {
									var npath = AssetDatabase.GetAssetPath (LightmapSettings.lightmaps [index].lightmapNear);
									importerN = (TextureImporter)TextureImporter.GetAtPath (npath);
									importerN.maxTextureSize = resize;
									//importerN.SetTextureSettings(settings);
									AssetDatabase.ImportAsset (npath);
								}
								if (LightmapSettings.lightmaps [index].lightmapFar != null) {
									var fpath = AssetDatabase.GetAssetPath (LightmapSettings.lightmaps [index].lightmapFar);
									importerF = (TextureImporter)TextureImporter.GetAtPath (fpath);
									//importerF.SetTextureSettings(settings);
									importerF.maxTextureSize = resize;
									AssetDatabase.ImportAsset (fpath);
								}

								//  float dif = resize / original;

							}
							if (GUILayout.Button ("Resize All Lightmaps")) {
								if (EditorUtility.DisplayDialog ("Confirm", "Are you sure you want to resize all lightmaps?", "Yes")) {
									foreach (LightmapData lmd in LightmapSettings.lightmaps) {
										TextureImporter importerN;
										TextureImporter importerF;
										if (lmd.lightmapNear != null) {
											var npath = AssetDatabase.GetAssetPath (lmd.lightmapNear);
											importerN = (TextureImporter)TextureImporter.GetAtPath (npath);
											importerN.maxTextureSize = resize;
											//importerN.SetTextureSettings(settings);
											AssetDatabase.ImportAsset (npath);
										}
										if (lmd.lightmapFar != null) {
											var fpath = AssetDatabase.GetAssetPath (lmd.lightmapFar);
											importerF = (TextureImporter)TextureImporter.GetAtPath (fpath);
											//importerF.SetTextureSettings(settings);
											importerF.maxTextureSize = resize;
											AssetDatabase.ImportAsset (fpath);
										}

									}

								}
							}
							GUILayout.EndHorizontal ();
						}
						GUILayout.Space (5);
						GUILayout.TextArea ("Note that the following tools require you to add its effect as a component to your GameObject in order to run");
						 GUILayout.Space(5);
						flood1 = EditorGUILayout.Foldout (flood1, "Brightness");
						if (flood1) {
							 GUILayout.Space(5);
							brightness = EditorGUILayout.Slider (brightness, -1f, 1f);
							GUILayout.BeginHorizontal ();
							if (GUILayout.Button ("Test Selected")) {
								draw = false;
								SetBrightness (LightmapSettings.lightmaps [index].lightmapNear, brightness);
								SetBrightness (LightmapSettings.lightmaps [index].lightmapFar, brightness);
								draw = true;

							}
							if (GUILayout.Button ("Test All")) {
								draw = false;
								// int j = 0;
								for (int j =0; j<LightmapSettings.lightmaps.Length; j++) {// LightmapData lm in LightmapSettings.lightmaps)
									SetBrightness (LightmapSettings.lightmaps [j].lightmapNear, brightness);
									SetBrightness (LightmapSettings.lightmaps [j].lightmapFar, brightness);
									j++;
								}
								draw = true;
                                    
								//j = 0;
							}
							// if (GUILayout.Button("Save Current"))
							// {
							//     string near =  AssetDatabase.GetAssetPath(LightmapSettings.lightmaps[index].lightmapNear);
							//     string far = AssetDatabase.GetAssetPath(LightmapSettings.lightmaps[index].lightmapFar);

							//Texture2D texture = LightmapSettings.lightmaps[index].lightmapNear;
							//byte[] arr = texture.EncodeToPNG();
							//while(!AssetDatabase.DeleteAsset(near));
							//texture = new Texture2D(255, 255);
							//while(!texture.LoadImage(arr))
							//AssetDatabase.CreateAsset(texture,near);
							//texture = LightmapSettings.lightmaps[index].lightmapFar;
							//arr = texture.EncodeToPNG();
							//while (!AssetDatabase.DeleteAsset(far)) ;
							//texture = new Texture2D(255, 255);
							//while (!texture.LoadImage(arr))
							//    AssetDatabase.CreateAsset(texture, far);
							//AssetDatabase.CreateAsset(LightmapSettings.lightmaps[index].lightmapFar, far);

							//}
							if (GUILayout.Button ("Add Component")) {
								//   SetIsReadable(LightmapSettings.lightmaps[index].lightmapNear);
								//   SetIsReadable(LightmapSettings.lightmaps[index].lightmapFar);
								if (Selection.activeGameObject.GetComponent (typeof(Brightness)) == null) {
									Brightness com = (Brightness)Selection.activeGameObject.AddComponent (typeof(Brightness));
									com.options = Brightness.AdjustLightmap.Both;
									com.level = brightness;
								} else {
									Brightness com = (Brightness)Selection.activeGameObject.GetComponent (typeof(Brightness));
									com.options = Brightness.AdjustLightmap.Both;
									com.level = brightness;
								}

							}

							GUILayout.EndHorizontal ();
							 GUILayout.Space(5);
						}
						GUILayout.Space (5);
						flood2 = EditorGUILayout.Foldout (flood2, "Blend");
						if (flood2) {
							 GUILayout.Space(5);
							GUILayout.TextArea ("Click on the 'Add Blend Effect' button to add the Blend component to the current object");
							if (GUILayout.Button ("Add Blend Effect")) {
								//   SetIsReadable(LightmapSettings.lightmaps[index].lightmapNear);
								//   SetIsReadable(LightmapSettings.lightmaps[index].lightmapFar);
								if (Selection.activeGameObject.GetComponent (typeof(Blend)) == null) {
									Selection.activeGameObject.AddComponent (typeof(Blend));
								}
							}
							 GUILayout.Space(5);
						}
						GUILayout.Space (5);
						flood3 = EditorGUILayout.Foldout (flood3, "Switch");
						if (flood3) {
                    GUILayout.Space(5);
							GUILayout.TextArea ("Click on the 'Add Switch Effect' button to add the Switch component to the current object");
							if (GUILayout.Button ("Add Switch Effect")) {    
								//SetIsReadable(LightmapSettings.lightmaps[index].lightmapNear);
								//SetIsReadable(LightmapSettings.lightmaps[index].lightmapFar);
								if (Selection.activeGameObject.GetComponent (typeof(Switch)) == null) {
									Selection.activeGameObject.AddComponent (typeof(Switch));
								}
							}
							 GUILayout.Space(5);
						}
						//  flood4 = EditorGUILayout.Foldout(flood4, "Tint");
						if (flood4) {
                               
							 GUILayout.Space(5);
							tint = EditorGUILayout.ColorField (tint);
							GUILayout.BeginHorizontal ();
							if (GUILayout.Button ("Test Selected")) {
								draw = false;
								SetTint (LightmapSettings.lightmaps [index].lightmapNear, tint);
								SetTint (LightmapSettings.lightmaps [index].lightmapFar, tint);
								draw = true;

							}
							if (GUILayout.Button ("Test All")) {
								draw = false;
								// int j = 0;
								for (int j=0; j<LightmapSettings.lightmaps.Length; j++) {// LightmapData lm in LightmapSettings.lightmaps)
									SetTint (LightmapSettings.lightmaps [j].lightmapNear, tint);
									SetTint (LightmapSettings.lightmaps [j].lightmapFar, tint);
									j++;
								}
								draw = true;
								// j = 0;
							}
							GUILayout.EndHorizontal ();
							GUILayout.TextArea ("Click on the 'Add Tint Effect' button to add the Tint component to the current object");
							if (GUILayout.Button ("Add Tint Effect")) {
								//  SetIsReadable(LightmapSettings.lightmaps[index].lightmapNear);
								//  SetIsReadable(LightmapSettings.lightmaps[index].lightmapFar);
								if (Selection.activeGameObject.GetComponent (typeof(Tint)) == null) {
									Tint tintComponent = (Tint)Selection.activeGameObject.AddComponent (typeof(Tint));
									tintComponent.TintColor = tint;
								} else {
									Tint tintComponent = (Tint)Selection.activeGameObject.GetComponent (typeof(Tint));
									tintComponent.TintColor = tint;
								}
							}
							 GUILayout.Space(5);
						}

						GUILayout.TextArea ("You need to either use the ADD button to add effects or fix the import settings for the lightmap by clicking the following button or manually.\n\n Blend and switch effects require Textures having the same size in order to work.");
						if (GUILayout.Button ("Fix import settings")) {
							// SetIsReadable(LightmapSettings.lightmaps[index].lightmapNear);
							// SetIsReadable(LightmapSettings.lightmaps[index].lightmapFar);

						}

						//  LightmapSettings.lightmaps[index].lightmapNear.Resize(w1, h1);
						//  LightmapSettings.lightmaps[index].lightmapNear.Resize(w1, h1);
					}



				} else
					GUILayout.Label ("No selected lightmapped object");
				GUILayout.EndScrollView ();
			}
              

		} else if (toolbarInt == 3) {
			ADVtoolbarInt = GUILayout.Toolbar (ADVtoolbarInt, AdvancedStrings);
			if (ADVtoolbarInt == 0) {
				scrollLoads3 = GUILayout.BeginScrollView (scrollLoads3);

				DirectoryInfo dirinfo = new DirectoryInfo (Application.dataPath + "/LightmappedObjects/Objects");

				if (dirinfo.Exists) {
					if (dirinfo.GetFiles ().Length == 0)
						GUILayout.Label ("No saved lightmapped objects.");
					// int k = 0;
					foreach (FileInfo prefab in dirinfo.GetFiles()) {
						if (prefab.Name.Contains (".prefab")) {
							GUILayout.BeginHorizontal ();

							GUILayout.Space (5);
							EditorGUILayout.ObjectField (AssetDatabase.LoadAssetAtPath ("Assets/LightmappedObjects/Objects/" + prefab.Name, typeof(GameObject)), typeof(GameObject), false);
							GUILayout.Label (prefab.CreationTimeUtc.ToShortDateString () + ":" + prefab.CreationTimeUtc.ToShortTimeString (), GUILayout.MinWidth (30));

							GUILayout.EndHorizontal ();
							//Debug.Log(prefab.DirectoryName + "\\" + prefab.Name.Replace(".prefab","") + "_Description.txt");
							if (File.Exists (prefab.DirectoryName + "\\Descriptions\\" + prefab.Name.Replace (".prefab", "") + "_Description.txt"))
								GUILayout.TextArea (File.ReadAllText (prefab.DirectoryName + "\\Descriptions\\" + prefab.Name.Replace (".prefab", "") + "_Description.txt"), GUILayout.Height (40));
						}
					}
				}

			
                   
                   
				SaveAsOnePrefab = GUILayout.Toggle (SaveAsOnePrefab, "Save selection as one prefab");
				SaveInSpecialDirectory = GUILayout.Toggle (SaveInSpecialDirectory, "Save in special Directory (logged in the above list)");
				prefabName = EditorGUILayout.TextField ("Name", prefabName);
				PrefabDescription = EditorGUILayout.TextArea (PrefabDescription);

				if (GUILayout.Button ("Save as Prefab")) {
					if (Selection.gameObjects.Length > 0)
						CreatLightmappedObjects ();
					else
						EditorUtility.DisplayDialog ("Empty Selection", "Please select static gameObject(s) in order to save them", "Ok");
				}
                GUILayout.EndScrollView();
			} else if (ADVtoolbarInt == 1) {
				scrollLoads4 = GUILayout.BeginScrollView (scrollLoads4);

				if (Selection.activeGameObject != null && Selection.activeGameObject.light != null && Selection.activeGameObject.animation != null) {
					animatedLight = Selection.activeGameObject; 
					RecordAnimatedLightmaps record = animatedLight.GetComponent<RecordAnimatedLightmaps> ();
					RecordAnimatedLM recordProgress = animatedLight.GetComponent<RecordAnimatedLM> ();
					GUILayout.Label ("Current selected animated light is : " + Selection.activeGameObject.name);
                      
					if ((recordProgress == null || !recordProgress.baking) && !Lightmapping.isRunning && animationName != "") {
						if (clip == null)
							clip = Selection.activeGameObject.animation.clip;

						clip = (AnimationClip)EditorGUILayout.ObjectField (clip, typeof(AnimationClip), false);
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Label ("Capture Frame Every ");
						RecordRate = EditorGUILayout.FloatField (RecordRate);
						GUILayout.Label ("Second");
						EditorGUILayout.EndHorizontal ();
						animationName = EditorGUILayout.TextField ("Name", animationName);
						animationDescription = GUILayout.TextArea (animationDescription);
						animatedLight = Selection.activeGameObject;

                      
						if (GUILayout.Button ("Record animated lightmaps")) {

							if (recordProgress != null)
								recordProgress.DestroyMe (this);

							if (record != null)
								record.DestroyMe ();
								
							  
							record = animatedLight.AddComponent<RecordAnimatedLightmaps> ();
							record.animatedLight = animatedLight;
							record.clip = clip;
							record.description = animationDescription;
							record.Label = animationName;
								
							recordProgress = animatedLight.AddComponent<RecordAnimatedLM> ();
							RecordAnimatedLMParam param = new RecordAnimatedLMParam ();
							param.animatedLightmaps = record;
							param.caller = this;
							recordProgress.StartCoroutine ("RecordAnimation", param);

							//    RecordAnimatedLightmaps();

						}
						//skipFrame = GUILayout.Toggle(skipFrame,"Skip existing images");
					} else if (recordProgress != null && recordProgress.baking == true) {
						EditorGUI.ProgressBar (new Rect (5, 5, position.width - 5, 20), recordProgress.Sample / clip.length, "Animation Baking Progress( Recording at : " + recordProgress.Sample + " sec)");
						if (GUILayout.Button ("Stop animation baking")) { 
							recordProgress.DestroyMe (this);
							record.DestroyMe ();
						}
					}

					if (record != null && !record.baking) {
						if (previewIndex > record.animationFrames.Length - 1)
							previewIndex = 0;
						GUILayout.Space (10);
						  
						GUILayout.Label ("Switch lightmap to animation frame:");
						int changed = EditorGUILayout.IntSlider (previewIndex, 0, record.animationFrames.Length - 1);
						autoPreview = EditorGUILayout.Toggle ("Auto switch", autoPreview);
						   
						if (changed != previewIndex) {
							previewIndex = changed;
							enterPreview = true;
						}
						if ((autoPreview && enterPreview) || GUILayout.Button ("Switch")) {
							for (int i = 0; i < LightmapSettings.lightmaps.Length; i++) {
								// LightmapData data = LightmapSettings.lightmaps[i];
								if (record.animationFrames [previewIndex].lightmaps.Length > i) {
									//	if(!CheckifLightmap(record.animationFrames[previewIndex].lightmaps[i].far))
									//                SetIsLightmap(record.animationFrames[previewIndex].lightmaps[i].far);
									//   if(!CheckifLightmap(record.animationFrames[previewIndex].lightmaps[i].near))
									//				SetIsLightmap(record.animationFrames[previewIndex].lightmaps[i].near);
									SwitchLM.switchmaps (i, record.animationFrames [previewIndex].lightmaps [i].far, record.animationFrames [previewIndex].lightmaps [i].near);
								   
								}// data.lightmapFar = frdata.lightmaps[i].far;
								// data.lightmapNear = frdata.lightmaps[i].near;
								// LightmapSettings.lightmaps[i] = data;
							}
							enterPreview = false;
						}
						  
					}
					//  AnimationFPS = EditorGUILayout.IntSlider(AnimationFPS, 0, 30);
				} else
					GUILayout.Label ("No Animated light is selected");
                GUILayout.EndScrollView();
			}
			if (GUILayout.Button ("Bake Selection")) {
				Label = "TEMPMXD";
                   
				selectedObjs = Selection.GetFiltered (typeof(MeshRenderer), SelectionMode.Editable | SelectionMode.Deep);
		
				SaveCurrentLightmapping ();
				
			
				
				BakeSelection bake = null;

				IndexManager manager = null;

  
        
	
				GameObject indexerGO = GameObject.Find ("SceneIndexer");
				if (indexerGO == null) {
					indexerGO = new GameObject ("SceneIndexer");
					indexerGO.hideFlags = HideFlags.HideInHierarchy;
                      
				}
					
				bake = (BakeSelection)indexerGO.GetComponent (typeof(BakeSelection));
				if (bake == null)
					bake = (BakeSelection)indexerGO.AddComponent (typeof(BakeSelection));
					
				manager = (IndexManager)indexerGO.GetComponent (typeof(IndexManager));
				if (manager == null)
					manager = (IndexManager)indexerGO.AddComponent (typeof(IndexManager));
					
				if (bake != null)
					bake.BakeSelectedObjects (selectedObjs, this);


              
			}

		} 
		else if (toolbarInt == 4) {
				scrollLoads3 = GUILayout.BeginScrollView (scrollLoads3);
			 GUILayout.Space(20);
              
                GUILayout.Label("About Lightmap Manager 2");
                GUILayout.Space(10);
               // GUILayout.TextArea("Lightmap Manager 2 helps you in saving/loading lightmaps, doing effects such as blending and brightness control of lightmaps as well as baking of selected objects, saving prefabs with lightmap data and animated lightmaps");
                GUILayout.Space(10);
                GUILayout.BeginHorizontal();
                if (GUILayout.Button(supportContent[1], GUILayout.Height(65)))
                {
				AssetDatabase.OpenAsset(manual);
					//EditorUtility.DisplayDialog("Coming Soon","Manual is coming soon","OK");
                   // Help.BrowseURL("http://www.GameDraw3d.com/?page_id=13");
                }
                if (GUILayout.Button(supportContent[2], GUILayout.Height(65)))
                {
				  Help.BrowseURL("http://www.youtube.com/mxdtools");
				  //	EditorUtility.DisplayDialog("YouTube Channle","Tutorials are available on the youtube channel","OK");
                  //  Help.BrowseURL("http://www.GameDraw3d.com/?page_id=17");
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                if (GUILayout.Button(supportContent[3], GUILayout.Height(65)))
                {
                    Help.BrowseURL("mailto:support@mixeddimensions.net");
                }
                if (GUILayout.Button(supportContent[4], GUILayout.Height(65)))
                {
                    Help.BrowseURL("http://mxd.userecho.com/");
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                if (GUILayout.Button(supportContent[5], GUILayout.Height(65)))
                {
                    Help.BrowseURL("http://www.youtube.com/mxdtools");
                }
                if (GUILayout.Button(supportContent[6], GUILayout.Height(65)))
                {
                    Help.BrowseURL("mailto:sales@mixeddimensions.net");
                }
              //  if (GUILayout.Button(supportContent[0], new GUIStyle(), GUILayout.Width(200), GUILayout.Height(291)))
              //  {
              //      Help.BrowseURL("http://www.mixeddimensions.net");
              //  }
                GUILayout.EndHorizontal();
			GUILayout.EndScrollView();
			}
        
		GUILayout.Space (5);
		if(toolbarInt== 0 || toolbarInt==3)
		{
		if (!baking) {
			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Bake",GUILayout.Width(this.position.width/2))) {
                if (EditorUtility.DisplayDialog("Bake", "You will lose the current lightmapping if not saved, do you want to continue?", "Yes", "No"))
                {


                    if (Lightmapping.BakeAsync())
                        baking = true;

                }
			}
			if (GUILayout.Button ("Clear Lightmaps",GUILayout.Width(this.position.width/2))) {
				int val = EditorUtility.DisplayDialogComplex ("Alert", "This will clear the current lightmapping", "Clear and Save", "Cancel", "Clear Only");
				if (val == 0) {
					SaveCurrentLightmapping ();
				} else if (val == 1) {
					//nothing
				} else if (val == 2) {
					Lightmapping.Clear ();
				}

			}
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();
					
			if (GUILayout.Button ("Clear Indexing",GUILayout.Width(this.position.width/2))) {
				GameObject SI = GameObject.Find ("SceneIndexer");
				if (SI != null) {
					bool val = EditorUtility.DisplayDialog ("Alert", "This will clear the indexing of your objects leading to lose previously saved settings thus you want be able to load lightmaps correctly", "Clear", "Cancel");
					if (val) {
						GameObject.DestroyImmediate (SI);
					}
				} else {
					EditorUtility.DisplayDialog ("No indexing Available", "There is no indexing available in this scene, indexing will be generated when you save lightmaps.", "OK");

				}
			}
			if (GUILayout.Button ("Inspect Indexing",GUILayout.Width(this.position.width/2))) {
					
				EditorWindow window;
      
				window = (EditorWindow)EditorWindow.GetWindowWithRect (typeof(IndexerEditor), new Rect (100, 100, 400, 300), false, "Indexing");
				window.autoRepaintOnSceneChange = true;
				window.Show ();
			}
			GUILayout.EndHorizontal ();
                    
		} else {
			if (GUILayout.Button ("Cancel")) {
				Lightmapping.Cancel ();
				baking = false;
			}
		}
		}
       
	}
	public void lockAtlas(bool val)
	{
		LightmapEditorSettings.lockAtlas = val;
	}
	public bool CheckifLightmap (Texture2D tex)
	{
		var path = AssetDatabase.GetAssetPath (tex);
		TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath (path);
		//importerF.SetTextureSettings(settings);
		return (importer.textureType == TextureImporterType.Lightmap);
	}

	public void SetIsLightmap (Texture2D tex)
	{
		var path = AssetDatabase.GetAssetPath (tex);
		TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath (path);
		//importerF.SetTextureSettings(settings);
		importer.textureType = TextureImporterType.Lightmap;
		//  importer.isReadable = true;
		importer.lightmap = true;
		//   importer.textureFormat = TextureImporterFormat.ARGB32;
		AssetDatabase.ImportAsset (path);
	}

	public static void SetIsReadable (Texture2D tex)
	{
		if(tex!=null)
		{
		var path = AssetDatabase.GetAssetPath (tex);
		TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath (path);
		//importerF.SetTextureSettings(settings);
		
        importer.lightmap = true;
		importer.isReadable = true;
		//importer.textureFormat = TextureImporterFormat.ARGB32;
        importer.textureType = TextureImporterType.Lightmap;
       
		AssetDatabase.ImportAsset (path);
        AssetDatabase.Refresh();
		}
		}

	public static bool isReadable (Texture2D tex)
	{
		if(tex!=null)
		{
		var path = AssetDatabase.GetAssetPath (tex);
		TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath (path);
		//importerF.SetTextureSettings(settings);
		return (importer.isReadable && importer.lightmap && importer.textureType == TextureImporterType.Lightmap);
		}
		return false;
		}

	void SetTint (Texture2D tex, Color val)
	{
		//  SetIsReadable(tex);


		AssetDatabase.StartAssetEditing ();
		if (tex != null) {
			var original = tex.GetPixels ();
			Color[] colors = new Color[original.Length];
			for (int i = 0; i < original.Length; i++) {


				UnityEngine.Color c = original [i] * val;


				colors [i] = new UnityEngine.Color (c.r, c.g, c.b, c.a);
				//        tex.SetPixel(x, y, new UnityEngine.Color(r, g, c.b, c.a));


			}
			tex.SetPixels (colors);
			tex.Apply ();
		}
		AssetDatabase.StopAssetEditing ();
	}

	void SetBrightness (Texture2D tex, float val)
	{
    
      
		//     SetIsReadable(tex);
        

		AssetDatabase.StartAssetEditing ();
		var original = tex.GetPixels ();
		Color[] colors = new Color[original.Length];
		for (int i = 0; i < original.Length; i++) {


			UnityEngine.Color c = original [i];

			float r = c.r + val;
			float g = c.g + val;
			float b = c.b + val;
           
			if (r > 1) {
                
				r = 1;

			} else if (r < 0) {
				r = 0;
			}
			if (g > 1) {

				g = 1;
			} else if (g < 0) {
				g = 0;
			}
			if (b > 1) {

				b = 1;
			} else if (b < 0) {
				b = 0;
			}
           
			colors [i] = new UnityEngine.Color (r, g, b, c.a);
			//        tex.SetPixel(x, y, new UnityEngine.Color(r, g, c.b, c.a));


		}
		tex.SetPixels (colors);
		tex.Apply ();
		// Debug.Log(AssetDatabase.GetAssetPath(tex.GetInstanceID()));

        
		//  File.WriteAllBytes(AssetDatabase.GetAssetPath(tex.GetInstanceID()).Replace("exr","png"), tex.EncodeToPNG());
		// AssetDatabase.SaveAssets();
		// EditorApplication.SaveAssets();
		//  AssetDatabase.AddObjectToAsset(tex,AssetDatabase.GetAssetPath(tex.GetInstanceID()));
        
		AssetDatabase.StopAssetEditing ();
		//AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
	}
    
	public void SaveCurrentLightmapping ()
	{
		var del = true;
		IndexManager manager;

		GameObject indexerGO = GameObject.Find ("SceneIndexer");
		if (indexerGO == null) {
			indexerGO = new GameObject ("SceneIndexer");
			indexerGO.hideFlags = HideFlags.HideInHierarchy;
			manager = (IndexManager)indexerGO.AddComponent (typeof(IndexManager));
		} else {
			manager = (IndexManager)indexerGO.GetComponent (typeof(IndexManager));
		}
		//   LightmapEditorSettings.lockAtlas = true;
		if (Label == string.Empty)
			EditorUtility.DisplayDialog ("Label Empty", "Please specify a name for this lightmap version, note that if available it will override the previous one", "Ok");
		else {
			var scenepath = EditorApplication.currentScene;
			string[] pathelements = scenepath.Split (new char[] { '.' });
			string fullscenepath = pathelements [0].Replace ("Assets/", string.Empty);
			pathelements = pathelements [0].Split (new char[] { '/' });
			// string scene = pathelements[pathelements.Length - 1];
      
              
			var path = Application.dataPath;
			//  DirectoryInfo dirinfopath = new DirectoryInfo(path);
			//  path = dirinfopath.parent.FullName;


			if (!Directory.Exists (path + "/Lightmaps")) {
				Directory.CreateDirectory (path + "/Lightmaps");
            
			}
			if (Directory.Exists (path + "/Lightmaps/" + Label)) {
				if (Label == "TEMPMXD" || EditorUtility.DisplayDialog ("Confirm", "There is a lightmap version with the same name, write over it?", "Yes", "No")) {

					Directory.Delete (path + "/Lightmaps/" + Label, true);
                  
				} else
					del = false;
                       
			}
			if (del) {
				Directory.CreateDirectory (path + "/Lightmaps/" + Label);
				DirectoryInfo dirinfo = new DirectoryInfo (path + "/" + fullscenepath);
				//while (!Directory.Exists(path + "/Lightmaps/" + Label)) ;
				if (!File.Exists (path + "/Lightmaps/" + Label + "/Description.txt")) {
					TextWriter tw = File.CreateText (path + "/Lightmaps/" + Label + "/Description.txt");

					//    new StreamWriter(path + "/Lightmaps/" + Label + "/Description.txt");

					// write a line of text to the file
					tw.Write (description);
					tw.Close ();
				}
				// close the stream
				// EditorApplication.isPlaying = true;
				// while (EditorApplication.isCompiling) ;
              
				Application.CaptureScreenshot (path + "/Lightmaps/" + Label + "/Screenshot.png");
				// RenderTexture texture = new RenderTexture(100,100,1000);
				//  Camera.main.targetTexture = texture;
				//  Camera.main.Render();
				// RenderTexture.active = texture;
				// Texture2D myTexture2D = new Texture2D(100,100);
				// myTexture2D.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
				// myTexture2D.Apply();
				// byte[] bytes = myTexture2D.EncodeToPNG();
				// File.WriteAllBytes(path + "/Lightmaps/" + Label + "/Screenshot.png", bytes);
				// EditorApplication.isPlaying = false;
				// AssetDatabase.ImportAsset(path + "/Lightmaps/" + Label + "/Screenshot.png", ImportAssetOptions.ForceSynchronousImport);
				//if (SceneView.sceneViews[0] != null && ((SceneView)SceneView.sceneViews[0]).camera != null)
				//{
                   
				//    RenderTexture rt = new RenderTexture((int)position.width, (int)position.height,16, RenderTextureFormat.ARGB32);
				//    RenderTexture.active = rt;
				//    SceneView.lastActiveSceneView.camera.targetTexture = rt;
				//    SceneView.lastActiveSceneView.camera.Render();
             
				//    Texture2D texture = new Texture2D((int)position.width, (int)position.height);
				//    texture.ReadPixels(new Rect(0, 0, (int)position.width, (int)position.height), 0, 0);
				//    texture.Apply();
                   
				//    Camera.main.targetTexture = null;
				//    File.WriteAllBytes(path + "/Lightmaps/" + Label + "/Screenshot.png", texture.EncodeToPNG());
				//}
				LightmappedScene lscene = new LightmappedScene ();

				int i = 0;

				for (i = 0; i < LightmapSettings.lightmaps.Length; i++) {
					
					if(LightmapSettings.lightmapsMode == LightmapsMode.Dual)
					{
					// (LightmapData data in LightmapSettings.lightmaps)
					var lm = new Lightmap ("LightmapFar-" + i.ToString () + ".exr", "LightmapNear-" + i.ToString () + ".exr");
					lscene.Lightmaps.Add (i, lm);
					}
					else if(LightmapSettings.lightmapsMode == LightmapsMode.Single)
					{
					// (LightmapData data in LightmapSettings.lightmaps)
					var lm = new Lightmap ("LightmapFar-" + i.ToString () + ".exr","");
					lscene.Lightmaps.Add (i, lm);
					}
					else if(LightmapSettings.lightmapsMode == LightmapsMode.Directional)
					{
					// (LightmapData data in LightmapSettings.lightmaps)
					var lm = new Lightmap ("LightmapColor-" + i.ToString () + ".exr", "LightmapScale-" + i.ToString () + ".exr");
					lscene.Lightmaps.Add (i, lm);
					}

				}

				foreach (FileInfo file in dirinfo.GetFiles()) {
                    if (file.Name != "LightProbes.asset" && !file.Name.EndsWith(".meta"))
                    {
                        file.CopyTo(path + "/Lightmaps/" + Label + "/" + file.Name);
                        if (file.Name.EndsWith(".exr"))
                        {
                            AssetDatabase.Refresh();
                            TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath("Assets" + "/Lightmaps/" + Label + "/" + file.Name);
                            importer.textureType = TextureImporterType.Lightmap;
                            importer.isReadable = true;
                            importer.lightmap = true;
                            importer.textureFormat = TextureImporterFormat.AutomaticTruecolor;
                        }

                    }
                    else
                    {

                        AssetDatabase.CopyAsset(scenepath.Replace(".unity", "") + "/LightProbes.asset", path + "/Lightmaps/" + Label + "/LightProbes.asset");
                    }
				}
                AssetDatabase.Refresh();

				UnityEngine.Object[] activeGOs;

				if (selection)
					activeGOs = Selection.GetFiltered (typeof(MeshRenderer), SelectionMode.Editable | SelectionMode.Deep);
				else
					activeGOs = GameObject.FindObjectsOfType (typeof(MeshRenderer));

				foreach (MeshRenderer activeGO in activeGOs) {
					if (activeGO.gameObject.isStatic) {
						ObjectIndexer indexer;
						Vector4 vec = activeGO.lightmapTilingOffset;

						indexer = manager.FindByID (activeGO.gameObject.GetInstanceID ());

						if (indexer == null) {
							// indexer = ObjectIndexer.CreateInstance<ObjectIndexer>();
                         
							// indexer.ID = activeGO.gameObject.GetInstanceID();
							// indexer.obj = activeGO.gameObject;
							indexer = new ObjectIndexer (activeGO.gameObject.GetInstanceID (), activeGO.gameObject);
							manager.SceneObjects.Add (indexer);

							//  indexer = (ObjectIndexer)activeGO.gameObject.AddComponent(typeof(ObjectIndexer));
							//  indexer.index = activeGO.gameObject.GetInstanceID();
						}
                    
						if (lscene != null && lscene.LightmappedObjects != null && !lscene.LightmappedObjects.ContainsKey (indexer.ID))
							lscene.LightmappedObjects.Add (indexer.ID, new LightmappedObject (vec.x, vec.y, vec.z, vec.w, activeGO.lightmapIndex));

					}
				}
				//  if (!File.Exists(path + "/Lightmaps/Lightmap.xml"))
				//   File.Create(path + "/Lightmaps/Lightmap.xml");
				Stream ms = File.OpenWrite (path + "/Lightmaps/" + Label + "/Lightmap.xml");
				//   BinaryFormatter formatter = new BinaryFormatter();
				//   formatter.Serialize(ms, lscene);
				//   ms.Flush();
				//   ms.Close();
				//   ms.Dispose();
				//   formatter = null;
				System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer (lscene.GetType ());
				x.Serialize (ms, lscene);
				ms.Close ();
				// EditorApplication.isPlaying = true;
               
              
				// EditorApplication.isPlaying = false;
				// AssetDatabase.Refresh();

			}
		}
	}

	void ConvertToLegacy ()
	{
		UnityEngine.Object[] activeGOs = Selection.GetFiltered (typeof(MeshRenderer), SelectionMode.Editable | SelectionMode.Deep);

		Shader sh1 = Shader.Find ("Bumped Diffuse");
		Shader sh2 = Shader.Find ("Bumped Specular");
		Shader sh3 = Shader.Find ("Specular");
		Shader sh4 = Shader.Find ("vertexlit");


		Shader sh1l = Shader.Find ("Legacy Shaders/Lightmapped/Bumped Diffuse");
		Shader sh2l = Shader.Find ("Legacy Shaders/Lightmapped/Bumped Specular");
		Shader sh3l = Shader.Find ("Legacy Shaders/Lightmapped/Specular");
		Shader sh4l = Shader.Find ("Legacy Shaders/Lightmapped/vertexlit");

		Vector4 offset;
            
		int index;

		var scenepath = EditorApplication.currentScene;
		string[] pathelements = scenepath.Split (new char[] { '.' });
		pathelements = pathelements [0].Split (new char[] { '/' });
		string scene = pathelements [pathelements.Length - 1];

		foreach (MeshRenderer activeGO in activeGOs) {
			if (activeGO.gameObject.isStatic) {
				foreach (Material mat in activeGO.sharedMaterials) {
					offset = activeGO.lightmapTilingOffset;
					index = activeGO.lightmapIndex;

					if (mat.shader == sh1) { 
						mat.shader = sh1l;
						mat.SetTextureOffset ("_LightMap", new Vector2 (offset.z, offset.w));
						mat.SetTextureScale ("_LightMap", new Vector2 (offset.x, offset.y));
						// Debug.Log("Assets/"+scene + "/LightmapNear-" + index.ToString() + ".exr");
						Texture2D tex = (Texture2D)AssetDatabase.LoadAssetAtPath ("Assets/" + scene + "/LightmapNear-" + index.ToString () + ".exr", typeof(Texture2D));
						mat.SetTexture ("_LightMap", tex);
						activeGO.lightmapIndex = 255;

						//   MeshFilter mesh =(MeshFilter) activeGO.gameObject.GetComponent(typeof(MeshFilter));
                            
						//    mesh.mesh.uv2 = mesh.mesh.uv1;
                        


					} else if (mat.shader == sh2) {
						mat.shader = sh2l;
						mat.SetTextureOffset ("_LightMap", new Vector2 (offset.z, offset.w));
						mat.SetTextureScale ("_LightMap", new Vector2 (offset.x, offset.y));
						Texture2D tex = (Texture2D)AssetDatabase.LoadAssetAtPath ("Assets/" + scene + "/LightmapNear-" + index.ToString () + ".exr", typeof(Texture2D));
						mat.SetTexture ("_LightMap", tex);

					} else if (mat.shader == sh3) {
						mat.shader = sh3l;
						mat.SetTextureOffset ("_LightMap", new Vector2 (offset.z, offset.w));
						mat.SetTextureScale ("_LightMap", new Vector2 (offset.x, offset.y));
						Texture2D tex = (Texture2D)AssetDatabase.LoadAssetAtPath ("Assets/" + scene + "/LightmapNear-" + index.ToString () + ".exr", typeof(Texture2D));
						mat.SetTexture ("_LightMap", tex);

					} else if (mat.shader == sh4) {
						mat.shader = sh4l;
						mat.SetTextureOffset ("_LightMap", new Vector2 (offset.z, offset.w));
						mat.SetTextureScale ("_LightMap", new Vector2 (offset.x, offset.y));
						Texture2D tex = (Texture2D)AssetDatabase.LoadAssetAtPath ("Assets/" + scene + "/LightmapNear-" + index.ToString () + ".exr", typeof(Texture2D));
						mat.SetTexture ("_LightMap", tex);

					}


				}
			}
		}
    
	}

	public void CreatLightmappedObjects ()
	{
        #region for tracking purposes
		//LightmappedObjectManager manager;

		//GameObject indexerGO = GameObject.Find("LightmappedObjectIndexer");
		//if (indexerGO == null)
		//{
		//    indexerGO = new GameObject("LightmappedObjectIndexer");
		//    indexerGO.hideFlags = HideFlags.HideInHierarchy;
		//    manager = (LightmappedObjectManager)indexerGO.AddComponent(typeof(LightmappedObjectManager));
		//}
		//else
		//{
		//    manager = (LightmappedObjectManager)indexerGO.GetComponent(typeof(LightmappedObjectManager));
		//}
        #endregion

		if (prefabName == "")
			prefabName = "LightmappedObject";

		GameObject parent = null;
		string path = "";
		if (SaveAsOnePrefab) {
			if (!EditorUtility.DisplayDialog ("Warnning", "Saving as one prefab may lead to losing the current hierarchy and objects may change there parent, are you sure you want to save as one prefab?", "Yes", "No (save every object alone)"))
				SaveAsOnePrefab = false;
		}
		if (SaveAsOnePrefab) {
           
			parent = new GameObject ();
			parent.transform.position = Selection.activeTransform.position;
			// LightmappedObjectPrefabs prefabindex = new LightmappedObjectPrefabs(parent);

			if (SaveInSpecialDirectory) {
				if (!Directory.Exists (Application.dataPath + "/LightmappedObjects/Objects"))
					Directory.CreateDirectory (Application.dataPath + "/LightmappedObjects/Objects");

				path = "Assets/LightmappedObjects/Objects/" + prefabName + ".prefab";

			} else {
				path = EditorUtility.SaveFilePanelInProject ("Save lightmapped prefab",
                                     prefabName + ".prefab",
                                    "prefab",
                                    "Please enter a file name to save the lightmapped prefab to");
			}
			if (path != "" && parent != null) {
				string[] names = path.Split (new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
				parent.name = names [names.Length - 1].Replace (".prefab", "");
			}
		}
		//Selection.GetTransforms(SelectionMode.Deep)
		int counter = 0;
		foreach (Transform tr in Selection.transforms) {
			GameObject go = tr.gameObject;

			if (!SaveAsOnePrefab) {
				if (SaveInSpecialDirectory) {
					if (!Directory.Exists (Application.dataPath + "/LightmappedObjects/Objects"))
						Directory.CreateDirectory (Application.dataPath + "/LightmappedObjects/Objects");
					
					if (Selection.transforms.Length > 1)
						path = "Assets/LightmappedObjects/Objects/" + prefabName + counter.ToString () + ".prefab";
					else
						path = "Assets/LightmappedObjects/Objects/" + prefabName + ".prefab";
				} else {
					path = EditorUtility.SaveFilePanelInProject ("Save lightmapped prefab",
                                      prefabName + ".prefab",
                                     "prefab",
                                     "Please enter a file name to save the lightmapped prefab to");
				}
			} else if (path != "" && parent != null)
				go.transform.parent = parent.transform;

              
			SaveLightmapData (go, path);

			if (path != "") {
				String localPath = path;
				if (!SaveAsOnePrefab) {
					if (AssetDatabase.LoadAssetAtPath (localPath, typeof(GameObject))) {
						if (EditorUtility.DisplayDialog ("Are you sure?",
                               "The prefab already exists. Do you want to overwrite it?",
                               "Yes",
                               "No"))
							createNew (go, localPath);
					} else {
						createNew (go, localPath);
					}

					if (SaveInSpecialDirectory) {
						if (!Directory.Exists (Application.dataPath + "/LightmappedObjects/Objects/Descriptions/"))
							Directory.CreateDirectory (Application.dataPath + "/LightmappedObjects/Objects/Descriptions/");
						
						TextWriter tw;
						
						if (Selection.transforms.Length > 1)
							tw = File.CreateText (Application.dataPath + "/LightmappedObjects/Objects/Descriptions/" + prefabName + counter.ToString () + "_Description.txt");
						else
							tw = File.CreateText (Application.dataPath + "/LightmappedObjects/Objects/Descriptions/" + prefabName + "_Description.txt");
						//    new StreamWriter(path + "/Lightmaps/" + Label + "/Description.txt");

						// write a line of text to the file
						tw.Write (PrefabDescription);
						tw.Close ();
					}
				}
			}
			counter++;
		}
           
		if (SaveAsOnePrefab && path != "") {
			if (AssetDatabase.LoadAssetAtPath (path, typeof(GameObject))) {
				if (EditorUtility.DisplayDialog ("Are you sure?",
                       "The prefab already exists. Do you want to overwrite it?",
                       "Yes",
                       "No"))
					createNew (parent, path);
			} else {
				createNew (parent, path);
			}
			if (SaveInSpecialDirectory) {
				if (!Directory.Exists (Application.dataPath + "/LightmappedObjects/Objects/Descriptions/"))
					Directory.CreateDirectory (Application.dataPath + "/LightmappedObjects/Objects/Descriptions/");

				TextWriter tw = File.CreateText (Application.dataPath + "/LightmappedObjects/Objects/Descriptions/" + prefabName + "_Description.txt");

				//    new StreamWriter(path + "/Lightmaps/" + Label + "/Description.txt");

				// write a line of text to the file
				tw.Write (PrefabDescription);
				tw.Close ();
			}
		}
	}

	public  void SaveLightmapData (GameObject go1, string path)
	{
		foreach (UnityEngine.Object obj in EditorUtility.CollectDeepHierarchy(new UnityEngine.Object[] { go1 })) {
			if (obj.GetType () == typeof(GameObject)) {
				GameObject go = (GameObject)obj;

				if (go.isStatic && go.renderer != null && go.renderer.lightmapIndex != -1 && go.renderer.lightmapIndex != 255 && go.renderer.lightmapIndex != 254 && path != "") {
					MeshRenderer renderer = go.renderer as MeshRenderer;
					LightmapData data = LightmapSettings.lightmaps [renderer.lightmapIndex];
					string far = AssetDatabase.GetAssetPath (data.lightmapFar);
					string near = AssetDatabase.GetAssetPath (data.lightmapNear);
					//Debug.Log(far+" : "+near);



					if (path != "") {
						AddLightmap addlm = go.GetComponent<AddLightmap> ();

						if (addlm == null)
							addlm = go.AddComponent<AddLightmap> ();

						// var apppath = Application.dataPath + path.Remove(0, 6);
						string[] filename = path.Split (new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
						if (!Directory.Exists (Application.dataPath + "/LightmappedObjects/Lightmaps/" + filename [filename.Length - 1]))
							Directory.CreateDirectory (Application.dataPath + "/LightmappedObjects/Lightmaps/" + filename [filename.Length - 1]);
						string[] farnames = far.Split (new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
						string[] nearnames = near.Split (new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

                        if (File.Exists(Application.dataPath + "/LightmappedObjects/Lightmaps/" + filename[filename.Length - 1] + "/" + farnames[farnames.Length - 1]))
                        {
                            TextureImporter importer = TextureImporter.GetAtPath("Assets/LightmappedObjects/Lightmaps/" + filename[filename.Length - 1] + "/" + farnames[farnames.Length - 1]) as TextureImporter;
                            importer.lightmap = true;
                            importer.textureType = TextureImporterType.Lightmap;
                            AssetDatabase.ImportAsset("Assets/LightmappedObjects/Lightmaps/" + filename[filename.Length - 1] + "/" + farnames[farnames.Length - 1],ImportAssetOptions.Default);
                           
                            addlm.far = (Texture2D)Resources.LoadAssetAtPath("Assets/LightmappedObjects/Lightmaps/" + filename[filename.Length - 1] + "/" + farnames[farnames.Length - 1], typeof(Texture2D));
                            
                        }
                        else
                        {
							FileInfo info = new FileInfo (Application.dataPath + far.Remove (0, 6));
							info.CopyTo (Application.dataPath + "/LightmappedObjects/Lightmaps/" + filename [filename.Length - 1] + "/" + farnames [farnames.Length - 1]);
                            AssetDatabase.Refresh();
                            TextureImporter importer = TextureImporter.GetAtPath("Assets/LightmappedObjects/Lightmaps/" + filename[filename.Length - 1] + "/" + farnames[farnames.Length - 1]) as TextureImporter;
                            importer.lightmap = true;
                            importer.textureType = TextureImporterType.Lightmap;
                            AssetDatabase.ImportAsset("Assets/LightmappedObjects/Lightmaps/" + filename[filename.Length - 1] + "/" + farnames[farnames.Length - 1], ImportAssetOptions.Default);
							//  AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
							addlm.far = (Texture2D)Resources.LoadAssetAtPath ("Assets/LightmappedObjects/Lightmaps/" + filename [filename.Length - 1] + "/" + farnames [farnames.Length - 1], typeof(Texture2D));
							//   SetIsReadable((Texture2D)Resources.LoadAssetAtPath("Assets/LightmappedObjects/" + filename[filename.Length - 1] + "/" + farnames[farnames.Length - 1], typeof(Texture2D)));
						}
						if(nearnames.Length>0)
						{
						if (File.Exists (Application.dataPath + "/LightmappedObjects/Lightmaps/" + filename [filename.Length - 1] + "/" + nearnames [nearnames.Length - 1])) {
                            TextureImporter importer = TextureImporter.GetAtPath("Assets/LightmappedObjects/Lightmaps/" + filename[filename.Length - 1] + "/" + nearnames[nearnames.Length - 1]) as TextureImporter;
                            importer.lightmap = true;
                            importer.textureType = TextureImporterType.Lightmap;
                            AssetDatabase.ImportAsset("Assets/LightmappedObjects/Lightmaps/" + filename[filename.Length - 1] + "/" + nearnames[nearnames.Length - 1], ImportAssetOptions.Default);
                            addlm.near = (Texture2D)Resources.LoadAssetAtPath ("Assets/LightmappedObjects/Lightmaps/" + filename [filename.Length - 1] + "/" + nearnames [nearnames.Length - 1], typeof(Texture2D));
						} else {
							FileInfo info = new FileInfo (Application.dataPath + near.Remove (0, 6));
							info.CopyTo (Application.dataPath + "/LightmappedObjects/Lightmaps/" + filename [filename.Length - 1] + "/" + nearnames [nearnames.Length - 1]);
                            AssetDatabase.Refresh();
                            TextureImporter importer = TextureImporter.GetAtPath("Assets/LightmappedObjects/Lightmaps/" + filename[filename.Length - 1] + "/" + nearnames[nearnames.Length - 1]) as TextureImporter;
                            importer.lightmap = true;
                            importer.textureType = TextureImporterType.Lightmap; 
                            AssetDatabase.ImportAsset("Assets/LightmappedObjects/Lightmaps/" + filename[filename.Length - 1] + "/" + nearnames[nearnames.Length - 1], ImportAssetOptions.Default);
							// AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
							addlm.near = (Texture2D)Resources.LoadAssetAtPath ("Assets/LightmappedObjects/Lightmaps/" + filename [filename.Length - 1] + "/" + nearnames [nearnames.Length - 1], typeof(Texture2D));
							//  SetIsReadable((Texture2D)Resources.LoadAssetAtPath("Assets/LightmappedObjects/" + filename[filename.Length - 1] + "/" + nearnames[nearnames.Length - 1], typeof(Texture2D)));
						}
						}

					}
				}
			}
		}
	}

	public  void createNew (GameObject obj, String localPath)
	{
        

		UnityEngine.Object prefab = PrefabUtility.CreateEmptyPrefab (localPath);
      
		PrefabUtility.ReplacePrefab (obj, prefab);
       
		// AssetDatabase.Refresh();
      
		//DestroyImmediate(obj);
		//GameObject clone = EditorUtility.InstantiatePrefab(prefab) as GameObject; 
	}

	public void AddLightmapComponent (GameObject obj)
	{
		AddLightmap addLM = obj.GetComponent<AddLightmap> ();
		if(addLM != null)
		addLM.Update ();
	}

	public	void CreatLightmappedObjects (UnityEngine.Object[] objs, ArrayList cached)
	{
        #region for tracking purposes
		//LightmappedObjectManager manager;

		//GameObject indexerGO = GameObject.Find("LightmappedObjectIndexer");
		//if (indexerGO == null)
		//{
		//    indexerGO = new GameObject("LightmappedObjectIndexer");
		//    indexerGO.hideFlags = HideFlags.HideInHierarchy;
		//    manager = (LightmappedObjectManager)indexerGO.AddComponent(typeof(LightmappedObjectManager));
		//}
		//else
		//{
		//    manager = (LightmappedObjectManager)indexerGO.GetComponent(typeof(LightmappedObjectManager));
		//}
        #endregion

      
		// string prefabName = "TEMPMXD";
		bool SaveInSpecialDirectory = true;
		// GameObject parent = null;
		string path = "";


		//Selection.GetTransforms(SelectionMode.Deep)
      
		foreach (MeshRenderer obj in objs) {
          

			GameObject go = obj.gameObject;

			if (SaveInSpecialDirectory) {
				if (Directory.Exists (Application.dataPath + "/LightmappedObjects/Objects/TMP/" + go.GetInstanceID ()))
					Directory.Delete (Application.dataPath + "/LightmappedObjects/Objects/TMP/" + go.GetInstanceID (), true);

				Directory.CreateDirectory (Application.dataPath + "/LightmappedObjects/Objects/TMP/" + go.GetInstanceID ());

				path = "Assets/LightmappedObjects/Objects/TMP/" + go.GetInstanceID () + "/" + go.name + ".prefab";
			}
             
           

			SaveLightmapDataTMP (go, path);

			if (path != "") {
				string localPath = path;
                
				if (AssetDatabase.LoadAssetAtPath (localPath, typeof(GameObject))) {
                       
					createNew (go, localPath, cached);
				} else {
					createNew (go, localPath, cached);
				}

                  
                
			}
		}

	}

	public   void SaveLightmapDataTMP (GameObject go1, string path)
	{
		foreach (UnityEngine.Object obj in EditorUtility.CollectDeepHierarchy(new UnityEngine.Object[] { go1 })) {
			if (obj.GetType () == typeof(GameObject)) {
				GameObject go = (GameObject)obj;

				if (go.isStatic && go.renderer != null && go.renderer.lightmapIndex != -1 && go.renderer.lightmapIndex != 255 && go.renderer.lightmapIndex != 254 && path != "") {
					MeshRenderer renderer = go.renderer as MeshRenderer;
					LightmapData data = LightmapSettings.lightmaps [renderer.lightmapIndex];
					string far = AssetDatabase.GetAssetPath (data.lightmapFar);
					string near = AssetDatabase.GetAssetPath (data.lightmapNear);
					//Debug.Log(far+" : "+near);



					if (path != "") {
						AddLightmap addlm = go.GetComponent<AddLightmap> ();

						if (addlm == null)
							addlm = go.AddComponent<AddLightmap> ();

						// var apppath = Application.dataPath + path.Remove(0, 6);
						string[] filename = path.Split (new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
						if (Directory.Exists (Application.dataPath + "/LightmappedObjects/Lightmaps/TMP/" + go.GetInstanceID () + "/" + filename [filename.Length - 1]))
							Directory.Delete (Application.dataPath + "/LightmappedObjects/Lightmaps/TMP/" + go.GetInstanceID () + "/" + filename [filename.Length - 1], true);

						Directory.CreateDirectory (Application.dataPath + "/LightmappedObjects/Lightmaps/TMP/" + go.GetInstanceID () + "/" + filename [filename.Length - 1]);
						string[] farnames = far.Split (new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
						string[] nearnames = near.Split (new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

                       
						FileInfo info = new FileInfo (Application.dataPath + far.Remove (0, 6));
						info.CopyTo (Application.dataPath + "/LightmappedObjects/Lightmaps/TMP/" + go.GetInstanceID () + "/" + filename [filename.Length - 1] + "/" + farnames [farnames.Length - 1]);
						// AssetDatabase.ImportAsset("Assets/LightmappedObjects/Lightmaps/TMP/" + go.GetInstanceID() + "/" + filename[filename.Length - 1] + "/" + farnames[farnames.Length - 1], ImportAssetOptions.ImportRecursive);
						//AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
						addlm.far = (Texture2D)Resources.LoadAssetAtPath ("Assets/LightmappedObjects/Lightmaps/TMP/" + go.GetInstanceID () + "/" + filename [filename.Length - 1] + "/" + farnames [farnames.Length - 1], typeof(Texture2D));
						   SetIsReadable((Texture2D)Resources.LoadAssetAtPath("Assets/LightmappedObjects/" + filename[filename.Length - 1] + "/" + farnames[farnames.Length - 1], typeof(Texture2D)));
                       

						if(near!=null && near.Length>6)
						{
						info = new FileInfo (Application.dataPath + near.Remove (0, 6));
						info.CopyTo (Application.dataPath + "/LightmappedObjects/Lightmaps/TMP/" + go.GetInstanceID () + "/" + filename [filename.Length - 1] + "/" + nearnames [nearnames.Length - 1]);
						// AssetDatabase.ImportAsset("Assets/LightmappedObjects/Lightmaps/TMP/" + go.GetInstanceID() + "/" + filename[filename.Length - 1] + "/" + nearnames[nearnames.Length - 1], ImportAssetOptions.ImportRecursive);
						//AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
						addlm.near = (Texture2D)Resources.LoadAssetAtPath ("Assets/LightmappedObjects/Lightmaps/TMP/" + go.GetInstanceID () + "/" + filename [filename.Length - 1] + "/" + nearnames [nearnames.Length - 1], typeof(Texture2D));
						 SetIsReadable((Texture2D)Resources.LoadAssetAtPath("Assets/LightmappedObjects/" + filename[filename.Length - 1] + "/" + nearnames[nearnames.Length - 1], typeof(Texture2D)));
						}

					}
				}
			}
		}
	}
	public void RefreshAssetDatabase()
	{
		AssetDatabase.Refresh();
	}
	
	public void createNew (GameObject obj, string localPath, ArrayList cached)
	{


		UnityEngine.Object prefab = PrefabUtility.CreateEmptyPrefab (localPath);
		//  AddLightmap addlm = obj.GetComponent<AddLightmap>();

		//  if (addlm == null)
		//      addlm = obj.AddComponent<AddLightmap>();

		PrefabUtility.ReplacePrefab (obj, prefab);

		//  AssetDatabase.Refresh();


		// EditorUtility.InstantiatePrefab(prefab);
       

		cached.Add (obj);
		// clone.transform.position = obj.transform.position;
      

	}

	public void LoadTempLightmap ()
	{
	
		string path = Application.dataPath;
		DirectoryInfo dir = new DirectoryInfo (path + "/Lightmaps/TEMPMXD");
		var scenepath = EditorApplication.currentScene;
		string[] pathelements = scenepath.Split (new char[] { '.' });
		string fullscenepath = pathelements [0].Replace ("Assets/", string.Empty);

		object obj = null;

		foreach (FileInfo file in dir.GetFiles()) {
			if (file.Name == "Lightmap.xml") {
				Stream ms = file.Open (FileMode.Open, FileAccess.Read, FileShare.Read);
				System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer (typeof(LightmappedScene));
				obj = x.Deserialize (ms);
				ms.Close ();

			} else if (file.Name != "Description.txt" && file.Name != "Screenshot.png" && (file.Name.Contains (".exr") || file.Name.Contains (".asset")) && !file.Name.EndsWith(".meta")) {
				//  Debug.Log(file.Name);
				file.CopyTo (path + "/" + fullscenepath + "/" + file.Name, true);
			}
			//  Debug.Log("Assets" + "/" + fullscenepath + "/" + file.Name);
			try {               
				//AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
			} catch {
				Debug.Log ("Refresh database is not supported in this build, refreshing the database has been skipped");
			}
            if (file.Name.EndsWith(".exr") && !file.Name.EndsWith(".meta"))
            {
				TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath ("Assets" + "/" + fullscenepath + "/" + file.Name);
				importer.textureType = TextureImporterType.Lightmap;
				importer.isReadable = true;
				importer.lightmap = true;
				//importer.textureFormat = TextureImporterFormat.AutomaticTruecolor;
                AssetDatabase.Refresh();
				try {
					//  AssetDatabase.ImportAsset("Assets" + "/" + fullscenepath + "/" + file.Name, ImportAssetOptions.ForceSynchronousImport);
				} catch {
					Debug.Log ("Import assets is not supported in this build, importing has been skipped");
				}
			}
		}

		if (obj != null) {
			//Debug.Log("Loading Objects");
				
			LightmappedScene sceneobj = (LightmappedScene)obj;
			if (sceneobj != null && sceneobj.Lightmaps != null) {
				
				
				//Debug.Log("Loading original lightmaps");
				foreach (var lobj in sceneobj.Lightmaps) {
					//  Debug.Log(sceneobj.Lightmaps.Count);
					LightmapData d = new LightmapData ();
					// if (lobj.Value != null)
					// d.lightmapFar =(Texture2D) Resources.LoadAssetAtPath(dir.FullName + "\\" + lobj.Value.far, typeof(Texture2D));
					AssetDatabase.ImportAsset ("Assets\\" + fullscenepath + "\\" + lobj.Value.far);
					AssetDatabase.ImportAsset ("Assets\\" + fullscenepath + "\\" + lobj.Value.near);
					d.lightmapFar = (Texture2D)AssetDatabase.LoadAssetAtPath ("Assets\\" + fullscenepath + "\\" + lobj.Value.far, typeof(Texture2D));
					d.lightmapNear = (Texture2D)Resources.LoadAssetAtPath ("Assets\\" + fullscenepath + "\\" + lobj.Value.near, typeof(Texture2D));
					//Debug.Log(lobj);
					if (lobj.Key < LightmapSettings.lightmaps.Length)
					{	
				
						LightmapSettings.lightmaps [lobj.Key].lightmapFar = d.lightmapFar;
						LightmapSettings.lightmaps [lobj.Key].lightmapNear = d.lightmapNear;
					}
					else {

						ResizeArray.Add (lobj.Key, d);
					}

				}
                 
			}


			IndexManager manager = (IndexManager)GameObject.FindObjectOfType (typeof(IndexManager));

			if (manager != null) {
				//Debug.Log("Index Manager Found");
				foreach (var key in sceneobj.LightmappedObjects.Keys) {
					//	Debug.Log("Searching for :"+key.ToString());
					ObjectIndexer indexer = manager.FindByID (int.Parse (key.ToString ()));
					if (indexer != null && indexer.obj != null) {
						//	Debug.Log(key.ToString()+" status : Found , index = "+sceneobj.LightmappedObjects[key].mapindex.ToString());
						GameObject LMobj = (GameObject)EditorUtility.InstanceIDToObject (indexer.obj.GetInstanceID ());
						if (LMobj != null && LMobj.renderer != null && LMobj.isStatic) {
							//		Debug.Log("Setting new index");
							LMobj.renderer.lightmapIndex = sceneobj.LightmappedObjects [key].mapindex;
							LMobj.renderer.lightmapTilingOffset = new Vector4 (sceneobj.LightmappedObjects [key].tilex, sceneobj.LightmappedObjects [key].tiley, sceneobj.LightmappedObjects [key].offsetx, sceneobj.LightmappedObjects [key].offsety);
						} else {
							Debug.Log ("Lightmap Manager 2:Object (" + indexer.ID + ") with the instance ID (" + indexer.obj.GetInstanceID () + ") can't retrieve its saved data, check it either to be non- static, the mesh renderer is removed or it is leaking in the sceneview indexing.");
									
						}

					}
				}
			}

		}
		try {
				
			//  AssetDatabase.Refresh();
		} catch {
			Debug.Log ("Refresh database is not supported in this build, refreshing the database has been skipped");
		}
	}

	public void SaveCurrentLightmappingforAnimation (RecordAnimatedLightmaps referance)
	{
		var del = true;
		IndexManager manager;

		GameObject indexerGO = GameObject.Find ("SceneIndexer");
		if (indexerGO == null) {
			indexerGO = new GameObject ("SceneIndexer");
			indexerGO.hideFlags = HideFlags.HideInHierarchy;
			manager = (IndexManager)indexerGO.AddComponent (typeof(IndexManager));
		} else {
			manager = (IndexManager)indexerGO.GetComponent (typeof(IndexManager));
		}
       
		//var scenepath = EditorApplication.currentScene;
		// string[] pathelements = scenepath.Split(new char[] { '.' });
		// string fullscenepath = pathelements[0].Replace("Assets/", string.Empty);
		//pathelements = pathelements[0].Split(new char[] { '/' });
		// string scene = pathelements[pathelements.Length - 1];


		var path = Application.dataPath;
		//  DirectoryInfo dirinfopath = new DirectoryInfo(path);
		//  path = dirinfopath.parent.FullName;


		if (!Directory.Exists (path + "/AnimLightmaps")) {
			Directory.CreateDirectory (path + "/AnimLightmaps");

		}
		if (!Directory.Exists (path + "/AnimLightmaps/" + referance.Label)) {
			Directory.CreateDirectory (path + "/AnimLightmaps/" + referance.Label);
		} else if (EditorUtility.DisplayDialog ("Confirm", "There is an animated lightmap version with the same name, write over it?", "Yes", "No")) {
                
			Directory.Delete (path + "/AnimLightmaps/" + referance.Label, true);
            

		} else
			del = false;
            
		if (del) {
			Directory.CreateDirectory (path + "/AnimLightmaps/" + referance.Label);
			//DirectoryInfo dirinfo = new DirectoryInfo(path + "/" + fullscenepath);
			//while (!Directory.Exists(path + "/Lightmaps/" + Label)) ;
			if (!File.Exists (path + "/AnimLightmaps/" + referance.Label + "/Description.txt")) {
				TextWriter tw = File.CreateText (path + "/AnimLightmaps/" + referance.Label + "/Description.txt");

				//    new StreamWriter(path + "/Lightmaps/" + Label + "/Description.txt");

				// write a line of text to the file
				tw.Write (description);
				tw.Close ();
			}
			// close the stream
			// EditorApplication.isPlaying = true;
			// while (EditorApplication.isCompiling) ;

			// Application.CaptureScreenshot(path + "/Lightmaps/" + Label + "/Screenshot.png");
			// EditorApplication.isPlaying = false;
			// AssetDatabase.ImportAsset(path + "/Lightmaps/" + Label + "/Screenshot.png", ImportAssetOptions.ForceSynchronousImport);
			//if (SceneView.sceneViews[0] != null && ((SceneView)SceneView.sceneViews[0]).camera != null)
			//{

			//    RenderTexture rt = new RenderTexture((int)position.width, (int)position.height,16, RenderTextureFormat.ARGB32);
			//    RenderTexture.active = rt;
			//    SceneView.lastActiveSceneView.camera.targetTexture = rt;
			//    SceneView.lastActiveSceneView.camera.Render();

			//    Texture2D texture = new Texture2D((int)position.width, (int)position.height);
			//    texture.ReadPixels(new Rect(0, 0, (int)position.width, (int)position.height), 0, 0);
			//    texture.Apply();

			//    Camera.main.targetTexture = null;
			//    File.WriteAllBytes(path + "/Lightmaps/" + Label + "/Screenshot.png", texture.EncodeToPNG());
			//}
			LightmappedScene lscene = new LightmappedScene ();

			int i = 0;

			for (i = 0; i < LightmapSettings.lightmaps.Length; i++) {// (LightmapData data in LightmapSettings.lightmaps)
				var lm = new Lightmap ("LightmapFar-" + i.ToString () + ".exr", "LightmapNear-" + i.ToString () + ".exr");
				lscene.Lightmaps.Add (i, lm);


			}

			// foreach (FileInfo file in dirinfo.GetFiles())
			// {
			//    file.CopyTo(path + "/AnimLightmaps/" + Label + "/" + file.Name);

			// }


			UnityEngine.Object[] activeGOs;

			activeGOs = GameObject.FindObjectsOfType (typeof(MeshRenderer));

			foreach (MeshRenderer activeGO in activeGOs) {
				if (activeGO.gameObject.isStatic) {
					ObjectIndexer indexer;
					Vector4 vec = activeGO.lightmapTilingOffset;

					indexer = manager.FindByID (activeGO.gameObject.GetInstanceID ());

					if (indexer == null) {
						// indexer = ObjectIndexer.CreateInstance<ObjectIndexer>();

						// indexer.ID = activeGO.gameObject.GetInstanceID();
						// indexer.obj = activeGO.gameObject;
						indexer = new ObjectIndexer (activeGO.gameObject.GetInstanceID (), activeGO.gameObject);
						manager.SceneObjects.Add (indexer);

						//  indexer = (ObjectIndexer)activeGO.gameObject.AddComponent(typeof(ObjectIndexer));
						//  indexer.index = activeGO.gameObject.GetInstanceID();
					}

					if (lscene != null && lscene.LightmappedObjects != null && !lscene.LightmappedObjects.ContainsKey (indexer.ID))
						lscene.LightmappedObjects.Add (indexer.ID, new LightmappedObject (vec.x, vec.y, vec.z, vec.w, activeGO.lightmapIndex));

				}
			}
			//  if (!File.Exists(path + "/Lightmaps/Lightmap.xml"))
			//   File.Create(path + "/Lightmaps/Lightmap.xml");
			Stream ms = File.OpenWrite (path + "/AnimLightmaps/" + referance.Label + "/Lightmap.xml");
			//   BinaryFormatter formatter = new BinaryFormatter();
			//   formatter.Serialize(ms, lscene);
			//   ms.Flush();
			//   ms.Close();
			//   ms.Dispose();
			//   formatter = null;
			System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer (lscene.GetType ());
			x.Serialize (ms, lscene);
			ms.Close ();
			// EditorApplication.isPlaying = true;


			// EditorApplication.isPlaying = false;
			 AssetDatabase.Refresh();

		}
       
        
	}

	public void CopyLightmapsToDirectoryforAnimation (RecordAnimatedLightmaps referance, float  sampleValue)
	{
		bool skip = false;
		AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
		
		//sampleVal = sampleValue;
		var path = Application.dataPath;
		var scenepath = EditorApplication.currentScene;
		string[] pathelements = scenepath.Split (new char[] { '.' });
		string fullscenepath = pathelements [0].Replace ("Assets/", string.Empty);
		DirectoryInfo dirinfo = new DirectoryInfo (path + "/" + fullscenepath);

		if (!Directory.Exists (path + "/AnimLightmaps/" + referance.Label + "/" + sampleValue.ToString ())) {
			Directory.CreateDirectory (path + "/AnimLightmaps/" + referance.Label + "/" + sampleValue.ToString ());

		} else
			skip = true;
		
		if (referance.animationFrames == null) {
			referance.animationFrames = new AnimatedFrameData[Mathf.RoundToInt (clip.length / RecordRate)];
		}
		int pos = Mathf.RoundToInt (sampleValue * (1 / RecordRate));
           
		referance.animationFrames [pos] = new AnimatedFrameData ();
		referance.animationFrames [pos].frame = sampleValue;
		FileInfo[] files = dirinfo.GetFiles ();
		if(LightmapSettings.lightmapsMode == LightmapsMode.Single)
		referance.animationFrames [pos].lightmaps = new DualLightmaps[files.Length];
		else
		referance.animationFrames [pos].lightmaps = new DualLightmaps[files.Length / 2];
		
		
		
		foreach (FileInfo file in files) {
			if (!skip)
				file.CopyTo (path + "/AnimLightmaps/" + referance.Label + "/" + sampleValue.ToString () + "/" + file.Name);
                    
			//SetIsLightmap( "Assets/AnimLightmaps/" + referance.Label + "/" + sampleValue.ToString() + "/" + file.Name);
			//     AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
			if (file.Extension == ".exr" || file.Extension == "exr") {
				string[] parts = file.Name.Split (new char[] { '-' });
				int num = int.Parse (parts [1].Split (new char[]{'.'}) [0]);
				if (referance.animationFrames [pos].lightmaps [num] == null) {
					referance.animationFrames [pos].lightmaps [num] = new DualLightmaps ();
				}
				if (parts [0].Contains ("Far") || parts [0].Contains ("Color"))
				{
					AssetDatabase.ImportAsset("Assets/AnimLightmaps/" + referance.Label + "/" + sampleValue.ToString () + "/" + file.Name);
					referance.animationFrames [pos].lightmaps [num].far = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/AnimLightmaps/" + referance.Label + "/" + sampleValue.ToString () + "/" + file.Name, typeof(Texture2D));
				}
				else if (parts [0].Contains ("Near") || parts [0].Contains ("Scale"))
				{
					AssetDatabase.ImportAsset("Assets/AnimLightmaps/" + referance.Label + "/" + sampleValue.ToString () + "/" + file.Name);
					referance.animationFrames [pos].lightmaps [num].near = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/AnimLightmaps/" + referance.Label + "/" + sampleValue.ToString () + "/" + file.Name, typeof(Texture2D));
				}
				}
		}
	}
	
	public bool SkipBakingFrame (RecordAnimatedLightmaps referance, float sample)
	{
		var path = Application.dataPath;
		//var scenepath = EditorApplication.currentScene;
		//string[] pathelements = scenepath.Split (new char[] { '.' });
//		string fullscenepath = pathelements [0].Replace ("Assets/", string.Empty);
		//DirectoryInfo dirinfo = new DirectoryInfo (path + "/" + fullscenepath);
		
		if (!Directory.Exists (path + "/AnimLightmaps/" + referance.Label + "/" + sample.ToString ())) {
			return false;
		} else
			return true;
		
		
	}

	public void SetIsLightmap (string path)
	{
		// var path = AssetDatabase.GetAssetPath(tex);
		AssetImporter aimporter = TextureImporter.GetAtPath (path);
		if (aimporter.GetType () == typeof(TextureImporter)) {
			TextureImporter importer = (TextureImporter)aimporter;
			//importerF.SetTextureSettings(settings);
			importer.textureType = TextureImporterType.Lightmap;
			importer.lightmap = true;
			//importer.textureFormat = TextureImporterFormat.ARGB32;
			AssetDatabase.ImportAsset (path);
		}
	}

	public void StartAsyncBaking ()
	{
		Lightmapping.BakeSelectedAsync ();
	}

	public bool isLightmappingRunning ()
	{
		return Lightmapping.isRunning;
	}

	public void RefreshDatabase ()
	{
			AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
	}

	public void ResetObjectPrefabState (UnityEngine.Object obj)
	{
		PrefabUtility.ResetToPrefabState (obj);
	}

	public void stopLightmapping ()
	{
		Lightmapping.Cancel ();
	}
		
}
