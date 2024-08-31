﻿using PublicCommon.Common;
using CleanArchitecture.Blazor.Domain.Common.Entities;
using PublicCommon;
using System.ComponentModel.DataAnnotations;

namespace MyTown.Domain;

public class CardTrash : BaseAuditableEntity
    {
    //non auto generated key
    [Key]
    public override int Id { get; set; }
    public required string CardDataAsJsonString { get; set; }
    }

