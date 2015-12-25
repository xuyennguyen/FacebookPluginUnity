using UnityEngine;
using System.Collections;

public class FB_LoginResult: FB_Result {

	private bool isCanceled;
	private string userId;
	private string accessToken;

	public FB_LoginResult(string _rawData, string _error, bool _isCanceled):base(_rawData, _error){
		isCanceled = _isCanceled;
	}

	public void SetCredential( string _userId, string _accessToken){

		userId = _userId;
		accessToken = _accessToken;
	}

	public string UserId{

		get{
			return userId;
		}
	}

	public string AccessToken{

		get{
			return accessToken;
		}
	}

	public bool IsCanceled{
		get{
			return isCanceled;
		}
		
	}



}
