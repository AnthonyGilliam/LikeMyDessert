const port = '2112'
let express = require("express"),
    path = require('path'),
    app = express()

app.use(express.static(__dirname))

app.get('*', function(request, response){
    response.sendFile(__dirname+'/index.html')
})

app.listen(port)

console.log(`server started on port :: ${port}`)