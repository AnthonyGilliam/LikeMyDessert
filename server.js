const express = require("express")

const app = express()
const PORT = process.env.PORT || '8080';

app.use(express.static(__dirname))

app.get('*', function(request, response){
    response.sendFile(__dirname + '/index.html')
})

app.listen(PORT)

console.log(`server started on port :: ${PORT}`)