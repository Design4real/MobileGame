using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;

public static class SaveExr  {
	
	
	[DllImport("MiniExr",EntryPoint="Write",CharSet=CharSet.Auto)]
private static extern void Write(int width, int height, IntPtr rgba16f,string location);
	
	public static void Save(Texture2D tex,string loc)
	{
		GCHandle m_PixelsHandle;
		Color[] m_Pixels = tex.GetPixels (0);
        // "pin" the array in memory, so we can pass direct pointer to it's data to the plugin,
        // without costly marshaling of array of structures.
        m_PixelsHandle = GCHandle.Alloc(m_Pixels, GCHandleType.Pinned);
		Write(tex.width,tex.height,m_PixelsHandle.AddrOfPinnedObject(),loc);
		
	}
}
