rem create the solution
dotnet new sln -n SpyStore
rem create the class library for the Data Access Layer and add it to the solution
dotnet new classlib -n SpyStore.Dal -o .\SpyStore.Dal -f netcoreapp3.1
dotnet sln SpyStore.sln add SpyStore.Dal
rem create the class library for the Models and add it to the solution
dotnet new classlib -n SpyStore.Models -o .\SpyStore.Models -f netcoreapp3.1
dotnet sln SpyStore.sln add SpyStore.Models
rem create the XUnit project for the Data Access Layer and add it to the solution
dotnet new xunit -n SpyStore.Dal.Tests -o .\SpyStore.Dal.Tests -f netcoreapp3.1
dotnet sln SpyStore.sln add SpyStore.Dal.Tests
rem create the XUnit project for the Service and add it to the solution
dotnet new xunit -n SpyStore.Service.Tests -o .\SpyStore.Service.Tests -f netcoreapp3.1
dotnet sln SpyStore.sln add SpyStore.Service.Tests
rem create the ASP.NET Core RESTful service project and add it to the solution
rem NOTE THE NEXT TWO LINES MUST BE ON ONE LINE IN THE COMMAND FILE
dotnet new webapi -n SpyStore.Service -au none --no-https -o .\SpyStore.Service -f netcoreapp3.1
dotnet sln SpyStore.sln add SpyStore.Service
rem create the ASP.NET Core Web Application project and add it to the solution
dotnet new mvc -n SpyStore.Mvc -au none --no-https -o .\SpyStore.Mvc -f netcoreapp3.1
dotnet sln SpyStore.sln add SpyStore.Mvc
rem Add references between projects
dotnet add SpyStore.Mvc reference SpyStore.Models
dotnet add SpyStore.Dal reference SpyStore.Models
dotnet add SpyStore.Dal.Tests reference SpyStore.Models
dotnet add SpyStore.Dal.Tests reference SpyStore.Dal
dotnet add SpyStore.Service reference SpyStore.Dal
dotnet add SpyStore.Service reference SpyStore.Models
dotnet add SpyStore.Service.Tests reference SpyStore.Models
dotnet add SpyStore.Service.Tests reference SpyStore.Dal
