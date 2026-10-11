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
