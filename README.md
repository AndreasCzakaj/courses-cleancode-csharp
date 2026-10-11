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

## User Self Service: Login (`Dirty/Uss`)

`src/CleanCodeCSharp/Dirty/Uss/Server.cs` implements the login. It works, but everything happens in the
handler of the route `/uss/login`: validation, database calls, error handling.
There are no tests.

Scenarios:

* Credentials (username + password) have invalid syntax => 400
  * username: min 8, max 20 chars; `[a-zA-Z0-9\-_]`
  * password: min 12, max 32 chars; `[a-zA-Z0-9\-_.,+]`
* Credentials have valid syntax, but no such username => 401
* Username exists, but wrong password => SAME(!) error 401
* Credentials OK, but account status is not "verified" => 400
* All OK => 200 + session object including account ID, username, email

### Your task

* Extract functions and classes
* Separate Operation code (logic: validation, password check, decisions) from
  Integration code (HTTP, database)
* Add tests along the way
* Done when: all scenarios are covered by tests

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
