import { containerless, bindable } from 'aurelia-framework'

@containerless()
export class Post {
    @bindable id
    @bindable img
    @bindable description
}