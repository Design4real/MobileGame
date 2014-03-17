using UnityEngine;
using System.Collections;
using UnityEditor;

public class IndexerEditor : EditorWindow {

	Vector2 scroller;
	IndexManager manager;
	
  [MenuItem("Lightmap Manager/Indexed Objects")] 
    static void ShowWindow () {
         
        EditorWindow window;
      
        window = (EditorWindow)EditorWindow.GetWindowWithRect(typeof(IndexerEditor), new Rect(100, 100, 400, 300), false, "Indexing");
        window.autoRepaintOnSceneChange = true;
        window.Show();
        
    }
	  void OnGUI () {
		
		      GameObject indexerGO = GameObject.Find("SceneIndexer");
		   scroller = GUILayout.BeginScrollView(scroller, GUILayout.Height(Screen.height - 50));
		  
		 
                    if (indexerGO == null)
                    {
						GUILayout.Label("No Indexer is available for the scene");
						
					}
					else 
					{
						 manager = (IndexManager)indexerGO.GetComponent(typeof(IndexManager));
						
						if(manager !=null && manager.SceneObjects !=null)
						{
							
							    foreach (ObjectIndexer index in manager.SceneObjects)
								{
									GUILayout.BeginHorizontal();
									GUILayout.Label(index.ID.ToString());
									EditorGUILayout.ObjectField(index.obj,typeof(GameObject),false);
									GUILayout.EndHorizontal();
								}
						}
					}
		  GUILayout.EndScrollView();
					    if (indexerGO != null)
                    {
		    if (GUILayout.Button("Clear SceneView Indexing"))
                    {
                        GameObject SI =   GameObject.Find("SceneIndexer");
                        if (SI != null)
                        {
                            bool val = EditorUtility.DisplayDialog("Alert", "This will clear the indexing of your objects leading to lose previously saved settings thus you want be able to load lightmaps correctly", "Clear", "Cancel");
                            if (val)
                            {
                                GameObject.DestroyImmediate(SI);
                            }
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("No indexing Available", "There is no indexing available in this scene, indexing will be generated when you save lightmaps.", "OK");

                        }
                    }
				}
		  
	  }
}
