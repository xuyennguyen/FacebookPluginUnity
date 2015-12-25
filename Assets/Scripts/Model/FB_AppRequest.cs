using UnityEngine;
using System.Collections;
using System;
using Facebook.Unity;

public class FB_AppRequest {

	public string id;
	public string applicationId;
	public string message = "";

	public FB_AppRequestState appRequestState = FB_AppRequestState.Pending;

	public string fromId;
	public string fromName;
	public DateTime createdTime;
	public string createdTimeString;

	public FB_Object fbObject;
	public  Action<FB_Result> OnDeleteRequestFinished = delegate {
	};

	public void SetCreatedTime(string time_string){

		createdTimeString = time_string;
		createdTime = DateTime.Parse (time_string);

	}

	public void Delete(){
	}





}
