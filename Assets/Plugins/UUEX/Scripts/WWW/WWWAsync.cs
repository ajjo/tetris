using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace UUEX.UI
{
	public interface IWWWAsync
	{
		void AsyncUpdate(WWWAsync.DownloadState progression, object result, object userData);
	}

	public partial class WWWAsync
	{
		private static Dictionary<string, UnityEngine.Object> mDownloadedObjects = new Dictionary<string, UnityEngine.Object>();
		private static Dictionary<string, List<IWWWAsync>> mDownloadingAssets = new Dictionary<string, List<IWWWAsync>>();

		public enum DownloadState
		{
			NONE,
			IN_PROGRESS,
			COMPLETED,
			ABORTED,
			ERROR
		}

		public class WWWDownload : BaseMonoBehavior
		{
			private string mURL;
			private DownloadState mState;
			private IWWWAsync mReceiver;
			private WWW mWWW;
			private System.Type mType;

			public void StartDownload(IWWWAsync receiver, string url, System.Type type)
			{
				mType = type;
				mReceiver = receiver;
				mURL = url;

				StartCoroutine (StartDownload ());
			}

			private IEnumerator StartDownload()
			{
				mReceiver.AsyncUpdate(DownloadState.IN_PROGRESS, null, null);
				
				mWWW = new WWW (mURL);
				yield return mWWW;
				
				if (string.IsNullOrEmpty (mWWW.error)) 
				{
					List<IWWWAsync> receivers = mDownloadingAssets[mURL];

					if(mType == typeof(Texture2D))
					{
						foreach(IWWWAsync receiver in receivers)
							receiver.AsyncUpdate(DownloadState.COMPLETED, mWWW.texture, null);

						mDownloadingAssets.Remove(mURL);
		
						mDownloadedObjects.Add(mURL, mWWW.texture);
					}
				}
				else
				{
					mReceiver.AsyncUpdate(DownloadState.ERROR, mWWW.error, null);
				}
				
				if (gameObject != null)
					GameObject.Destroy (gameObject);
			}
		}

		public void DownloadAsset(IWWWAsync receiver, string url, System.Type type)
		{
			if(!mDownloadedObjects.ContainsKey(url))
			{
				// Add to the downloading list
				if(!mDownloadingAssets.ContainsKey(url))
				{
					List<IWWWAsync> receivers = new List<IWWWAsync>();
					receivers.Add(receiver);
					mDownloadingAssets.Add(url, receivers);
					GameObject newDownload = new GameObject (url);
					WWWDownload wwwDownload = newDownload.AddComponent<WWWDownload> ();
					wwwDownload.StartDownload(receiver, url, type);
				}
				else
				{
					List<IWWWAsync> receivers = mDownloadingAssets[url];
					receivers.Add(receiver);
					mDownloadingAssets[url] = receivers;
				}
			}
			else
			{
				// Get it from the cache
				receiver.AsyncUpdate(DownloadState.COMPLETED, mDownloadedObjects[url], null);
			}
		}
	}
}