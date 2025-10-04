using System;

namespace PharmaTrack.Domain.Models.user;

public class UserProfile
{
    public string PhoneNumber { get; private set; }
    public string Address { get; private set; }
    public string Speciality { get; private set; }  // useful for delegues, doctors, etc.
    public string AvatarUrl { get; private set; }   // profile picture

    // Parameterless ctor for Mongo
    private UserProfile() { }

    public UserProfile(string phoneNumber, string address, string speciality, string avatarUrl)
    {
        PhoneNumber = phoneNumber;
        Address = address;
        Speciality = speciality;
        AvatarUrl = avatarUrl;
    }

    public void UpdateProfile(string phoneNumber, string address, string speciality, string avatarUrl)
    {
        PhoneNumber = phoneNumber;
        Address = address;
        Speciality = speciality;
        AvatarUrl = avatarUrl;
    }
}
