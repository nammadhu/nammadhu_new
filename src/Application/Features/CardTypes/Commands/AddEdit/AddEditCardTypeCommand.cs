﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.CardTypes.DTOs;
using CleanArchitecture.Blazor.Application.Features.CardTypes.Caching;
namespace CleanArchitecture.Blazor.Application.Features.CardTypes.Commands.AddEdit;

public class AddEditCardTypeCommand: ICacheInvalidatorRequest<Result<int>>
{
      [Description("Id")]
      public int Id { get; set; }
          [Description("Name")]
    public string Name {get;set;} = String.Empty; 
    [Description("Short Name")]
    public string? ShortName {get;set;} 
    [Description("Description")]
    public string? Description {get;set;} 
    [Description("Price")]
    public int Price {get;set;} 
    [Description("Priority Order")]
    public byte? PriorityOrder {get;set;} 
    [Description("Required Approval Count")]
    public byte? RequiredApprovalCount {get;set;} 


      public string CacheKey => CardTypeCacheKey.GetAllCacheKey;
      public CancellationTokenSource? SharedExpiryTokenSource => CardTypeCacheKey.GetOrCreateTokenSource();

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CardTypeDto,AddEditCardTypeCommand>(MemberList.None);
            CreateMap<AddEditCardTypeCommand,CardType>(MemberList.None);
         
        }
    }
}

    public class AddEditCardTypeCommandHandler : IRequestHandler<AddEditCardTypeCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditCardTypeCommandHandler> _localizer;
        public AddEditCardTypeCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<AddEditCardTypeCommandHandler> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<int>> Handle(AddEditCardTypeCommand request, CancellationToken cancellationToken)
        {
            if (request.Id > 0)
            {
                var item = await _context.CardTypes.FindAsync(new object[] { request.Id }, cancellationToken) ?? throw new NotFoundException($"CardType with id: [{request.Id}] not found.");
                item = _mapper.Map(request, item);
				// raise a update domain event
				item.AddDomainEvent(new CardTypeUpdatedEvent(item));
                await _context.SaveChangesAsync(cancellationToken);
                return await Result<int>.SuccessAsync(item.Id);
            }
            else
            {
                var item = _mapper.Map<CardType>(request);
                // raise a create domain event
				item.AddDomainEvent(new CardTypeCreatedEvent(item));
                _context.CardTypes.Add(item);
                await _context.SaveChangesAsync(cancellationToken);
                return await Result<int>.SuccessAsync(item.Id);
            }
           
        }
    }

