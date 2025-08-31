namespace CleanCodeTraining;

using System;
using System.Text;

public class UuidGeneratorNaiveRandomImpl : UuidGenerator {
    private static readonly Random RANDOM = new Random();

    public string Create() {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < 32; i++) {
            sb.Append(CreateOne());
        }
        return sb.ToString();
    }

    string CreateOne() {
        return RANDOM.Next(15).ToString("x");
    }
}