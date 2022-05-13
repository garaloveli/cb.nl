dotnet restore 
dotnet build
write-host "Execute unit test"
dotnet test --filter FullyQualifiedName~UnitTest --no-build
write-host "Execute virtual test"
dotnet test --filter FullyQualifiedName~VirtualTest --no-build

$product_api = Start-Job -ScriptBlock { dotnet ./ProductData.Api/ProductData.Api.dll } # http://localhost:5002/swagger/index.html
$insurance_api = Start-Job -ScriptBlock { dotnet run --project ./src/Insurance.Api/Insurance.Api.csproj } # http://localhost:5001

write-host "Execute integration test"
dotnet test --filter FullyQualifiedName~IntegrationTest --no-build

write-host "Execute Functional test"
dotnet test --filter FullyQualifiedName~FunctionalTest --no-build

Read-Host -Prompt "Enter to clean hosts"

Stop-Job $product_api
Stop-Job $insurance_api