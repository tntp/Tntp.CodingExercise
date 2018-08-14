angular.module('CommentsClient', []).controller('CommentsController', function($scope, $http) {
    const BadRequestStatusCode = 400;

    $http.get("http://localhost:61781/api/comments").then(function(response) {
        $scope.commentList = response.data;
    });

    $scope.postComment = function() {
        $http.post("http://localhost:61781/api/comments", { Username: $scope.username, Content: $scope.comment }).catch(function(response) {
            if(response.status == BadRequestStatusCode) {
                $scope.error = response.data.Message;
            }
        });
    }
});