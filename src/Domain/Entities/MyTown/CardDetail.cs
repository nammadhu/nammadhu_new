﻿using System.ComponentModel.DataAnnotations;
using PublicCommon;
using PublicCommon.Common;
using CleanArchitecture.Blazor.Domain.Common.Entities;


namespace MyTown.Domain;
public class CardDetail : BaseAuditableEntity, IEquatable<CardDetail>
    {
    public bool? IsOpenNow { get; set; }//true is open,false is closed

    //in json
    public string? TimingsToday { get; set; }//if not set will take from TimingsUsualJson of today

    //in json
    public string? TimingsUsual { get; set; }

    //in json
    public string? Queue { get; set; } //json of List<QueueItem>

    [Key]
    public override int Id { get; set; }//same as idCard,not auto generated

    public string? DetailDescription { get; set; }

    //here Id is cardId only ,not autogenerated
    public string? Image1 { get; set; }

    public string? Image2 { get; set; }

    public string? Image3 { get; set; }

    public string? Image4 { get; set; }

    public string? Image5 { get; set; }

    public string? Image6 { get; set; }


    public string? MoreImages { get; set; }//json format
    //"storageaccounc.om/1.jpg,storageaccounc.om/2.png,storageaccounc.om/3.jpeg,google.com/4.jpeg"

    public static bool IsNullOrDefault(CardDetail? details) => (details == null ||
        (!ImageInfoBase64Url.IsUrl(details.Image1) && !ImageInfoBase64Url.IsUrl(details.Image2)
        && !ImageInfoBase64Url.IsUrl(details.Image3) && !ImageInfoBase64Url.IsUrl(details.Image4)
        && !ImageInfoBase64Url.IsUrl(details.Image5) && !ImageInfoBase64Url.IsUrl(details.Image6)
        && string.IsNullOrEmpty(details.DetailDescription)));
    public void ResetImages()
        {
        Image1 = null;
        Image2 = null;
        Image3 = null;
        Image4 = null;
        Image5 = null;
        Image6 = null;
        }
    //IEquatable<CardProduct> implementation
    public bool Equals(CardDetail? otherDetails)//compares including id
        {//usage bool isEqual1 = person1.Equals(person2);
        if (otherDetails == null) return false; // Not the same type
        return Equals(this, otherDetails);
        }

    public static bool Equals(CardDetail? source, CardDetail? otherDetails)//compares including id
        {//usage bool isEqual1 = person1.Equals(person2);
        if (source == null && otherDetails == null) return true; // Not the same type
        if (source == null || otherDetails == null) return false;

        return source.Id == otherDetails.Id && EqualImages(source, otherDetails)
            && source.DetailDescription == otherDetails.DetailDescription; // Compare properties
        }

    public static bool EqualImages(CardDetail source, CardDetail? otherDetails)//compares without id
        {//usage bool isEqual1 = person1.EqualImages(person2);
        if (otherDetails == null) return false; // Not the same type

        //IdCardBrand == otherCard.IdCardBrand //here wont check for id
        return
            StringExtensions.Equals(source.Image1, otherDetails.Image1) &&
            StringExtensions.Equals(source.Image2, otherDetails.Image2) &&
            StringExtensions.Equals(source.Image3, otherDetails.Image3) &&
            StringExtensions.Equals(source.Image4, otherDetails.Image4) &&
            StringExtensions.Equals(source.Image5, otherDetails.Image5) &&
            StringExtensions.Equals(source.Image6, otherDetails.Image6) &&
            StringExtensions.Equals(source.MoreImages, otherDetails.MoreImages);
        }

    //[JsonIgnore]
    //[ForeignKey(nameof(IdCard))]
    public Card? DraftCard { get; set; }

    //[JsonIgnore]
    ////[ForeignKey(nameof(IdCard))]
    public VerifiedCard? VerifiedCard { get; set; }


    }
