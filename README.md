# Technical Test for Credit Suisse

### Getting started

clone or download the repo and
```
dotnet test
```

```
cd CreditSuisse.VirtualCashCard.Console
dotnet run
```

```
cd CreditSuisse.VirtualCashCard.Console
docker build .
docker run creditsuissevirtualcashcardconsole:dev
```

### Approach

Since a virtual cash card was effectively a bank account I approached it as such - though I didn't want to go down the whole double entry ledger system.  I use a very striped down implementation of CRQS with commands (no read side) and an in-memory Event Stream.  This approach should guarantee a cash card can't be overdrawn even if it is used simultaneously in two places.

There are probably more classes than desired by the specification below; however, a) most of them are simple POCOs and b) I feel it helps code readability. It does follow the principle of 'Write the code as you would write a part of a production grade system.'

### Technology

.NET Core 2.2, Visual Studio 2019, Microsoft Dependency Injection, a very light CQRS/ES implementation, xUnit, xBehavior, Moq, FluentAssertions and last but not least a little Docker for fun

### If I had more time

Definiteyly would add more tests - especially for the Console and Infrastructure side of it. 

I would also implement the R in CQRS.

## Specification
### Intro:
This is meant to be a very simple ~ 30 min exercise. If you find yourself creating more than 10 classes or spending more than two hours on it please assume that you're trying to do too much. There are no hidden traps, we’d just like to get a very small sample of your coding style. There will be a pair programming interview in a later round to really shine. 
### Task:
Implement a very basic virtual cash card in C#. 
### Requirements:
1.	Can withdraw money if a valid PIN is supplied. The balance on the card needs to adjust accordingly.
2.	Can be topped up any time by an arbitrary amount.
3.	The cash card, being virtual, can be used in many places at the same time.

### Principles:
1.	Well tested code (test driven would be best).
2.	Write the code as you would write a part of a production grade system.
3.	Requirements must be met but please don’t go overboard.

### Submission:
Please upload to a publicly accessible repository; own server, Github or ask agency to pass to us if they provide that facility. 

