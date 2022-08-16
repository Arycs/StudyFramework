using UnityEngine;
using System.Collections;

public class DestoryBolt : MonoBehaviour 
{
	private float timer = 1;
	private Transform m_transform;
	// Use this for initialization
	void Start () 
	{
		m_transform = this.transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		timer -= Time.deltaTime;
		if(timer<=0)
		{
			Destroy(m_transform.gameObject);
		}
	}
//	public void LookAtPlayer(Vector3 other)
//	{
//		m_transform.LookAt (other);
//	}
}
