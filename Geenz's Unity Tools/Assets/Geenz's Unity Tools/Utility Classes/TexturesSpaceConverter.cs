/* Geenz's Unity3D Tools
 * TextureSpaceConverter.cs
 * Provides common functionality with regards to converting textures stored in one space (i.e., DLM-space normals) to another space (i.e., tangent space normals).
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
using UnityEngine;
using System.Collections;

public class TexturesSpaceConversions {
	
	/// <summary>
	/// Converts vector data stored within a texture using the specified basis.  Assume all calculations are performed in tangent space.
	/// </summary>
	/// <param name='basis'>
	/// Basis to multiply the vector values against.
	/// </param>
	/// <param name='vectorData'>
	/// Texture2D containing vector data.  Textures are passed by reference, so make sure to copy beforehand if you don't want to overwrite your data!
	/// </param>
	public static void ConvertTangentBasis(Matrix4x4 basis, ref Texture2D vectorData, bool recomputeZ = false)
	{
		Color[] colorData = vectorData.GetPixels();
		Texture2D tmpTexture = new Texture2D(vectorData.width, vectorData.height, TextureFormat.ARGB32, false);
		
		for (int i = 0; i < colorData.Length; i++)
		{
			Color vecData = new Color(colorData[i].r, colorData[i].g, colorData[i].b, 1);
			vecData.r = Vector3.Dot(new Vector3(basis.m00, basis.m01, basis.m02), UnpackUnitVector(new Vector3(colorData[i].r, colorData[i].g, colorData[i].b))) * 0.5f + 0.5f;
			vecData.g = Vector3.Dot(new Vector3(basis.m10, basis.m11, basis.m12), UnpackUnitVector(new Vector3(colorData[i].r, colorData[i].g, colorData[i].b))) * 0.5f + 0.5f;
			if (recomputeZ)
			{
				vecData.r = vecData.r * 2 - 1;
				vecData.g = vecData.g * 2 - 1;
				vecData.b = Mathf.Sqrt(1 - vecData.r * vecData.r - vecData.g * vecData.g) * 0.5f + 0.5f;
				vecData.r = vecData.r * 0.5f + 0.5f;
				vecData.g = vecData.g * 0.5f + 0.5f;
			} else {
				vecData.b = Vector3.Dot(new Vector3(basis.m20, basis.m21, basis.m22), UnpackUnitVector(new Vector3(colorData[i].r, colorData[i].g, colorData[i].b))) * 0.5f + 0.5f;
			}
			colorData[i] = vecData;
		}
		tmpTexture.SetPixels(colorData);
		tmpTexture.Apply();
		vectorData = tmpTexture;
	}
	
	public static Vector3 UnpackUnitVector(Vector3 vec)
	{
		return new Vector3(vec.x * 2 - 1, vec.y * 2 - 1, vec.z * 2 - 1);
	}
	
	public static Vector3 PackUnitVector(Vector3 vec)
	{
		return new Vector3(vec.x * 0.5f + 0.5f, vec.y * 0.5f + 0.5f, vec.z * 0.5f + 0.5f);
	}
}
