# Clean Code Training: C# examples

## Prerequisites

* [.NET SDK 8](https://dotnet.microsoft.com/download) or newer (`dotnet --version`)
* An IDE: JetBrains Rider, Visual Studio 2022 or VS Code with the "C# Dev Kit" extension

Open `CleanCodeCSharp.sln` in your IDE. The NuGet packages are restored automatically.

## Structure

| Folder                 | Content                                               |
|------------------------|-------------------------------------------------------|
| `src/CleanCodeCSharp`  | Production code                                       |
| `tests/CleanCodeCSharp.Tests` | Tests (NUnit 4), mirrors the folders of `src`  |
| `.../Dirty`            | Code that is dirty **on purpose**: to be cleaned up in the exercises |
| `.../Clean`            | Code that must not be modified in the exercises       |
| `.../Examples`         | Examples and demos from the slides                    |

Namespaces follow the folders, e.g. `CleanCodeCSharp.Dirty.Naming`
and `CleanCodeCSharp.Tests.Dirty.Naming` for its tests.

## Commands

```
# Build
dotnet build

# Run all tests
dotnet test

# Run the tests of a single class
dotnet test --filter FibonacciTest

# Run tests with verbose output
dotnet test --logger "console;verbosity=detailed"

# Watch mode (re-runs the tests on file changes)
dotnet watch test --project tests/CleanCodeCSharp.Tests/
```

## User Self Service: Login (`Clean/Uss`)

The solution of the legacy code task: on branch `main`, `src/CleanCodeCSharp/Dirty/Uss/Server.cs` does
everything in the handler of the route `/uss/login`. Here, the code is split up and tested:

| File | Kind | What |
|------|------|------|
| `Validation.cs` | Operation | syntax of username and password |
| `Password.cs` | Operation | hash and verify passwords |
| `UserSelfService.cs` | Operation | the login scenarios |
| `ControllerUtils.cs` | Operation | exceptions => HTTP status + message |
| `LoginController.cs` | Integration | request body => service => `HttpResult` |
| `IAccountDao.cs` | Boundary | interface to the database |
| `AccountDaoDictionaryImpl.cs`, `AccountDaoThrowingImpl.cs` | Boundary | fakes for the tests |
| `AccountDaoSqliteImpl.cs` | Integration | SQLite |
| `Server.cs` | Integration | wires the parts, defines the route, starts ASP.NET |

Scenarios:

* Credentials (username + password) have invalid syntax => 400
  * username: min 8, max 20 chars; `[a-zA-Z0-9\-_]`
  * password: min 12, max 32 chars; `[a-zA-Z0-9\-_.,+]`
* Credentials have valid syntax, but no such username => 401
* Username exists, but wrong password => SAME(!) error 401
* Credentials OK, but account status is not "verified" => 400
* All OK => 200 + session object including account ID, username, email

### Tests

```
# all tests of the login
dotnet test --filter "FullyQualifiedName~Clean.Uss"

# unit tests only: no database, no HTTP
dotnet test --filter "FullyQualifiedName~Clean.Uss&TestCategory!=Integration"

# integration tests only (Clean/Uss/Integration): SQLite in memory, HTTP server
dotnet test --filter "TestCategory=Integration"
```

### Run it

```
# SQLite in memory + some accounts, see the console output
dotnet run --project src/CleanCodeCSharp

# if port 3000 is taken (PowerShell: $env:PORT=3055; dotnet run --project src/CleanCodeCSharp)
PORT=3055 dotnet run --project src/CleanCodeCSharp
```

```
curl -i -X POST localhost:3000/uss/login \
  -H 'Content-Type: application/json' \
  -d '{"username": "alice_verified", "password": "Correct-Horse_42"}'
```

PowerShell:

```
Invoke-RestMethod -Method Post -Uri http://localhost:3000/uss/login -ContentType 'application/json' `
  -Body '{"username": "alice_verified", "password": "Correct-Horse_42"}'
```
