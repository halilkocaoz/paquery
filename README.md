# paquery

Simple extension for your API to make requested(IQueryCollection) query string(parameters) compatible with any pagination.

![api-response](https://github.com/halilkocaoz/paquery/blob/main/assets/api-response.gif "api-response")

## Usage: Code Example

```csharp
// * Example
var paginationUrl = Request.CreatePaginationUrl(totalPageCount: totalPageCount, currentPage: page);

var pagination = new
{
    paginationUrl.Next,
    paginationUrl.Previous,
    ..
    ...
};

return Ok(new { pagination, movies });
```

Look at [MoviesController.cs](https://github.com/halilkocaoz/paquery/blob/main/Example.WebAPI/Controllers/MoviesController.cs) for example.
