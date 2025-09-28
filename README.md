# Run tests with verbose output
dotnet test --logger "console;verbosity=detailed" tests/CleanCodeCSharp.Tests/

# Run specific test project
dotnet test tests/CleanCodeCSharp.Tests/

# Watch mode (runs tests on file changes)
dotnet watch test --project tests/CleanCodeCSharp.Tests/
