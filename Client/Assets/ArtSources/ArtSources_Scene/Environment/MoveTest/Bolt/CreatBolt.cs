using UnityEngine;
using System.Collections;

public class CreatBolt : MonoBehaviour 
{
	public float minTime;
	public float maxTime;
	private float m_time;
	public Transform boltPrefab;
    public Transform boltPrefab02;
    public Transform boltPrefab03;
	private Transform m_transform;
//	private GameObject bolt;
	private float desTime = 0.1f;
	private SphereCollider m_collider;
	private Transform m_bolt;
    private float angle;
//	public GameObject player;
	// Use this for initialization
	void Start () 
	{
		//m_transform = this.transform;
//		m_collider = this.GetComponent<SphereCollider>();
		//m_time = Random.Range(minTime,maxTime);
	}
	void OnDrawGizmosSelected() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, 1000 * transform.localScale.x);
	}
	// Update is called once per frame
	void Update () 
	{


//		m_time -= Time.deltaTime;
//        if (m_time <= 0)
//        {
//            m_time = Random.Range(minTime, maxTime);
//            //int a = Random.Range(0,3);
//            //if (a == 0)
//            //    m_bolt = Instantiate(boltPrefab, NewPos(), Quaternion.identity) as Transform;
//            //if (a == 1)
//            //    m_bolt = Instantiate(boltPrefab02, NewPos(), Quaternion.identity) as Transform;
//            //if (a == 2)
//            //    m_bolt = Instantiate(boltPrefab03, NewPos(), Quaternion.identity) as Transform;

//            angle = Random.Range(0.0f,360.0f);
//            float hudu = (angle / 180) * Mathf.PI;
////            Debug.Log(hudu);

//			float xx = m_transform.position.x + (1000 * Mathf.Sin(hudu))*m_transform.localScale.x;
//			float zz = m_transform.position.z + (1000 * Mathf.Cos(hudu))*m_transform.localScale.z;
//            m_bolt = Instantiate(boltPrefab03, new Vector3(xx, 0, zz), Quaternion.identity) as Transform;
//        }
	}
//	IEnumerator DestoryObject( GameObject b)
//	{
//		yield return new WaitForSeconds(0.1f);
//		Debug.Log ("haha");
//		Debug.Log (b.name);
//		Destroy (b);
//	}
	//Vector3 NewPos()
	//{
	//	int pos = Random.Range(1,5);
	//	Vector3 newPos = Vector3.one;
	//	if (pos == 1)
	//		newPos = new Vector3 (m_transform.position.x + m_collider.radius, m_transform.position.y, m_transform.position.z);
	//	if (pos == 2)
	//		newPos = new Vector3 (m_transform.position.x - m_collider.radius, m_transform.position.y, m_transform.position.z);
	//	if (pos == 3)
	//		newPos = new Vector3 (m_transform.position.x, m_transform.position.y, m_transform.position.z + m_collider.radius);
	//	if (pos == 4)
	//		newPos = new Vector3 (m_transform.position.x, m_transform.position.y, m_transform.position.z - m_collider.radius);
	//	return newPos;
	//}
}
