var MongoClient = require('mongodb').MongoClient,
    settings = require('./config.js'),
    Promise = require('promise'),
    Guid = require('guid');

var fullMongoUrl = settings.mongoConfig.serverUrl + settings.mongoConfig.database;

var exports = module.exports = {};

MongoClient.connect(fullMongoUrl)
    .then(function(db) {
        var chat_collection = db.collection("tntpchatroomdb1");

        // setup your body
        exports.createComment = function(_comment, _name) {
            console.log('In Create Comment');
            // throws an error if there has been invalid input
            if (_comment == '' || _comment == undefined) {
                console.log('Comment was empty.');
                return Promise.reject('Can not enter EMPTY string');
            } else if (_name == '' || _name == undefined) {
                console.log('name was empty.');
                return Promise.reject('name can not be null.');
            } else {
                // return a promise that resolves the new comment
                var _date = new Date();
                console.log('Create a post at : ' + _date);
                var jsonData = {
                    _id: Guid.create().toString(),
                    comment: _comment,
                    name: _name,
                    date: _date
                };
                return chat_collection.insertOne(jsonData).then(function(newDoc) {
                    return newDoc.insertedId;
                });
            }

        };

        exports.getAllComments = function() {
            return chat_collection.find().sort({ date: -1 }).toArray().then(function(params) {
                //console.log(" All posts : " + JSON.stringify(params));
                return params;
            });
        }
    });