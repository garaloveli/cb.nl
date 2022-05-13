dotnet restore 
dotnet build 
Remove-Item –path ./reports/*  –recurse
coverlet ./tests/Insurance.Tests/bin/Debug/net6.0/Insurance.Api.dll --target "dotnet" --targetargs "test ./tests/Insurance.Tests --filter FullyQualifiedName~VirtualTest|FullyQualifiedName~UnitTest --no-build" -f opencover --include-test-assembly  --output ./reports/coverage.xml 
reportgenerator -reports:"./reports/coverage.xml" -targetdir:"reports" -reporttypes:Html