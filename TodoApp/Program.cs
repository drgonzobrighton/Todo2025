using TodoApp;

// ConsoleHelper.GetSelection("Type y or n", ["y", "n"]);
//
// Console.ReadKey();

var p = new Person(3);
p.SetAge(18);
Console.WriteLine(p.Age);
Console.WriteLine(p.Parent);


Console.ReadKey();


public class Person(int age)
{
    private int _age;

    public int Age => _age;

    public void SetAge(int age) { _age = age; }

    public Person Parent { get; set; }
}
