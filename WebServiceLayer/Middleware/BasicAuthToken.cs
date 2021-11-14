using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using DataAccessLayer;
using DataAccessLayer.Domain;
using DataAccessLayer.Repository;
using DataAccessLayer.Repository.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;


namespace WebServiceLayer.Middleware
{
    public static class JwtAuthMiddlewareExtension
    {
        public static IApplicationBuilder UseJwtAuth(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtAuthMiddleware>();
        }
    }

    public class JwtAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public JwtAuthMiddleware(RequestDelegate next, IUserRepository userRepository, IConfiguration configuration)
        {
            _next = next;
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (token == null)
                {
                    throw new InvalidOperationException("Authorization token not found in request");
                }
                byte[] credentialBytes = Convert.FromBase64String(token);
                string[] credentials = Encoding.ASCII.GetString(credentialBytes).Split(":");

                var user = _userRepository.LoginUser(credentials[0].TrimEnd(), credentials[1].TrimEnd());
                if (user == null)
                {
                    context.Response.StatusCode = (int)(HttpStatusCode.Unauthorized);
                    context.Response.WriteAsync("Authentication Failed");
                }
                else
                {
                    context.Items["User"] = user;
                }
            }
            catch (Exception e)
            {
                context.Response.WriteAsync(e.ToString());
            }

            await _next(context);
        }
    }
}