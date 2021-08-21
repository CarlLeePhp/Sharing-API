## Configuration
File appsettings.json

"TokenKey": "Super secret ungussable key",
"SendinblueKey": "yourKey",
  
"CloudinarySettings": {
"CloudName": "yourCloudName",
"ApiKey": "yourApiKey",
"ApiSecret": "yourApiSecret"
}

File appsettings.Development.json & appsettings.Production.json
"ConnectionStrings": {
	"DefaultConnection": "Server=(LocalDb)\\MSSQLLocalDb;Database=SharingDb;"
},

"CrosHost": "http://localhost:4200"