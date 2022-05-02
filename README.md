# Hotel Booking API
.NET Core Web API application for booking hotel rooms

# Technology
- REST API built with .NET Core 3.1 and Visual Studio 2022
- SQL Server database

# Run the project locally
## Requirements
- [.NET Core 3.1 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/3.1)
- [SQL Server 2019 Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

## Steps
- Clone repository
- Update the ConnectionStrings:HotelBookingDatabase in the appsettings.json file (in the HotelBooking.API project) with your local SQL Server information
- In the Package Manager Console (targeting the DataAccess.EF project) run the command ``Update-Database`` to deploy the database
- You can now run the applicaiton (In Visual Studio, before sarting the app, make sure the API project is set as the default Startup project) 

# Database diagram
![Database Diagram](.Documentation/DB_Diagram.PNG)


