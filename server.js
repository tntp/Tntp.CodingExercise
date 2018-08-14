//server.js

//------------------------------------------------------------------------------
//	Constants and globals for the server
//
const MY_PORT 		= 8321;
const SALT_ROUNDS	= 10;
const MAX_COMMENT_LEN	= 140;
const bodyParser	= require('body-parser');
const assert		= require('assert');
const bcrypt 		= require('bcryptjs');
const express 		= require('express');
const resource		= require('express-resource');
const path 			= require('path');
const http			= require('http');
var app 			= express();
var mongoClient		= require('mongodb').MongoClient;
var tokens			= [];
var randomQuotes	= require('./random.json');

console.log("Starting server.");
app.listen(MY_PORT, function()
{
    console.log("Server listening on: http://localhost:%s", MY_PORT);
});

//------------------------------------------------------------------------------
//	Middleware - for handling JSON-encoded data and static resources
//
app.use(bodyParser.json());
app.use('/www', express.static('www'));

//------------------------------------------------------------------------------
//	App startup.
//
app.get('/', function(req, res) {
	res.sendFile(path.join(__dirname + '/www/index.html'));
});

//------------------------------------------------------------------------------
//	These are all the actions that the client can request.
//	Some of them require the user to be signed in already and provide a token
//	for verification.
//
app.post('/', function(req, res) {
	//console.log("This request = " + JSON.stringify(req.body));
	var reqJson = req.body;
	var RequestHandled = function(args) {
		res.send({ "status": 'SUCCESS' , "args": args});
	}
	switch (reqJson.action)
	{
		case ("SignIn"):
		{
			mongoClient.connect("mongodb://localhost:27017/tntpTwitterDb", function (err, db) {
				if (err)
				{
					console.log("Failed to connect to MongoDB server.");
					return;
				}
				//console.log("Connected to MongoDB server.");
				SignIn(db, RequestHandled, reqJson);
			});
			break;
		}
		case ("SignOut"):
		{
			TokenRetire(reqJson.token);
			RequestHandled({valid:true});
			break;
		}
		case ("CreateUser"):
		{
			mongoClient.connect("mongodb://localhost:27017/tntpTwitterDb", function (err, db) {
				if (err)
				{
					console.log("Failed to connect to MongoDB server.");
					return;
				}
				CreateUser(db, RequestHandled, reqJson);
			});
			break;
		}
		case ("AddComment"):
		{
			if (-1 === tokens.indexOf(reqJson.token))
			{
				RequestHandled({valid:false});
				return;
			}
			if (!reqJson.name || !reqJson.text || (MAX_COMMENT_LEN < reqJson.text.length))
			{
				RequestHandled({valid:false});
				return;
			}
			mongoClient.connect("mongodb://localhost:27017/tntpTwitterDb", function (err, db) {
				if (err)
				{
					console.log("Failed to connect to MongoDB server.");
					return;
				}
				//console.log("Connected to MongoDB server.");
				AddComment(db, RequestHandled, reqJson);
			});
			break;
		}
		case ("GetComments"):
		{
			if (-1 === tokens.indexOf(reqJson.token))
			{
				RequestHandled({valid:false});
				return;
			}
			mongoClient.connect("mongodb://localhost:27017/tntpTwitterDb", function (err, db) {
				if (err)
				{
					console.log("Failed to connect to MongoDB server.");
					return;
				}
				//console.log("Connected to MongoDB server.");
				GetComments(db, RequestHandled, req.body);
			});
			break;
		}
		case ("RandomComment"):
		{
			if (-1 === tokens.indexOf(reqJson.token))
			{
				RequestHandled({valid:false});
				return;
			}
			RandomCommentGet(RequestHandled);
			break;
		}
		default:
		{
			console.log("Unsupported request - " + reqJson.action);
			RequestHandled({valid:false});
			break;
		}
	}
	return;
});

function RandomCommentGet(cb)
{
	var quotesLen = randomQuotes.length;
	var currQuote = randomQuotes[Math.floor(Math.random() * quotesLen)];
	cb({"valid":true, "text":currQuote});
}

//------------------------------------------------------------------------------
//	TokenGenerate - Take username, pre-hashed password, displayname, and 
//	appends a timestamp and returns the hashed result as a token.
//
function TokenGenerate(uname, hashPwd, displayName, cb)
{
	var token = uname + hashPwd + displayName + Date.now();
	bcrypt.hash(token, SALT_ROUNDS, function(err, hash) {
		tokens.push(hash);
		console.log("All tokens = " + JSON.stringify(tokens));
		cb({"valid":true, "token":hash, "displayName":displayName});
	});
}
//------------------------------------------------------------------------------
//	TokenRetire - Make this token invalid.
//
function TokenRetire(token)
{
	var index;
	if (-1 !== (index = tokens.indexOf(token)))
	{
		tokens.splice(index, 1);
	}
}

//------------------------------------------------------------------------------
//	Operations that use DB
//

//------------------------------------------------------------------------------
//	AddComment - Add a single comment to the DB.
//
var AddComment = function(db, cb, args) {
	var commentCollection = db.collection("documents");
	commentCollection.insertOne({
		name: args.name,
		text: args.text,
		time: Date.now()
	}, function(err, result) {
		if (err)
		{
			cb({valid:false});
		}
		else
		{
	    	console.log("Inserted a comment into the collection");
	   		cb({valid:true});
		}
  	});
};
//------------------------------------------------------------------------------
//	CreateUser - Accept username, password, and display name. Store user
//	info in DB with pwd hashed using bcryptjs. Return status plus token.
//
var CreateUser = function(db, cb, args) {
	var userCollection = db.collection("users");
	console.log("Doing CreateUser");
	userCollection.find({"uname":args.uname}).toArray(function(err, user) {
		if (0 !== user.length)
		{
			console.log("User " + args.uname + " already exists!");
			cb({valid:false});
		}
		else
		{
			bcrypt.hash(args.pwd, SALT_ROUNDS, function(err, hash) {
				if (err)
				{
					console.log("Could not hash password " + args.pwd + " for some reason.");
					cb({valid:false});
				}
				else
				{
					console.log("Hashed password from " + args.pwd + " to " + hash);
					userCollection.insertOne({
						uname:args.uname,
						pwd:hash,
						displayName:args.displayName
					}, function(err, result) {
						assert.equal(err, null);
					    assert.equal(1, result.result.n);
					    assert.equal(1, result.ops.length);
					    console.log("Inserted a user into the collection");
					    TokenGenerate(args.uname, hash, args.displayName, cb);
				  	});
				}
			});
		}
	});
};
//------------------------------------------------------------------------------
//	GetComments - Return all comments found in the DB since the time given.
//
var GetComments = function(db, cb, args)
{
	var commentCollection = db.collection("documents");
	if (!args.since)
	{
		args.since = 0;
	}
	console.log("Since time = " + args.since);
	commentCollection.find({ time: { $gt: args.since } }).toArray(function(err, comments) {
		console.log("Found the following records");
		console.log(comments);
		cb({valid:true, "comments":comments});
	});
};
//------------------------------------------------------------------------------
//	SignIn - Accept username, pwd and return a status plus return a token.
//
var SignIn = function(db, cb, args) {
	var userCollection = db.collection("users");
	console.log("Doing SignIn");
	userCollection.find({"uname":args.uname}).toArray(function(err, user) {
		if (1 !== user.length)
		{
			console.log("Failed to find user (count = " + user.length + ")");
			cb({valid:false});
		}
		else
		{
			bcrypt.compare(args.pwd, user[0].pwd, function(err, res) {
				if (!res)
				{
					console.log("Password " + args.pwd + " does not match the user's PWD.");
					cb({valid:false});
				}
				else
				{
					TokenGenerate(args.uname, user[0].pwd, user[0].displayName, cb);
				}
			});
		}
	});
};