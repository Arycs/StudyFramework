using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavManager : MonoBehaviour
{
    public NavMeshAgent Agent;

    private NavMeshPath path;

    private void Start()
    {
        path = new NavMeshPath();
    }

    private int add = 200;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            float beginTime = Time.realtimeSinceStartup;

            Agent.enabled = true;
            // 对齐到起点
            Agent.Warp(new Vector3(171.9f, add + 25.5f, 345.6f));
            Agent.CalculatePath(new Vector3(172.1f, add + 25.5f, 331.6f), path);
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                
            }
        }

        if (path == null)
        {
            return;
        }

#if UNITY_EDITOR
        for (int i = 1; i < path.corners.Length; ++i)
        {
            Debug.DrawLine(path.corners[i-1],path.corners[i],Color.yellow);
        }
#endif
    }
}
