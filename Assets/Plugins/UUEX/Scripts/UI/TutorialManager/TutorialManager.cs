using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UUEX.UI.Effects;

namespace UUEX.UI.Tutorial
{
	public enum StepAction
	{
		BEGIN,
		BEGIN_STEP,
		END_STEP,
		END
	}

	public class TutorialManager : BaseMonoBehavior
	{
		private UnityAction<NormalStep, StepAction> onStepUpdate = null;
		private NormalStep [] mSteps = null;
		private int mCurrentStep = 1;

		public override void Awake()
		{
			base.Awake ();

			mSteps = GetComponentsInChildren<NormalStep> ();

			for (int i=1; i<mSteps.Length; i++)
				mSteps [i].enabled = false;
		}

		protected virtual void Start()
		{
			if (onStepUpdate != null)
				onStepUpdate (mSteps [mCurrentStep - 1], StepAction.BEGIN);

			Begin ();
		}

		public void AddStepUpdateListener(UnityAction<NormalStep, StepAction> action)
		{
			onStepUpdate += action;
		}

		public void RemoveStepUpdateListener(UnityAction<NormalStep, StepAction> action)
		{
			onStepUpdate -= action;
		}

		public void Begin()
		{
			if (onStepUpdate != null)
				onStepUpdate (mSteps [mCurrentStep - 1], StepAction.BEGIN_STEP);

			mSteps [mCurrentStep - 1].Begin ();
		}

		public void End()
		{
			if (onStepUpdate != null)
				onStepUpdate (mSteps [mCurrentStep - 1], StepAction.END);

			GameObject.Destroy (gameObject);
		}

		public void NextStep()
		{
			Debug.Log ("Called next step " + mCurrentStep);
			if(mCurrentStep == mSteps.Length)
				End ();
			else
			{
				if (onStepUpdate != null)
					onStepUpdate (mSteps [mCurrentStep - 1], StepAction.END_STEP);

				mCurrentStep++;
				Begin ();
			}
		}
	}
}
