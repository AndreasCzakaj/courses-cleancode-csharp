public class Person {
    private string name;
    private int age;
    private string email;

    private Person(string name, int age, string email) {
        this.name = name;
        this.age = age;
        this.email = email;
    }

    public static PersonBuilder Builder() => new PersonBuilder();

    public class PersonBuilder {
        private string name;
        private int age;
        private string email;

        public PersonBuilder Name(string name) {
            this.name = name;
            return this;
        }

        public PersonBuilder Age(int age) {
            this.age = age;
            return this;
        }

        public PersonBuilder Email(string email) {
            this.email = email;
            return this;
        }

        public Person Build() {
            return new Person(name, age, email);
        }
    }
}