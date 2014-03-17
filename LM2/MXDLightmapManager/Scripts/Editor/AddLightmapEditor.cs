using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(AddLightmap))]
public class AddLightmapEditor : Editor
{
void OnSceneGUI()
{ 
	AddLightmap obj = (AddLightmap)target;

//	if(!IsLightmap(obj.far))
//	FixImportSettings(obj.far);
	
//	if(!IsLightmap(obj.near))
//	FixImportSettings(obj.near);
	
	obj.enter = true;
	obj.Update();
}

public bool IsLightmap(Texture2D tex)
	{
		var path = AssetDatabase.GetAssetPath(tex);
		  TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(path);
		return   importer.textureType == TextureImporterType.Lightmap;
	}
	public void FixImportSettings(Texture2D tex)
	{
			AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);

        var path = AssetDatabase.GetAssetPath(tex);
        TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(path);
        //importerF.SetTextureSettings(settings);
        importer.textureType = TextureImporterType.Lightmap;
        importer.lightmap = true;
       importer.isReadable = true;
        AssetDatabase.ImportAsset(path);
	}
	
}
