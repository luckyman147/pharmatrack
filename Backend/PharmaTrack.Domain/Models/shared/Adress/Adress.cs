using System;

namespace PharmaTrack.Domain.Models.shared;


public class Adress
{
    public  string Zone { get; set; }
    public  string Gouvernerat { get; set; }
    public  string delegation { get; set; }
    public string? Rue { get; set; }

    public Adress() { }

    public Adress(string Zone, string Gouvernerat, string Delegation, string? street)
    {
        this.Zone = Zone;
        this.Gouvernerat = Gouvernerat;delegation = Delegation;this.Rue = street;
    }

}
