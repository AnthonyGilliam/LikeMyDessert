var slide_show = function() {
    var showTime = 2500;
    var fadeTime = 500;
    var intervalId;
    $slides = function () { return $('#top-slide-pictures .picture:visible') };
    this.play =  function () {
        var numOfSlides = $slides.length;
        var i = 0;
        intervalId = setInterval(function () {
            $.ajax("/Picture/GetNextTopSlidePicture",
                {
                    data: $slides.eq(i).attr('id')
                }).done(/*...*/).fail(/*...*/);
                
            $slides.eq(i).fadeIn(fadeTime).delay(showTime).fadeOut(fadeTime);
            i = i < numOfSlides - 1 ? i + 1 : 0;
        }, showTime + fadeTime * 2);
    }
    this.stop = function(){
        if(intervalId){
            clearInterval(intervalId);
        }
    }
}();
