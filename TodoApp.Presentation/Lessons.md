- [Lesson 1](#lesson-1)
- [Lesson 2](#lesson-2)
- [Lesson 3](#lesson-3)
- [Lesson 4](#lesson-4)
- [Lesson 5](#lesson-5)

## Lesson 1

### Setting Up the Project & Creating a Console Helper

#### Project Setup

Create a new Console Application project named `TodoApp.Console`.
If you haven’t done so already, connect Rider to your GitHub account.
Push the solution to GitHub and make it public, so I can access it.

#### Implementing a Console Input Helper

- Add a new static class called `ConsoleHelper`.
- Inside this class, create a static method:

```csharp
public static string GetSelection(string prompt, IEnumerable<string> validOptions, string? errorMessage = null)
```

This method should:

- Display the given prompt to the user.
- Continuously loop until the user enters a valid input.
- Ensure the input is not empty and matches one of the allowed values.
- If the input is invalid, display an error message (or a default one).
- Return the validated input.

#### Expected Behavior & Example

**Example Usage:**

```csharp
string choice = ConsoleHelper.GetSelection("Enter 'y' or 'n': ", new[] { "y", "n" }, "Invalid choice. Please enter 'y' or 'n'.");
Console.WriteLine($"You selected: {choice}");
```

**Example Interaction:**

```text
Enter 'y' or 'n': x
'x' is not a valid value, please choose from 'y' or 'n'
Enter 'y' or 'n': y
You selected: y
```

## Lesson 2

### Building a Console Menu System

#### Objective

In this lesson, you'll create a `ConsoleMenu` class that allows users to navigate through different menu options in the console.
You'll build on what we did in Lesson 1 by reusing `ConsoleHelper.GetSelection()` to ensure valid user input.

#### Implementing ConsoleMenu

- The menu will store a collection of menu items.
- Each menu item will have:
    - A description (what the option does).
    - A key (the character the user must press).
    - An action (a method to run when the item is selected).
        - For now this will just print the option selected
- The menu should have a Show() method that:
    - Prints the menu options.
    - Waits for a valid selection (reusing GetSelection).
    - Executes the corresponding action.
    - Asks if the user wants to select another item (again, using GetSelection).

#### Expected Behavior & Example

**Example Menu Display:**

```text
Select an option:

[l] List all items
[c] Create item
[u] Update item
[d] Delete item
[x] Exit
Enter your choice: p
'p' is not a valid value, please choose from 'l', 'c', 'u', 'd', or 'x'

Enter your choice: l
Option selected: List all items

Would you like to select another item? (y/n): y
```

#### Example Code Usage:

```csharp
var menu = new ConsoleMenu(new[]
{
    new MenuItem("List all items", "l", () => Console.WriteLine("Option selected: List all items")),
    new MenuItem("Create item", "c", () => Console.WriteLine("Option selected: Create item")),
    new MenuItem("Update item", "u", () => Console.WriteLine("Option selected: Update item")),
    new MenuItem("Delete item", "d", () => Console.WriteLine("Option selected: Delete item")),
    new MenuItem("Exit", "x", () => Console.WriteLine("Exiting..."))
});

menu.Show();
```

#### Tasks for This Lesson

✅ 1. Create a new class `MenuItem` with:

- Description (string)
- Key (string)
- Action (delegate Action)

✅ 2. Create a new class `ConsoleMenu` with:

- A collection of `MenuItem` objects.
- A `Show()` method that:
    - Displays the menu items.
    - Uses `ConsoleHelper.GetSelection()` to validate user input.
    - Executes the selected item's action.
    - Asks if the user wants to pick another option.

✅ 3. Create a new `ConsoleMenu` instance with the following options:

- List all items [l]
- Create item [c]
- Update item [u]
- Delete item [d]
- Exit [x]

## Lesson 3

### Introducing Strongly Typed Input Handling

**Goal:**

Right now, our `GetSelection` method only works with predefined string options. But real-world applications often need to take
different types of user input—numbers, booleans, dates, etc. In this lesson, we’ll start handling different types explicitly,
without using generics yet.

**What You’ll Learn:**

- ✅ How to read and validate integer input.
- ✅ How to handle boolean input (yes/no/true/false).
- ✅ How to loop until valid input is provided.

#### Step 1: Create `GetInt()` method

- Create a method that prompts the user for a number and ensures they enter a valid integer.
- If the user enters something invalid (like `"abc"`), they should see an error and be asked again.

```csharp
public static int GetInt(string prompt){...}
```

**Example:**

```csharp
int age = GetInt("Enter your age:");
Console.WriteLine($"Your age is {age}");
```

#### Step 2: Create `GetBool()` method

- Now, create a method for boolean input (true/false).
- Accept y/n and true/false as valid inputs.
- Normalize y to true and n to false.

```csharp
public static bool GetBool(string prompt){...}
```

**Example:**

```csharp
bool wantsNewsletter = GetBool("Would you like to subscribe to the newsletter? (y/n/true/false)");
Console.WriteLine($"Subscription status: {wantsNewsletter}");
```

## Lesson 4

### Introducing Generics (`GetInput<T>`)

**Goal:**

Instead of writing a separate method for each type (`GetInt`, `GetBool`), we’ll create a single generic method that can handle
multiple types.

**What You’ll Learn:**

- ✅ What generics are and why they’re useful.
- ✅ How to convert string input to different types dynamically.
- ✅ How to reuse code by generalizing our methods.

#### Step 1: Create `GetInput<T>` method

Right now, both `GetInt()` and `GetBool()` are hardcoded for integers/booleans. Instead, let’s make a generic method, so it can
work for any type.

```csharp
public static T GetInput<T>(string prompt)
```

**Try This:**

```csharp
int age = GetInput<int>("Enter your age:");
bool wantsNewsletter = GetInput<bool>("Subscribe to the newsletter? (true/false):");

Console.WriteLine($"Age: {age}, Subscribed: {wantsNewsletter}");
```

Once you have implemented this new method we can delete `GetInt()` and `GetBool()`

## Lesson 5

### Adding Validation with Delegates `(Func<T, string?>)`

**Goal:**

Sometimes we need extra validation beyond just type conversion. Let’s introduce validation.

**What You’ll Learn:**

✅ How to pass a validation function to `GetInput<T>`.

- Modify GetInput<T> to Accept a Validation Function

```csharp
public static T GetInput<T>(string prompt, Func<T, string?>? validateFunc = null)
```

The function passed in will take the converted user input and will return an optional string. If the string is `null` then
validation has passed, otherwise it will contain an error message

- Only execute this function if it was provided (not null)
-

**Example**

```csharp
string? ValidateAge(int age) =>
age is >= 18 and <= 120 ? null : "Age must be between 18 and 120.";

int age = GetInput("Enter your age:", ValidateAge);
Console.WriteLine($"Your age: {age}");
```

### Refactor `GetSelection()`

Once you have implemented the validation function, revisit the `GetSelection` method and try to refactor it so it
uses `GetInput<T>` with a validator function.


