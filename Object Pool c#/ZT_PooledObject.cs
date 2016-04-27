using UnityEngine;
using System.Collections;

public class ZT_PooledObject : MonoBehaviour {

	//the pool this belongs to.
	public ZT_Pool pool{get;private set;}

	public void SetPool(ZT_Pool poolObj)
	{
		pool = poolObj;
	}

	//returns this item to the pool it came from
	public void ReturnItem()
	{
		if(pool != null)
			pool.ReturnPoolItem(gameObject);
	}
}
