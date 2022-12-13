using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DropItems : MonoBehaviour
{

    [SerializeField] 
    private SpetificationsItem[] dropItems = new SpetificationsItem[3];
    
    [SerializeField]
    private GameObject itemSpawn;
    
    [SerializeField] 
    private bool randomTypeItems;
    
    private int _maxCountItemsSpawn;

    private SpetificationsItem[] _allDropItems = new SpetificationsItem[6];
    private void Awake()
    {
        for (int i = 0; i < dropItems.Length; i++)
        {
            _allDropItems[i] = dropItems[i];
        }
        _allDropItems[3] = DataHolder.GreenFlaskEffect;
        _allDropItems[4] = DataHolder.YellowFlaskEffect;
        _allDropItems[5] = DataHolder.RedFlaskEffect;
    }

    public void Drop()
    {
        if (randomTypeItems)
        {
            for (int i = 0; i < Random.Range(0, 5); i++)
            {
                int indexItem = Random.Range(0, _allDropItems.Length);
                itemSpawn.GetComponent<Item>().SetItemType(_allDropItems[indexItem]);
                GameObject newItem = Instantiate(itemSpawn);
                newItem.transform.position = transform.position;
            }
        }
        else
        {
            for (int i=0; i<_allDropItems.Length;i++)
            {
                float chanceSpawn = _allDropItems[i].ChanceSpawnItem;
                if (Random.Range(0f, 1f) < chanceSpawn)
                {
                    itemSpawn.GetComponent<Item>().SetItemType(_allDropItems[i]);
                    _maxCountItemsSpawn = _allDropItems[i].MaxCountItems;
                }
            }

            for (int i = 0; i < Random.Range(0, _maxCountItemsSpawn); i++)
            {
                GameObject newItem = Instantiate(itemSpawn);
                RandomSplashItem randomSplash = newItem.GetComponent<RandomSplashItem>();
                if (randomSplash == null)
                {
                    Destroy(newItem);
                    continue;
                }
                newItem.transform.position = transform.position;
            }
         
        }
    }
    
}
