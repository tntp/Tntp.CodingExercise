/**
 * Created by Aloma on 10/18/2016.
 */
var express = require('express');
var app = express();
var ejs = require('ejs');
var mysql = require('mysql');
var bodyParser = require('body-parser');
app.use(bodyParser.urlencoded({ extended: true }));
var HOST = 'localhost';
var port = 3360;
var MYSQL_USER = 'root';
var MYSQL_PASS = 'root';
var DATABASE = 'articles';
var TABLE = 'article_table';

app.get("/", function (req, res) {
    res.redirect("/index.html");
});

var connection = mysql.createConnection({
   host: HOST,
    user: MYSQL_USER,
    password: MYSQL_PASS,
    database: DATABASE
});

connection.connect(function(err){
    if(!err) {
        console.log("Database is connected ... \n\n");
    } else {
        console.log("Error connecting database ... \n\n");
    }
});

app.get('/sendFormData', function(req, res) {

    var info = req.query;
    console.log(info);

    res.send([]);

    /* FORM DATA STORED IN INFO
     name = info.name
     COMMENT = info.comment

     */
//  SQL COMMANDS HERE
    var query = "Insert into "+TABLE+" (name,comment) VALUES ('"+info.name+"','"+info.comment+"')";
    connection.query(query,function(err, result) {
        console.log(query.sql);
    });

});

app.use(bodyParser());
app.use(express.static(__dirname));
console.log("Simple static server listening at http://:" + port);
app.listen(port);

