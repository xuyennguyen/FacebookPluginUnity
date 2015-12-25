using UnityEngine;
using System.Collections;

public class FB_Result {

	private string rawData = "";
	private string error = "";
	protected bool isSucceeded = false;

	public FB_Result(string _rawData, string _error){
		if ( string.IsNullOrEmpty (_error) ) {

			if (!string.IsNullOrEmpty (_rawData)) {

				isSucceeded = true;
			}
		}
		this.rawData = _rawData;
		this.error = _error;
	}

	public bool IsSucceeded{
		get{
			return isSucceeded;
		}
	}

	public bool IsFailed{

		get{
			return !isSucceeded;
		}
	}

	public string RawData{

		get{
			return rawData;
		}
	}


	public string Error{

		get{
			return error;
		}
	}


}
