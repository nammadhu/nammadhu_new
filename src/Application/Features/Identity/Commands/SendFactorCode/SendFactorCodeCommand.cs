﻿using System.Text;
using System.Web;
using CleanArchitecture.Blazor.Application.Common.Interfaces;
using CleanArchitecture.Blazor.Domain.Identity;
using DocumentFormat.OpenXml.Wordprocessing;
using FluentEmail.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CleanArchitecture.Blazor.Application.Features.Identity.Commands.SendWelcome;

public record SendFactorCodeCommand(string Email) : IRequest<Result>;

public class SendFactorCodeCommandHandler : IRequestHandler<SendFactorCodeCommand, Result>
{
    private readonly IStringLocalizer<SendFactorCodeCommandHandler> _localizer;
    private readonly IMailService _mailService;
    private readonly IApplicationSettings _settings;
    private readonly UserManager<ApplicationUser> _userManager;

    public SendFactorCodeCommandHandler(UserManager<ApplicationUser> userManager,
        IStringLocalizer<SendFactorCodeCommandHandler> localizer,
        IMailService mailService,
        IApplicationSettings settings)
    {
        _userManager = userManager;
        _localizer = localizer;
        _mailService = mailService;
        _settings = settings;
    }


    public async Task<Result> Handle(SendFactorCodeCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null) return Result.Failure(_localizer["No user found by email, please contact the administrator"]);
        var providers = await _userManager.GetValidTwoFactorProvidersAsync(user);
        if (!providers.Contains("Email"))
        {
            return Result.Failure(_localizer["Email-based two-factor authentication is not enabled for this account. Please choose an available two-factor authentication method."]);
        }
        var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
        var subject = _localizer["Your Verification Code"];

        var sendMailResult = await _mailService.SendAsync(
            request.Email,
            subject,
            "_authenticatorcode",
            new { AuthenticatorCode= token, _settings.AppName, user.Email, user.UserName, _settings.Company });

        return sendMailResult.Successful
            ? Result.Success()
            : Result.Failure(string.Format(_localizer["{0}, please contact the administrator"],
                sendMailResult.ErrorMessages.FirstOrDefault()));
    }
}