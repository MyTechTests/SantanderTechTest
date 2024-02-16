using BestOfHackerNews.Core.Implementations;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NSubstitute;

namespace BestOfHackerNews.Core.Tests.Implementations;

[TestClass]
public class ApiKeyValidatorTests
{
    [TestMethod]
    public void Validate_Should_Return_Unauthorized_When_Null_ApiKey_Provided()
    {
        // Arrange
        var configuration = Substitute.For<IConfiguration>();
        var apiKeyValidator = new ApiKeyValidator(configuration);

        // Act
        var (success, httpStatusCode, httpStatusMessage) = apiKeyValidator.Validate(null);

        // Assert
        success.Should().BeFalse();
        httpStatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        httpStatusMessage.Should().Be("An API key is required, but was not supplied");
    }

    [TestMethod]
    public void Validate_Should_Return_Unauthorized_When_Empty_ApiKey_Provided()
    {
        // Arrange
        var configuration = Substitute.For<IConfiguration>();
        var apiKeyValidator = new ApiKeyValidator(configuration);

        // Act
        var (success, httpStatusCode, httpStatusMessage) = apiKeyValidator.Validate("");

        // Assert
        success.Should().BeFalse();
        httpStatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        httpStatusMessage.Should().Be("An API key is required, but was not supplied");
    }

    [TestMethod]
    public void Validate_Should_Return_Unauthorized_When_Invalid_ApiKey_Provided()
    {
        // Arrange
        const string apiKey = "invalid-api-key";
        var configuration = Substitute.For<IConfiguration>();
        configuration.GetSection("ApiKeys").GetChildren()
            .Returns(new List<IConfigurationSection>());
        var apiKeyValidator = new ApiKeyValidator(configuration);

        // Act
        var (success, httpStatusCode, httpStatusMessage) = apiKeyValidator.Validate(apiKey);

        // Assert
        success.Should().BeFalse();
        httpStatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        httpStatusMessage.Should().Be("The supplied API key is invalid");
    }

    [TestMethod]
    public void Validate_Should_Return_Success_When_Valid_ApiKey_Provided()
    {
        // Arrange
        const string apiKey = "valid-api-key";
        var configuration = Substitute.For<IConfiguration>();
        var apiKeySection = Substitute.For<IConfigurationSection>();
        apiKeySection.Key.Returns(apiKey);
        configuration.GetSection("ApiKeys").GetChildren()
            .Returns(new List<IConfigurationSection> { apiKeySection }); // Mock valid API key
        var apiKeyValidator = new ApiKeyValidator(configuration);

        // Act
        var (success, httpStatusCode, httpStatusMessage) = apiKeyValidator.Validate(apiKey);

        // Assert
        success.Should().BeTrue();
        httpStatusCode.Should().Be(StatusCodes.Status200OK);
        httpStatusMessage.Should().Be("Successfully validated the supplied api key");
    }
}