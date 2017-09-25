using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace UUEX.UI
{
	public class AtlasInfo
	{
		public Texture2D _Atlas = null;
		public List<Sprite> _Sprites = new List<Sprite> ();

		public Sprite GetSprite(string spriteName)
		{
			foreach(Sprite sprite in _Sprites)
			{
				if(sprite.name == spriteName)
					return sprite;
			}

			return null;
		}
	}

	public class ResourceManager : BaseMonoBehavior
	{
		public static List<AtlasInfo> mAtlases = new List<AtlasInfo>();

		public static void AddAtlas(Texture2D atlas, Sprite sprite)
		{
			AtlasInfo info = mAtlases.Find(a => a._Atlas == atlas);

			if (info == null)
			{
				info = new AtlasInfo ();
				info._Atlas = atlas;
				info._Sprites.Add (sprite);

				mAtlases.Add(info);
			}
			else
			{
				if (!info._Sprites.Contains (sprite))
					info._Sprites.Add (sprite);
			}
		}

		public static Texture2D GetAtlas(string atlasName)
		{
			foreach(AtlasInfo atlas in mAtlases)
			{
				if(atlas._Atlas.name == atlasName)
					return atlas._Atlas;
			}

			return null;
		}

		public static Sprite GetSprite(string atlasName, string spriteName)
		{
			foreach(AtlasInfo atlas in mAtlases)
			{
				if(atlas._Atlas.name == atlasName)
					return atlas.GetSprite(spriteName);
			}
			
			return null;
		}

		public static void DownloadAsset(IWWWAsync receiver, string url, System.Type type)
		{
			WWWAsync wwwAsync = new WWWAsync();
			wwwAsync.DownloadAsset(receiver, url, type);
		}
	}
}
