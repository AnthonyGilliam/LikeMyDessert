import Masonry from 'masonry'

export class App {
    attached(){
        setTimeout(() => {
        const msnry = new Masonry('.wall', {
            itemSelector: '.post-wrapper',
            columnWidth: '.grid-sizer',
            percentPosition: true
        })}, 10)
    }
}