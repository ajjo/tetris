using UnityEngine;
using System.Collections;

public class TileS : Tile 
{
	protected override void SpawnUp(Transform tileprefab)
	{
		TileData dependantObject = 	AddBlock(tileprefab, new Vector3(5.0f,22,0), 0);
		
		mA = AddBlock(tileprefab, new Vector3(5.0f,22,0), 1, dependantObject);
		mB = AddBlock(tileprefab, new Vector3(4.0f,22,0), 0, dependantObject);
		mC = AddBlock(tileprefab, new Vector3(6.0f,22,0), 1, dependantObject);
		
		GetMaxUpHeight();
	}
	
	protected override void SpawnDown(Transform tileprefab)
	{
		SpawnUp (tileprefab);
	}
	
	protected override void SpawnRight(Transform tileprefab)
	{
		TileData dependantObject = 	AddBlock(tileprefab, new Vector3(5.0f,22,0), 0);
		
		mA = AddBlock(tileprefab, new Vector3(5.0f,22,0), 1, dependantObject);
		mB = AddBlock(tileprefab, new Vector3(4.0f,22,0), 1, dependantObject);
		AddBlock(tileprefab, new Vector3(4.0f,22,0), 2, dependantObject);
		
		GetMaxRightHeight();
	}
	
	protected override void SpawnLeft(Transform tileprefab)
	{
		SpawnRight (tileprefab);
	}

	protected override bool RotateRight()
	{
		TileData dependantObject = mSpawnObjects[0];
		Vector3 dependantPos = dependantObject._SpawnObject.position;
		
		mA = UpdateBlock(mSpawnObjects[1], dependantPos.x,  1, dependantObject);
		mB = UpdateBlock(mSpawnObjects[2], dependantPos.x - 1, 1, dependantObject);
		UpdateBlock(mSpawnObjects[3], dependantPos.x - 1, 2, dependantObject);
		
		GetMaxRightHeight();
		return true;
	}

	protected override bool RotateDown()
	{
		TileData dependantObject = mSpawnObjects[0];
		Vector3 dependantPos = dependantObject._SpawnObject.position;
		
		if(dependantPos.x == 0)
			return false;
		
		mB = UpdateBlock(mSpawnObjects[1], dependantPos.x - 1, 0, dependantObject);
		mA = UpdateBlock(mSpawnObjects[2], dependantPos.x, 1, dependantObject);
		mC = UpdateBlock(mSpawnObjects[3], dependantPos.x + 1, 1, dependantObject);
		
		GetMaxDownHeight();
		return true;
	}

	protected override bool RotateLeft()
	{
		TileData dependantObject = mSpawnObjects[0];
		Vector3 dependantPos = dependantObject._SpawnObject.position;
		
		mA = UpdateBlock(mSpawnObjects[1], dependantPos.x, 1, dependantObject);
		mB = UpdateBlock(mSpawnObjects[2], dependantPos.x - 1, 1, dependantObject);
		UpdateBlock(mSpawnObjects[3], dependantPos.x - 1, 2, dependantObject);
		
		GetMaxLeftHeight();
		return true;
	}

	protected override bool RotateUp()
	{
		TileData dependantObject = mSpawnObjects[0];
		Vector3 dependantPos = dependantObject._SpawnObject.position;
		
		if(dependantPos.x == 0)
			return false;
		
		mB = UpdateBlock(mSpawnObjects[1], dependantPos.x - 1, 0, dependantObject);
		mA = UpdateBlock(mSpawnObjects[2], dependantPos.x, 1, dependantObject);
		mC = UpdateBlock(mSpawnObjects[3], dependantPos.x + 1, 1, dependantObject);
		
		GetMaxDownHeight();
		return true;
	}

	protected override void GetMaxUpHeight()
	{
		mMaxHeight = 0;
		
		int hA = GetMaxGridHeight(Mathf.FloorToInt(mA._SpawnObject.position.x));
		int hB = GetMaxGridHeight(Mathf.FloorToInt(mB._SpawnObject.position.x));
		int hC = GetMaxGridHeight(Mathf.FloorToInt(mC._SpawnObject.position.x));

		if(hC == hA + 1)
			mMaxHeight = hA;
		else if(hA < hC - 1)
			mMaxHeight = hC - 1;
		else if(hA < hB - 1)
			mMaxHeight = hB - 1;
		else if(hB > hA)
			mMaxHeight = hB;
		else
			mMaxHeight = hA;
	}
	
	protected override void GetMaxDownHeight()
	{
		GetMaxUpHeight();
	}
	
	protected override void GetMaxRightHeight()
	{
		int hA = GetMaxGridHeight(Mathf.FloorToInt(mA._SpawnObject.position.x));
		int hB = GetMaxGridHeight(Mathf.FloorToInt(mB._SpawnObject.position.x));

		if(hA == hB - 1)
			mMaxHeight =  hA;
		else if(hA < hB - 1)
			mMaxHeight =  hB - 1;
		else if(hB == hA - 1)
			mMaxHeight = hA;
		else if(hB < hA - 1)
			mMaxHeight = hA;
		else
			mMaxHeight = hB;
	}

	protected override void GetMaxLeftHeight()
	{
		GetMaxRightHeight();
	}
}
