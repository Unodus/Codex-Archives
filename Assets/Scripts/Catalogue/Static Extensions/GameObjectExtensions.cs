using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class GameObjectExtensions 
{

	/// <summary>
	/// Combines child objects of a prefab into a single mesh saving draw calls
	/// 
	/// The Child objects must all have the same material. The mesh must be non-moveable
	/// </summary>
	public static void CombineMultiStaticObjects(this GameObject parent, string sParentName)
	{
#if UNITY_EDITOR
		Undo.IncrementCurrentGroup();
#endif
		MeshFilter[] meshFilters = parent.GetComponentsInChildren<MeshFilter>();
		if (meshFilters.Length > 0)
		{
			Material m_Material = null;
			MeshRenderer meshRenderer = meshFilters[0].gameObject.GetComponent<MeshRenderer>();
			if (null != meshRenderer)
			{
				m_Material = meshRenderer.material;
			}

			//Get num of vertices per mesh
			int numVerts = meshFilters[0].mesh.vertexCount;
			//Debug.Log("Mesh: has Vertices: " + numVerts + " so num per parents allowed is: " + numPerParent + " ... Item count is: " + meshFilters.Length);

			int numParents = 0;

			GameObject newParent = new GameObject(sParentName + "_" + numParents);

			int iCount = meshFilters.Length;

			//For all items in mesh list
			for (int i = 0; i < iCount; i++)
			{
				if (numVerts + meshFilters[i].mesh.vertexCount < 65000)
				{
					meshFilters[i].gameObject.transform.parent = newParent.transform;
					numVerts += meshFilters[i].mesh.vertexCount;
				}
				else
				{
					numVerts = 0;
					CombineChildren(newParent, m_Material);

					GameObject.Destroy(newParent);
					newParent = new GameObject(sParentName + "_" + numParents);
					numParents++;
					meshFilters[i].gameObject.transform.parent = newParent.transform;
				}

				if (i == iCount - 1)
				{
					CombineChildren(newParent, m_Material);
					GameObject.Destroy(newParent);
				}

				//If parent max is reached or we have reached the end of the list
				//				if ((currentNum == numPerParent) || (i == iCount-1))
				//				{
				//					//Combine the items into the parent
				//					currentNum = 0;
				//					CombineChildren(newParent, m_Material);
				//					numParents++;
				//
				//					//if not at the end of the list... prepare a new parent for the next items
				//					if (i != iCount-1)
				//					{
				//					}
				//				}
			}

			//			GameObject.Destroy(parent);
		}
	}

	/// <summary>
	/// Combines the children.
	/// </summary>
	private static void CombineChildren(this GameObject parent, Material material)
	{
		MeshFilter[] meshFilters = parent.GetComponentsInChildren<MeshFilter>();
		CombineInstance[] combine = new CombineInstance[meshFilters.Length];

		int i = 0;
		while (i < meshFilters.Length)
		{
			combine[i].mesh = meshFilters[i].sharedMesh;
			combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
			meshFilters[i].gameObject.SetActive(false);
			i++;
		}

		MeshFilter meshFilter = parent.GetComponent<MeshFilter>();
		if (null == meshFilter)
		{
			meshFilter = parent.AddComponent<MeshFilter>();
		}

		MeshRenderer meshRenderer = parent.GetComponent<MeshRenderer>();
		if (null == meshRenderer)
		{
			meshRenderer = parent.AddComponent<MeshRenderer>();
			meshRenderer.material = material;
		}

		parent.transform.GetComponent<MeshFilter>().mesh = new Mesh();
		parent.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
		parent.transform.gameObject.SetActive(true);
	}

	public static void CombineTexturesForChildObjects(this GameObject parent, bool bCombine = false, string sParentName = "parent")
	{
		List<Material> uniqueMaterials = new List<Material>();
		Dictionary<string, List<Material>> matchingShader = new Dictionary<string, List<Material>>();

		MeshRenderer[] meshRenderer = parent.GetComponentsInChildren<MeshRenderer>();

		int iMatCount = meshRenderer.Length;
		for (int i = 0; i < iMatCount; i++)
		{
			if (false == uniqueMaterials.Contains(meshRenderer[i].sharedMaterial))
			{
				//unique material found... adding
				uniqueMaterials.Add(meshRenderer[i].sharedMaterial);
			}
		}

		//Have all unique materials... find if any use the same shader... i.e textures can be combined without consequence for visuals
		foreach (Material mat in uniqueMaterials)
		{
			if (true == matchingShader.ContainsKey(mat.shader.name))
			{
				//append to end of list of materials
				matchingShader[mat.shader.name].Add(mat);
			}
			else
			{
				List<Material> mats = new List<Material>();
				mats.Add(mat);
				matchingShader.Add(mat.shader.name, mats);
			}
		}

		//pack all subtextures with matching shaders

		foreach (string key in matchingShader.Keys)
		{
			GameObject matParent = new GameObject("parent");

			int iCount = matchingShader[key].Count;
			//If more than one material.. pack texture
			//more than one
			Texture2D[] textures = new Texture2D[iCount];
			Rect[] rects = new Rect[iCount];

			for (int i = 0; i < iCount; i++)
			{
				textures[i] = matchingShader[key][i].mainTexture as Texture2D;
			}

			//Combine and return textures
			Texture packedtexture = CombineTextures(textures, ref rects, 8192);

			if (null != packedtexture)
			{
				//Apply new texture to all objects with same material
				for (int iMaterialIndex = 0; iMaterialIndex < iMatCount; iMaterialIndex++)
				{
					string sShaderName = meshRenderer[iMaterialIndex].sharedMaterial.shader.name;
					Material packedMaterial = new Material(Shader.Find(sShaderName));
					packedMaterial.name = meshRenderer[iMaterialIndex].name + iMaterialIndex;
					if (null != packedMaterial)
					{
						packedMaterial.mainTexture = packedtexture;
					}
					else
					{
						Debug.LogError("Couldnt find shader: " + sShaderName);
					}

					if (sShaderName == key)
					{
						//For all the textures that were packed
						for (int iTextureIndex = 0; iTextureIndex < textures.Length; iTextureIndex++)
						{
							//Find if this texture exists on the object
							if (meshRenderer[iMaterialIndex].sharedMaterial.mainTexture == textures[iTextureIndex])
							{
								MeshFilter meshFilter = meshRenderer[iMaterialIndex].gameObject.GetComponent<MeshFilter>();
								meshRenderer[iMaterialIndex].gameObject.transform.parent = matParent.transform;

								if (null != meshFilter)
								{
									int iUV1Count = meshFilter.mesh.uv.Length;
									Vector2[] uvs = meshFilter.mesh.uv;

									for (int iUVIndex = 0; iUVIndex < iUV1Count; iUVIndex++)
									{
										uvs[iUVIndex].x = (uvs[iUVIndex].x * rects[iTextureIndex].width) + rects[iTextureIndex].x;
										uvs[iUVIndex].y = (uvs[iUVIndex].y * rects[iTextureIndex].height) + rects[iTextureIndex].y;
									}

									meshFilter.mesh.uv = uvs;
								}
								else
								{
									Debug.LogWarning("No mesh Filter found!!!");
								}
							}
						}

						meshRenderer[iMaterialIndex].material = packedMaterial;
					}
				}
			}

			if (true == bCombine)
			{
				CombineMultiStaticObjects(matParent, sParentName);
			}

			GameObject.Destroy(matParent);
		}
	}

	private static Texture2D CombineTextures(Texture2D[] atlasTextures, ref Rect[] rects, int iTextureSize)
	{
		Texture2D atlas = new Texture2D(iTextureSize, iTextureSize);
		rects = atlas.PackTextures(atlasTextures, 0, 512);
		return atlas;
	}

	public static void AddComponentIfNull<T>(this GameObject obj , ref T component ) where T : Component
	{
		component = obj.GetComponent<T>();
		if (null == component)
		{
			component = obj.AddComponent<T>();
		}
	}

	public static void AddComponentIfNull<T>(this GameObject obj) where T : Component
	{
		if (null == obj.GetComponent<T>())
		{
			obj.AddComponent<T>();
		}
	}
}
