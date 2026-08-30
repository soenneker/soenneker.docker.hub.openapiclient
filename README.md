[![](https://img.shields.io/nuget/v/soenneker.docker.hub.openapiclient.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.docker.hub.openapiclient/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.docker.hub.openapiclient/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.docker.hub.openapiclient/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.docker.hub.openapiclient.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.docker.hub.openapiclient/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.docker.hub.openapiclient/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.docker.hub.openapiclient/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Docker.Hub.OpenApiClient

A Kiota-generated .NET client for the Docker Hub API.

## Installation

```bash
dotnet add package Soenneker.Docker.Hub.OpenApiClient
```

## Create a client

```csharp
using System.Net.Http.Headers;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Soenneker.Docker.Hub.OpenApiClient;

var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", accessToken);

var adapter = new HttpClientRequestAdapter(
    new AnonymousAuthenticationProvider(),
    httpClient: httpClient)
{
    BaseUrl = "https://hub.docker.com"
};

var client = new DockerHubOpenApiClient(adapter);
```

Keep the token outside source control and reuse the `HttpClient`, adapter, and generated client. The companion `Soenneker.Docker.Hub.OpenApiClientUtil` package supplies dependency-injection registration and cached construction.

## List personal access tokens

```csharp
using Soenneker.Docker.Hub.OpenApiClient.Models;

GetAccessTokensResponse? response = await client.V2.AccessTokens.GetAsync(
    request =>
    {
        request.QueryParameters.Page = 1;
        request.QueryParameters.PageSize = 25;
    },
    cancellationToken);

IReadOnlyList<GetAccessTokensResponseResultsItem> tokens = response?.Results ?? [];
```

Pagination is explicit. Use `Next`, `Previous`, and `Count` from the response to decide which page to request next; the client does not automatically enumerate all results.

## API shape and failures

Request builders follow the URL hierarchy, starting at `client.V2`. Collection indexers supply path parameters, such as an access-token UUID or organization name.

Kiota maps documented non-success responses to generated models deriving from `ApiException`. The exact error type depends on the endpoint—for example, listing access tokens maps `400` to `ValueError` and `401` to `Error`. Preserve those details when logging or translating failures, and let cancellation flow to the caller.

The client and models are generated. Avoid editing them directly because regeneration replaces those changes. Endpoint names, nullability, error mappings, and model shapes can change when Docker Hub’s specification changes; review package updates before upgrading production consumers.
