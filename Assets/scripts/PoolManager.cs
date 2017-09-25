using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour
{
	public static int counter = 1;

	[System.Serializable]
	public class PoolData
	{
		public string _ObjectName;
		public Transform _Object;
		public int _SpawnCount;

		[System.NonSerialized]
		public List<GameObject> _PoolList = new List<GameObject>();

		public GameObject GetObject(Transform parentTransform)
		{
			if (_PoolList.Count > 0) 
			{
				GameObject objectInPool = _PoolList[0];
				_PoolList.RemoveAt(0);
				return objectInPool;
			}
			else
			{
				GameObject newObject = (GameObject)GameObject.Instantiate(_Object.gameObject);
				newObject.name = _ObjectName;
				newObject.transform.parent = parentTransform;
				newObject.SetActive (false);
				return newObject;
			}
		}

		public void Instantiate(Transform parentTransform)
		{
			GameObject newObject = (GameObject)GameObject.Instantiate(_Object.gameObject);
			newObject.name = _ObjectName;
			newObject.transform.parent = parentTransform;
			newObject.SetActive (false);
			_PoolList.Add(newObject);
		}

		public void AddObject(GameObject reclaimObject)
		{
			//Debug.Log ("Adding " + reclaimObject.name);
			_PoolList.Add (reclaimObject);
			reclaimObject.transform.parent = mInstance.transform;
			reclaimObject.SetActive (false);
		}
	}
	
	public List<PoolData> _PoolData = new List<PoolData>();
	private static PoolManager mInstance = null;

	public void Awake ()
	{
		if (mInstance != null)
		{
			Debug.Log ("Destroying duplicate instance of pool manager");
			GameObject.Destroy (gameObject);
		}

		mInstance = this;
		
		foreach(PoolData pd in _PoolData)
		{
			for(int i=0; i<pd._SpawnCount; i++)
			{
				pd.Instantiate(transform);
			}
		}
	}

	public static GameObject GetObject(string objectName)
	{
		PoolData poolData = mInstance._PoolData.Find (p => p._ObjectName == objectName);
		if(poolData != null)
			return poolData.GetObject(mInstance.transform);

		return null;	
	}

	public static void AddObject(string objectName, GameObject reclaimObject)
	{
		PoolData poolData = mInstance._PoolData.Find (p => p._ObjectName == objectName);
		if(poolData != null)
		{
			if(!poolData._PoolList.Contains(reclaimObject))
				poolData.AddObject(reclaimObject);
		}
	}

	public static void Clear()
	{
		mInstance._PoolData.Clear ();
	}
}
