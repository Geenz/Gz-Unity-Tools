/* Geenz's Unity3D Tools
 * SSBumpWizard.cs
 * Provides a wizard to convert SSBump maps into tangent space normal maps.
 * Copyright (C) 2012 Jonathan Goodman
 * 
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
 */

using UnityEditor;
using UnityEngine;

public class SSBumpWizard : ScriptableWizard {
    public Texture2D SSBumpMap;
	public bool recomputeZ;
	public Vector3[] basis = {
		new Vector3(0.81649658f, 0.0f, 0.57735028f),
		new Vector3(-0.40824830f, 0.70710679f, 0.57735027f),
		new Vector3(-0.40824829f, -0.70710678f, 0.57735026f)
	};
    
    [MenuItem ("Geenz's Tools/Convert SSBump To Normal")]
    static void CreateWizard () {
        ScriptableWizard.DisplayWizard<SSBumpWizard>("Convert SSBump Map to Normal Map", "Convert");
    }
	
    void OnWizardCreate () {
        Texture2D tmpTexture = SSBumpMap;
		Matrix4x4 mat = new Matrix4x4();
		mat.SetRow(0, basis[0]);
		mat.SetRow(1, basis[1]);
		mat.SetRow(2, basis[2]);
		Matrix4x4 convMat = new Matrix4x4();
		convMat.SetRow(0, (mat.MultiplyVector(new Vector3(1, 0, 0))));
		convMat.SetRow(1, (mat.MultiplyVector(new Vector3(0, 1, 0))));
		convMat.SetRow(2, (mat.MultiplyVector(new Vector3(0, 0, 1))));
		TexturesSpaceConversions.ConvertTangentBasis(convMat, ref tmpTexture, recomputeZ);
		string path = Application.dataPath + "/Converted Normal Maps";
		FileSystemUtilities.CreateDirectory(path);
		FileSystemUtilities.SavePNG(ref tmpTexture, path + "/" + SSBumpMap.name + "_normal.png");
    }
	
    void OnWizardUpdate () {
        helpString = "Select a texture map, set a basis, and click Convert!";
    }
}