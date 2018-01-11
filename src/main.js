export function configure(aurelia){
    aurelia.use
        .standardConfiguration()
        .developmentLogging();

    aurelia.start().then(au => au.setRoot('app'));
}