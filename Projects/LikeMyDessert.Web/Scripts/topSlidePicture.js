$lmd.slide_show = function () {
    var showTime = 2500;
    var fadeTime = 600;
    var intervalId = 0;
    function $getSlides() {
        return $('#top-slide-pictures .picture:visible');
    }
    function $ajaxNextTopSlidePicture(index) {
        return $.ajax('/Picture/GetNextTopSlidePicture', {
            data: {
                id: $getSlides().eq(index).attr('id')
            }
        });
    }
    function transitionNextSlide(picture, index) {
        var $target = $getSlides().eq(index);
        $target.animate({
            opacity: '0'
        }, fadeTime, function () {
            var $newSlide = $(picture).css('opacity', '0');
            console.log($newSlide);
            $target.replaceWith($newSlide);
            $newSlide.animate({
                opacity: 1
            }, fadeTime, 'swing');
        });
    }
    this.play = function () {
        var i = 0;
        intervalId = setInterval(function () {
            $ajaxNextTopSlidePicture(i).done(function (picture) {
                console.log(picture);
                transitionNextSlide(picture, i);
            });
            var numOfSlides = $getSlides().length;
            i = i < numOfSlides - 1 ? i + 1 : 0;
            console.log(i);
        }, showTime + fadeTime * 2);
        return true;
    };
    this.stop = function () {
        if (intervalId) {
            clearInterval(intervalId);
        }
        return true;
    };
    return this;
}();

$(function(){
    $lmd.slide_show.play();
});