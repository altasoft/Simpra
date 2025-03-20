# AltaSoft.Simpra
[![Version](https://img.shields.io/nuget/v/AltaSoft.Simpra?label=Version&color=0c3c60&style=for-the-badge&logo=nuget)](https://www.nuget.org/profiles/AltaSoft)
[![Dot NET 8+](https://img.shields.io/static/v1?label=DOTNET&message=8%2B&color=0c3c60&style=for-the-badge)](https://dotnet.microsoft.com)

**AltaSoft.Simpra** is a powerful expression language designed for evaluating dynamic expressions in .NET applications. It enables developers to execute logic dynamically within their applications, similar to how scripting languages work but optimized for business rules.

## Installation

To install **AltaSoft.Simpra**, use NuGet:

```sh
dotnet add package AltaSoft.Simpra
```

Or in your `.csproj`:

```xml
<PackageReference Include="AltaSoft.Simpra" Version="1.0.0" />
```

## Features

- **Dynamic Expression Execution**: Evaluate expressions at runtime.
- **Rich Syntax**: Supports conditionals, arithmetic operations, lists, pattern matching, and more.
errors.
- **Mutability Control**: Allows toggling between immutable and mutable execution modes.
- **String and List Operations**: Advanced manipulation of strings, lists, and data structures.
- **Regex Support**: Enables pattern matching for strings.
- **Custom Functions**: Extendable with user-defined functions.

## Language Specifics

### Allowed Features
- **Arithmetic Operations**: Addition, subtraction, multiplication, division, and modulus.
- **Comparison Operators**: `is`, `is not`, `<`, `<=`, `>`, `>=`, `==`, `!=`.
- **Logical Operators**: `and`, `or`, `not`.
- **Membership Checks**: `in`, `not in`.
- **Pattern Matching**: `matches` (Regex), `like` (SQL-like patterns).
- **Null Handling**: `has value`.
- **Conditional Statements**: `when` and `if` structures.
- **String Manipulations**: Concatenation, uppercase/lowercase transformations.
- **List Operations**: Indexing, summation, filtering, and set-like operations.
- **Variable Assignments**: Using `let` keyword.

### Disallowed Features
- **Looping Constructs**: `for`, `while`, and recursion are not supported.
- **Direct Function Calls**: Only predefined and user-defined functions are allowed.
- **Side Effects**: Expressions should be pure and should not modify external states unless mutability is enabled.
- **Accessing External Resources**: No direct database calls, file access, or network operations.
- **Complex Data Structures**: Only lists and primitive types are supported (no dictionaries, classes, etc.).

## Usage

### Evaluating Simple Expressions

```csharp
var simpra = new Simpra();
var model = new TestModel { Transfer = new Transfer { Amount = 50, Currency = "USD" } };

const string expression = "return Transfer.Amount * 2";
var result = simpra.Execute<int, TestModel>(model, expression);

Console.WriteLine(result); // Output: 100
```

### Conditionals & Logical Operators

```csharp
const string expression = """
let transfer = Transfer
return transfer.Amount > 100 and transfer.Currency is 'USD'
""";

var result = simpra.Execute<bool, TestModel>(model, expression);
Console.WriteLine(result); // Output: false
```

### Handling Lists & Aggregations

```csharp
const string expression = """
let values = [10, 20, 30]
return sum(values) / length(values)
""";

var result = simpra.Execute<decimal, TestModel>(model, expression);
Console.WriteLine(result); // Output: 20
```

### Using Regular Expressions

```csharp
const string expression = """
let value = 'abc123'
return value matches '[a-zA-Z_][a-zA-Z_0-9]*'
""";

var result = simpra.Execute<bool, TestModel>(model, expression);
Console.WriteLine(result); // Output: true
```

### Safe Division with `when`

```csharp
const string expression = """
let amount = Transfer.Amount
let divisor = 0
let result = when divisor is not 0 then amount / divisor else 0 end
return result
""";

var result = simpra.Execute<int, TestModel>(model, expression);
Console.WriteLine(result); // Output: 0
```

### String Manipulation

```csharp
const string expression = """
let str = 'hello'
return Upper(str) + ' WORLD'
""";

var result = simpra.Execute<string, TestModel, TestFunctions>(model, new TestFunctions(), expression);
Console.WriteLine(result); // Output: "HELLO WORLD"
```

## Advanced Features

### Mutable vs Immutable Execution

By default, expressions in Simpra are immutable. However, you can enable mutability:

```csharp
const string expression = """
$mutable on
let x = 10
x = x + 5
return x
""";

var result = simpra.Execute<int, TestModel>(model, expression);
Console.WriteLine(result); // Output: 15
```

### Custom Functions

You can define custom functions and use them in expressions:

```csharp
public class CustomFunctions
{
    public static string Reverse(string input) => new string(input.Reverse().ToArray());
}

const string expression = "return Reverse('hello')";
var result = simpra.Execute<string, TestModel, CustomFunctions>(model, new CustomFunctions(), expression);
Console.WriteLine(result); // Output: "olleh"
```

### Async Method Execution

Simpra supports calling asynchronous methods within expressions:

```csharp
public class AsyncFunctions
{
    public async Task<int> GetExchangeRateAsync() => await Task.FromResult(3);
}

const string expression = "return GetExchangeRateAsync() * 10";
var result = await simpra.ExecuteAsync<int, TestModel, AsyncFunctions>(model, new AsyncFunctions(), expression);

Console.WriteLine(result); // Output: 30
```

### Mutability: Modifying Class Properties

Mutability can be enabled to allow modifying object properties:

```csharp
const string expression = @"
$mutable on
Transfer.Amount = Transfer.Amount + 50
return true
";

var model = new TestModel { Transfer = new Transfer { Amount = 100 } };
var result = simpra.Execute<bool, TestModel>(model, expression);

Console.WriteLine(result); // Output: true
Console.WriteLine(model.Transfer.Amount); // Output: 150
```


## Supported Syntax

### Operators
- Arithmetic: `+`, `-`, `*`, `/`, `%`
- Logical: `and`, `or`, `not`
- Comparison: `is`, `is not`, `<`, `<=`, `>`, `>=`, `==`, `!=`
- Membership: `in`, `not in`
- Pattern Matching: `matches`, `like`
- Null Handling: `has value`

### Control Structures
- `when` conditionals:
  ```csharp
  let x = 10
  let result = when x > 5 then 'High' else 'Low' end
  return result
  ```
- `if` statements:
  ```csharp
  let x = 20
  if x > 10 then return 'Greater' else return 'Smaller' end
  ```

### Lists
- Initialization: `let items = [1, 2, 3]`
- Access: `items[0]`
- Operations: `sum(items)`, `length(items)`, `items + [4,5]`, `items - [2]`

## License

**AltaSoft.Simpra** is licensed under the MIT License.

