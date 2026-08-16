using Soenneker.Tests.HostedUnit;

namespace Soenneker.Docker.Hub.OpenApiClient.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class DockerHubOpenApiClientTests : HostedUnitTest
{
    public DockerHubOpenApiClientTests(Host host) : base(host)
    {
    }

    [Test]
    public void Default()
    {

    }
}
