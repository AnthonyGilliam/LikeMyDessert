import $ from 'jquery'
import Masonry from 'masonry'
import ImagesLoaded from 'imagesloaded'
import { inject } from 'aurelia-framework'
import { DessertRepo } from 'repositories/dessertRepo'
import toastr from 'toastr'

@inject(DessertRepo)
export class App {
    constructor(dessertRepo){
        this.dessertRepo = dessertRepo
    }

    async activate(){
    }

    async attached(){
        try {
            this.posts = await this.dessertRepo.getDesserts()
            await this.layoutImages()
        }
        catch (error){
            toastr.error('Sorry, desserts could not be loaded')
            console.log(error)
        }
    }

    async layoutImages() {
        return new Promise((resolve) => {
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
                    resolve()
                })
            })
        })
    }
}