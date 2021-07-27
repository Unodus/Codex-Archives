using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class GameObjectExtensions 
{
	public static void DestroyChildren(this GameObject parent)
	{
		Transform[] children = new Transform[parent.transform.childCount];
		for (int i = 0; i < parent.transform.childCount; i++)
			children[i] = parent.transform.GetChild(i);
		for (int i = 0; i < children.Length; i++)
			GameObject.Destroy(children[i].gameObject);
	}
	public static void MoveChildren(this GameObject from, GameObject to)
	{
		Transform[] children = new Transform[from.transform.childCount];
		for (int i = 0; i < from.transform.childCount; i++)
			children[i] = from.transform.GetChild(i);
		for (int i = 0; i < children.Length; i++)
			children[i].SetParent(to.transform);
	}


	public static int GetCollisionMask(this GameObject gameObject, int layer = -1)
	{
		if (layer == -1)
			layer = gameObject.layer;

		int mask = 0;
		for (int i = 0; i < 32; i++)
			mask |= (Physics.GetIgnoreLayerCollision(layer, i) ? 0 : 1) << i;

		return mask;
	}



	#region Tag & Layers




	public static void SetTagRecursive(this GameObject myObject, string sTag)
	{
		myObject.tag = sTag;
		foreach (Transform t in myObject.transform) t.gameObject.SetTagRecursive(sTag);
	}

	public static void SetTagRecursive(this GameObject myObject, string sTag, string sOnlyContains)
	{
		if (true == myObject.name.Contains(sOnlyContains)) myObject.tag = sTag;
		foreach (Transform t in myObject.transform) t.gameObject.SetTagRecursive(sTag);
	}

	public static void SetLayerRecursive(this GameObject myObject, int iLayer)
	{
		myObject.layer = iLayer;
		foreach (Transform t in myObject.transform) t.gameObject.SetLayerRecursive(iLayer);
	}
	public static void SetLayerRecursiveIfNotDefault(this GameObject myObject, int iLayer)
	{
		if (myObject.layer != LayerMask.NameToLayer("Default") ) myObject.layer = iLayer;
		foreach (Transform t in myObject.transform) t.gameObject.SetLayerRecursiveIfNotDefault(iLayer);
	}
	
 
	#endregion


	public static void SetParent(this GameObject lThis, GameObject lOther)
	{
		if (null == lOther) lThis.transform.SetParent(null);
		else lThis.transform.SetParent(lOther.transform);
	}
	public static void SetTransformLocals(this GameObject lGameObject, Vector3 lLocalPosition, Quaternion lRotation)
	{
		lGameObject.transform.SetTransformLocals(lLocalPosition, lRotation);
	}
	

	public static void SetTransformLocals(this GameObject lGameObject)
	{
		lGameObject.transform.ResetTransform();
	}

	public static void SetTransformLocals(this GameObject lGameObject, Vector3 lLocalPosition)
	{
		lGameObject.transform.SetTransform(lLocalPosition);
	}

	public static void SetTransformLocals(this GameObject lGameObject, Quaternion lLocalRotation)
	{
		lGameObject.transform.SetTransformLocals(lLocalRotation);
	}









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


			}
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

	public static void AdvancedMerge(this GameObject i)
	{
		// All our children (and us)
		MeshFilter[] filters = i.GetComponentsInChildren<MeshFilter>(false);

		if (filters.Length == 0) return;

		// All the meshes in our children (just a big list)
		List<Material> materials = new List<Material>(); // List materials = new List();

		MeshRenderer[] renderers = i.GetComponentsInChildren<MeshRenderer>(false); // <-- you can optimize this
		foreach (MeshRenderer renderer in renderers)
		{
			if (renderer.transform == i.transform)
				continue;
			Material[] localMats = renderer.sharedMaterials;
			foreach (Material localMat in localMats)
				if (!materials.Contains(localMat))
					materials.Add(localMat);
		}


		// Each material will have a mesh for it.
		List<Mesh> submeshes = new List<Mesh>(); // List submeshes = new List();
		foreach (Material material in materials)
		{
			// Make a combiner for each (sub)mesh that is mapped to the right material.
			System.Collections.Generic.List<CombineInstance> combiners = new System.Collections.Generic.List<CombineInstance>();
			foreach (MeshFilter filter in filters)
			{
				if (filter.transform == i.transform) continue;
				// The filter doesn't know what materials are involved, get the renderer.
				MeshRenderer renderer = filter.GetComponent<MeshRenderer>();  // <-- (Easy optimization is possible here, give it a try!)
				if (renderer == null)
				{
					Debug.LogError(filter.name + " has no MeshRenderer");
					continue;
				}

				// Let's see if their materials are the one we want right now.
				Material[] localMaterials = renderer.sharedMaterials;
				for (int materialIndex = 0; materialIndex < localMaterials.Length; materialIndex++)
				{
					if (localMaterials[materialIndex] != material)
						continue;
					// This submesh is the material we're looking for right now.
					CombineInstance ci = new CombineInstance();
					ci.mesh = filter.sharedMesh;
					ci.subMeshIndex = materialIndex;
					ci.transform = Matrix4x4.identity;
					combiners.Add(ci);
				}
			}
			// Flatten into a single mesh.
			Mesh mesh = new Mesh();
			mesh.CombineMeshes(combiners.ToArray(), true);
			submeshes.Add(mesh);
		}

		// The final mesh: combine all the material-specific meshes as independent submeshes.
		List<CombineInstance> finalCombiners = new System.Collections.Generic.List<CombineInstance>();
		foreach (Mesh mesh in submeshes)
		{
			CombineInstance ci = new CombineInstance();
			ci.mesh = mesh;
			ci.subMeshIndex = 0;
			ci.transform = Matrix4x4.identity;
			finalCombiners.Add(ci);
		}
		Mesh finalMesh = new Mesh();
		finalMesh.CombineMeshes(finalCombiners.ToArray(), false);
		i.GetComponent<MeshFilter>().sharedMesh = finalMesh;
		Debug.Log("Final mesh has " + submeshes.Count + " materials.");
	}


	 


}
