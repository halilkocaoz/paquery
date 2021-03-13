# paquery
Simple extension for your API to make requested(IQueryCollection) query string(parameters) compatible with any pagination.

![api-response](https://github.com/halilkocaoz/paquery/blob/main/assets/api-response.gif "api-response")

## Usage: Code Example 
```csharp
var pageInfo = Request.Query.GetPageInfo($"https://{Request.Host}{Request.Path}", totalPageCount, page);
var information = new { pageInfo.Next, pageInfo.Previous };
return Ok(new { information,  ....});
```
Look at [MoviesController.cs](https://github.com/halilkocaoz/paquery/blob/main/Example.WebAPI/Controllers/MoviesController.cs) for a detailed example.

## Setup
~~dotnet add package PaQuery~~

## Status
No release. Development continues.
