using UnityEngine;

//Walter 's tip of the day - 11/25/2020

//Do you now how Layers and LayerMasks work in Unity?
//When you're just getting started, probably not..

//Well, behind the scenes, they're actually bit  flags. 
//They're a clever way of packing the combinations of layers neatly into one variable.
//As opposed to having an array of booleans or something..

//Go to the Layer Window the next time you're in Unity, you'll notice it has a max of 32 layers. 
//That's because it's a 32-bit flag, represented by an integer.

//Unity actually already has something called GetMask to get a combined LayerMask for the names you give it, super handy!
//It can take as many variables as you want, however, it doesn't work in constructors or initializers, so do it in Reset() or Start()/Awake().

// However, these functions should work much better:

public static partial class LayerMaskUtilities
{

	//or          |
	//and         &
	//xor         ^
	//not         ~
	//shift left  <<
	//shift right >>

	#region Add

	/// <summary> Adds layers that are in *other* <see cref="LayerMask"/> to the *original*. </summary>
	 
	public static LayerMask Add(this LayerMask original, in LayerMask other)
		=> (original | other);
	/// <summary> Adds a layer to the *original* <see cref="LayerMask"/>. </summary>
	 
	public static LayerMask Add(this LayerMask original, in int layer)
		=> (original | layer);

	#endregion

	#region Remove

	/// <summary> Removes layers that are in *other* <see cref="LayerMask"/> from the *original*. </summary>
 
	public static LayerMask Remove(this LayerMask original, in LayerMask other)
		=> (original & ~other);
	/// <summary> Removes a layer from the *original* <see cref="LayerMask"/>. </summary>
 
	public static LayerMask Remove(this LayerMask original, in int layer)
		=> (original & ~layer);

	#endregion

	#region Toggle

	/// <summary> Toggles layers that are enabled within *other* <see cref="LayerMask"/> in the *original*. </summary>
 
	public static LayerMask Toggle(this LayerMask original, in LayerMask other)
		=> (original ^ other);
	/// <summary> Toggles a layer in the *original* <see cref="LayerMask"/>. </summary>
 
	public static LayerMask Toggle(this LayerMask original, in int layer)
		=> (original ^ layer);

	#endregion

	#region Has Layer

	/// <summary> Whether the <see cref="LayerMask"/> contains a layer with the name *layerName*. </summary>
	/// <returns> True if it has the layer, False if it does not. </returns>
 
	public static bool HasLayer(this LayerMask mask, in string layerName)
	  => (mask == (mask | (1 << LayerMask.NameToLayer(layerName))));


	/// <summary> Whether the <see cref="LayerMask"/> contains the layer *layer*. (Well it contains all layers, but is it toggled *true*) </summary>
	/// <remarks> Use for SINGLE LAYERS ONLY!! </remarks>
	/// <returns> True if it has the layer, False if it does not. </returns>

	public static bool HasLayer(this LayerMask mask, in int layer)
		=> (mask == (mask | (1 << layer)));


	/// <summary> Whether *original* has ANY of the layers there are in *other*.</summary>
 
	public static bool HasAnyLayers(this LayerMask original, in LayerMask other)
	{
		for (byte __layer = 0; __layer < 32; __layer++)
		{
			//If a layer is in both layermasks, return true.
			if (other.HasLayer(__layer) && original.HasLayer(__layer)) return true;
		}

		return false;
	}

	/// <summary> Whether *original* has ALL of the layers there are in *other*.</summary>
 
	public static bool HasAllLayers(this LayerMask original, in LayerMask other)
		=> ((other & original.value) == other);

	#endregion
 
	public static bool ContainsAll(this LayerMask mask, params int[] layers)
	{
		foreach (int __layer in layers)
		{
			if (!mask.HasLayer(__layer)) return false;
		}

		return true;
	}

 
	public static bool ContainsAny(this LayerMask mask, params int[] layers)
	{
		foreach (int __layer in layers)
		{
			if (mask.HasLayer(__layer)) return true;
		}

		return false;
	}
}