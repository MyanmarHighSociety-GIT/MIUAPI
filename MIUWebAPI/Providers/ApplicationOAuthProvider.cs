﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using MIUWebAPI.DBContext;
using MIUWebAPI.Models;

namespace MIUWebAPI.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            ApplicationUser user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
               OAuthDefaults.AuthenticationType);
            ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,
                CookieAuthenticationDefaults.AuthenticationType);

            AuthenticationProperties properties = CreateProperties(user);
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(ApplicationUser user)
        {
            using (MIUEntities db = new MIUEntities())
            {
                //string userLoginName = db.AspNetUsers.Where(a => a.Id == user.Id).Select(a => a.UserName).FirstOrDefault();
                string userLoginName = db.AspNetUsers.Where(a => a.UserName == user.UserName).Select(a => a.UserName).SingleOrDefault();
                User userInfo = db.Users.Where(a => a.LoginName == userLoginName).SingleOrDefault();
                BatchDetail batchDetail = db.BatchDetails.Where(b => b.StudentID == userInfo.ID).FirstOrDefault();

                IDictionary<string, string> data = new Dictionary<string, string>
                {
                    { "fullName",userInfo.FullName },
                    { "loginName",user.UserName },
                    { "email",user.Email },
                    { "role", userInfo.Role != null ? userInfo.Role : string.Empty },
                    //{ "role", userInfo.Role },
                    { "userID",userInfo.ID.ToString() },
                    { "batchID", batchDetail != null ? batchDetail.BatchID.ToString() : string.Empty},
                    { "batchCode", batchDetail != null ? batchDetail.Batch.BatchCode.ToString() : string.Empty }
                };
                return new AuthenticationProperties(data);
            }               
        }
    }
}