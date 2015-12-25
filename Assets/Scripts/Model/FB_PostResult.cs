using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Facebook.MiniJSON;
using Facebook.Unity;

public class FB_PostResult : FB_Result {

	private string postId = "";

	public FB_PostResult(string rawData, string error):base(rawData, error){

		if (isSucceeded) {
			try{
				Dictionary<string, object> data = Facebook.MiniJSON.Json.Deserialize (rawData) as Dictionary<string, object>;
				postId = System.Convert.ToString(data["id"]);
					isSucceeded = true;
			} catch(System.Exception ex){
				isSucceeded = false;

			}
		}
	}

	public string PostId{

		get{
			return postId;
		}
						
	}



}
