using TodoApp.Presentation;

var f = (bool b) => Result.Ok();
var selection = ConsoleHelper.GetSelection("Select from x or y", ["x", "y"]);
var boolean = ConsoleHelper.GetInput("Enter bool val", f);
ConsoleHelper.Print(selection, ConsoleColor.Blue);
ConsoleHelper.Print(boolean.GetType().ToString(), ConsoleColor.Blue);
