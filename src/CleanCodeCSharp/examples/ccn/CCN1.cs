int GetPort() {
    string portAsString = Environment.GetEnvironmentVariable("port");
    if (portAsString == null) {
        return 8080;
    }
    try {
        return int.Parse(portAsString);
    } catch (FormatException e) {
        return 8080;
    }
}