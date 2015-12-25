using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FB_Object{

	public string id;
	public List<string> imageUrls = new List<string>();
	public string title;
	public string type;
	public DateTime createdTime;
	public string createdTimeString;

	public void SetCreatedTime(string time_string){

		createdTimeString = time_string;
		createdTime = DateTime.Parse (time_string);
	}

	public void AddImageUrl(string url){

		imageUrls.Add (url);
	}

}
