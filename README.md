# paquery
Simple extension for your API to make requested(IQueryCollection) query string(parameters) compatible with any pagination.

# Setup
dotnet add package PaQuery

# Usage: Code Example 
```csharp
var pageInfo = Request.Query.GetPageInfo($"https://{Request.Host}{Request.Path}", totalPageCount, page);
var information = new { pageInfo.Next, pageInfo.Previous };
return Ok(new { information,  ....});
```
Look at [MoviesController.cs](https://github.com/halilkocaoz/paquery/blob/main/Example.WebAPI/Controllers/MoviesController.cs) for a detailed example.
# Status
No release. Development continues.
