angular.module("CommentsClient", []).controller("CommentsController", function($scope, $http) {
    const BadRequestStatusCode = 400;
    const CommentCharacterLimit = 140;

    $http.get("http://localhost:61781/api/comments").then(function(response) {
        $scope.commentList = response.data;

        $scope.webSocket = new WebSocket("ws://localhost:61781/api/comments/feed");

        $scope.webSocket.onmessage = function(event) {
            $scope.commentList.unshift(JSON.parse(event.data));
        }
    });

    $scope.commentLength = CommentCharacterLimit;

    $scope.updateCommentLength = function() {
        $scope.commentLength = CommentCharacterLimit - $scope.comment.length;

        if($scope.commentLength < 0) {
            $scope.commentLength = 0;
        }
    }

    $scope.postComment = function() {
        $http.post("http://localhost:61781/api/comments", { Username: $scope.username, Content: $scope.comment }).then(function(response) {
            $scope.username = "";
            $scope.comment = "";
            $scope.error = "";
            $scope.commentLength = CommentCharacterLimit;
        }).catch(function(response) {
            if(response.status == BadRequestStatusCode) {
                $scope.error = response.data.Message;
            }
        });
    }
});