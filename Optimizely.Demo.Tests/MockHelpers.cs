using HttpContextMoq;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Optimizely.Demo.Tests;

internal static class MockHelpers
{
    private static readonly Lazy<HttpContextMock> _httpContextMock = new(CreateHttpContextMock());
    public static HttpContextMock HttpContextMock => _httpContextMock.Value;

    public static Mock<IHttpContextAccessor> CreateHttpContextAccessorMock()
    {
        var mock = new Mock<IHttpContextAccessor>();

        mock.Setup(x => x.HttpContext)
            .Returns(_httpContextMock.Value);

        return mock;
    }

    private static HttpContextMock CreateHttpContextMock()
    {
        var mock = new HttpContextMock();
        //	.SetupRequestContentType("application/json")
        //	.SetupRequestHeaders(new Dictionary<string, StringValues>
        //	{
        //			{
        //				"Authorization",
        //				new StringValues("Bearer eyJhbGciOiJSUzI1NiIsImtpZCI6IkZBOTEyRUI1RjlERkJDRTVCODQzMENEMkYwQTdCQ0QzIiwidHlwIjoiSldUIn0.eyJuYmYiOjE2NjM1ODkwMTEsImV4cCI6MTY2MzU5MjYxMSwiaXNzIjoiaHR0cHM6Ly9hdXRoLWFwaS10ZXN0LmNvbnN0cnVvLm5vIiwiYXVkIjpbIklkZW50aXR5U2VydmVyQXBpIiwicHJkdC1LeWNBcGkiLCJodHRwczovL2F1dGgtYXBpLXRlc3QuY29uc3RydW8ubm8vcmVzb3VyY2VzIl0sImNsaWVudF9pZCI6InByZHQtQXV0aEFwaSIsImlhdCI6MTY2MzU4OTAxMSwic2NvcGUiOlsiSWRlbnRpdHlTZXJ2ZXJBcGkiLCJLeWNBcGkiXX0.j9-293CFaofDFueStY38zFZLjyWBEKiOA7C2j54P9KVbfX4ENnQwAAZXtyCjro7r5CBaxZOcBfmwQlSh2fFs-N2WkSp9Ke-oLNWcpNAoPqJb_n1Cjm9SOPToofJsW55UUIJk6irs6P2LcZGhFsnJfS2dgpKwC9-99o_GgJzyu9CnTbUjm8T2jjXkivDTO31Hu8ZtiwVTt_LnQlPwTy74ePuoFtR3EMA4tJq2djV7AZZpKDsZsnJHsh5aKxbY_Ge8k8a-cLI98dqb9jr0YG56iPk0FKL5CJ1HnFNsjIp8uFVwLNuPIMj4DWy5vvH6exdlYPfi3oLFgYzPAM4sptCorQ")
        //			}
        //	});

        //mock.UserMock = new ClaimsPrincipalMock
        //{
        //	IdentityMock = new ClaimsIdentityMock()
        //};

        return mock;
    }
}
