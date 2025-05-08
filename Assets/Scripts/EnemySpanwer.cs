using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EnemySpanwer : NetworkBehaviour
{

    public List<GameObject> enemyPerfabs = new List<GameObject>();
    public int Length;
    public int numberOfEnemies;
    public List<GameObject> StarPos = new List<GameObject>();

    public override void OnStartServer()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {

            Length = Random.Range(0, enemyPerfabs.Count);

            Vector3 position = StarPos[Random.Range(0,StarPos.Count)].transform.position;
                //new Vector3((float)-0.4, (float)-4.5, 0);

            Quaternion rotation = Quaternion.Euler(0, 0, 0);

            GameObject enemy = Instantiate(enemyPerfabs[Length] , position, rotation) as GameObject;

            NetworkServer.Spawn(enemy);

        }
    }


}
