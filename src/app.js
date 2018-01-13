import $ from 'jquery'
import Masonry from 'masonry'
import ImagesLoaded from 'imagesloaded'

export class App {
    attached(){
        this.layoutImages();
    }

    layoutImages() {
        $(function() {
            console.log('jQuery Ready')
            ImagesLoaded('#dessertsPane', () => {
                console.log("dessertsPane has finished loading!")
                const msnry = new Masonry('.wall', {
                    itemSelector: '.post-wrapper',
                    columnWidth: '.grid-sizer',
                    percentPosition: true
                })
                console.log('Masonry called')
            })
        })
    }
}