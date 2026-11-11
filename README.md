# AltaSoft.Simpra
[![Version](https://img.shields.io/nuget/v/AltaSoft.Simpra?label=Version&color=0c3c60&style=for-the-badge&logo=nuget)](https://www.nuget.org/profiles/AltaSoft)
[![Dot NET 10+](https://img.shields.io/static/v1?label=DOTNET&message=10%2B&color=0c3c60&style=for-the-badge)](https://dotnet.microsoft.com)

**Simpra** is a powerful expression language designed for evaluating dynamic expressions in .NET applications. It enables developers to execute logic dynamically within their applications, similar to how scripting languages work but optimized for business rules.

## Installation

To install **AltaSoft.Simpra**, use NuGet:

```sh
dotnet add package AltaSoft.Simpra
```

Or in your `.csproj` (see the NuGet badge above for the latest version):

```xml
<PackageReference Include="AltaSoft.Simpra" Version="2.1.5" />
```

## Features

- **Dynamic Expression Execution**: Evaluate expressions at runtime.
- **Rich Syntax**: Conditionals, arithmetic, lists, pattern matching, regex, membership, null checks.
- **Mutability Control**: Toggle between immutable and mutable execution.
- **String & List Operations**: Manipulate and aggregate data easily.
- **Custom Functions**: Extend with user-defined or async functions.
- **Safe Evaluation**: No loops, recursion, or external resource access.

## Language Specifics

### Literals & Comments

- **Numbers**: `10`, `2.5`
- **Booleans**: `true`, `false`
- **Strings**: `'single'` or `"double"` quoted. There are **no escape sequences**: a string cannot contain a backslash (`\`) or its own quote character.
- **Lists**: `[1, 2, 3]`, `['USD', 'EUR']`
- **Comments**: `# line comment` and `/* block comment */`
- There is **no `null` literal** — use `has value` to check for missing values.

### Supported Operators

- **Arithmetic**
  `+`, `-`, `*`, `/`, `//` (integer division), postfix `%` (percent), `min`, `max`
  ```simpra
  10 // 3     # 3
  50%         # 0.5
  5 min 2     # 2
  5 max 2     # 5
  ```

- **Comparison**
  `is`, `is not`, `<`, `<=`, `>`, `>=`
  ```simpra
  x is 10
  currency is not 'USD'
  a < b and < c   # chained comparison
  ```

- **Logical**
  `and`, `or`, `not`

- **Membership**
  `in`, `not in` test a single value against a list; `any in`, `all in`, `any not in`, `all not in` compare a list against a list (see [Extended Membership Operators](#extended-membership-operators)).
  ```simpra
  x in [1, 2, 3]
  x not in [1, 2, 3]
  ```

- **Pattern Matching**
  - `matches` (regular expression — matches anywhere in the string; use `^…$` to match the whole string)
  - `like` (SQL-like patterns: `%` matches any sequence, `_` matches a single character)
  ```simpra
  email matches '^[a-z]+@[a-z]+[.][a-z]+$'
  name like 'Jo%'
  ```
  Because strings cannot contain a backslash, use character classes instead of escapes in regular expressions (`[.]` instead of `\.`, `[0-9]` instead of `\d`).

- **Null Handling**
  `has value`
  ```simpra
  NullableAmount has value
  ```

---

### Control Flow

- **if … then … else if … else … end** (statement)
- **when … then … when … then … else … end** (expression; `else` is required)
- **return**

```simpra
if x > 10 then
    return 'big'
else if x > 5 then
    return 'medium'
else
    return 'small'
end
```

```simpra
let color = 'red'
return when color is 'red' then 'Stop'
       when color is 'green' then 'Go'
       else 'Unknown'
       end
```

---

### Directives

Simpra supports directives that influence evaluation of the code that follows them:

- **$mutable on/off** – Enables or disables assignment to **model properties** (local variables are always mutable). `$mutable on` is only allowed when `SimpraCompilerOptions.MutabilityOption` is `DefaultImmutable` or `DefaultMutable`; with the default `Immutable` option it throws a `SimpraException`. See [Compiler Options](#compiler-options).
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
  x += 5      # 15
  x -= 2      # 13
  x = x * 2   # 26
  ```

- **Model properties are read-only by default.** To assign them, enable mutability in the compiler options and (unless the option is `DefaultMutable`) turn it on with `$mutable on`:

  ```simpra
  $mutable on
  Transfer.Amount = Transfer.Amount + 50
  ```

---

### Lists

List indexes are **1-based**:

```simpra
let items = [10, 20, 30]
items[1]         # 10
items[3]         # 30
sum(items)       # 60
length(items)    # 3
```

An out-of-range index does not throw — for the list above, `items[0]` and `items[4]` return `0`.

---

### Functions & Access

- Function calls: `method('hi')`
- Properties: `obj.prop`
- Indexers: `list[1]`, `dictionary['key']`

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
  - `round(x)` – round to the nearest integer
  - `round(x, decimals)` – round to the given number of decimal places
  - For the smallest/largest of two values use the `min` / `max` **operators** (`a min b`, `a max b`).

- **Lists**
  - `sum(list)` – sum of values
  - `length(list)` – number of elements

- **Strings**
  - `length(str)` – number of characters
  - `substring(str, index)` – the character at a **1-based** position
  - `substring(str, index, count)` – `count` characters starting at a **1-based** position (`substring('hello', 2, 3)` is `'ell'`)

- **Conversion**
  - `number(str)` – converts a string (or boolean) to a number
  - `date(str)` – converts a string to a date
  - `string(x)` – converts a number, boolean or date to a string

---

### Extended Membership Operators

In addition to `in` and `not in`, Simpra supports `any in`, `all in`, `any not in`, and `all not in`. Both sides must be lists. Each `not` form is the negation of its positive form:

| Operator | Meaning |
|---|---|
| `a any in b` | at least one element of `a` is in `b` |
| `a all in b` | every element of `a` is in `b` |
| `a any not in b` | `not (a any in b)` — **no** element of `a` is in `b` |
| `a all not in b` | `not (a all in b)` — at least one element of `a` is not in `b` |

```simpra
let values = [1, 2, 3]

[2, 4] any in values       # true  (2 is in values)
[2, 4] all in values       # false (4 is not in values)
[4, 5] any not in values   # true  (neither 4 nor 5 is in values)
[2, 4] any not in values   # false (2 is in values)
[2, 4] all not in values   # true  (4 is not in values)
[1, 2] all not in values   # false (both are in values)
```

For a single value use `in` / `not in`: `2 in values`.

---

### Percent Operator

The `%` operator in Simpra is **postfix percent**, not modulus.

```simpra
50%     # 0.5
200%    # 2
```

This makes it easy to express percentages directly inside formulas.

---

### Reserved Words

The following words are keywords and cannot be used as variable names:
`let`, `if`, `then`, `else`, `end`, `when`, `return`, `true`, `false`, `is`, `not`, `in`, `and`, `or`, `min`, `max`, `matches`, `like`, `has`, `value`, `on`, `off`.

## Usage

The examples below use this model and functions class:

```csharp
public class Transfer
{
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "";
}

public class TestModel
{
    public Transfer Transfer { get; set; } = new();
}

public class TestFunctions
{
    public string Upper(string str) => str.ToUpperInvariant();
}

var simpra = new Simpra();
var model = new TestModel { Transfer = new Transfer { Amount = 50, Currency = "USD" } };
```

The `compilerOptions` argument is passed as `null` (defaults) unless an example needs specific options — see [Compiler Options](#compiler-options).

### Evaluating Simple Expressions

```csharp
const string expression = "return Transfer.Amount * 2";
var result = simpra.Execute<int, TestModel>(model, expression, null);

Console.WriteLine(result); // Output: 100
```

### Conditionals & Logical Operators

```csharp
const string expression = """
let transfer = Transfer
return transfer.Amount > 100 and transfer.Currency is 'USD'
""";

var result = simpra.Execute<bool, TestModel>(model, expression, null);
Console.WriteLine(result); // Output: False
```

### Handling Lists & Aggregations

```csharp
const string expression = """
let values = [10, 20, 30]
return sum(values) / length(values)
""";

var result = simpra.Execute<decimal, TestModel>(model, expression, null);
Console.WriteLine(result); // Output: 20
```

### Using Regular Expressions

```csharp
const string expression = """
let code = 'abc123'
return code matches '^[a-zA-Z_][a-zA-Z_0-9]*$'
""";

var result = simpra.Execute<bool, TestModel>(model, expression, null);
Console.WriteLine(result); // Output: True
```

### Safe Division with `when`

```csharp
const string expression = """
let amount = Transfer.Amount
let divisor = 0
let result = when divisor is not 0 then amount / divisor else 0 end
return result
""";

var result = simpra.Execute<int, TestModel>(model, expression, null);
Console.WriteLine(result); // Output: 0
```

### String Manipulation

```csharp
const string expression = """
let str = 'hello'
return Upper(str) + ' WORLD'
""";

var result = simpra.Execute<string, TestModel, TestFunctions>(model, new TestFunctions(), expression);
Console.WriteLine(result); // Output: HELLO WORLD
```

### Arithmetic Operators (//, %, min, max)

```csharp
const string expression = """
let a = 10
let b = 3
let half = 50%
return [ a // b, half, a min b, a max b ]
""";

var result = simpra.Execute<List<decimal>, TestModel>(model, expression, null);
Console.WriteLine(string.Join(", ", result)); // Output: 3, 0.50, 3, 10
```

## Advanced Features

### Compiler Options

`SimpraCompilerOptions` controls how code is compiled. It is the `compilerOptions` argument of `Execute`, `ExecuteAsync`, `CompileCode` and `CompileAsyncCode`:

- The overloads **without** external functions (`Execute<TResult, TDataModel>`, `ExecuteAsync<TResult, TDataModel>`, `CompileCode<TResult, TDataModel>`, `CompileAsyncCode<TResult, TDataModel>`) require it — pass `null` to use the defaults, or an options instance. `ExecuteAsync<TResult, TDataModel>` also requires a `CancellationToken`.
- `Execute` / `ExecuteAsync` **with** external functions take it as an optional argument.

```csharp
// defaults (case-sensitive, immutable)
var result = simpra.Execute<bool, TestModel>(model, expression, null);

// custom options
var options = new SimpraCompilerOptions { StringComparisonOption = StringComparisonOption.IgnoreCase };
var result = simpra.Execute<bool, TestModel>(model, expression, options);
```

| Option | Values | Default |
|---|---|---|
| `MutabilityOption` | `Immutable` – model properties can never be assigned; `$mutable on` throws<br>`DefaultImmutable` – read-only until `$mutable on`<br>`DefaultMutable` – assignable unless `$mutable off` | `Immutable` |
| `StringComparisonOption` | `CaseSensitive`, `IgnoreCase` (can be changed in code with `$case_sensitive`) | `CaseSensitive` |

### Mutability: Modifying Model Properties

```csharp
const string expression = """
$mutable on
Transfer.Amount = Transfer.Amount + 50
return true
""";

var options = new SimpraCompilerOptions { MutabilityOption = MutabilityOption.DefaultImmutable };
var model = new TestModel { Transfer = new Transfer { Amount = 100 } };
var result = simpra.Execute<bool, TestModel>(model, expression, options);

Console.WriteLine(result); // Output: True
Console.WriteLine(model.Transfer.Amount); // Output: 150
```

Without `MutabilityOption.DefaultImmutable` (or `DefaultMutable`), the `$mutable on` line throws a `SimpraException`.

### Custom Functions

You can define custom functions (instance or static) and use them in expressions:

```csharp
public class CustomFunctions
{
    public static string Reverse(string input) => new string(input.Reverse().ToArray());
}

const string expression = "return Reverse('hello')";
var result = simpra.Execute<string, TestModel, CustomFunctions>(model, new CustomFunctions(), expression);
Console.WriteLine(result); // Output: olleh
```

### Async Method Execution

Simpra supports calling asynchronous methods (`Task<T>` / `ValueTask<T>`) within expressions. The `Async` suffix may be omitted, and a trailing `CancellationToken` parameter is supplied automatically:

```csharp
public class AsyncFunctions
{
    public Task<int> GetExchangeRateAsync() => Task.FromResult(3);
}

const string expression = "return GetExchangeRate() * 10";
var result = await simpra.ExecuteAsync<int, TestModel, AsyncFunctions>(model, new AsyncFunctions(), expression);

Console.WriteLine(result); // Output: 30
```

### Compiling Once, Executing Many Times

`Execute` and `ExecuteAsync` parse and compile the code on every call. To evaluate the same code repeatedly, compile it once and reuse the delegate:

```csharp
var isLargeTransfer = simpra.CompileCode<bool, TestModel>("return Transfer.Amount > 1000", null);

var result = isLargeTransfer(model);
Console.WriteLine(result); // Output: False
```

## License

**AltaSoft.Simpra** is licensed under the MIT License.
