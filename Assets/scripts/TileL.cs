using UnityEngine;
using System.Collections;

public class TileL : Tile 
{
	// Spawn blocks in the specific direction
	protected override void SpawnUp(Transform tileprefab)
	{
		TileData dependantObject = 	AddBlock(tileprefab, new Vector3(5.0f,22,0), 0);
		
		AddBlock(tileprefab, new Vector3(5.0f,22,0), 1, dependantObject);
		mA = AddBlock(tileprefab, new Vector3(5.0f,22,0), 2, dependantObject);
		mB = AddBlock(tileprefab, new Vector3(6.0f,22,0), 0, dependantObject);
		
		GetMaxUpHeight();
	}
	
	protected override void SpawnDown(Transform tileprefab)
	{
		TileData dependantObject = 	AddBlock(tileprefab, new Vector3(6.0f,21,0), 0);
		
		AddBlock(tileprefab, new Vector3(6.0f,21,0), 1, dependantObject);
		mB = AddBlock(tileprefab, new Vector3(6.0f,21,0), 2, dependantObject);
		mA = AddBlock(tileprefab, new Vector3(5.0f,21,0), 2, dependantObject);
		
		GetMaxDownHeight();
	}
	
	protected override void SpawnRight(Transform tileprefab)
	{
		TileData dependantObject = 	AddBlock(tileprefab, new Vector3(5.0f,21,0), 0);
		
		mA = AddBlock(tileprefab, new Vector3(5.0f,21,0), 1, dependantObject);
		mB = AddBlock(tileprefab, new Vector3(6.0f,21,0), 1, dependantObject);
		mC = AddBlock(tileprefab, new Vector3(7.0f,21,0), 1, dependantObject);
		
		GetMaxRightHeight();
	}
	
	protected override void SpawnLeft(Transform tileprefab)
	{
		TileData dependantObject = AddBlock(tileprefab, new Vector3(7.0f,22.0f), 0);
		
		mA = AddBlock(tileprefab, new Vector3(5.0f,22,0), 0, dependantObject);
		mB = AddBlock(tileprefab, new Vector3(6.0f,22,0), 0, dependantObject);
		mC = AddBlock(tileprefab, new Vector3(7.0f,22,0), 1, dependantObject);
		
		GetMaxLeftHeight();
	}

	protected override bool RotateRight()
	{
		foreach(TileData td in mSpawnObjects)
		{
			if(td._DependantObject == null && td._SpawnObject.position.x > 7)
				return false;
		}

		TileData dependantObject = mSpawnObjects[0];
		Vector3 dependantPos = dependantObject._SpawnObject.position;
		
		mA = UpdateBlock(mSpawnObjects[3], dependantPos.x, 1, dependantObject);
		mB = UpdateBlock(mSpawnObjects[1], dependantPos.x + 1, 1, dependantObject);
		mC = UpdateBlock(mSpawnObjects[2], dependantPos.x + 2, 1, dependantObject);
		
		GetMaxRightHeight();
		return true;
	}

	protected override bool RotateDown()
	{
		TileData dependantObject = mSpawnObjects[0];
		Vector3 dependantPos = dependantObject._SpawnObject.position;
		
		if(dependantPos.x == 0)
			return false;
		
		UpdateBlock(mSpawnObjects[1], dependantPos.x, 1, dependantObject);
		mB = UpdateBlock(mSpawnObjects[2], dependantPos.x, 2, dependantObject);
		mA = UpdateBlock(mSpawnObjects[3], dependantPos.x - 1, 2, dependantObject);
		
		GetMaxDownHeight();
		return true;
	}

	protected override bool RotateLeft()
	{
		foreach(TileData td in mSpawnObjects)
		{
			if(td._DependantObject == null && td._SpawnObject.position.x < 2)
				return false;
		}

		TileData dependantObject = mSpawnObjects[0];
		Vector3 dependantPos = dependantObject._SpawnObject.position;
		
		mA = UpdateBlock(mSpawnObjects[3], dependantPos.x - 2, 0, dependantObject);
		mB = UpdateBlock(mSpawnObjects[1], dependantPos.x - 1, 0, dependantObject);
		mC = UpdateBlock(mSpawnObjects[2], dependantPos.x, 1, dependantObject);
		
		GetMaxLeftHeight();
		return true;
	}

	protected override bool RotateUp()
	{
		TileData dependantObject = mSpawnObjects[0];
		Vector3 dependantPos = dependantObject._SpawnObject.position;
		
		if(dependantPos.x == 9)
			return false;
		
		UpdateBlock(mSpawnObjects[1], dependantPos.x, 1, dependantObject);
		mA = UpdateBlock(mSpawnObjects[2], dependantPos.x, 2, dependantObject);
		mB = UpdateBlock(mSpawnObjects[3], dependantPos.x + 1, 0, dependantObject);
		
		GetMaxUpHeight();
		return true;
	}
	
	protected override void GetMaxUpHeight()
	{
		int hA = GetMaxGridHeight(Mathf.FloorToInt(mA._SpawnObject.position.x));
		int hB = GetMaxGridHeight(Mathf.FloorToInt(mB._SpawnObject.position.x));

		if(hA > hB)
			mMaxHeight = hA;
		else
			mMaxHeight = hB;
	}

	protected override void GetMaxDownHeight()
	{
		int hA = GetMaxGridHeight(Mathf.FloorToInt(mA._SpawnObject.position.x));
		int hB = GetMaxGridHeight(Mathf.FloorToInt(mB._SpawnObject.position.x));

		if(hB < hA - 1)
			mMaxHeight = hA - 1;
		if(hB < hA - 2)
			mMaxHeight = hA - 2;
		else if(hB == hA - 1)
			mMaxHeight = hB;
		else
			mMaxHeight = hB;
	}

	protected override void GetMaxRightHeight()
	{
		int hA = GetMaxGridHeight(Mathf.FloorToInt(mA._SpawnObject.position.x));
		int hB = GetMaxGridHeight(Mathf.FloorToInt(mB._SpawnObject.position.x));
		int hC = GetMaxGridHeight(Mathf.FloorToInt(mC._SpawnObject.position.x));

		if(hA == hB - 1)
			mMaxHeight = hA;
		else if(hA == hC - 1)
			mMaxHeight = hA;
		else if(hA < hC - 1)
			mMaxHeight = hC - 1;
		else if(hA < hB - 1)
			mMaxHeight = hB - 1;
		else
			mMaxHeight = hA;
	}

	protected override void GetMaxLeftHeight()
	{
		int hA = GetMaxGridHeight(Mathf.FloorToInt(mA._SpawnObject.position.x));
		int hB = GetMaxGridHeight(Mathf.FloorToInt(mB._SpawnObject.position.x));
		int hC = GetMaxGridHeight(Mathf.FloorToInt(mC._SpawnObject.position.x));
		
		if(hA > hB && hA > hC)
			mMaxHeight = hA;
		else if(hB > hA && hB > hC)
			mMaxHeight = hB;
		else
			mMaxHeight = hC;
	}
}
