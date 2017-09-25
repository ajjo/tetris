using UnityEngine;
using System.Collections;
using UUEX.UI;

public class UIGameOver : UI 
{
	public TetrisManager _TetrisManager;

	public override void OnItemClick (UIItem item)
	{
		base.OnItemClick (item);

		if(item.GetName() == "BtnReplay")
		{
			SetVisibility(false);
			_TetrisManager.RestartGame();
		}
	}
}
