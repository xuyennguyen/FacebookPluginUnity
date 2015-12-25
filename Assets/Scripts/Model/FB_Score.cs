using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook;
using System;

public class FB_Score {

	public string userId;
	public string userName;
	public string AppId;
	public int value;

	private Dictionary<FB_ProfileImageSize, Texture2D> profileImages = new Dictionary<FB_ProfileImageSize, Texture2D>();
	public event Action<FB_Score> OnProfileImageLoaded = delegate{};

	public string GetProfileUrl(FB_ProfileImageSize size){

		return  "https://graph.facebook.com/" + userId + "/picture?type=" + size.ToString();
	}


	
}
