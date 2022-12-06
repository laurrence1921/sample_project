
## Getting Started

Administrator
username: administrator@localhost 
password: Administrator1!

Normal user
username: user@localhost
password: DefaultUser1!

### Dependencies
Run `npm i` in the `WebUI/ClientApp` to install javascript dependencies

### Database Configuration

The default connection string in appsettings.json will be as follows. Change to your own development database as needed.

`Server=localhost;Database=adrateam;User Id=laurrence;Password=asdf1234;Trusted_Connection=True;MultipleActiveResultSets=true;`

### Database Migrations

To run the migrations.

 `dotnet ef database update --project src\Infrastructure --startup-project src\WebUI`

## Overview

### Domain

This will contain all entities, enums, exceptions, interfaces, types and logic specific to the domain layer.

### Application

This layer contains all application logic. It is dependent on the domain layer, but has no dependencies on any other layer or project. This layer defines interfaces that are implemented by outside layers. For example, if the application need to access a notification service, a new interface would be added to application and an implementation would be created within infrastructure.

### Infrastructure

This layer contains classes for accessing external resources such as file systems, web services, smtp, and so on. These classes should be based on interfaces defined within the application layer.

### WebUI

This layer is a single page application based on Angular 13 and ASP.NET Core 6. This layer depends on both the Application and Infrastructure layers, however, the dependency on Infrastructure is only to support dependency injection. Therefore only *Startup.cs* should reference Infrastructure.




