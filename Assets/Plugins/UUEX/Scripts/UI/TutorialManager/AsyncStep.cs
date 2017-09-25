using UnityEngine;
using System.Collections;
using UUEX.UI.Effects;

namespace UUEX.UI.Tutorial
{
	public class AsyncStep : NormalStep, IWWWAsync
	{
		public string _BundleURL;

		public override void Begin ()
		{
			if(Application.isEditor)
			{
				GameObject obj = (GameObject)Resources.Load ("PfUIAsyncTest");
				obj = GameObject.Instantiate(obj);
				UpdateStep(obj);
				base.Begin();
			}
			else
			{
				ResourceManager.DownloadAsset (this, _BundleURL, typeof(GameObject));
			}
		}

		public virtual void AsyncUpdate(WWWAsync.DownloadState progression, object result, object userData)
		{
			if (progression == UUEX.UI.WWWAsync.DownloadState.COMPLETED) 
			{
				// Instantiate and update
			}
		}

		private void UpdateStep(GameObject obj)
		{
			UI ui = obj.GetComponentInChildren<UI> ();
			UIItem item = ui.GetItem (_ItemName);

			foreach(BaseEffect effect in mEffects)
			{
				effect._Ui = ui;
				effect._UiItem = item;
			}

			_UI = ui;
		}

	}
}
