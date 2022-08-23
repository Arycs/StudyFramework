using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YouYou;

public class TestPool : MonoBehaviour
{
    private Queue<Transform> m_RoleObjList;
    // Start is called before the first frame update
    void Start()
    {
        m_RoleObjList = new Queue<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.B))
        {
            GameEntry.Pool.GameObjectSpawn(1,((trans, isNewInstance) =>
            {
                m_RoleObjList.Enqueue(trans);
                trans.GetComponent<RoleCtrl>().InitPlayerData(100001);
            }));
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            if (m_RoleObjList .Count > 0)
            {
                var obj = m_RoleObjList.Dequeue();
                GameEntry.Pool.GameObjectDeSpawn(obj);
            }
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            GameEntry.Data.RoleDataManager.DeSpawnAllRole();
        }
    }
}
