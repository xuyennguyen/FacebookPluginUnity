using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine;

public class MainMenuTest : MonoBehaviour {

	Texture2D snap = null;

	void Start () {

		OnGUI ();
	
	}

	void Update () {
	
	}

	protected void OnGUI(){

		GUILayout.BeginVertical ();
		if (GUILayout.Button ("FBInit", GUILayout.Width(600), GUILayout.Height(100))) {
			SGSFacebook.Instance.Init ();
		}
		
		if (GUILayout.Button ("Login", GUILayout.Width(600), GUILayout.Height(100))) {
			SGSFacebook.Instance.LoginWithReadPermissions ();
			
		}
		if (GUILayout.Button ("LoginWithPermission", GUILayout.Width (600), GUILayout.Height (100))) {
			SGSFacebook.Instance.LoginWithPublishPermissions ();
		}
		if (GUILayout.Button ("postText", GUILayout.Width(600), GUILayout.Height(100))) {
			
			SGSFacebook.Instance.PostText ("hihi");
		}
		if (GUILayout.Button ("postScreeenShot", GUILayout.Width(600), GUILayout.Height(100))) {
			this.StartCoroutine (TakeScreenshot ());
				Debug.Log("screen shot is not null");
			SGSFacebook.Instance.PostImage ("HEHE");
		
		}
		if (GUILayout.Button ("ShareDiaglog", GUILayout.Width(600), GUILayout.Height(100))) {
			
			SGSFacebook.Instance.ShareDialog( SGSFacebook.Instance.UserId,
				new System.Uri("https://developers.facebook.com/"),
				"Test Title",
				"Test caption",
				"Test Description",
				new System.Uri("http://i.imgur.com/zkYlB.jpg"),"");
		}
		GUILayout.EndVertical ();
	}

	private IEnumerator TakeScreenshot()
	{
		yield return new WaitForEndOfFrame();

		var width = Screen.width;
		var height = Screen.height;
		var tex = new Texture2D(width, height, TextureFormat.RGB24, false);

		// Read screen contents into the texture
		tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
		tex.Apply();
		snap = tex;


	}

}
