using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QixiLuo.Tool.Singleton;
public class GameObjcetPool : Singleton<GameObjcetPool>
{
    public Queue<GameObject> queues = new Queue<GameObject>();

    public GameObject prefab;

    public int NUM_MAX = 15;

    //private void Awake()
    //{
    //    instacne = this;
    //}


    private void FullPool()
    {
        for (int i = 0; i < NUM_MAX; i++)
        {
            GameObject go = Instantiate(prefab);
            go.transform.SetParent(transform);
            
            ReturnToPool(go);
        }
    }

    public GameObject TakeFromPool()
    {
        if (queues.Count <= 0)
        {
            FullPool();
        }

        queues.Dequeue().SetActive(true);

        return queues.Dequeue();
    }

    public void ReturnToPool(GameObject go)
    {
        go.SetActive(false);
        queues.Enqueue(go);
    }


}
