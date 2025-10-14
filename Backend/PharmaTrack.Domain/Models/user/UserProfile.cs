using System;
using PharmaTrack.Domain.Models.shared;

namespace PharmaTrack.Domain.Models.user;

public class UserProfile
{
    public string PhoneNumber { get;  set; }
    public string Speciality { get;  set; }  // useful for delegues, doctors, etc.
    public string AvatarUrl { get;  set; }  
    
    public Adress adress{ get; set; }
     // profile picture

    // Parameterless ctor for Mongo
  public UserProfile() {}
    public UserProfile(string phoneNumber,Adress  address, string speciality, string avatarUrl)
    {
        PhoneNumber = phoneNumber;
        this.adress = address;
        Speciality = speciality;
        AvatarUrl = avatarUrl;
    }


}
