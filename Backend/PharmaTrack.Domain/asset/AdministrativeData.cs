using System;
using PharmaTrack.Domain.Models.shared;

namespace PharmaTrack.Domain.asset;


// Note: This class is static and houses the administrative hierarchy data.
public static class AdministrativeData
{
    // Static Random instance for generating random numbers
    private static readonly Random Random = new Random();

    // --- Helper Records for Hierarchy (Private to this file for clarity) ---
    public record ZoneData(string Name, List<GouverneratData> Governorates);
    public record GouverneratData(string Name, List<string> Delegations);

    // --- The Complete Administrative Hierarchy ---
    private static readonly IReadOnlyList<ZoneData> AdministrativeHierarchy = new List<ZoneData>
    {
        // Nord-Ouest Zone
        new("Nord-Ouest", new List<GouverneratData>
        {
            new("Bizerte", new List<string> { "Bizerte Nord", "Jarzouna", "Bizerte Sud", "Sejnane", "Joumine", "Mateur", "Ghezala", "Menzel Bourguiba", "Tinja", "Utique", "Ghar El Melh", "Menzel Jemil", "El Alia", "Ras Jebel" }),
            new("Beja", new List<string> { "Beja Nord", "Beja Sud", "Amdoun", "Nefza", "Teboursouk", "Thibar", "Testour", "Goubellat", "Mejez El Bab" }),
            new("Jendouba", new List<string> { "Jendouba", "Jendouba Nord", "Boussalem", "Tabarka", "Ain Drahem", "Fernana", "Ghardimaou", "Oued Mliz", "Balta Bouaouene" }),
            new("Le Kef", new List<string> { "Kef Ouest", "Kef Est", "Nebeur", "Sakiet Sidi Youssef", "Tejerouine", "Kalaat sinane", "Kalaa El khasba", "Jerissa", "Gsour", "Dahmani", "Le Sers" })
        }),

        // Nord-Est Zone - Includes Greater Tunis
        new("Nord-Est", new List<GouverneratData>
        {
            new("Tunis", new List<string> { "Carthage", "La Medina", "Bab Bhar", "Bab Souika", "Omrane", "Omrane Superieur", "Attahrir", "El Menzah", "Cite Alkhadhra", " Le Bardo", "Sejoumi", "Azzouhour", "Alhrairia", "Sidi Hessine", "Ouardia", "Kabaria", "Sidi Elbechir", "Jebel Jelloud", "La Goulette", "Le Kram", "La Marsa" }),
            new("Ariana", new List<string> { "Ariana", "Soukra", "Raouede", "Kalaat Andalous", "Sidi Thabet", "Cite Attadhamon", "El Mnihla" }),
            new("Ben Arous", new List<string> { "Ben Arous", "Medina Jedida", "Mourouj", "Hammam Lif", "Hammam Chatt", "Boumhel Bassatine", "Ezzahra", "Rades", "Megrine", "M'hamdia", "Fouchana", "Mornag" }),
            new("Manouba", new List<string> { "Manouba", "Oued Ellil", "Tebourba", "Battan", "Jedaida", "Mornaguia", "Borj Amri", "Douar Hicher" }),
            new("Nabeul", new List<string> { "Nabeul", "Dar Chaabane Elfehri", "Beni Khiar", "Korba", "Menzel Temime", "Mida", "Kelibia", "Hammam Ghezaz", "Haouaria", "Takelsa", "Slimane", "Menzel Bouzelfa", "Beni Khalled", "Grombalia", "Bouargoub", "Hammamet" }),
            new("Zaghouan", new List<string> { "Zaghouan", "Zriba", "Bir Mechargua", "El Fahs", "Nadhour", "Saouaf","El Emayem","Jebel El Wast" })
        }),

        // Centre-Ouest/Interieur Zone
        new("Centre-Ouest/Intérieur", new List<GouverneratData>
        {
            new("Siliana", new List<string> { "Siliana Nord", "Siliana Sud", "Bou Arada","Bouarada", "Gaafour", "El Aroussa", "Krib", "Bourouis", "Makther", "Rouhia", "Kesra", "Bargou","Habebsa","Sidi Marched","Siliana" }),
            new("Kasserine", new List<string> { "Kasserine Nord", "Kasserine Sud", "Azzouhour", "Hassi Ferid", "Sbitla", "Sbiba", "Jedliane", "El Ayoun", "Tela", "Hidra", "Foussana", "Feriana", "Mejel Bel Abbes" }),
            new("Kairouan", new List<string> { "Kairouan Nord", "Kairouan Sud", "Chebika", "Sebikha", "Oueslatia", "Haffouz", "El Ala", "Hajeb El Ayoun", "Nasrallah", "Cherarda", "Bouhajla" }),
            new("Sidi Bouzid", new List<string> { "Sidi Bouzid Ouest", "Sidi Bouzid Est", "Jelma", "Sabbalet Ouled Askar", "Bir El Hfay Hfay", "Sidi Ali Benôun", "Menzel Bouzayane", "Meknassi", "Souk Jedid", "Mezouna", "Regueb", "Ouled Haffouz" })
        }),

        // Centre-Est/Sahel Zone
        new("Centre-Est/Sahel", new List<GouverneratData>
        {
            new("Sousse", new List<string> { "Sousse", "Zaouiet Sousse,Ezzouhour, Ksiba Thrayet", "Sousse Ryadh", "Sousse Jawhara", "Sousse Sidi Abdelhamid", "Hammam Sousse", "Akouda", "Kalaa Kebira", "Sidi Bou Ali", "Hergla", "Enfidha", "Bouficha", "Kondar", "Sidi El Hani", "M'saken", "Kalaa Sghira" }),
            new("Monastir", new List<string> { "Monastir", "Ouerdanine", "Sahline", "Zeramdine", "Beni Hassen", "Jemmal", "Bembla", "Moknine", "Bekalta", "Teboulba", "Ksar Hellal", "Ksibet Mediouni", "Sayada Lamta Bouhjar" }),
            new("Mahdia", new List<string> { "Mahdia", "Boumerdes", "Ouled Chamekh", "Chorbane", "Hbira", "Souassi", "El Jem", "Chebba", "Mellouleche", "Sidi Alouane", "Ksour Essef" }),
            new("Sfax", new List<string> { "Sfax", "Sfax Ouest", "Sakiet Ezzit", "Sakiet Eddaier", "Sfax Sud", "Thyna", "Agareb", "Jebeniana", "El Amra", "El Hancha", "Menzel Chaker", "Ghriba", "Bir Ali Ben Khalifa", "Skhira", "Mahres", "Kerkennah" })
        }),

        // Sud Zone
        new("Sud", new List<GouverneratData>
        {
            new("Gafsa", new List<string> { "Gafsa Nord", "Sidi Aich", "El Ksar", "Gafsa Sud", "Moulares", "Redyef", "Metlaoui", "El Mdhilla", "El Guettar", "Belkhir", "Sned" }),
            new("Tozeur", new List<string> { "Tozeur", "Degueche", "Tamegza", "Nefta", "Hezoua","Hamet Jerid","Hezoua" }),
            new("Kebili", new List<string> { "Kebili Sud", "Kebili Nord", "Souk El Ahad", "Douz Nord", "Douz Sud", "El Faouar" }),
            new("Gabes", new List<string> { "Gabes Ville", "Gabes Ouest", "Gabes Sud", "Ghannouch", "Metouia", "Menzel Habib", "Hamma", "Matmata", "Matmata Nouvelle", "Mareth" }),
            new("Medenine", new List<string> { "Mednine Nord", "Mednine Sud", "Beni Khedach", "Ben Guerdene", "Zazis", "Jerba Houmet Souk", "Jerba Midoun", "Jerba Ajim", "Sidi Makhlouf" }),
            new("Tataouine", new List<string> { "Tataouine Nord", "Tataouine Sud", "Smar", "Bir Lahmer", "Ghomrassen", "Dhehiba", "Remada","Beni Mhira","Tataouine" })
        })
    };

    /// <summary>
    /// Gets the ZoneData record matching the provided Zone name (case-insensitive).
    /// </summary>
    public static ZoneData? GetZoneData(string zoneName)
    {
        if (string.IsNullOrWhiteSpace(zoneName)) return null;
        return AdministrativeHierarchy
            .FirstOrDefault(z => z.Name.Equals(zoneName, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Generates a complete, randomly selected administrative address.
    /// NOTE: This function assumes the 'Adress' class is defined elsewhere (e.g., in Adress.cs).
    /// </summary>
    public static Adress GetRandomAddress()
    {
        // 1. Select a random Zone
        var zoneIndex = Random.Next(AdministrativeHierarchy.Count);
        var selectedZone = AdministrativeHierarchy[zoneIndex];

        // 2. Select a random Gouvernerat within that Zone
        var governorateIndex = Random.Next(selectedZone.Governorates.Count);
        var selectedGovernorate = selectedZone.Governorates[governorateIndex];

        // 3. Select a random Delegation within that Gouvernerat
        var delegationIndex = Random.Next(selectedGovernorate.Delegations.Count);
        var selectedDelegation = selectedGovernorate.Delegations[delegationIndex];

        // 4. Optionally generate a random Rue (Street)
        var rue = Random.Next(3) == 0 // 1 in 3 chance of having a Rue
            ? $"Rue Al-Furat n°{Random.Next(1, 150)}" // Example French/Arabic street name
            : null;

        // 5. Return the new Adress object
        return new Adress
        {
            Zone = selectedZone.Name,
            Gouvernerat = selectedGovernorate.Name,
            delegation = selectedDelegation,
            Rue = rue
        };
    }
}
