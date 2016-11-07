$lmd.slide_show = function () {
    var showTime = 2500;
    var fadeTime = 600;
    var intervalId = 0;
    var slideIndex = 0;
    function $getSlides() {
        return $('#top-slide-pictures .picture:visible');
    }
    function $ajaxNextTopSlidePicture(index) {
        var currentSlideIds = $getSlides().map(function (index, picture) {
            return picture.id;
        });
        var idArray = $.makeArray(currentSlideIds);
        return $.ajax('/Picture/GetNextTopSlidePicture', {
            type: 'POST',
            dataType: 'html',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({
                referencePictureIDs: idArray
            })
        });
    }
    function transitionNextSlide(picture, index) {
        var $target = $getSlides().eq(index);
        $target.animate({
            opacity: '0'
        }, fadeTime, function () {
            var $newSlide = $(picture).css('opacity', '0');
            $target.replaceWith($newSlide);
            $newSlide.animate({
                opacity: 1
            }, fadeTime, 'swing');
        });
    }
    function incrementSlideIndex() {
        var numOfSlides = $getSlides().length;
        slideIndex = slideIndex < numOfSlides - 1 ? slideIndex + 1 : 0;
    }
    this.play = function () {
        //setInterval doesn't actually execute code until after the first iteration
        intervalId = setInterval(function () {
            $ajaxNextTopSlidePicture().done(function (picture) {
                transitionNextSlide(picture, slideIndex);
            }).fail(function (jqXHR, status, error) {
                console.log('failed to load top slide picture');
                console.log(jqXHR);
                console.log(status);
                console.log(error);
            });
            incrementSlideIndex();
        }, showTime + fadeTime * 2);
        return true;
    };
    this.stop = function () {
        if (intervalId) {
            clearInterval(intervalId);
        }
        return true;
    };
    this.once = function() {
        $ajaxNextTopSlidePicture().done(function (picture) {
            transitionNextSlide(picture, slideIndex);
        }).fail(function (jqXHR, status, error) {
            console.log('failed to load top slide picture');
            console.log(jqXHR);
            console.log(status);
            console.log(error);
        });
        incrementSlideIndex();
    };
    return this;
}();

$(function(){
    $lmd.slide_show.play();
});