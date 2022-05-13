# Software Development Assignment

Language: C# .Net Core

## Build

If when trying to compile there are some compilation issues, please check this [link](https://aka.ms/dotnet-core-applaunch?framework=Microsoft.NETCore.App&framework_version=6.0.5&arch=x64&rid=win10-x64)

## Coverage

### Prerequisites
Coverage execution uses coverlet, to run the script for coverage just install it globally with this commands:
```
dotnet tool install --global coverlet.console
dotnet tool install --global dotnet-reportgenerator-globaltool
```

There is a PowerShell file to Check how much of the percentage of code is covered by tests, called `.\coverage.ps1`.
Execute it in a PoweShell terminal, when finishing it will show the percentages in the console:
```
Calculating coverage result...
  Generating report '.\reports\coverage.xml'
+-----------------+--------+--------+--------+
| Module          | Line   | Branch | Method |
+-----------------+--------+--------+--------+
| Insurance.Api   | 58.74% | 58.75% | 72.85% |
+-----------------+--------+--------+--------+
| Insurance.Tests | 73.01% | 31.81% | 69.76% |
+-----------------+--------+--------+--------+

+---------+--------+--------+--------+
|         | Line   | Branch | Method |
+---------+--------+--------+--------+
| Total   | 67.59% | 52.94% | 71.68% |
+---------+--------+--------+--------+
| Average | 65.87% | 45.28% | 71.3%  |
+---------+--------+--------+--------+
```
Also the visual version is contained in `reports\index.html`.

## Tests

To execute all tests, execute `.\functionaltesting.ps1`.

```
Microsoft (R) Test Execution Command Line Tool Version 17.1.0
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...
A total of 1 test files matched the specified pattern.

Passed!  - Failed:     0, Passed:     6, Skipped:     0, Total:     6, Duration: 70 ms - Insurance.Tests.dll (net6.0)
Enter to clean hosts:
```

It is important to press `ENTER` after finishing execution to kill all processes.

