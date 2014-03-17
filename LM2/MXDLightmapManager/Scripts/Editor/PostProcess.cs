using UnityEngine;
using System.Collections;
using UnityEditor;

public class PostProcess : AssetPostprocessor {
	
	public static bool prompt = true;
	
    void OnPreprocessTexture () {
		
		if(prompt)
		{
		int num = EditorUtility.DisplayDialogComplex("Make Readable","Do you want to make lightmaps readable","Yes","No","No(Don't ask Again)");
		if(num == 2)
			{
				prompt = false;
				
			}
			else if(num == 0)
			{
        if (assetPath.Contains("/Lightmap")) {
            TextureImporter textureImporter  =(TextureImporter) assetImporter;
            textureImporter.isReadable = true;
			textureImporter.lightmap = true;
			textureImporter.textureType = TextureImporterType.Lightmap;
        }
		}
    }}

}
