# flowerApi

A C# API running on .NET 7 that will receive request and return flower json records and their respective images.

The flowers must be hosted in json files, in the folder mentioned in the appsettings.json on the FilesPath key.

The json files will be loaded, read and deserialized and then kept in memory.

All files are read on API's initialization, so to load changes please restart the API.

# examples
- get all flowers records
`http://localhost:38527/flowers`

- get all flowers that have pink or tender color
`http://localhost:38527/flowers?colors=pink, tender`

- get all flowers that have pink color and dar background
`http://localhost:38527/flowers?colors=pink&background=dark`

- get the image file that belongs to image with id 1
`http://localhost:38527/flowers/image/1`