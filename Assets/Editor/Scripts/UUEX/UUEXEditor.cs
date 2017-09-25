using UnityEngine;
using System.Collections;
using UnityEditor;

namespace UUEX
{
	public class UUEXEditor 
	{
		// Use this for initialization
		void Start () 
		{
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		[MenuItem("UUEX/UI/Create UI")]
		private static void CreateUI()
		{
			string path = "Assets/Plugins/UUEX/Objects/PfUI.prefab";
			GameObject obj = (GameObject)AssetDatabase.LoadAssetAtPath (path, typeof(GameObject));
			GameObject.Instantiate (obj);
		}

		[MenuItem("UUEX/UIMenu/Horizontal Grid Menu")]
		private static void CreateHorizontalGridMenuUI()
		{
			string path = "Assets/Plugins/UUEX/Objects/PfUIHorizontalGridMenu.prefab";
			GameObject obj = (GameObject)AssetDatabase.LoadAssetAtPath (path, typeof(GameObject));
			GameObject.Instantiate (obj);
		}

		[MenuItem("UUEX/UIMenu/Horizontal List Menu")]
		private static void CreateHorizontalListMenuUI()
		{
			string path = "Assets/Plugins/UUEX/Objects/PfUIHorizontalListMenu.prefab";
			GameObject obj = (GameObject)AssetDatabase.LoadAssetAtPath (path, typeof(GameObject));
			GameObject.Instantiate (obj);
		}
		
		[MenuItem("UUEX/UIMenu/Vertical Grid Menu")]
		private static void CreateVerticalGridMenuUI()
		{
			string path = "Assets/Plugins/UUEX/Objects/PfUIVerticalGridMenu.prefab";
			GameObject obj = (GameObject)AssetDatabase.LoadAssetAtPath (path, typeof(GameObject));
			GameObject.Instantiate (obj);
		}
		
		[MenuItem("UUEX/UIMenu/Vertical List Menu")]
		private static void CreateVerticalListMenuUI()
		{
			string path = "Assets/Plugins/UUEX/Objects/PfUIVerticalListMenu.prefab";
			GameObject obj = (GameObject)AssetDatabase.LoadAssetAtPath (path, typeof(GameObject));
			GameObject.Instantiate (obj);
		}

		private static void CreateUIElement(string prefabName, string elementName)
		{
			Transform selectedObject = Selection.activeTransform;
			if(selectedObject == null)
				Debug.LogError("Please select an object in the heirarchy");
			else
			{
				Canvas canvas = selectedObject.GetComponentInChildren<Canvas>();
				if(canvas == null)
					canvas = selectedObject.parent.GetComponent<Canvas>();
				
				if(canvas != null)
				{
					string path = "Assets/Plugins/UUEX/Objects/" + prefabName;
					GameObject obj = (GameObject)AssetDatabase.LoadAssetAtPath (path, typeof(GameObject));
					obj = GameObject.Instantiate (obj);
					obj.transform.SetParent(canvas.transform);
					obj.transform.localPosition = Vector3.zero;
					obj.name = elementName;
				}
				else
				{
					Debug.LogError("Canvas object not found in the heirarchy");
				}
			}
		}

		[MenuItem("UUEX/UI/Button")]
		private static void CreateButton()
		{
			CreateUIElement("PfUIButton.prefab","Button");
		}

		[MenuItem("UUEX/UI/Toggle Button")]
		private static void CreateToggleButton()
		{
			CreateUIElement("PfUIToggleButton.prefab","Toggle");
		}

		[MenuItem("UUEX/UI/Dropdown")]
		private static void CreateDropdown()
		{
			CreateUIElement("PfUIDropdown.prefab","Dropdown");
		}

		[MenuItem("UUEX/UI/Radio Buttons")]
		private static void CreateRadioButtons()
		{
			CreateUIElement("PfUIRadioButtons.prefab","Toggle Group");
		}

		[MenuItem("UUEX/UI/Text")]
		private static void CreateText()
		{
			CreateUIElement("PfUIText.prefab","Text");
		}

		[MenuItem("UUEX/UI/Image")]
		private static void CreateImage()
		{
			CreateUIElement("PfUIImage.prefab","Image");
		}

		[MenuItem("UUEX/UI/Raw Image")]
		private static void CreateRawImage()
		{
			CreateUIElement("PfUIRawImage.prefab","Raw Image");
		}

		[MenuItem("UUEX/UI/Input Field")]
		private static void CreateInputField()
		{
			CreateUIElement("PfUIInput.prefab","Input Field");
		}
	}
}