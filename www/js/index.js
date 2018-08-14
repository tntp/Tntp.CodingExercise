const NODE_SERVER = "http://localhost:8321";
const CONTENT_TYPE = "Content-Type";
const CONTENT_TYPE_JSON = "application/json;charset=UTF-8";
const ONE_DAY_MSEC		= 86400*1000;

var myToken = "";
var myUsername = "";
var myPwd = "";
var myDisplayName;
var myCharElem = null;

var getting = false;
var latestCommentTime = null;

var commentRefreshTask = null;

var actionNames = {
	signOut: "SignOut",
	signIn: "SignIn",
	createUser: "CreateUser",
	getComments: "GetComments",
	addComment: "AddComment",
	randomComment: "RandomComment"
};

var statuses = {
	notSignedIn: "User is signed out.",
	userCreateFailed: "Failed to create a new user."
}

function DialogDisplay()
{
	var signInDialog = document.getElementById("dialogScn");
	var appContainer = document.getElementById("container");
	signInDialog.style.zIndex = 2;
	signInDialog.style.opacity = 1;
	appContainer.style.opacity = 0.5; 
}

function AboutDialogDisplay()
{
	DialogDisplay();
	var aboutDialog = document.getElementById("aboutDialogContent");
	aboutDialog.style.zIndex = 2;
	aboutDialog.style.opacity = 1.0;
}

function CreateUserDialogDisplay()
{
	DialogDisplay();
	var aboutDialog = document.getElementById("createUserDialogContent");
	aboutDialog.style.zIndex = 2;
	aboutDialog.style.opacity = 1.0;
}

function ErrDialogDisplay()
{
	DialogDisplay();
	var errDialog = document.getElementById("errDialogContent");
	errDialog.style.zIndex = 2;
	errDialog.style.opacity = 1.0;
}

function SigninDialogDisplay()
{
	DialogDisplay();
	var signinDialog = document.getElementById("signInDialogContent");
	signinDialog.style.zIndex = 2;
	signinDialog.style.opacity = 1.0;
}

function Cancel()
{
	var dialog = document.getElementById("dialogScn");
	var appContainer = document.getElementById("container");
	var aboutDialog = document.getElementById("aboutDialogContent");
	var signinDialog = document.getElementById("signInDialogContent");
	var errDialog = document.getElementById("errDialogContent");
	var userDialog = document.getElementById("createUserDialogContent");
	aboutDialog.style.zIndex = -1;
	aboutDialog.style.opacity = 0;
	signinDialog.style.zIndex = -1;
	signinDialog.style.opacity = 0;
	errDialog.style.zIndex = -1;
	errDialog.style.opacity = 0;
	userDialog.style.zIndex = -1;
	userDialog.style.opacity = 0;
	dialog.style.zIndex = -1;
	dialog.style.opacity = 0;
	appContainer.style.opacity = 1.0;
}

function CommentBoxGenerate(comment)
{
	var commentFeed = document.getElementById("commentFeed");
	var text = comment.text;
	var name = comment.name;
	var time = comment.time;
	var timeDisp;
	var span;
	var commentBox = document.createElement("div");
	var commentTextArea = document.createElement("textarea");
	commentBox.className = "commentBox postedComment";
	commentTextArea.innerHTML = text;
	commentTextArea.disabled = true;
	commentBox.appendChild(commentTextArea);
	commentBox.innerHTML += "</br>";

	span = document.createElement("span");
	span.innerHTML = name;
	commentBox.appendChild(span);
	span = document.createElement("span");
	span.innerHTML = ", ";
	commentBox.appendChild(span);
	span = document.createElement("span");
	timeDisp = new Date(time);
	span.innerHTML = timeDisp.toTimeString();
	commentBox.appendChild(span);

	commentFeed.insertBefore(commentBox, commentFeed.firstChild);
}

//All server actions routed through here.
function ServerAction (postData, callback)
{
	var req = new XMLHttpRequest();
	req.onload = function() {
		console.log("Loaded request, status = " + req.status + ", response = " + req.responseText);
		var resJson = JSON.parse(req.responseText);
		callback(resJson);
	};
	req.open("POST", NODE_SERVER, true);
	req.setRequestHeader(CONTENT_TYPE, CONTENT_TYPE_JSON);
	req.send(postData);
}

//Specific server actions and their handlers.
function SignIn(cb)
{
	var status = true;
	var postData = JSON.stringify({action:actionNames.signIn, "uname":myUsername, "pwd":myPwd});
	var handleRes = function(resJson) {
		if (!resJson.args.valid)
		{
			document.getElementById("errText").innerHTML = "Username or password invalid.";
			Cancel();
			ErrDialogDisplay();
			status = false;
		}
		else
		{
			myToken = resJson.args.token;
			myDisplayName = resJson.args.displayName;
			document.getElementById("commentName").innerHTML = resJson.args.displayName;
			document.getElementById("commentText").disabled = false;
			document.getElementById("commentText").placeholder = "";
			Cancel();
			var signInBtn = document.getElementById("userSignin");
			signInBtn.childNodes[0].innerHTML = "Sign Out";
			signInBtn.onclick = SignOut;
			LoadAddImage();
			console.log("ABOUT TO ASK FOR ALL COMMENTS EVER");
			GetComments(0);
			commentRefreshTask = setInterval(GetComments, 100);
		}
		if (typeof cb === "function")
		{
			cb(status);
		}
	};
	ServerAction(postData, handleRes);
}
function SignOut(cb)
{
	var status = true;
	var postData = JSON.stringify({action:actionNames.signOut, "token":myToken});
	var handleRes = function(resJson) {
		if (!resJson.args.valid)
		{
			document.getElementById("errText").innerHTML = "Failed to sign out on server.";
			ErrDialogDisplay();
			status = false;
		}
		else
		{
			myToken = "";
			var signInBtn = document.getElementById("userSignin");
			var commentFeed = document.getElementById("commentFeed");
			document.getElementById("commentName").innerHTML = "";
			document.getElementById("commentText").disabled = true;
			signInBtn.childNodes[0].innerHTML = "Sign In";
			signInBtn.onclick = SigninDialogDisplay;
			clearInterval(commentRefreshTask);
			while (commentFeed.firstChild) {
				commentFeed.removeChild(commentFeed.firstChild);
			} 
		}
		if (typeof cb === "function")
		{
			cb(status);
		}
	};
	ServerAction(postData, handleRes);
}
function CreateNewUser(cb)
{
	var status = true;
	var username = document.getElementById("newUsername").value;
	var pwd = document.getElementById("newPwd").value;
	var displayName = document.getElementById("newDisplayName").value;
	var postData = JSON.stringify({action:actionNames.createUser, "uname":username, "pwd":pwd, "displayName":displayName});
	var handleRes = function(resJson) {
		if (!resJson.args.valid)
		{
			document.getElementById("errText").innerHTML = statuses.userCreateFailed;
			Cancel();
			ErrDialogDisplay();
			status = false;
		}
		else if (resJson.token)
		{
			myToken = resJson.token;
			myUsername = username;
			myPwd = pwd;
			myDisplayName = displayName;
		}
		if (typeof cb === "function")
		{
			cb(status);
		}
	};
	ServerAction(postData, handleRes);
}

function GetComments(sinceTime, cb)
{
	if (getting)
	{
		return;
	}
	getting = true;
	if (!sinceTime && (0 !== sinceTime))
	{
		sinceTime = latestCommentTime;
	}
	var comment;
	var status = true;
	var postData = JSON.stringify({action:actionNames.getComments, "token":myToken, "since":sinceTime});
	var handleRes = function(resJson) {
		if (!resJson.args.valid)
		{
			document.getElementById("errText").innerHTML = statuses.notSignedIn;
			ErrDialogDisplay();
			status = false;
		}
		if (0 !== resJson.args.comments.length)
		{
			for (var nCount = 0; nCount < resJson.args.comments.length; nCount++)
			{
				comment = resJson.args.comments[nCount];
				CommentBoxGenerate(comment)
			}
			latestCommentTime = comment.time;
		}
		if (typeof cb === "function")
		{
			getting = false;
			cb(status);
		}
	};
	ServerAction(postData, handleRes);
}
function AddComment(cb)
{
	var status = true;
	var textBox = document.getElementById("commentText");
	var text = textBox.value;
	var postData = JSON.stringify({action:actionNames.addComment, "token":myToken, "text":text, "name": myDisplayName});
	var handleRes = function(resJson) {
		if (!resJson.args.valid)
		{
			document.getElementById("errText").innerHTML = statuses.notSignedIn;
			ErrDialogDisplay();
			status = false;
		}
		else
		{
			textBox.value = "";
			GetComments(latestCommentTime);
		}
		if (typeof cb === "function")
		{
			cb(status);
		}
	};
	ServerAction(postData, handleRes);
}
function RandomComment(cb)
{
	var status = true;
	var textBox = document.getElementById("commentText");
	var text = textBox.value;
	var postData = JSON.stringify({action:actionNames.randomComment, "token":myToken, "text":text});
	var handleRes = function(resJson) {
		if (!resJson.args.valid)
		{
			document.getElementById("errText").innerHTML = statuses.notSignedIn;
			ErrDialogDisplay();
			status = false;
		}
		else
		{
			textBox.value = resJson.args.text;
		}

		if (typeof cb === "function")
		{
			cb(status);
		}
	};
	ServerAction(postData, handleRes);
}

function UpdateCharCount(textArea)
{
	var count = textArea.value.length;
	if (!myCharElem)
	{
		myCharElem = document.getElementById("characterCount");
	}
	myCharElem.innerHTML = count + "/140";
	if (140 == count)
	{
		myCharElem.style.color = "red";
	}
	else
	{
		myCharElem.style.color = "";
	}
}



function LoadAddImage()
{
	document.getElementById("addBtn").src = "./www/img/icons8.com-web-app-1090-Enter-Inverted.png";
}