﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PoolByID : MonoSingleton<PoolByID>
{
    private IDictionary<int, List<GameObject>> pools;
    private IDictionary<string, GameObject> poolPrefabs;

    protected override void Awake()
    {
        base.Awake();
        pools = new Dictionary<int, List<GameObject>>();
        poolPrefabs = new Dictionary<string, GameObject>();
    }

    public GameObject InstantiatePrefab(GameObject obj)
    {
        var _obj = Instantiate(obj) as GameObject;
        _obj.name = _obj.name.Replace("(Clone)", "");
        return _obj;
    }

    public GameObject GetPrefab(string path)
    {
        if(poolPrefabs.ContainsKey(path))
        {
            return GetPrefab(poolPrefabs[path]);
        }
        else
        {
            var _prefab = Resources.Load<GameObject>(path);
            poolPrefabs.Add(path, _prefab);
            return GetPrefab(_prefab);
        }
    }

    public GameObject GetPrefab(GameObject obj)
    {
        var id = obj.GetInstanceID();
        if (pools.ContainsKey(id))
        {
            for (int i = 0; i < pools[id].Count; i++)
            {
                if (!pools[id][i].activeInHierarchy)
                {
                    pools[id][i].SetActive(true);
                    return pools[id][i];
                }
            }
        }
        else
        {
            pools.Add(id, new List<GameObject>());
        }

        var _obj = Instantiate(obj) as GameObject;
        _obj.name = _obj.name.Replace("(Clone)", "");
        pools[id].Add(_obj);
        return _obj;
    }

    public GameObject GetPrefab(GameObject obj, Vector3 pos, Quaternion quaternion)
    {
        var id = obj.GetInstanceID();
        if (pools.ContainsKey(id))
        {
            //Find obj from pool
            for (int i = 0; i < pools[id].Count; i++)
            {
                if (!pools[id][i].activeSelf)
                {
                    pools[id][i].transform.position = pos;
                    pools[id][i].transform.rotation = quaternion;
                    pools[id][i].SetActive(true);

                    return pools[id][i];
                }
            }
        }
        else
        {
            //Create pool
            pools.Add(id, new List<GameObject>());
        }

        var _obj = Instantiate(obj, pos, quaternion) as GameObject;
        _obj.name = _obj.name.Replace("(Clone)", "");
        pools[id].Add(_obj);
        return _obj;
    }

    public T GetPrefab<T>(GameObject obj, Vector3 pos, Quaternion quaternion)
    {
        var _obj = GetPrefab(obj, pos, quaternion);
        return _obj.GetComponent<T>();
    }

    public T GetPrefab<T>(GameObject obj)
    {
        var _obj = GetPrefab(obj);
        return _obj.GetComponent<T>();
    }

    public void PushToPool(GameObject obj)
    {
        if (obj != null)
        {
            var id = obj.GetInstanceID();
            if (pools.ContainsKey(id))
            {
                if (!pools[id].Contains(obj))
                    pools[id].Add(obj);
            }
            else
            {
                //Create pool
                pools.Add(id, new List<GameObject>());
                pools[id].Add(obj);
            }

            obj.transform.parent = null;
            obj.SetActive(false);
        }
    }
}
