using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZT_Pool : MonoBehaviour {

	//The bag of objects that can be used.  
	List<GameObject> _poolList = new List<GameObject>();
	public List<GameObject> poolList {get{return _poolList;}}
	
	//A list of items from the bag that CURRENTLY being used.
	List<GameObject> currentPoolObjectsInUse = new List<GameObject>();
	
	//Use the Unity inspector to drag a prefab or type in a prefab name to use as the object for the pool
	[SerializeField]
	GameObject preFab; 
	[SerializeField]
	string prefabLoadString;
	
	//If set to prefab, must provide a prefab to be used, otherwise a new blank GameObject will be used.
	public enum PoolTypes{PreFab, BlankGameObject};
	public PoolTypes poolType = PoolTypes.BlankGameObject;
	
	//Create the pool when this pool object is sent the Awake message
	[SerializeField]
	bool createOnAwake = false; 
	
	//The Transform for the object that contains the pooled objects not in use
	Transform newPoolTrans;
	
	//Name of this Pool used for Getting the Pool from the Static cache of pools
	[SerializeField]
	string _poolName = "PooledObject";

	//On Awake, create this amount of pooled objects.  Set in Unity Inspector.
	[SerializeField]
	int awakePoolAmount = 10;
	
	//Can check if the last used item was created or was from the pool.  Could be used as a way to stop
	//spawning objects once the pool is used up.
	bool _lastItemWasInPool = true;
	public bool lastItemWasInPool {get{return _lastItemWasInPool;}}

	//Disable objects when they are returned to the pool
	[SerializeField]
	bool disableOnReturn = false;
	
	//Set a local transform scale to pooled objects.
	//Helpful for prefabs since instantiating a prefab can have unknown scales
	[SerializeField]
	bool setNewLocalScale = false;
	[SerializeField]
	Vector3 newLocalScale = new Vector3(1,1,1);

	//A cache of pools for easy static access via GetPool method.
	static List<ZT_Pool> cachedPools = new List<ZT_Pool>();
	[SerializeField]
	bool registerThisPool = false;
	
	//When new Unity level is loaded don't destroy this pool
	[SerializeField]
	bool dontDestroyOnLoad = false;

	public static ZT_Pool GetPool(string poolName)
	{
		foreach(ZT_Pool pool in cachedPools)
		{
			if(pool._poolName == poolName || pool.name == poolName)
				return pool;
		}

		Debug.Log("No Pool: " + poolName + " exists!");
		return null;
	}

	/****
	**Initialization
	**/

	//Called by Unity when this Object is instantiated
	void Awake()
	{
		//load the prefab object for pooling
		if(prefabLoadString != "")
			preFab = Resources.Load(prefabLoadString) as GameObject;
					
		if(!createOnAwake)
			return;
		else
			CreatePool(awakePoolAmount,_poolName);

		if(registerThisPool && !cachedPools.Contains(this))
			cachedPools.Add(this);

		if(dontDestroyOnLoad)
			DontDestroyOnLoad(gameObject);

		ExtraAwake();
	}

	//Can be overridden in derived classes to do extra steps during Awake
	public virtual void ExtraAwake()
	{
		//
	}

	void OnDestroy()
	{
		//remove from pool cache when pool is destroyed
		if(registerThisPool && cachedPools.Contains(this))
			cachedPools.Remove(this);
	}


	/**
	Pool Creation and deletion
	**/
	
	public void CreatePool(int poolAmount, string poolName, Transform poolObjectParent)
	{
		if(_poolList != null)
			DestroyPoolObjects();

		GameObject newPoolGameObj = new GameObject(poolName);
		newPoolTrans = newPoolGameObj.transform;
		newPoolTrans.SetParent(poolObjectParent);
		newPoolTrans.localPosition = Vector3.zero;
		
		_poolName = poolName;

		for (int i = 0; i<poolAmount; i++)
		{
			NewPoolItem(newPoolTrans,poolName);
		}
	}	

	void DestroyPoolObjects()
	{
		foreach(GameObject obj in _poolList)
		{
			Destroy(obj);
		}

		_poolList = new List<GameObject>();
	}

	/**
	Create a single pooled object
	**/
	
	public GameObject NewPoolItem(Transform poolParent, string poolName)
	{
		GameObject newPoolObj;
		
		//use prefab or create blank object
		if(poolType == PoolTypes.PreFab)
			newPoolObj = Instantiate(preFab) as GameObject;
		else{
			newPoolObj = new GameObject(poolName);
		}
		
		//set the transform parent to the pools parent transform
		newPoolObj.transform.SetParent(poolParent);		
		
		//check if pool object component exists on new object, if not add it.
		ZT_PooledObject pooledObj = newPoolObj.GetComponent<ZT_PooledObject>();		
		if(pooledObj == null)
			pooledObj = newPoolObj.AddComponent<ZT_PooledObject>();
		
		//Tell the pooled object which pool it belongs too by adding the PooledObject script and set its pool
		pooledObj.SetPool(this);
		
		if(setNewLocalScale)
			newPoolObj.transform.localScale = newLocalScale;
			
		if(disableOnReturn)
			newPoolObj.SetActive(false);

		_poolList.Add(newPoolObj);

		PoolCreateExtra(newPoolObj);
		
		return newPoolObj;
	}
	
	//on derived classes allow for adding extra steps when creating a pooled item
	public virtual void PoolCreateExtra(GameObject poolObj)
	{
	}


	/*
	Single Pool object usage 
	*/
	
	public GameObject usePoolItem()
	{
		//create a new object if needed
		if(_poolList.Count < 1) {
			NewPoolItem(newPoolTrans,_poolName);
			_lastItemWasInPool = false;
		} else
			_lastItemWasInPool = true;
			
		GameObject newObj = _poolList[0];
		
		//remove from bag of pool items
		_poolList.RemoveAt(0);
		
		//add to in use items list
		currentPoolObjectsInUse.Add(newObj);

		return newObj;
	}

	//useful to immidiately set the returned pool item to active
	public GameObject usePoolItem(bool setActive)
	{
		GameObject obj = usePoolItem();
		obj.SetActive(setActive);
		return obj;
	}

	public GameObject usePoolItem(Vector3 worldPos, Vector3 localRot, Vector3 localScale, bool setActive)
	{
		GameObject poolObj = usePoolItem();
		Transform poolObjTrans = poolObj.transform;
		poolObjTrans.position = worldPos;
		poolObjTrans.localScale = localScale;
		poolObjTrans.localEulerAngles = localRot;
		poolObj.SetActive(setActive);
		return poolObj;
	}

	/***
	Returning Pool Items to the pool
	**/
	public void ReturnPoolItem(GameObject obj)
	{
		//make sure this is a pooled object
		if(obj.GetComponent<ZT_PooledObject>() == null)
		{
			Debug.Log("Not a pool object");
			return;		
		}
		
		//add back to pool list
		_poolList.Add(obj);
		
		//set transform parent to pool transform container
		obj.transform.SetParent(newPoolTrans);

		//remove from current in use list
		if(currentPoolObjectsInUse.Contains(obj))
			currentPoolObjectsInUse.Remove(obj);

		//disable object
		if(disableOnReturn)
			obj.SetActive(false);
	}

	public void ReturnAllPoolItems()
	{
		while(currentPoolObjectsInUse.Count > 0)
			ReturnPoolItem(currentPoolObjectsInUse[0]);
	}
}
