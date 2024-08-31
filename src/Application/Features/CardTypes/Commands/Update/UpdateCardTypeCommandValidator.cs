﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.CardTypes.Commands.Update;

public class UpdateCardTypeCommandValidator : AbstractValidator<UpdateCardTypeCommand>
{
        public UpdateCardTypeCommandValidator()
        {
           RuleFor(v => v.Id).NotNull();
           RuleFor(v => v.Name).MaximumLength(256).NotEmpty();
          
        }
    
}

