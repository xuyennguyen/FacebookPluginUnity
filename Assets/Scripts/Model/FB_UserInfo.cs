using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Facebook.MiniJSON;



public class FB_UserInfo {

	private string id = "";
	private string name = "";
	private string firstName = "";
	private string lastName = "";
	private string userName = "";
	private string profileUrl = "";
	private string email = "";
	private string location = "";
	private string locale = "";
	private string rawJson = "";
	private DateTime birthday = new DateTime();
	private FB_Gender gender = FB_Gender.Male;
	private Dictionary<FB_ProfileImageSize, Texture2D> profileImages = new Dictionary<FB_ProfileImageSize, Texture2D>();
	public event Action <FB_UserInfo> OnProfileImageLoaded = delegate {};

	public FB_UserInfo(string data){
		
		rawJson = data;
		IDictionary json = Facebook.MiniJSON.Json.Deserialize (rawJson) as IDictionary;
		InitializeData (json);
	

	}

	public FB_UserInfo( IDictionary json){

		InitializeData (json);

	}

	public void InitializeData(IDictionary json){

		if (json.Contains ("id")) {

			id = System.Convert.ToString (json ["id"]);
		}

		if (json.Contains ("birthday")) {

			birthday = DateTime.Parse(System.Convert.ToString(json["birthday"]));
		}

		if (json.Contains("name")){

			name = System.Convert.ToString (json ["name"]);
		}

		if (json.Contains("first_name")){
			
			firstName = System.Convert.ToString (json ["first_name"]);

		}

		if (json.Contains ("last_name")) {

			lastName = System.Convert.ToString (json ["last_name"]);
		}

		if (json.Contains ("username")) {

			userName = System.Convert.ToString (json ["username"]);
		}

		if (json.Contains ("link")) {

			profileUrl = System.Convert.ToString (json ["link"]);
		}

		if (json.Contains ("email")) {

			email = System.Convert.ToString (json ["email"]);
		}

		if (json.Contains ("locale")) {

			locale = System.Convert.ToString (json ["locale"]);
		}

		if (json.Contains ("location")) {
		
			IDictionary loc = json ["location"] as IDictionary;
			location = System.Convert.ToString(loc["name"]);
		}

		if (json.Contains("gender")){

			string _gender = System.Convert.ToString(json["gender"]);
			if (_gender.Equals ("male")) {

				gender = FB_Gender.Male;
			} else {

				gender = FB_Gender.Female;
			}
		}


	}

	public string GetProfileUrl(FB_ProfileImageSize size){

		return "https://graph.facebook.com/" + id + "/picture?type=" + size.ToString();
	}

	public Texture2D GetProfileImage(FB_ProfileImageSize size){

		if (profileImages.ContainsKey (size)) {
			return profileImages [size];
		} else
			return null;
	
	}

	public void LoadProfileImage(FB_ProfileImageSize size){

		if (GetProfileImage (size) != null) {

			OnProfileImageLoaded (this);
			Debug.Log ("Profile image already loaded, size: " + size);

		}
			
	}

	public string RawJSON {
		get{
			return rawJson;
		}
	}

	public string Id{
	
		get{
			return Id;
		}
	}

	public DateTime Birthday{

		get{

			return birthday;
		}
	}

	public string Name{
	
		get{
			return name;
		}
	}

	public string FirstName{
	
		get{
			return firstName;
		}
	}

	public string LastName{
	
		get{
			return lastName;
		}
	}

	public string UserName{
	
		get{
			return userName;
		}
	}

	public string ProfileUrl{

		get{
			return profileUrl;
		}
	}

	public string Locale{
		get{

			return locale;
		}
	}

	public string Location{

		get{
			return location;
		}
	}
	public FB_Gender Gender{

		get{
			return gender;
		}
	}
}


