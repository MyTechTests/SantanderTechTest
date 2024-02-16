using BestOfHackerNews.Core.Extensions;
using BestOfHackerNews.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace BestOfHackerNews.Core.Tests.Extensions;


[TestClass]
public class BohnApplicationBuilderExtensionsTests
{
    [TestMethod]
    public async Task Middleware_Should_Set_Status_Code_And_Write_Message_If_Validation_Fails()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var services = Substitute.For<IServiceProvider>();
        var apiKeyValidator = Substitute.For<IApiKeyValidator>();

        context.Request.Headers["X-API-KEY"] = "invalid-api-key";
        services.GetService(typeof(IApiKeyValidator)).Returns(apiKeyValidator);
        context.RequestServices = services;
        apiKeyValidator.Validate("invalid-api-key").Returns((false, 403, "Forbidden"));

        // Act
        await BohnApplicationBuilderExtensions.CheckApiKey(context, () => Task.CompletedTask);

        // Assert
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
    }

    [TestMethod]
    public async Task Middleware_Should_Invoke_Next_If_Validation_Passes()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var services = Substitute.For<IServiceProvider>();
        var apiKeyValidator = Substitute.For<IApiKeyValidator>();

        context.Request.Headers["X-API-KEY"] = "valid-api-key";
        services.GetService(typeof(IApiKeyValidator)).Returns(apiKeyValidator);
        context.RequestServices = services;
        apiKeyValidator.Validate("valid-api-key").Returns((true, 0, ""));

        // Act
        await BohnApplicationBuilderExtensions.CheckApiKey(context, () => Task.CompletedTask);

        // Assert
        context.Response.StatusCode = StatusCodes.Status200OK;
    }
}