using UnityEngine;
using System.Collections;

public class TileI : Tile 
{
	// Spawn blocks in the specific direction
	protected override void SpawnUp(Transform tileprefab)
	{
		TileData dependantObject = 	AddBlock(tileprefab, new Vector3(5.0f,22,0), 0);
		
		AddBlock(tileprefab, new Vector3(5.0f,22,0), 1, dependantObject);
		AddBlock(tileprefab, new Vector3(5.0f,22,0), 2, dependantObject);
		AddBlock(tileprefab, new Vector3(5.0f,22,0), 3, dependantObject);
		
		GetMaxUpHeight();
	}
	
	protected override void SpawnRight(Transform tileprefab)
	{
		TileData dependantObject = AddBlock(tileprefab, new Vector3(4.0f,22.0f), 0);
		
		AddBlock(tileprefab, new Vector3(5.0f,22,0), 0, dependantObject);
		AddBlock(tileprefab, new Vector3(6.0f,22,0), 0, dependantObject);
		AddBlock(tileprefab, new Vector3(7.0f,22,0), 0, dependantObject);
		
		GetMaxRightHeight();
	}
	
	protected override void SpawnDown(Transform tileprefab)
	{
		SpawnUp (tileprefab);
	}
	
	protected override void SpawnLeft(Transform tileprefab)
	{
		SpawnRight (tileprefab);
	}

	protected override bool RotateRight()
	{
		foreach(TileData td in mSpawnObjects)
		{
			if(td._DependantObject == null &&  td._SpawnObject.position.x < 1 || td._SpawnObject.position.x > 7)
				return false;
		}

		TileData dependantObject = mSpawnObjects[0];
		Vector3 dependantPos = dependantObject._SpawnObject.position;
		
		UpdateBlock(mSpawnObjects[1], dependantPos.x - 1, 0, dependantObject);
		UpdateBlock(mSpawnObjects[2], dependantPos.x + 1, 0, dependantObject);
		UpdateBlock(mSpawnObjects[3], dependantPos.x + 2, 0, dependantObject);
		
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
		UpdateBlock(mSpawnObjects[2], dependantPos.x, 2, dependantObject);
		UpdateBlock(mSpawnObjects[3], dependantPos.x, 3, dependantObject);
		
		GetMaxDownHeight();
		return true;
	}

	protected override bool RotateLeft()
	{
		return RotateRight();
	}

	protected override bool RotateUp()
	{
		return RotateDown();
	}
	
	protected override void GetMaxUpHeight()
	{
		mMaxHeight = 0;
		
		foreach(TileData td in mSpawnObjects)
		{
			//if(td._DependantObject != null)
			{
				Vector3 pos = td._SpawnObject.position;
				int height = GetMaxGridHeight(Mathf.FloorToInt(pos.x));
				if(height > mMaxHeight)
					mMaxHeight = height;
			}
		}
	}
	
	protected override void  GetMaxDownHeight()
	{
		GetMaxUpHeight();
	}

	protected override void GetMaxRightHeight()
	{
		GetMaxUpHeight();
	}

	protected override void GetMaxLeftHeight()
	{
		GetMaxUpHeight();
	}
}

