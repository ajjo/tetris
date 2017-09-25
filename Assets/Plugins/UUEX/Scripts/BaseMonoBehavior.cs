using UnityEngine;
using System.Collections;

namespace UUEX
{
	/// <summary>
	/// BaseMonoBehavior
	/// Extend MonoBehaviour to cache variables and add base helper functions
	/// </summary>
	public class BaseMonoBehavior : MonoBehaviour 
	{
		// Caching Unity's variables to avoid GetComponent calls
		[System.NonSerialized]
		protected new Transform transform;

		public virtual void Awake()
		{
			transform = GetComponent<Transform> ();
		}

		public virtual void Update()
		{
		}
	}
}
