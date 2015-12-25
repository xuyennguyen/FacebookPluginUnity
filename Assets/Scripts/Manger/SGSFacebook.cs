using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Facebook.Unity;
using Facebook.MiniJSON;

public class SGSFacebook: MonoBehaviour {

	private static SGSFacebook _instance;

	static public SGSFacebook Instance{

		get{
			if (_instance == null) {
				_instance = new SGSFacebook ();
			}
			return _instance;
		}
	}

	public delegate void FB_Delegate(FB_Result result);
	private FB_UserInfo userInfor = null;
	private Dictionary<string, FB_UserInfo> friends = new Dictionary<string, FB_UserInfo>();
	private Dictionary<string, FB_UserInfo>invitableFriends = new Dictionary<string, FB_UserInfo>();
	private bool isInited = false;

	private Dictionary<string, FB_Score> userScores = new Dictionary<string, FB_Score>();
	private Dictionary<string, FB_Score> appScores = new Dictionary<string, FB_Score>();
	private int lastSubmitedScore = 0;

	private Dictionary<string, Dictionary<string, FB_LikeInfo>> _likes = new Dictionary< string, Dictionary<string, FB_LikeInfo>>();
	private List<FB_AppRequest> _appRequests = new List<FB_AppRequest>();

	public delegate void LoginStarted();
	public static event LoginStarted OnLoginStarted;

	public delegate void LoginFinished();
	public static event LoginFinished OnLoginFinished;

	public delegate void LogoutStarted();
	public static event LogoutStarted OnLogoutStarted;

	public delegate void LogoutFinished();
	public static event LogoutFinished OnLogoutFinished;

	public delegate void FeedShareStarted();
	public static event FeedShareStarted OnFeedShareStared;

	public delegate void FeedShareFinished();
	public static event FeedShareFinished OnFeedShareFinished;

	public delegate void PostStarted();
	public static event PostStarted OnPostStarted;

	public delegate void PostFinished();
	public static event PostFinished OnPostFinished;

	public delegate void SubmitScoreStarted ();
	public static event SubmitScoreStarted OnSubmitScoreStarted;

	public delegate void SubmitScoreFinished();
	public static event SubmitScoreFinished OnSubmitScoreFinished;

	public delegate void DeleteScoreStarted ();
	public static event DeleteScoreStarted OnDeletedScoreStarted;

	public delegate void DeleteScoreFinished();
	public static event DeleteScoreFinished OnDeletedScoreFinished;

	public delegate void LoadUserDataStarted();
	public static event LoadUserDataStarted OnLoadUserDataStarted;

	public delegate void LoadUserDataFinished ();
	public static event LoadUserDataFinished OnLoadUserDataFinished;

	public delegate void AppScoresRequestStarted ();
	public static event AppScoresRequestStarted OnAppScoresRequestStarted;

	public delegate void AppScoresRequestFinished ();
	public static event AppScoresRequestFinished OnAppScoresRequestFinished;

	public delegate void PlayerScoresRequestStarted ();
	public static event PlayerScoresRequestStarted OnPlayerScoresRequestStarted;

	public delegate void PlayerScoreRequestFinished ();
	public static event PlayerScoreRequestFinished OnPlayerScoresRequestFinished;




	private string userId = "";
	private string accessToken = "";


	void Awake(){
		DontDestroyOnLoad (gameObject);
	}

	void Start(){
	
		_instance = this;
	}


	// Initialized faceviij
	public void Init(){
	
		FB.Init (OnInitComplete, OnHideUnity);
	}

	private void OnInitComplete(){
		isInited = true;
		IsLoginRequestInProgress = false;
	


	}

	private void OnHideUnity(bool isGameShown){
		
	}

	public bool IsLoginRequestInProgress = false;

	public void LoginWithReadPermissions(){
		if (OnLoginStarted != null) {
			OnLoginStarted ();
		}

		if (!IsLoginRequestInProgress && FB.IsInitialized && !FB.IsLoggedIn) {
			IsLoginRequestInProgress = true;
			FB.LogInWithReadPermissions (new List<string> (){ "public_profile", "email", "user_friends" }, LoginCallBack);
		}


	}

	public void LoginWithPublishPermissions(){
		
		 FB.LogInWithPublishPermissions(new List<string>() { "publish_actions" }, LoginCallBack);
	}

	public void LoginCallBack(ILoginResult result){

		if (OnLoginFinished != null) {
			OnLoginFinished ();
		}
		if (result == null) {

			Debug.Log ("Login: Null response");
		} 
		else {
			if (!result.Cancelled && (result.Error == null) && result.AccessToken != null) {
				userId = result.AccessToken.UserId;
				accessToken = result.AccessToken.TokenString;
				Debug.Log ("Login successed: userId = " + userId + "accessToken: " + accessToken);
			}
		}

	}

	public void Logout(){

		if(OnLogoutFinished != null)
			OnLogoutFinished ();
		
		FB.LogOut ();
		IsLoginRequestInProgress = false;
		userId = "";
		accessToken = "";
	}


	public void LoadUserData(){
	
		if (OnLoadUserDataStarted != null) {
			OnLoadUserDataStarted ();
		}
		if (FB.IsLoggedIn) {

			FB.API ("/me?fields=id,birthday,name,first_name,last_name,link,email,locale,location,gender", HttpMethod.GET, UserDataCallBack);  
		} else {
			Debug.Log ("User dont login");

		}
	}

	public void UserDataCallBack(IResult iResult){

		if (OnLoadUserDataFinished != null) {

			OnLoadUserDataFinished ();
		}
		FB_Result result = new FB_Result (iResult.RawResult, iResult.Error);
		if (result.IsFailed) {
			
			Debug.Log (result.Error);
		}
		else {
			Debug.Log ("UserDataCallback result.rawData: " + result.RawData);
			userInfor = new FB_UserInfo(result.RawData);
		}

	}

	public void PostImage(string caption, Texture2D image){

		OnPostStarted ();
		byte[] imageBytes = image.EncodeToPNG ();
		WWWForm wwwForm = new WWWForm ();
		wwwForm.AddField ("messages", caption);
		wwwForm.AddBinaryData ("image", imageBytes, "screenshoot.png");
		wwwForm.AddField ("name", caption);
		FB.API ("me/photos", HttpMethod.POST, PostCallback, wwwForm);
	}

	public void PostImage(string caption){

		if (OnPostStarted != null) {
			OnPostStarted();
		}
		Texture2D image = new Texture2D (Screen.width/2, Screen.height/2, TextureFormat.RGB24, false);
		image.ReadPixels (new Rect (0, 0, Screen.width/2, Screen.height/2), 0, 0);
		image.Apply ();
		byte[] imageBytes = image.EncodeToPNG ();
		WWWForm wwwForm = new WWWForm ();
		wwwForm.AddField ("message", caption);
		wwwForm.AddBinaryData ("image", imageBytes, "picture.png");
		wwwForm.AddField ("name", caption);
		FB.API ("me/photos", HttpMethod.POST, PostCallback, wwwForm);


	}

	public void PostCallback(IGraphResult result){
		if (OnPostFinished != null) {
			OnPostFinished ();
		}
		Debug.Log("PostCallback:" + result.ToString());
	
	}

	public void PostText(string message){
		if (OnPostStarted != null) {
			OnPostStarted ();
		}

		WWWForm wwwForm = new WWWForm ();
		wwwForm.AddField ("message", message);
		FB.API ("me/feed", HttpMethod.POST, PostCallback, wwwForm);

	}

	public void ShareDialog(string toId="", System.Uri link= null, string linkName= "", string linkCaption ="", string linkDescription ="", System.Uri picture= null, string actionName =""){

		if(OnFeedShareStared != null)
			OnFeedShareStared ();
		
		FB.FeedShare (toId, link, linkName, linkCaption, linkDescription, picture, actionName, ShareDialogCallback);

	}

	public void ShareDialogCallback(IShareResult result){

		if (OnFeedShareFinished != null) {
			OnFeedShareFinished ();
			Debug.Log ("ShareDialogCallback:" + result.PostId);
		}

	}


	// load score

	public void LoadPlayerScores(){

		if (OnPlayerScoresRequestStarted != null) {

			OnPlayerScoresRequestStarted ();
		}
		FB.API ("/" + UserId + "/scores", HttpMethod.GET, OnLoadPlayerScoresComplete);


	}

	public void OnLoadPlayerScoresComplete(IResult iResult){

		if (OnPlayerScoresRequestFinished != null) {
		
			OnPlayerScoresRequestFinished ();
		}
		FB_Result result  = new FB_Result(iResult.RawResult, iResult.Error);
		if (result.IsFailed != null) {
			return;
		}

		Dictionary<string, object> json = Facebook.MiniJSON.Json.Deserialize (result.RawData) as Dictionary<string, object>;
		Debug.Log ("OnLoadPlayerScoresCompleted: " + result.ToString());
		List<object> data = json ["data"] as List<object>;
	
		foreach (object row in data) {

			FB_Score score = new FB_Score ();
			Dictionary<string, object> dataRow = row as Dictionary<string, object>;
			Dictionary<string, object> _userInfo = dataRow ["user"] as Dictionary<string, object>;
			score.userId = System.Convert.ToString (_userInfo ["id"]);
			score.userName = System.Convert.ToString (_userInfo ["name"]);
			score.value = Int32.Parse(System.Convert.ToString (_userInfo ["score"]));

			Dictionary<string, object> _appInfo = dataRow ["application"] as Dictionary<string, object>;
			score.AppId = System.Convert.ToString (_appInfo ["id"]);

			AddToUserScores (score);



		}

	}

	private void AddToUserScores(FB_Score score){

		if (userScores.ContainsKey (score.AppId)) {
			userScores [score.AppId] = score;
		} else {
			userScores.Add (score.AppId, score);
		}

		if (appScores.ContainsKey (score.AppId)) {
			appScores [score.userId] = score;
		} else {

			appScores.Add (score.userId, score);
		}
	}

	public void LoadAppScores(){

		if (OnAppScoresRequestStarted != null) {
			OnAppScoresRequestStarted ();
		}
		FB.API ("/" + FB.AppId + "/scores", HttpMethod.GET, OnAppScoreComplete);
	}

	public void OnAppScoreComplete(IResult iResult){

		if (OnAppScoresRequestFinished != null) {

			OnAppScoresRequestFinished ();
		}
		FB_Result result  = new FB_Result(iResult.RawResult, iResult.Error);

		if (result.IsFailed) {
			return;
		}


		Dictionary<string, object> jsonData = Facebook.MiniJSON.Json.Deserialize (result.RawData) as Dictionary<string, object>;
		List<object> data = jsonData ["data"] as List<object>;

		foreach (object row in data) {

			FB_Score score = new FB_Score ();

			Dictionary<string, object> dataRow = row as Dictionary<string, object>;

			if (dataRow.ContainsKey ("user")) {
				
				Dictionary<string, object> _userInfo = dataRow ["user"] as Dictionary<string, object>;

				if (_userInfo.ContainsKey ("id")) {
					
					score.userId = System.Convert.ToString (_userInfo ["id"]);
				}
				if (_userInfo.ContainsKey ("name")) {

					score.userName = System.Convert.ToString (_userInfo ["name"]);
				
				}
			}

			if (dataRow.ContainsKey ("score")) {

				score.value = System.Convert.ToInt32 (dataRow ["score"]);
			}

			if (dataRow.ContainsKey ("application")) {

				Dictionary<string, object> _appInfo = dataRow ["application"] as Dictionary<string, object>;

				if (_appInfo.ContainsKey ("id")) {

					score.AppId = System.Convert.ToString (_appInfo ["id"]);
				}
				if (_appInfo.ContainsKey ("name")) {


				}

			}

			AddToAppScores (score);
		}


	
	}


	public void AddToAppScores(FB_Score score){
		
			if(appScores.ContainsKey(score.userId)) {
				appScores[score.userId] = score;
			} else {
				appScores.Add(score.userId, score);
			}

			if(userScores.ContainsKey(score.AppId)) {
				userScores[score.AppId] = score;
			} else {
				userScores.Add(score.AppId, score);
			}
	}

	public void SubmitScores(int score){

		if (OnSubmitScoreStarted != null) {

			OnSubmitScoreStarted ();
		}
		lastSubmitedScore = score;
		FB.API("/" + UserId + "/scores?score=" + score, HttpMethod.POST, OnScoreSubmited); 
		
	}

	//Delete scores for a player
	public void DeletePlayerScores() {

		if (OnDeletedScoreStarted != null) {

			OnDeletedScoreFinished ();
		}
		FB.API("/" + UserId + "/scores", HttpMethod.DELETE, OnScoreDeleted); 


	}

	private void OnScoreSubmited(IResult iResult){
	
		if (OnSubmitScoreFinished != null) {

			OnSubmitScoreFinished ();
		}
		FB_Result result  = new FB_Result(iResult.RawResult, iResult.Error);
		if (result.IsFailed) {
			
			//OnSubmitScoreRequestCompleteAction (result);
			return;
		}

		if (result.RawData.Equals ("true")) {
		
			FB_Score score = new FB_Score ();
			score.AppId = AppId;
			score.userId = UserId;
			score.value = lastSubmitedScore;

			if (appScores.ContainsKey (UserId)) {

				appScores [UserId] = score;
			} else {
				appScores.Add (score.userId, score);
			}

			if (userScores.ContainsKey (AppId)) {

				userScores [AppId].value = lastSubmitedScore;

			} else {
				userScores.Add (AppId, score);
			}
		}

		//OnSubmitScoreRequestCompleteAction (result);
	}

	private void OnScoreDeleted( IResult iResult){
	
		if (OnDeletedScoreFinished != null) {

			OnDeletedScoreFinished ();
		}
		FB_Result result  = new FB_Result(iResult.RawResult, iResult.Error);
		if(result.IsFailed) {
			//OnDeleteScoresRequestCompleteAction(result);
			return;
		}


		if(result.RawData.Equals("true")) {

			FB_Score score = new FB_Score();
			score.AppId = AppId;
			score.userId = UserId;
			score.value = 0;

			if(appScores.ContainsKey(UserId)) {
				appScores[UserId].value = 0;
			}  else {
				appScores.Add(score.userId, score);
			}


			if(userScores.ContainsKey(AppId)) {
				userScores[AppId].value = 0;
			} else {
				userScores.Add(AppId, score); 
			}

		} 

	//	OnDeleteScoresRequestCompleteAction(result);


	}


	// Get, set method

	public bool IsInited {

		get{
			return isInited;
		}
	}

	public bool IsLoggedIn{

		get{
			return FB.IsLoggedIn;
		}
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

	public string AppId{

		get{
			return FB.AppId;
		}
	}

	public FB_UserInfo UserInfo{

		get{
			return userInfor;
		}
	}

	public Dictionary<string, FB_Score> UserScores{

		get{
			return userScores;
		}
	}

	public Dictionary<string, FB_Score> AppScores{
	
		get{
			return appScores;
		}
	}
}
