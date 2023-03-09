# AirlyMonitor 

An engineering project for thesis. App is responsible for monitoring and alerting about the air quality based on Airly installations. For demonstration purpouses measurement simulator is simulating data for selected instruments. In real life application we would use real airly data or any other installation that would generate data. 

**App** can:
- Return current and previous measurements for installaion assinged previously by user.
- Add alert definitions with rules for raising alerts
- Alert users (emails, firebase messages) based on alert definitions set by them previously
- Simulate generating measurements for selected installations.

## Setup

**Angular CLI** - Solution has small Angular app for login/register.

**Database** - Solution requires SQL Server database installed to work

**RabbitMQ** - Solution requires RabbitMq server to work. Ideally docker image of MassTransit **RabbitMQ** from https://hub.docker.com/r/masstransit/rabbitmq. You can start docker image with command
```
docker run -p 15672:15672 -p 5672:5672 masstransit/rabbitmq
```

## Configs

**AirlyMonitor**
```
"ConnectionStrings": {
    "AirlyDb": "Server=tcp:<ip>,1433;Initial Catalog=airlydb;Persist Security Info=False;User ID=sa;Password=<password>;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;"
  },
  "RabbitConfiguration": {
    "UserName": "",
    "Password": "",
    "Address": ""
  },
    "Swagger": {
    "Client": "swagger",
    "Secret": "swagger-secret",
    "Scopes": "api",
    "AuthorizationUrl": "http://localhost:5010/connect/authorize",
    "TokenUrl": "http://localhost:5010/connect/token"
  },
    "Auth": {
    "Authority": "http://localhost:5010"
  },
  "AirlyApi": {
    "Url": "https://airapi.airly.eu/v2/",
    "ApiKey": "<key>"
  },
    "CORS": {
    "origins": [
      "http://localhost:5010",
      "https://localhost:7214"
    ]
  }
```

**IdentityServer**
```
"ConnectionStrings": {
    "AirlyDb": "Server=tcp:<ip>,1433;Initial Catalog=airlydb;Persist Security Info=False;User ID=sa;Password=<password>;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;"
},
"CORS": {
    "origins": [
      "http://localhost:5010",
      "https://localhost:7214"
    ]
 }
```

**AlertsMonitor**
```
"ConnectionStrings": {
    "AirlyDb": "Server=tcp:<ip>,1433;Initial Catalog=airlydb;Persist Security Info=False;User ID=sa;Password=<password>;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;"
},
"CORS": {
    "origins": [
      "http://localhost:5010",
      "https://localhost:7214"
    ]
 }
 "RabbitConfiguration": {
    "UserName": "",
    "Password": "",
    "Address": ""
  },
```

**MeasurementsSimulator**
```
 "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
 },
 "ConnectionStrings": {
    "AirlyDb": "Server=tcp:<ip>,1433;Initial Catalog=airlydb;Persist Security Info=False;User ID=sa;Password=<password>;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;"
}
```

**PushNotificationsService**
```
"ConnectionStrings": {
    "AirlyDb": "Server=tcp:<ip>,1433;Initial Catalog=airlydb;Persist Security Info=False;User ID=sa;Password=<password>;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;"
  },
"RabbitConfiguration": {
    "UserName": "",
    "Password": "",
    "Address": ""
 },
"Auth": {
    "Authority": "http://localhost:5010"
  },
"CORS": {
    "origins": [
      "http://localhost:5010",
      "https://localhost:7214"
    ]
  }
```

## Migrations

In Visual studio run commands
```
enable-migrations
add-migration InitialCreate
```

And then apply script located in /Database-scripts **SQLQuery_1**

## IdentityUI

Build frontend app into **wwwroot** folder
```
ng build --configuration=local
```

## Run

Now you can run all of the apps with joy :) 
