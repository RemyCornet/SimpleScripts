using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class PoolManager : MonoBehaviour
{
    
    [System.Serializable]
    public class PoolName
    {
        public string Name;
        public GameObject Prefab;
        public int Number;
    }
    public static PoolManager instance;
    private void Awake()
    {
        instance = this;
    }
    public Dictionary<string, Queue<GameObject>> poolList;
    public List<PoolName> pools;
    private void Start()
    {
        poolList = new Dictionary<string, Queue<GameObject>>();

        foreach (PoolName pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            {
                for (int i = 0; i< pool.Number; i++)
                {
                    GameObject obj = Instantiate(pool.Prefab);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                    obj.transform.parent = this.transform;
                }
                poolList.Add(pool.Name, objectPool);
            }
        }
    }
    public GameObject SpawItem(string Name, Transform trans, GameObject Target)
    {
        if (poolList.ContainsKey(Name))
        {
            GameObject objectToSpawn = poolList[Name].Dequeue();
            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = trans.position;
            objectToSpawn.transform.rotation = trans.rotation;
            
            if (objectToSpawn.GetComponent<HomingProjectile>() != null)
            {
                objectToSpawn.GetComponent<HomingProjectile>().assignetarget(Target);
            }
            poolList[Name].Enqueue(objectToSpawn);
            return objectToSpawn;
        }
        else
        {
            return null;
        }        
    }

}


