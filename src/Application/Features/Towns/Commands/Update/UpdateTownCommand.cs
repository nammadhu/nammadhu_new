﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System.ComponentModel;
using CleanArchitecture.Blazor.Application.Features.Towns.DTOs;
using CleanArchitecture.Blazor.Application.Features.Towns.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Towns.Commands.Update;

public class UpdateTownCommand: ICacheInvalidatorRequest<Result<int>>
{
      [Description("Id")]
      public int Id { get; set; }
            [Description("District")]
    public string? District {get;set;} 
    [Description("State")]
    public string? State {get;set;} 
    [Description("Url Name 1")]
    public string? UrlName1 {get;set;} 
    [Description("Url Name 2")]
    public string? UrlName2 {get;set;} 
    [Description("Type Of Profile Id")]
    public int TypeOfProfileId {get;set;} 
    [Description("Active")]
    public bool Active {get;set;} 
    [Description("Name")]
    public string Name {get;set;} = String.Empty; 
    [Description("Sub Title")]
    public string? SubTitle {get;set;} 
    [Description("Description")]
    public string? Description {get;set;} 
    [Description("Image Url")]
    public string? ImageUrl {get;set;} 
    [Description("Address")]
    public string? Address {get;set;} 
    [Description("Mobile Number")]
    public string? MobileNumber {get;set;} 
    [Description("Google Map Address Url")]
    public string? GoogleMapAddressUrl {get;set;} 
    [Description("End Date To Show")]
    public DateTime EndDateToShow {get;set;} 
    [Description("Priotiry Order")]
    public int? PriotiryOrder {get;set;} 
    [Description("Google Profile Url")]
    public string? GoogleProfileUrl {get;set;} 
    [Description("Face Book Url")]
    public string? FaceBookUrl {get;set;} 
    [Description("You Tube Url")]
    public string? YouTubeUrl {get;set;} 
    [Description("Instagram Url")]
    public string? InstagramUrl {get;set;} 
    [Description("Twitter Url")]
    public string? TwitterUrl {get;set;} 
    [Description("Other Reference Url")]
    public string? OtherReferenceUrl {get;set;} 

        public string CacheKey => TownCacheKey.GetAllCacheKey;
        public CancellationTokenSource? SharedExpiryTokenSource => TownCacheKey.SharedExpiryTokenSource();
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<TownDto,UpdateTownCommand>(MemberList.None);
            CreateMap<UpdateTownCommand,Town>(MemberList.None);
        }
    }
}

    public class UpdateTownCommandHandler : IRequestHandler<UpdateTownCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<UpdateTownCommandHandler> _localizer;
        public UpdateTownCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<UpdateTownCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<int>> Handle(UpdateTownCommand request, CancellationToken cancellationToken)
        {

           var item =await _context.Towns.FindAsync( new object[] { request.Id }, cancellationToken)?? throw new NotFoundException($"Town with id: [{request.Id}] not found.");
           item = _mapper.Map(request, item);
		    // raise a update domain event
		   item.AddDomainEvent(new TownUpdatedEvent(item));
           await _context.SaveChangesAsync(cancellationToken);
           return await Result<int>.SuccessAsync(item.Id);
        }
    }
