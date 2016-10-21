// 1. Form validation 
// 2. Timer call for constant refresh.
$(document).ready(function() {
    //alert('document loaded.');
    setTimeout(refresh_x1, 2000);

    function refresh_x1() {
        //alert('Need to refresh_x1...!');
        var get_all_posts = {
            url: "/getposts",
            method: "GET"
        };
        $.ajax(get_all_posts).then(function(response) {
            console.log(JSON.stringify(response));
            if (response != undefined) {
                refresh_content(response);
            }
        });
        setTimeout(refresh_x1, 2000);
    };

    function displayErrors(errorList) {
        $("#ulErr").empty();
        errorList.forEach(function(_error) {
            var list_item = '<div class="alert alert-danger" role="alert">' +
                '<span class="glyphicon glyphicon-exclamation-sign" aria-hidden="true"></span>' +
                '<span class="sr-only">Error:</span>' +
                _error +
                '</div>';
            $("#ulErr").append(list_item);
        }, this);

    }

    function refresh_content(comments) {

        $("#ulPosts").empty();
        comments.forEach(function(post) {

            var list_item = '';
            list_item = '<li id="' + post._id + '" class="list-group-item">' +
                '<div data-id="' + post._id + '">' +
                '<h3>' + post.name + '</h3>' +
                '<b>Says : </b>' + post.comment +
                '</div>' +
                '</li>';

            $("#ulPosts").append(list_item);

        }, this);
    };

    function is_More_Than_Threshold_Length(text) {
        if (text.length > 140) {
            var error_list = [];
            error_list[0] = 'comment not be more than 140 characters.';
            displayErrors(error_list);
            //alert('comment not be more than 141 characters.');
        }
        return text.length > 140;
    };

    $("#btnCreatePost").click(function() {
        var txtcomment = $("#comment");
        var txtname = $("#name");

        console.log('Is Name empty : ' + isEmpty("Name", txtname.val()));
        console.log('Is Comment Empty : ' + isEmpty("Comment", txtcomment.val()));
        console.log('Is comment length greater than 140 characters : ' + is_More_Than_Threshold_Length(txtcomment.val()));
        if (!isEmpty("Name", txtname.val()) && !isEmpty("Comment", txtcomment.val()) && !is_More_Than_Threshold_Length(txtcomment.val())) {
            var _name = txtname.val();
            var _comment = txtcomment.val();
            txtname.val('');
            txtcomment.val('');
            var create_post = {
                url: "/createpost",
                method: "POST",
                data: { name: _name, comment: _comment }
            };
            $.ajax(create_post).then(function() {
                refresh_x1();
            });
        }
    });

    function isEmpty(displayName, param) {
        var result = false;
        if (param == undefined ||
            param == "" ||
            param == null) {
            result = true;
            var error_list = [];
            error_list[0] = displayName + ' can not be empty';
            displayErrors(error_list);
        } else {

            //alert(displayName + ' can not be empty');
            result = false;
        }

        return result;
    }






});

// <div class="alert alert-danger" role="alert">
//                             <span class="glyphicon glyphicon-exclamation-sign" aria-hidden="true"></span>
//                             <span class="sr-only">Error:</span>

//                         </div>


(function($) {



})(jQuery);