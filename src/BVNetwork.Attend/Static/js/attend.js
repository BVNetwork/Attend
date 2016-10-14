

    $('.helpbutton').click(function () {
        var content = $(this).attr('data-helptext');
        var contentTitle = $(this).attr('data-helptitle');
        if ($("#helptextcontainer h4").text() == contentTitle) {
            $("#helptextcontainer h4").text("Title");
            $("#helptextcontainer").hide();
        }

        else {
            $("#helptextcontainer p").html(content);
            $("#helptextcontainer h4").text(contentTitle);
            $("#helptextcontainer").show();
        }
    });



