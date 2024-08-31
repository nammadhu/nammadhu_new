﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

#nullable disable
public class CardTypeConfiguration : IEntityTypeConfiguration<CardType>
{
    public void Configure(EntityTypeBuilder<CardType> builder)
    {
        builder.Property(t => t.Name).HasMaxLength(50).IsRequired();
        builder.Ignore(e => e.DomainEvents);
    }
}

