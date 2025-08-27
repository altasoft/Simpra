# AltaSoft.Simpra
[![Version](https://img.shields.io/nuget/v/AltaSoft.Simpra?label=Version&color=0c3c60&style=for-the-badge&logo=nuget)](https://www.nuget.org/profiles/AltaSoft)
[![Dot NET 8+](https://img.shields.io/static/v1?label=DOTNET&message=8%2B&color=0c3c60&style=for-the-badge)](https://dotnet.microsoft.com)

**Simpra** is a powerful expression language designed for evaluating dynamic expressions in .NET applications. It enables developers to execute logic dynamically within their applications, similar to how scripting languages work but optimized for business rules.

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
- **Rich Syntax**: Conditionals, arithmetic, lists, pattern matching, regex, membership, null checks.
- **Mutability Control**: Toggle between immutable and mutable execution.
- **String & List Operations**: Manipulate and aggregate data easily.
- **Custom Functions**: Extend with user-defined or async functions.
- **Safe Evaluation**: No loops, recursion, or external resource access.

## Language Specifics

### Supported Operators

- **Arithmetic**  
  `+`, `-`, `*`, `/`, `//` (integer division), postfix `%` (percent), `min`, `max`  
  ```simpra
  10 // 3     # 3
  50%        # 0.5
  5 min 2    # 2
  ```

- **Comparison**  
  `is`, `is not`, `<`, `<=`, `>`, `>=`  
  ```simpra
  x is 10
  y is not null
  a < b and < c   # chained comparison
  ```

- **Logical**  
  `and`, `or`, `not`

- **Membership**  
  `in`, `not in`, plus `any in`, `all in`, `any not in`, `all not in`  
  ```simpra
  x in [1, 2, 3]
  all in [true, true, false]
  ```

- **Pattern Matching**  
  - `matches` (regex)  
  - `like` (SQL-like patterns)  
  ```simpra
  email matches "[a-z]+@[a-z]+\\.[a-z]+"
  name like "Jo%"
  ```

- **Null Handling**  
  `has value`  
  ```simpra
  nullable has value
  ```

---

### Control Flow

- **if … then … else … end**
- **when … then … else … end**
- **return**

```simpra
if x > 10 then
    return "big"
else
    return "small"
end

when color then
    "red"   -> "Stop"
    "green" -> "Go"
    else "Unknown"
end
```

---

### Directives

Simpra supports runtime directives that influence evaluation:

- **$mutable on/off** – Enables or disables mutability for **model properties** (local variables are always mutable).
- **$case_sensitive on/off** – Controls case sensitivity in string comparisons (`is`, `like`, `matches`).

Example:

```simpra
let str = 'abc'
$case_sensitive off
return str like 'A%'    # true
```

---

### Variables & Mutability

- **Local variables** declared with `let` are always mutable:
  ```simpra
  let x = 10
  x += 5    # 15
  x -= 2    # 13
  ```

- **Model properties can only be reassigned if mutability is enabled with $mutable on:**

  ```simpra
  $mutable on
  Transfer.Amount = Transfer.Amount + 50
  ```

- **By default, $mutable is off, meaning model properties are read-only.**
---

### Lists

```simpra
let items = [1, 2, 3]
items[0]         # 1
sum(items)       # 6
length(items)    # 3
```

---

### Functions & Access

- Function calls: `method("hi")`
- Properties: `obj.prop`
- Indexers: `list[1]`

---

### Chained Comparisons

Simpra supports chained comparison expressions without repeating the left-hand operand:

```simpra
let x = 5
return x > 0 and < 10    # true
```

This allows constructs like `a < b and < c` for concise range checks.

---

### Built-in Functions

Simpra provides a set of internal functions out-of-the-box:

- **Math**
  - `abs(x)` – absolute value
  - `round(x)` – round to nearest integer
  - `min(a, b, …)`, `max(a, b, …)` – smallest/largest value

- **Lists**
  - `sum(list)` – sum of values
  - `length(list)` – number of elements
  
- **Strings**
  - `number(str)` – converts to number
  - `date(str)` – converts to date
---

### Extended Membership Operators

In addition to `in` and `not in`, Simpra supports `any in`, `all in`, `any not in`, and `all not in`:

```simpra
let values = [1, 2, 3]

2 any in values       # true
[2, 4] all in values  # false
2 any not in values   # false
[4, 5] all not in values  # true
```

---

### Percent Operator

The `%` operator in Simpra is **postfix percent**, not modulus.

```simpra
50%     # 0.5
200%    # 2.0
```

This makes it easy to express percentages directly inside formulas.

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

### Arithmetic Operators (//, %, min, max)

```csharp
const string expression = """
let a = 10
let b = 3
let half = 50%
return [ a // b, half, a min b, a max b ]
""";

var result = simpra.Execute<List<decimal>, TestModel>(model, expression);
Console.WriteLine(string.Join(", ", result)); // Output: "3, 0.5, 3, 10"
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

## License

**AltaSoft.Simpra** is licensed under the MIT License.
