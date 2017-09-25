using UnityEngine;
using System.Collections;
using UUEX.UI;

public class UIStart : UI 
{
	public TetrisManager _TetrisManager;
	void Start()
	{

	}

	public override void OnItemClick (UIItem item)
	{
		base.OnItemClick (item);

		if(item.GetName() == "BtnStart")
		{
			SetVisibility(false);
			_TetrisManager.BeginGame();
		}
	}
}
