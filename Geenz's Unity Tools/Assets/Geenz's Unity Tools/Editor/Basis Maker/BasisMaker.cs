/* Geenz's Unity3D Tools
 * BasisMaker.cs
 * Provides a wizard to create tangent space basis normal maps that may be used in 3D applications to bake illumination (and normal maps) into different basis.
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

public class BasisMaker : ScriptableWizard {
	public Vector3[] basis = {
		new Vector3(0.81649658f, 0.0f, 0.57735028f),
		new Vector3(-0.40824830f, 0.70710679f, 0.57735027f),
		new Vector3(-0.40824829f, -0.70710678f, 0.57735026f)
	};
    
    [MenuItem ("Geenz's Tools/Basis Maker")]
    static void CreateWizard () {
        ScriptableWizard.DisplayWizard<BasisMaker>("Create Basis Texutres", "Create");
    }
    void OnWizardCreate () {
		
		for( int i = 0; i < basis.Length; i++)
		{
			Texture2D basisTex = new Texture2D(8, 8, TextureFormat.RGB24, false);
			Color[] basisCol = basisTex.GetPixels();
			
			basis[i].Normalize();
			basis[i] = TexturesSpaceConversions.PackUnitVector(basis[i]);
			for (int ia = 0; ia < basisCol.Length; ia++)
			{
				basisCol[ia] = new Color(basis[i].x, basis[i].y, basis[i].z);
			}
			basisTex.SetPixels (basisCol);
			basisTex.Apply();
			string path = Application.dataPath + "/Basis Maps";
			FileSystemUtilities.CreateDirectory(path);
			FileSystemUtilities.SavePNG(ref basisTex, path + "/basis" + i.ToString() + ".png");
		}
    }  
    void OnWizardUpdate () {
        helpString = "Set a basis, click Create, then use the resulting basis textures in your favorite 3D program!";
    }
}