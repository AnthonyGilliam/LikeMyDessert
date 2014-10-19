﻿var dessert_boxes = {
    $like_button: $(".like-button"),
    $dislike_button: $(".dislike-button"),
    rateDessert: function ($rateButton, isLiked, ratecolor) {//If dislike was clicked, the isLiked bool should be set to false
        var $dessert = $rateButton.closest(".dessert-box");
        var id = $dessert.attr("id");
        var $ratingSpan = isLiked ? $rateButton.next(".like-count") : $rateButton.next(".dislike-count");
        var rating = $ratingSpan.text();
        var textColor = $ratingSpan.css("color");
        var action = isLiked ? "Like" : "Dislike";
        $.ajax({
            url: "/Dessert/" + action + "/" + id,
            type: "POST",
            context: $ratingSpan,
            success: function () {
                rating++;
                $(this).text(rating);
                animateRate($ratingSpan, ratecolor, textColor);
            },
            error: function () { alert("Fail:  This dessert couldn't be rated. The server must be actin' stupid right now :("); }
        });
    }
};

var add_form = {
    $add_form_container: $("#add-form-container"),
    $form: $("#add-form"),
    $name_label: $("#add-form Label[for='Name']"),
    $name: $("#add-form input[name='Name']"),
    $picture_label: $("#add-form Label[for='Picture']"),
    $picture_container: $("#add-form #PictureContainer"),
    $picture_uploader: $("#add-form #PictureUploader"),
    $temp_pic_pane: $("#add-form #TempPicture"),
    $picture_change_button: $("#add-form #change-button"),
    $description_label: $("#add-form Label[for='Description']"),
    $description: $("#add-form textarea[name='Description']"),
    $get_add_button: function () { return $('span:contains(Add)', '.ui-dialog-buttonset').closest('.ui-button') },
    $get_cancel_button: function () { return $('span:contains(Cancel)', '.ui-dialog-buttonset').closest('.ui-button') },
    $error_panel: $("#add-form #error-panel"),
    $get_errors: function () { return $("#add-form .error"); },
    $get_error_markers: function () { return $("#add-form span.error");},
    errors: [],
    error_marker: "<span class='error'>&nbsp;*</span>",
    IsPictureSet: false,
    IsValid: function () {
        var isValid = true;
        var minNameChars = 2;
        var minDescriptionChars = 5;

        add_form.clearErrors();

        if (!$lmd.TextIsSignificant(add_form.$name.val(), minNameChars)) {
            add_form.errors.push("The name must be at least " + minNameChars + " characters");
            add_form.$name_label.after(add_form.error_marker);
            isValid = false;
        }
        if (!add_form.IsPictureSet) {
            add_form.errors.push("The dessert must have a picture");
            add_form.$picture_label.after(add_form.error_marker);
            isValid = false;
        }
        if (!$lmd.TextIsSignificant(add_form.$description.val(), minDescriptionChars)) {
            add_form.errors.push("The description must be at least " + minDescriptionChars + " characters");
            add_form.$description_label.after(add_form.error_marker);
            isValid = false;
        }

        return isValid;
    },
    showErrors: function () {
        for (var i = 0; i < add_form.errors.length; i++) {
            add_form.$error_panel.append("<span>" + add_form.errors[i] + "</span><br/>");
        }

        add_form.$get_errors().show();
    },
    clearErrors: function () {
        add_form.errors = [];
        add_form.$error_panel.empty();
        add_form.$get_error_markers().remove();
    },
    create: function () {
        add_form.$add_form_container.dialog({
            title: "Add A Dessert",
            autoOpen: false,
            width: 600,
            draggable: false,
            modal: true,
            buttons: {
                "Add": function () {
                    if (add_form.IsValid())
                    {
                        add_form.$form.ajaxSubmit({
                            clearForm: true,
                            error: function () { alert("Fail:  This dessert couldn't be added. The server must be actin' stupid right now :("); },
                            success: function (successScreen) {
                                add_form.$add_form_container.hide();
                                add_form.$add_form_container.after(successScreen);
                                add_form.reset(true);
                                home_page.loadDessertPane();
                            }
                        });
                    } else {
                        add_form.showErrors();
                    }
                },
                "Cancel": function () {
                    add_form.reset(false);
                }
            }
        });
        add_form.$form.show();
    },
    reset: function (isDelayed) {
        $(".ui-dialog-buttonpane button:contains('Add')").button("disable");
        setTimeout(function () {
            add_form.clearErrors();
            add_form.$add_form_container.dialog("close");
            add_form.$add_form_container.dialog("destroy");
            add_form.render(add_form.$picture_uploader);
            add_form.$temp_pic_pane.empty();
            add_form.IsPictureSet = false;
            add_form.create();
        }, isDelayed ? 1500 : 0);
    },
    obscure: function ($element) {
        $element.css({ position: 'absolute', opacity: '0' });
    },
    render: function ($element) {
        $element.css({ position: 'static', opacity: '1' });
    }
};

function animateRate($rateSpan, fadeColor, normalColor) {
    $rateSpan.animate({ fontWeight: 900, color: fadeColor }, "fast")
        .delay(1000)
        .animate({ fontWeight: 400, color: normalColor }, "fast");
}

$(function () {
    //add form
    add_form.create();

    home_page.$add_button.live("click", function (e) {
        e.preventDefault();
        add_form.$add_form_container.dialog("open");
    });

    add_form.$picture_uploader.live("change", function (e) {
        add_form.$get_add_button().prop('disabled', true);
        add_form.$temp_pic_pane.empty();
        add_form.$form.addClass('loading');
        add_form.$form.ajaxSubmit({
            url: "/dessert/AddPictureChange",
            target: add_form.$temp_pic_pane,
            clearForm: false,
            error: function () { alert("Fail:  This picture couldn't be uploaded. The server must be actin' stupid right now :("); },
            success: function () {
                add_form.obscure(add_form.$picture_uploader);
                add_form.IsPictureSet = true;
            },
            complete: function () {
                add_form.$get_add_button().prop('disabled', false);
                add_form.$form.removeClass('loading');
            }
        });
    });

    add_form.$picture_change_button.live("click", function (e) {
        add_form.$picture_uploader.trigger("click");
    });

    // Like
    dessert_boxes.$like_button.live("click", function (e) {
        e.preventDefault();
        dessert_boxes.rateDessert($(this), true, "green");
    });

    // Dislike
    dessert_boxes.$dislike_button.live("click", function (e) {
        e.preventDefault();
        dessert_boxes.rateDessert($(this), false, "red");
    });
})