using UnityEngine;
using System.Collections;

public class TetrisManager : MonoBehaviour 
{
	public static Transform [,] mGrid = new Transform [10,25];
	public static bool mNewBlock = true;
	public GameObject _TilePrefab;
	public float _MaxSpeed = 10;
	public float _SpeedUpTime = 10.0f;
	public float _AnimDuration = 1.0f;
	public UIGameOver _UIGameOver = null;

	public Material [] _TileMaterials;
	
	private float mTime = 0.0f;
	private float mSpeed = 3.0f;
	private Tile [] mTiles = new Tile[6];
	private Tile mTile = null;
	private bool mIsGameOver = false;
	private bool mAnimate = false;
	private float mDelayedPress = 0.0f;

	private Vector3 mStartPosition = new Vector3(5.0f,8.0f,-16.0f);
	private Vector3 mEndPosition = new Vector3(3.0f,8.0f,-16.0f);

	private Vector3 mStartAngle = new Vector3(0.0f,0.0f,0.0f);
	private Vector3 mEndAngle = new Vector3(0.0f,5.0f,0.0f);

	// Use this for initialization
	void Start () 
	{
		mTiles[0] = new TileI();
		mTiles[1] = new TileL();
		mTiles[2] = new TileO();
		mTiles[3] = new TileS();
		mTiles[4] = new TileT();
		mTiles[5] = new TileZ();
		//mTiles[6] = new TileJ(); //Incomplete..

		for(int i=0;i<6;i++)
			mTiles[i]._Material = _TileMaterials[i];

		enabled = false;
	}

	void Update () 
	{
		if(mIsGameOver)
			return;

		if(mAnimate)
		{
			float t = (Time.realtimeSinceStartup - mTime) / _AnimDuration;

			if(t > 1.0f)
			{
				mAnimate = false;
				mDelayedPress = Time.realtimeSinceStartup;
			}
			else
			{
				Camera.main.transform.position = Vector3.Lerp(mStartPosition,mEndPosition,t);
				Quaternion quat = Camera.main.transform.rotation;
				quat.eulerAngles = Vector3.Lerp(mStartAngle, mEndAngle, t);
				Camera.main.transform.rotation = quat;
			}

			return;
		}

		if(mNewBlock)
		{
			for(int i=0;i<10;i++)
			{
				int height = GetMaxGridHeight(i);
				if(height >= 20)
				{
					Debug.Log ("GAME OVER");
					_UIGameOver.SetVisibility(true);
					mIsGameOver = true;
					mNewBlock = false;
					return;
				}
			}

			mNewBlock = false;
			int index = Random.Range(0,6);
			mTile = mTiles[index];
			mTile.Init(_TilePrefab.transform);
		}

		if(mTile != null)
		{
			if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
			{
				mTile.Snap();
			}
			else if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
			{
				mDelayedPress = Time.realtimeSinceStartup + 0.3f;
				mTile.MoveLeft();
			}
			else if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
			{
				mDelayedPress = Time.realtimeSinceStartup + 0.3f;
				mTile.MoveRight();
			}
			else if((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && DelayedPress())
			{
				mTile.MoveLeft();
			}
			else if((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && DelayedPress())
			{
				mTile.MoveRight();
			}
			else if(Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0))
			{
				mTile.Rotate();
			}

			if(!mIsGameOver)
			{
				if(mSpeed < _MaxSpeed)
				{
					float durationElapsed = (Time.realtimeSinceStartup - mTime);
					if(durationElapsed >= _SpeedUpTime)
					{
						mTime = Time.realtimeSinceStartup;
						mSpeed += 1.0f;
					}
				}

				mTile.Update(mSpeed);
			}
		}
	}

	public bool DelayedPress()
	{
		if((Time.realtimeSinceStartup - mDelayedPress) > 0.02f)
		{
			mDelayedPress = Time.realtimeSinceStartup + 0.02f;
			return true;
		}

		return false;
	}

	public void RestartGame()
	{
		mTime = Time.realtimeSinceStartup;
		mAnimate = true;
		mIsGameOver = false;
		mNewBlock = true;
		mSpeed= 3.0f;

		for(int i=0;i<25;i++)
		{
			for(int j=0;j<10;j++)
			{
				if(mGrid[j,i] != null)
				{
					PoolManager.AddObject("Block",mGrid[j,i].gameObject);
					mGrid[j,i] = null;
				}
			}
		}

		enabled = true;
	}

	public void BeginGame()
	{
		mTime = Time.realtimeSinceStartup;
		mAnimate = true;
		enabled = true;
	}

	public static void NextBlock()
	{
		mNewBlock = true;
	}

	public int GetMaxGridHeight(int col)
	{
		int maxHeight = 1;
		
		for(int i=1;i<20;i++)
		{
			if(mGrid[col,i] != null)
			{
				maxHeight = i+1;
			}
		}

		return maxHeight;
	}
}
