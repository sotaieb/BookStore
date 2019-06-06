# README

## Context
- See [Docs](docs/test_technique_2019.pdf)

## Building
- Visual Studio Code (Mac OS)
- .Net Core SDK : 3.0.100-preview5-011568
- Xunit

## Useful Commands : PShell, Git, Dotnet Cli

```
mkdir BookStore

cd BookStore

git init

touch .gitignore

touch README.MD

dotnet new sln -n BookStore

dotnet new classlib -n St.BookStore.Core

dotnet new classlib -n St.BookStore.Business

dotnet new classlib -n St.BookStore.Data

dotnet new xunit -name St.BookStore.Business.Tests

dotnet sln add St.BookStore.Core/St.BookStore.Core.csproj

dotnet sln add St.BookStore.Data/St.BookStore.Data.csproj

dotnet sln add St.BookStore.Business/St.BookStore.Business.csproj

dotnet sln add St.BookStore.Business.Tests/St.BookStore.Business.Tests.csproj

dotnet add St.BookStore.Business/St.BookStore.Business.csproj reference St.BookStore.Data/St.BookStore.Data.csproj

dotnet add St.BookStore.Data/St.BookStore.Data.csproj reference St.BookStore.Core/St.BookStore.Core.csproj

dotnet add St.BookStore.Business/St.BookStore.Business.csproj reference St.BookStore.Core/St.BookStore.Core.csproj

dotnet add St.BookStore.Business.Tests/St.BookStore.Business.Tests.csproj reference St.BookStore.Core/St.BookStore.Core.csproj

dotnet add St.BookStore.Business.Tests/St.BookStore.Business.Tests.csproj reference St.BookStore.Business/St.BookStore.Business.csproj

dotnet add St.BookStore.Business.Tests/St.BookStore.Business.Tests.csproj reference St.BookStore.Data/St.BookStore.Data.csproj

code .

dotnet build
dotnet test
```

## Add dependencies to project

```
dotnet add package Newtonsoft.Json
```

## How to a-dd content File to csproj

None - The file is not included in the project output group and is not compiled in the build process.

Content - The file is not compiled, but is included in the Content output group (ex. Html).

```
<ItemGroup>
    <None Update="appsettings.json">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
</ItemGroup>
```

## Todo
- See : .Net core 3 Validation model.