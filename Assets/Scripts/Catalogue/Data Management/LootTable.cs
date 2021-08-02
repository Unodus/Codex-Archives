// A loop drop table for Unity, somewhat inspired by http://www.lostgarden.com/2014/12/loot-drop-tables.html

// TODO: Use properties { get; set } to automatically call UpdateWeightRanges() when
//       any Weight is changed, or any item is added or removed from the list
//       We could then also add an autoupdating (readonly) Sum field, replacing calls to GetSumOfWeights()

// TODO: Rather than making Prefab a GameObject, call it Obj and make this a LootTable<T> generic so we
//       can use any type as the associated object.
//

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class LootTable : MonoBehaviour
{

    public string Name;
    public bool AutoRefreshOnEmpty = false;

    [System.Serializable]
    public class Item : ICloneable
    {
        public string Name;
        public GameObject Prefab;
        public int Weight;
        [System.NonSerialized]
        [HideInInspector]
        public float[] WeightRange = new float[2] { -1f, -1f };

        /*
         * This makes a simple shallow copy - if the object gets more complex fields (eg lists ?),
         * we need to change to using a deep copy, eg: 
         * https://stackoverflow.com/questions/129389/how-do-you-do-a-deep-copy-an-object-in-net-c-specifically
         */
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    [SerializeField]
    public List<Item> ItemWeights;
    List<Item> _savedItemWeights;

    void Start()
    {
        UpdateWeightRanges();
        SaveItemWeights();
        //_Test();
    }

    public int GetSumOfWeights()
    {
        // we avoid LINQ Sum here for iOS & other AOT platform compatibility
        //int sum = ItemWeights.Sum<Item>(i => i.Weight);

        int sum = 0;
        for (int i = 0; i < ItemWeights.Count; i++)
        {
            sum += ItemWeights[i].Weight;
        }

        //Debug.Log("Sum of weights: " + sum, this);
        return sum;
    }

    public void UpdateWeightRanges()
    {
        int sum = GetSumOfWeights();
        float prevItemMax = 0f;
        for (int i = 0; i < ItemWeights.Count; i++)
        {
            var item = ItemWeights[i];
            if (item.Weight == 0) continue;
            if (item.WeightRange == null) item.WeightRange = new float[2] { -1f, -1f };
            item.WeightRange[0] = prevItemMax;
            item.WeightRange[1] = prevItemMax + (((float)item.Weight) / sum);
            prevItemMax = item.WeightRange[1];
        }
    }

    public List<Item> CopyItemWeights()
    {
        return CopyItemWeights(ItemWeights);
    }

    List<Item> CopyItemWeights(List<Item> weights)
    {
        var itemWeights = new List<Item>();
        foreach (var item in weights)
        {
            itemWeights.Add((Item)item.Clone());
        }
        return itemWeights;
    }

    public void SaveItemWeights()
    {
        _savedItemWeights = CopyItemWeights();
    }

    public void RestoreItemWeights()
    {
        ItemWeights = CopyItemWeights(_savedItemWeights);
    }

    /*
     *  Sampling with replacement (eg dice roll).
     */
    public Item GetRandom()
    {
        UpdateWeightRanges();
        float rnd = UnityEngine.Random.value;
        foreach (var item in ItemWeights)
        {
            if (item.Weight == 0) continue;
            //Debug.Log("Item weight range:" + item.WeightRange[0] + "," + item.WeightRange[1], this);
            if (rnd > item.WeightRange[0] && rnd <= item.WeightRange[1]) return item;
        }
        if (!AutoRefreshOnEmpty) Debug.LogWarning("LootTable - get item failed. No items in table ?", this);
        return null;
    }

    /*
     *  Sampling without replacement (eg draw a card from a deck, don't put it back).
     */
    public Item RemoveRandom()
    {
        var item = GetRandom();
        if (item == null && AutoRefreshOnEmpty)
        {
            RestoreItemWeights();
            item = GetRandom();
        }

        if (item == null)
        {
            if (AutoRefreshOnEmpty) Debug.LogWarning(
                "LootTable - RemoveRandom with AutoRefreshOnEmpty failed. No items in table ?", this);
            return null;
        }

        if (item.Weight > 0) item.Weight--;

        UpdateWeightRanges();
        return item;
    }

    public void Decrement(string itemName)
    {
        foreach (var item in ItemWeights)
        {
            if (item.Name == itemName)
            {
                item.Weight--;
                return;
            }
        }
        throw new KeyNotFoundException();
    }

    public void Decrement(GameObject prefab)
    {
        foreach (var item in ItemWeights)
        {
            if (item.Prefab == prefab)
            {
                item.Weight--;
                return;
            }
        }
        throw new KeyNotFoundException();
    }

    void _Test()
    {

        var _originalWeights = CopyItemWeights();

        for (int i = 0; i < 10; i++)
        {
            var item = GetRandom();
            if (item != null)
            {
                Debug.Log("Loot drop with replacement - " + item.Name, this);
            }
            else
            {
                Debug.Log("No items in table !", this);
            }
        }

        for (int i = 0; i < 102; i++)
        {
            var item = RemoveRandom();
            if (item != null)
            {
                Debug.Log("Loot drop without replacement - " + item.Name, this);
            }
            else
            {
                Debug.Log("No items in table !", this);
            }
        }

        // restore to original state
        ItemWeights = _originalWeights;
    }
}