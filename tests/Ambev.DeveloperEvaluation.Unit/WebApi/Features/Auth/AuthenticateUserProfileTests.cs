using Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;
using AutoMapper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.WebApi.Features.Auth;

public class AuthenticateUserProfileTests
{
    private readonly IMapper _mapper;

    public AuthenticateUserProfileTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature.AuthenticateUserProfile>();
        });
        _mapper = configuration.CreateMapper();
    }

    [Fact]
    public void Configuration_ShouldBeValid()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature.AuthenticateUserProfile>();
        });
        
        configuration.AssertConfigurationIsValid();
    }

    [Fact]
    public void Should_Map_Request_To_Command()
    {
        var request = new AuthenticateUserRequest
        {
            Email = "test@example.com",
            Password = "password"
        };

        var command = _mapper.Map<AuthenticateUserCommand>(request);

        Assert.Equal(request.Email, command.Email);
        Assert.Equal(request.Password, command.Password);
    }
}