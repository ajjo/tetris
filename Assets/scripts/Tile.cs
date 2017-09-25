using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Tile 
{
	protected int mDirection = 1;
	protected bool mIsDone = false;
	protected int mMaxHeight = 0;
	protected float mMultiplier = 1.0f;
	protected TileData mA,mB,mC;

	public Material _Material;
	
	public class TileData
	{
		public Transform _SpawnObject;
		public TileData _DependantObject;
		public int _Offset;
	}

	protected List<TileData> mSpawnObjects = new List<TileData>();

	public virtual void Init(Transform tileprefab)
	{
		mIsDone = false;
		mSpawnObjects.Clear();
		mDirection = 1;//Random.Range(1,5);
		
		if(mDirection == 1)
			SpawnUp (tileprefab);
		else if(mDirection == 2)
			SpawnRight (tileprefab);
		else if(mDirection == 3)
			SpawnDown (tileprefab);
		else if(mDirection == 4)
			SpawnLeft(tileprefab);
	}
	
	protected abstract void SpawnUp(Transform tileprefab);
	protected abstract void SpawnRight(Transform tileprefab);
	protected abstract void SpawnDown(Transform tileprefab);
	protected abstract void SpawnLeft(Transform tileprefab);

	protected abstract void GetMaxUpHeight();
	protected abstract void GetMaxRightHeight();
	protected abstract void GetMaxDownHeight();
	protected abstract void GetMaxLeftHeight();

	protected abstract bool RotateUp();
	protected abstract bool RotateRight();
	protected abstract bool RotateDown();
	protected abstract bool RotateLeft();
	
	public virtual void Rotate ()
	{
		if(mIsDone)
			return;

		bool isRotationComplete = false;

		if(mDirection == 1) // Up to right
			isRotationComplete = RotateRight();
		else if(mDirection == 2) // Right to down
			isRotationComplete = RotateDown();
		else if(mDirection == 3) // Down to left
			isRotationComplete = RotateLeft();
		else if(mDirection == 4) // Left to up
			isRotationComplete = RotateUp();

		if(isRotationComplete)
		{
			mDirection++;
			
			if(mDirection == 5)
				mDirection = 1;
		}
	}


	public virtual void MoveLeft()
	{
		if(mIsDone)
			return;

		foreach(TileData td in mSpawnObjects)
		{
			if(td._SpawnObject.position.x - 1 < 0)
				return;
		}

		foreach(TileData td in mSpawnObjects)
		{
			Vector3 pos = td._SpawnObject.position;
			pos.x -= 1;
			td._SpawnObject.position = pos;
		}

		if(mDirection == 1)
			GetMaxUpHeight();
		else if(mDirection == 3)
			GetMaxDownHeight();
		else if(mDirection == 2)
			GetMaxRightHeight();
		else if(mDirection == 4)
			GetMaxLeftHeight();
	}

	public virtual void MoveRight()
	{
		if(mIsDone)
			return;

		foreach(TileData td in mSpawnObjects)
		{
			if(td._SpawnObject.position.x + 1 > 9)
				return;
		}

		foreach(TileData td in mSpawnObjects)
		{
			Vector3 pos = td._SpawnObject.position;
			pos.x += 1;
			td._SpawnObject.position = pos;
		}

		if(mDirection == 1)
			GetMaxUpHeight();
		else if(mDirection == 3)
			GetMaxDownHeight();
		else if(mDirection == 2)
			GetMaxRightHeight();
		else if(mDirection == 4)
			GetMaxLeftHeight();
	}
	
	public void Snap()
	{
		mMultiplier = 40;
	}

	public void Update(float speed)
	{
		if(mIsDone)
			return;
		
		foreach(TileData td in mSpawnObjects)
		{
			if(td._DependantObject == null)
			{
				Vector3 pos = td._SpawnObject.position;

				if(mMultiplier > 1.0f)
					pos.y -= mMultiplier * Time.deltaTime;
				else
					pos.y -= (speed * Time.deltaTime);
				
				if(pos.y < mMaxHeight)
				{
					pos.y = mMaxHeight;
					mIsDone = true;
				}
				
				td._SpawnObject.position = pos;
			}
			else
			{
				Vector3 depPos = td._DependantObject._SpawnObject.position;
				depPos.x = td._SpawnObject.position.x;
				depPos.y += td._Offset;
				td._SpawnObject.position = depPos;
			}
		}
		
		if(mIsDone)
		{
			mMultiplier = 1.0f;

			foreach(TileData td in mSpawnObjects)
			{
				Transform t = td._SpawnObject.transform;
				int xIndex = Mathf.FloorToInt(t.position.x);
				int yIndex = Mathf.FloorToInt(t.position.y);
				TetrisManager.mGrid[xIndex,yIndex] = t;
				t.name = xIndex+":"+yIndex;
			}
			
			BreakFree();
			TetrisManager.NextBlock();
		}
	}
	
	public void BreakFree()
	{
		// Break - move down - break.. the move down should know what to break next.. so the iteration repeats..
		for(int i=19;i>0;i--)
		{
			int setCount = 0;
			int j;
			
			for(j=0;j<10;j++)
			{
				if(TetrisManager.mGrid[j,i] != null)
					setCount++;
			}
			
			if(setCount == 10)
			{
				for(j=0;j<10;j++)
				{
					// GameObject.Destroy(TetrisManager.mGrid[j,i].gameObject);
					PoolManager.AddObject("Block",TetrisManager.mGrid[j,i].gameObject);
					TetrisManager.mGrid[j,i] = null;
				}
				
				MoveDown(i);
			}
		}
	}
	
	private void MoveDown(int startRow)
	{
		for(int i=startRow+1;i<20;i++)
		{
			for(int j=0;j<10;j++)
			{
				Transform t = TetrisManager.mGrid[j,i];
				
				if(t != null)
				{
					Vector3 pos = t.position;
					pos.y -= 1;
					t.position = pos;
					TetrisManager.mGrid[j,i-1] = t;
					TetrisManager.mGrid[j,i] = null;
				}
			}
		}
	}
	
	public virtual TileData AddBlock(Transform tileprefab, Vector3 pos, int offset, TileData dependantObject = null)
	{
		GameObject obj = PoolManager.GetObject("Block");//(GameObject)GameObject.Instantiate(tileprefab.gameObject);
		obj.transform.parent = null;
		obj.transform.gameObject.SetActive(true);
		obj.transform.position = pos;

		TileData td = new TileData();
		td._SpawnObject = obj.transform;
		td._DependantObject = dependantObject;
		td._Offset = offset;
		
		mSpawnObjects.Add(td);

		Renderer renderer = obj.GetComponent<Renderer>();
		renderer.material = _Material;
		
		return td;
	}
	
	public TileData UpdateBlock(TileData spawnObject, float x, int offset, TileData dependantObject)
	{
		Vector3 nextData = spawnObject._SpawnObject.position;
		nextData.x = x;
		spawnObject._SpawnObject.position = nextData;
		
		spawnObject._DependantObject = dependantObject;
		spawnObject._Offset = offset;
		
		return spawnObject;
	}
	
	public int GetMaxGridHeight(int col)
	{
		int maxHeight = 1;
		
		for(int i=1;i<20;i++)
		{
			if(TetrisManager.mGrid[col,i] != null)
			{
				maxHeight = i+1;
			}
		}
		
		//Debug.Log ("Getting height = " + col + " : " + maxHeight);
		return maxHeight;
	}
}
