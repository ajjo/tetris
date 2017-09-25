using UnityEngine;
using System.Collections;

public class TileO : Tile 
{
	protected override void SpawnUp(Transform tileprefab)
	{
		TileData dependantObject = 	AddBlock(tileprefab, new Vector3(5.0f,22,0), 0);

		AddBlock(tileprefab, new Vector3(6.0f,22,0), 0, dependantObject);
		mA = AddBlock(tileprefab, new Vector3(5.0f,22,0), 1, dependantObject);
		mB = AddBlock(tileprefab, new Vector3(6.0f,22,0), 1, dependantObject);

		GetMaxUpHeight();
	}

	protected override void SpawnDown (Transform tileprefab)
	{
		SpawnUp (tileprefab);
	}

	protected override void SpawnLeft (Transform tileprefab)
	{
		SpawnUp (tileprefab);
	}

	protected override void SpawnRight (Transform tileprefab)
	{
		SpawnUp (tileprefab);
	}

	protected override bool RotateRight()
	{
		return false;
	}

	protected override bool RotateDown()
	{
		return false;
	}

	protected override bool RotateLeft()
	{
		return false;
	}

	protected override bool RotateUp()
	{
		return false;
	}

	protected override void GetMaxUpHeight()
	{
		mMaxHeight = 0;
		int hA = GetMaxGridHeight(Mathf.FloorToInt(mA._SpawnObject.position.x));
		int hB = GetMaxGridHeight(Mathf.FloorToInt(mB._SpawnObject.position.x));

		if(hA > hB)
			mMaxHeight = hA;
		else
			mMaxHeight = hB;
	}

	protected override void GetMaxDownHeight()
	{
		GetMaxUpHeight();
	}

	protected override void GetMaxLeftHeight()
	{
		GetMaxUpHeight();
	}

	protected override void GetMaxRightHeight()
	{
		GetMaxUpHeight();
	}
}
