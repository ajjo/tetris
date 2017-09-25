using UnityEngine;
using System.Collections;

public class TileT : Tile 
{
	protected override void SpawnUp(Transform tileprefab)
	{
		TileData dependantObject = 	AddBlock(tileprefab, new Vector3(6.0f,22,0), 0);
		
		AddBlock(tileprefab, new Vector3(5.0f,22,0), 0, dependantObject);
		AddBlock(tileprefab, new Vector3(6.0f,22,0), 1, dependantObject);
		AddBlock(tileprefab, new Vector3(7.0f,22,0), 0, dependantObject);
		
		GetMaxUpHeight();
	}
	
	protected override void SpawnDown(Transform tileprefab)
	{
		TileData dependantObject = 	AddBlock(tileprefab, new Vector3(6.0f,21,0), 0);
		
		mA = AddBlock(tileprefab, new Vector3(5.0f,21,0), 1, dependantObject);
		mB = AddBlock(tileprefab, new Vector3(6.0f,21,0), 1, dependantObject);
		mC = AddBlock(tileprefab, new Vector3(7.0f,21,0), 1, dependantObject);
		
		GetMaxDownHeight();
	}
	
	protected override void SpawnRight(Transform tileprefab)
	{
		TileData dependantObject = AddBlock(tileprefab, new Vector3(5.0f,22.0f), 0);
		
		AddBlock(tileprefab, new Vector3(5.0f,22,0), 1, dependantObject);
		mA = AddBlock(tileprefab, new Vector3(5.0f,22,0), 2, dependantObject);
		mB = AddBlock(tileprefab, new Vector3(6.0f,22,0), 1, dependantObject);
		
		GetMaxRightHeight();
	}
	
	protected override void SpawnLeft(Transform tileprefab)
	{
		TileData dependantObject = AddBlock(tileprefab, new Vector3(6.0f,22.0f), 0);
		
		AddBlock(tileprefab, new Vector3(6.0f,22,0), 1, dependantObject);
		mA = AddBlock(tileprefab, new Vector3(6.0f,22,0), 2, dependantObject);
		mB = AddBlock(tileprefab, new Vector3(5.0f,22,0), 1, dependantObject);
		
		GetMaxLeftHeight();
	}

	protected override bool RotateRight()
	{
		TileData dependantObject = mSpawnObjects[0];
		Vector3 dependantPos = dependantObject._SpawnObject.position;
		
		UpdateBlock(mSpawnObjects[1], dependantPos.x, 1, dependantObject);
		mA = UpdateBlock(mSpawnObjects[2], dependantPos.x, 2, dependantObject);
		mB = UpdateBlock(mSpawnObjects[3], dependantPos.x + 1, 1, dependantObject);
		
		GetMaxRightHeight();
		return true;
	}

	protected override bool RotateDown()
	{
		TileData dependantObject = mSpawnObjects[0];
		Vector3 dependantPos = dependantObject._SpawnObject.position;
		
		if(dependantPos.x == 0)
			return false;
		
		mA = UpdateBlock(mSpawnObjects[1], dependantPos.x - 1, 1, dependantObject);
		mB = UpdateBlock(mSpawnObjects[2], dependantPos.x, 1, dependantObject);
		mC = UpdateBlock(mSpawnObjects[3], dependantPos.x + 1, 1, dependantObject);
		
		GetMaxDownHeight();
		return true;
	}

	protected override bool RotateUp()
	{
		TileData dependantObject = mSpawnObjects[0];
		Vector3 dependantPos = dependantObject._SpawnObject.position;
		
		if(dependantPos.x == 9)
			return false;
		
		UpdateBlock(mSpawnObjects[1], dependantPos.x - 1, 0, dependantObject);
		UpdateBlock(mSpawnObjects[2], dependantPos.x, 1, dependantObject);
		UpdateBlock(mSpawnObjects[3], dependantPos.x + 1, 0, dependantObject);
		
		GetMaxUpHeight();
		return true;
	}

	protected override bool RotateLeft()
	{
		TileData dependantObject = mSpawnObjects[0];
		Vector3 dependantPos = dependantObject._SpawnObject.position;
		
		UpdateBlock(mSpawnObjects[1], dependantPos.x, 1, dependantObject);
		mA = UpdateBlock(mSpawnObjects[2], dependantPos.x, 2, dependantObject);
		mB = UpdateBlock(mSpawnObjects[3], dependantPos.x - 1, 1, dependantObject);
		
		GetMaxLeftHeight();
		return true;
	}

	protected override void GetMaxUpHeight()
	{
		mMaxHeight = 0;

		foreach(TileData td in mSpawnObjects)
		{
			if(td._DependantObject != null)
			{
				Vector3 pos = td._SpawnObject.position;
				int height = GetMaxGridHeight(Mathf.FloorToInt(pos.x));
				if(height > mMaxHeight)
					mMaxHeight = height;
			}
		}
	}

	protected override void GetMaxDownHeight()
	{
		int hA = GetMaxGridHeight(Mathf.FloorToInt(mA._SpawnObject.position.x));
		int hB = GetMaxGridHeight(Mathf.FloorToInt(mB._SpawnObject.position.x));
		int hC = GetMaxGridHeight(Mathf.FloorToInt(mC._SpawnObject.position.x));

		if(hB == hC - 1 && hB == hA - 1)
			mMaxHeight = hB;
		else if(hB == hC - 1)
			mMaxHeight = hB;
		else if(hB == hA - 1)
			mMaxHeight = hB;
		else if(hB < hC - 1)
			mMaxHeight = hC - 1;
		else if(hB < hA - 1)
			mMaxHeight = hA - 1;
		else
			mMaxHeight = hB;
	}

	protected override void GetMaxRightHeight()
	{
		int hA = GetMaxGridHeight(Mathf.FloorToInt(mA._SpawnObject.position.x));
		int hB = GetMaxGridHeight(Mathf.FloorToInt(mB._SpawnObject.position.x));

		if(hA == hB - 1)
			mMaxHeight = hA;
		else if(hA < hB - 1)
			mMaxHeight = hB - 1;
		else if(hA > hB)
			mMaxHeight = hA;
		else
			mMaxHeight = hB;
	}

	protected override void GetMaxLeftHeight()
	{
		GetMaxRightHeight();
	}
}
