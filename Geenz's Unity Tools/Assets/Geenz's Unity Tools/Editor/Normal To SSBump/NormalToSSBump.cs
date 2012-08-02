/* Geenz's Unity3D Tools
 * NormalToSSBump.cs
 * Provides a wizard to convert normal maps into SSBumpMaps stored in Unity's DLM basis.
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

public class NormalToSSBump : ScriptableWizard {
    public Texture2D normalMap;
	public Vector3[] fromBasis = {
		new Vector3(0.81649658f, 0.0f, 0.57735028f),
		new Vector3(-0.40824830f, 0.70710679f, 0.57735027f),
		new Vector3(-0.40824829f, -0.70710678f, 0.57735026f)
	};
    
    [MenuItem ("Geenz's Tools/Convert Normal To SSBump")]
    static void CreateWizard () {
        ScriptableWizard.DisplayWizard<NormalToSSBump>("Convert Normal Map to SSBump Map", "Convert");
        //If you don't want to use the secondary button simply leave it out:
        //ScriptableWizard.DisplayWizard<WizardCreateLight>("Create Light", "Create");
    }
	
    void OnWizardCreate () {
        Texture2D tmpTexture = normalMap;
		Matrix4x4 mat = new Matrix4x4();
		mat.SetRow(0, fromBasis[0]);
		mat.SetRow(1, fromBasis[1]);
		mat.SetRow(2, fromBasis[2]);
		TexturesSpaceConversions.ConvertTangentBasis(mat, ref tmpTexture);
		string path = Application.dataPath + "/Converted SSBump Maps";
		FileSystemUtilities.CreateDirectory(path);
		FileSystemUtilities.SavePNG(ref tmpTexture, path + "/" + normalMap.name + "_SSBump.png");
    }
	
    void OnWizardUpdate () {
        helpString = "Please set the color of the light!";
    }
	
    // When the user pressed the "Apply" button OnWizardOtherButton is called.
    void OnWizardOtherButton () {
        if (Selection.activeTransform == null ||
            Selection.activeTransform.light == null) return;
        Selection.activeTransform.light.color = Color.red;
    }
}