using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Specifications;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser
{
    /// <summary>
    /// Handler responsible for authenticating a user.
    /// </summary>
    public class AuthenticateUserHandler : IRequestHandler<AuthenticateUserCommand, AuthenticateUserResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticateUserHandler"/> class.
        /// </summary>
        /// <param name="userRepository">User repository.</param>
        /// <param name="passwordHasher">Service for password hashing and verification.</param>
        /// <param name="jwtTokenGenerator">Service for generating JWT tokens.</param>
        public AuthenticateUserHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        
        /// <summary>
        /// Handles the user authentication command.
        /// </summary>
        /// <param name="request">The command containing the email and password for authentication.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>An <see cref="AuthenticateUserResult"/> containing authentication data.</returns>
        /// <exception cref="UnauthorizedAccessException">
        /// Thrown when the credentials are invalid or the user is not active.
        /// </exception>
        public async Task<AuthenticateUserResult> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.Password))
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var activeUserSpec = new ActiveUserSpecification();
            if (!activeUserSpec.IsSatisfiedBy(user))
            {
                throw new UnauthorizedAccessException("User is not active");
            }

            var token = _jwtTokenGenerator.GenerateToken(user);
            return new AuthenticateUserResult
            {
                Token = token,
                Email = user.Email,
                Name = user.Username,
                Role = user.Role.ToString()
            };
        }
    }
}