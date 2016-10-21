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
        //setTimeout(refresh_x1, 2000);
    };

    function displayErrors(errorList) {
        $("#ulErr").empty();
        errorList.forEach(function(_error) {

            var div_item = document.createElement('div');
            $(div_item).addClass('alert alert-danger');
            $(div_item).attr('role', 'alert');
            var span_icon = document.createElement('span');
            $(span_icon).addClass('glyphicon glyphicon-exclamation-sign');
            $(span_icon).attr('aria-hidden', 'true');
            $(div_item).append(span_icon);

            var span_msg = document.createElement('span');
            $(span_msg).addClass('sr-only');
            $(span_msg).text('Error : ');
            $(div_item).append(span_msg);
            $(div_item).append("  " + _error);

            $("#ulErr").append(div_item);
        }, this);

    }

    function refresh_content(comments) {

        $("#ulPosts").empty();
        comments.forEach(function(post) {


            var li_item = document.createElement('li');
            $(li_item).addClass('list-group-item');
            $(li_item).attr('id', post._id);

            var div_content = document.createElement('div');
            $(div_content).data('id', post._id);

            var header = document.createElement('h3');
            $(header).text(post.name);

            var comment = $('<b>Says : </b>');
            $(div_content).append(header);
            $(div_content).append(comment);
            $(div_content).append(post.comment);
            $(li_item).append(div_content);


            $("#ulPosts").append(li_item);

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