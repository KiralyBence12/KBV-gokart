using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace KBV_gokart
{
    internal class Program
    {
        static Random rnd = new Random();
        static void Main(string[] args)
        {
            //KBV-gokart
            //2025.09.15
            //Gokart idopontfoglalo - Egyeni kisproject

            TimeSpan nyitas = new TimeSpan(8, 0, 0);
            TimeSpan zaras = new TimeSpan(19, 0, 0);
            TimeSpan minIdoTartam = TimeSpan.FromHours(1);

            var helyszin = new
            {
                Nev = "Gokart Center",
                Cim = "6000 Kecskemet, Nyíri út 32.",
                Telefon = "36-30-555-1212",
                Web = "GokartCenter.hu"
            };

            Console.WriteLine("=== Gokart Center ===");
            Console.WriteLine($"Név: {helyszin.Nev}");
            Console.WriteLine($"Cím: {helyszin.Cim}");
            Console.WriteLine($"Telefon: {helyszin.Telefon}");
            Console.WriteLine($"Weboldal: {helyszin.Web}");

            Console.ReadKey();

            var vezeteknevek = File.ReadAllText("vezeteknevek.txt")
                                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(n => n.Trim().Trim('\''))
                                .ToArray();

            var keresztnevek = File.ReadAllText("keresztnevek.txt")
                                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(n => n.Trim().Trim('\''))
                                .ToArray();

            int versenyzoSzam = rnd.Next(8, 21);

            List<Versenyzo> versenyzok = new List<Versenyzo>();



            for (int i = 0; i < versenyzoSzam; i++)
            {
                string vezetek = vezeteknevek[rnd.Next(vezeteknevek.Length)];
                string kereszt = keresztnevek[rnd.Next(keresztnevek.Length)];

                DateTime szuletesiDatum = RandomDate(new DateTime(1960, 1, 1), DateTime.Now);
                bool elmult18 = (DateTime.Now.Year - szuletesiDatum.Year > 18);

                string id = GenerateId(vezetek, kereszt, szuletesiDatum);

                versenyzok.Add(new Versenyzo
                {
                    Vezeteknev = vezetek,
                    Keresztnev = kereszt,
                    SzuletesiDatum = szuletesiDatum,
                    Elmult18 = elmult18,
                    Azonosito = id,
                    Email = generateEmail(vezetek, kereszt)
                });
            
            }

            Console.WriteLine($"=== Generált versenyzők száma: {versenyzok.Count}");
            foreach (var v in versenyzok)
            {
                Console.WriteLine($"{v.Vezeteknev} {v.Keresztnev},\n\t" + $"Született: {v.SzuletesiDatum:yyyy.MM.dd},\n\t" + $"18+: {v.Elmult18},\n\t" + $"ID: {v.Azonosito},\n\t" + $"Email: {v.Email}@gmail.com");
            }
            Console.ReadKey();

        }

        static string generateEmail(string vezeteknev, string keresztnev)
        {
            string teljesNev = ekezetLevetel(vezeteknev + keresztnev).ToLower();
            return ekezetLevetel(vezeteknev).ToLower() + "." + ekezetLevetel(keresztnev).ToLower();
        }

        static DateTime RandomDate(DateTime start, DateTime end)
        {
            int range = (end - start).Days;
            return start.AddDays(rnd.Next(range));
        }

        static string GenerateId(string vezetek, string kereszt, DateTime szul)
        {
            string teljesNev = ekezetLevetel(vezetek + kereszt);
            string datum = szul.ToString("yyyyMMdd");
            return $"GO-{teljesNev}-{datum}";
        }

        static string ekezetLevetel(string input)
        {
            string normalis = input.Normalize(NormalizationForm.FormD);
            var chars = normalis.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark);
            return new string(chars.ToArray());
        }
    }
    class Versenyzo
    {
        public string Vezeteknev { get; set; }
        public string Keresztnev { get; set; }
        public DateTime SzuletesiDatum { get; set; }
        public bool Elmult18 { get; set; }
        public string Azonosito { get; set; }
        public string Email { get; set; }
    }
}
