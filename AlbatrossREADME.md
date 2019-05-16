# Bangazon! - Ambitious Albatrosses

Bangazon is a .NET Web API that makes each resource in the Bangazon ERD available to application developers throughout the entire company. The database contains the following resources:

1. Products
1. Product types
1. Customers
1. Orders
1. Payment types
1. Employees
1. Computers
1. Training programs
1. Departments

## Clone the project

## Create and populate the database

1. Open Azure Data Studio and run the following SQL script:
```
USE MASTER
GO

IF NOT EXISTS (
    SELECT [name]
    FROM sys.databases
    WHERE [name] = N'Bangazon'
)
CREATE DATABASE Bangazon
GO

USE Bangazon
GO
```
1. Run [this SQL script](https://github.com/nss-day-cohort-30/bangazon-api-ambitious-albatrosses/blob/master/AmbitiousAlbatrossesData.txt) to populate the database

## Enable CORS

CORS has been enabled to protect the API from calls originating outside of bangazon.com

1. Ensure the Microsoft.AspNet.WebApi.Cors Nuget package is intalled. To do this go to
Tools -> Nuget Package Manager -> Manage Nuget Packages for Solution -> Browse (Enter CORS) -> Install the Microsoft.AspNet.WebApi.Cors package.
1. Edit your /etc/hosts file to alias localhost as www.bangazon.com. To do this, edit your /etc/hosts file and add the following to the bottom:
```
127.0.0.1 localhost
127.0.0.1 www.bangazon.com
127.0.0.1	bangazon.com
::1 localhost
```

1. Start the Bangazon API on localhost:5000
1. Go to the CORS directory in the project folder and serve index.html on port 8080 using hs -o.
1. Open developer tools and see that the request has been blocked by CORS
1. Change the url to: http://www.bangazon.com:8080
1. Check the developer tools console to make sure the request has been approved using CORS

## Run the front-end application

1. In the terminal, navigate to BangazonAPIFrontEnd/bangazon-api-front-end
1. Run `npm install`
1. Run `npm-start`