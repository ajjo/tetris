using UnityEngine;
using System.Collections;

namespace UUEX.UI
{
	public class SoundManager : BaseMonoBehavior 
	{
		private static SoundManager mInstance = null;
		private AudioSource mAudioSource = null;

		public override void Awake ()
		{
			base.Awake ();

			mInstance = this;
			mAudioSource = GetComponent<AudioSource>();
		}
	
		public static void Play(AudioClip audioClip)
		{
			if(mInstance.mAudioSource != null)
			{
				mInstance.mAudioSource.clip = audioClip;
				mInstance.mAudioSource.Play();
			}
		}

		
		public virtual void AsyncUpdate(WWWAsync.DownloadState progression, object result, object userData)
		{
			if (progression == UUEX.UI.WWWAsync.DownloadState.COMPLETED && result != null && result is AudioClip) 
			{
				SoundManager.Play(result as AudioClip);
			}
		}
	}
}
