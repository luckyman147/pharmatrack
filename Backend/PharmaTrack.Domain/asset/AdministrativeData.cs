using System;
using System.Collections.Generic;
using System.Linq;
using PharmaTrack.Domain.Models.shared;

namespace PharmaTrack.Domain.asset
{
    public static class AdministrativeData
    {
        private static readonly Random Random = new Random();

        public record ZoneData(string Name, List<GouverneratData> Governorates);
        public record GouverneratData(string Name, List<string> Delegations);

        public static readonly IReadOnlyList<ZoneData> AdministrativeHierarchy = new List<ZoneData>
        {
            new("Nord-Est", new List<GouverneratData>
            {
                new("Ariana", new List<string> { "Cite Ettadhamen", "Commune Ariana", "El Mnihla", "Kalaat Andalous", "Raoued", "Sidi Thabet", "Soukra" }),
                new("Ben Arous", new List<string> { "Ben Arous", "Boumhel Bassatine", "El Mourouj", "Ezzahra", "Fouchana", "Hammam Chatt", "Hammam Lif", "M'Hamdia", "Medina Jedida", "Megrine", "Mornag", "Rades" }),
                new("Manouba", new List<string> { "Borj El Amri", "Douar Hicher", "El Battan", "Jedeida", "Manouba", "Mornaguia", "Oued Ellil", "Tebourba" }),
                new("Nabeul", new List<string> { "Beni Khalled", "Beni Khiar", "Bouargoub", "Dar Chaabane Fehri", "El Haouaria", "El Mida", "Grombalia", "Hammam Ghezaz", "Hammamet", "Kelibia", "Korba", "Menzel Bouzelfa", "Menzel Temime", "Nabeul", "Slimene", "Takelsa" }),
                new("Tunis", new List<string> { "Carthage", "Commune Sidi Hessine", "Commune Tunis", "La Goulette", "La Marsa", "Le Bardo", "Le Kram (Ancien El Bhira)" }),
                new("Zaghouan", new List<string> { "Bir Mchargua", "El Fahs", "Nadhour", "Saouaf", "Zaghouan", "Zriba" })
            }),

            new("Nord-Ouest", new List<GouverneratData>
            {
                new("Beja", new List<string> { "Amdoun", "Beja Nord", "Beja Sud", "Goubellat", "Mejez El Bab", "Nefza", "Teboursouk", "Testour", "Thibar" }),
                new("Bizerte", new List<string> { "Bizerte Nord", "Bizerte Sud", "El Alia", "Ghar El Melh", "Ghezala", "Jarzouna", "Joumine", "Mateur", "Menzel Bourguiba", "Menzel Jemil", "Ras Jebel", "Sejnane", "Tinja", "Utique" }),
                new("Jendouba", new List<string> { "Ain Drahem", "Balta Bouaouan", "Bousalem", "Fernana", "Ghardimaou", "Jendouba", "Jendouba Nord", "Oued Meliz", "Tabarka" }),
                new("Le Kef", new List<string> { "Dahmani", "Jerissa", "Kalaat Khasba", "Kalaat Senane", "Ksour", "Le Kef Est", "Le Kef Ouest", "Nebeur", "Sakiet Sidi Youssef", "Sers", "Tajerouine", "Touiref" })
            }),

            new("Centre-Ouest/Intérieur", new List<GouverneratData>
            {
                new("Kairouan", new List<string> { "Ain Jloula", "Chebika", "Cherarda", "El Ala", "Haffouz", "Hajeb Elayoun", "Kairouan Nord", "Kairouan Sud", "Menzel Mhiri", "Oueslatia", "Sbikha", "Sidi Ali Ben Nasrallah", "Sidi Amor Bouhajla" }),
                new("Kasserine", new List<string> { "El Ayoun", "Ezzouhour", "Feriana", "Foussana", "Haidra", "Hassi Ferid", "Jedliane", "Kasserine Nord", "Kasserine Sud", "Majel Bel Abbes", "Sbeitla", "Sbiba", "Thala" }),
                new("Sidi Bouzid", new List<string> { "Bir El Hfay", "Cebalet Ouled Asker", "Essaida", "Hichria", "Jelma", "Meknassy", "Menzel Bouzaiane", "Mezzouna", "Ouled Haffouz", "Regueb", "Sidi Ali Ben Aoun", "Sidi Bouzid Est", "Sidi Bouzid Ouest", "Souk Jedid" }),
                new("Siliana", new List<string> { "Bargou", "Bouarada", "Bourouis", "El Aroussa", "Gaafour", "Kesra", "Krib", "Makthar", "Rouhia", "Siliana Nord", "Siliana Sud" })
            }),

            new("Centre-Est/Sahel", new List<GouverneratData>
            {
                new("Mahdia", new List<string> { "Boumerdas", "Bradaa", "Chebba", "Chorbane", "El Jem", "Hbira", "Ksour Essaf", "Mahdia", "Mellouleche", "Ouled Chamekh", "Rejiche", "Sidi Alouane", "Souassi" }),
                new("Monastir", new List<string> { "Bekalta", "Bembla", "Beni Hassen", "Jemmal", "Ksar Hellal", "Ksibet Mediouni", "Moknine", "Monastir", "Ouerdanine", "Sahline", "Sayada Lamta Bouhjar", "Teboulba", "Zeramdine" }),
                new("Sfax", new List<string> { "Agareb", "Bir Ali Ben Khalifa", "Commune Sakiet Eddaier", "Commune Sfax", "El Amra", "El Hancha", "Ghriba", "Jebeniana", "Kerkennah", "Mahres", "Menzel Chaker", "Sakiet Ezzit", "Sfax Sud", "Skhira", "Thyna" }),
                new("Sousse", new List<string> { "Akouda", "Bouficha", "Commune Ezzouhour", "Commune Ksiba Thrayet", "Commune Sousse", "Commune Zaouiet Sousse", "Enfidha", "Hammam Sousse", "Hergla", "Kalaa Kebira", "Kalaa Sghira", "Kondar", "M'saken", "Sidi Bou Ali", "Sidi El Hani" })
            }),

            new("Sud", new List<GouverneratData>
            {
                new("Gabes", new List<string> { "Dkhilet Toujane", "El Hamma", "El Hamma Ouest", "Gabes Medina (Est)", "Gabes Ouest", "Gabes Sud", "Ghannouch", "Mareth", "Matmata", "Matmata Jadida", "Menzel Habib", "Metouia", "Ouedhref" }),
                new("Gafsa", new List<string> { "Bel Khir", "El Guetar", "El Ksar", "Gafsa Nord", "Gafsa Sud", "M'dhilla", "Metlaoui", "Oum Laraies", "Redeyef", "Sened", "Sidi Boubaker", "Sidi Yaich", "Zanouch" }),
                new("Kebili", new List<string> { "Douz Nord", "Douz Sud", "Faouar", "Kebili Nord", "Kebili Sud", "Rjim Maatoug", "Souk Lahad" }),
                new("Medenine", new List<string> { "Ben Guerdane", "Beni Khedache", "Houmt Souk", "Jerba Ajim", "Jerba Midoun", "Medenine Nord", "Medenine Sud", "Sidi Makhlouf", "Zarzis" }),
                new("Tataouine", new List<string> { "Beni Mhira", "Bir Lahmar", "Dhehiba", "Ghomrassen", "Remada", "Smar", "Tataouine Nord", "Tataouine Sud" }),
                new("Tozeur", new List<string> { "Degueche", "Hamet Jrid", "Hazoua", "Nefta", "Tamaghza", "Tozeur" })
            })
        };

        public static ZoneData? GetZoneData(string zoneName)
        {
            if (string.IsNullOrWhiteSpace(zoneName)) return null;
            return AdministrativeHierarchy.FirstOrDefault(z => z.Name.Equals(zoneName, StringComparison.OrdinalIgnoreCase));
        }

        public static Adress GetRandomAddress()
        {
            var zone = AdministrativeHierarchy[Random.Next(AdministrativeHierarchy.Count)];
            var gov = zone.Governorates[Random.Next(zone.Governorates.Count)];
            var del = gov.Delegations[Random.Next(gov.Delegations.Count)];

            var rue = Random.Next(3) == 0 ? $"Rue Al-Furat n°{Random.Next(1, 150)}" : null;

            return new Adress
            {
                Zone = zone.Name,
                Gouvernerat = gov.Name,
                delegation = del,
                Rue = rue
            };
        }
    }
}
