// We first require our express package
var express = require('express');
var bodyParser = require('body-parser');
var commentData = require('./data.js');

// This package exports the function to create an express instance:
var app = express();

// We can setup Jade now!
app.set('view engine', 'ejs');

// This is called 'adding middleware', or things that will help parse your request
app.use(bodyParser.json()); // for parsing application/json
app.use(bodyParser.urlencoded({ extended: true })); // for parsing application/x-www-form-urlencoded

// This middleware will activate for every request we make to 
// any path starting with /assets;
// it will check the 'static' folder for matching files 
app.use('/assets', express.static('static'));

app.get("/getposts", function(request, response) {
    //console.log('Getting all posts.');
    commentData.getAllComments().then(
        function(params) {
            response.json(params);
        });
});

app.get("/", function(request, response) {
    //console.log('Naviagting to home page');
    commentData.getAllComments().then(
        function(params) {
            response.render("pages/home", { error: "", comments: params });
        });
});

app.post("/createpost", function(request, response) {
    var comment = request.body.comment;
    var name = request.body.name;
    commentData.createComment(comment, name)
        .then(
            function(ID) {
                response.redirect("/");
            },
            function(errorMessage) {

                var jsonData = { error: errorMessage };
                response.render("pages/home", jsonData);
            });
});


app.get("/all1", function(request, response) {
    commentData.getAllComments().then(
        function(params) {
            console.log(params);
            response.json(params);
        });

});



// We can now navigate to localhost:3000
app.listen(3000, function() {
    console.log('Your server is now listening on port 3000! Navigate to http://localhost:3000 to access it');
});