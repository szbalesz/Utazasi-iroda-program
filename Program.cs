using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace utazas
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] menupontok = { "Utas felvétele", "Utas adatainak módosítása", "Utazás felvétele", "Utazásra jelentkezés" };
            for (int i = 0; i < menupontok.Length; i++)
            {
                Console.WriteLine($"[{i+1}] {menupontok[i]}");
            }
            char valasz = Console.ReadKey().KeyChar;
            List<Utas> utasok = new List<Utas>();
            List<Utazas> utazasok = new List<Utazas>();
            switch (valasz)
            {
                case '1':
                    Console.WriteLine("Adja meg az utas nevét:");
                    string utasneve = Console.ReadLine();
                    Console.WriteLine("Adja meg az utas lakcímét:");
                    string lakcime = Console.ReadLine();
                    Console.WriteLine("Adja meg az utas telefonszámát:");
                    string telefonszama = Console.ReadLine();
                    Utas ujutas = new Utas(utasneve, lakcime, telefonszama);
                    if (!utasok.Contains(ujutas))
                    {
                        utasok.Add(ujutas);
                        Console.WriteLine("Sikeres rögzítés!");
                    }
                    else
                    {
                        Console.WriteLine("Ez az utas már rögzítve van.");
                    }
                    break;
                case '2':
                    Console.WriteLine("Adja meg a módosítandó utas nevét:");
                    string modositandonev = Console.ReadLine();
                    foreach (Utas utas in utasok)
                    {
                        if (utas.GetNev() == modositandonev)
                        {
                            utasok.Remove(utas);
                            Console.WriteLine("Adja meg az új nevet:");
                            utasneve = Console.ReadLine();
                            Console.WriteLine("Adja meg az új lakcímet:");
                            lakcime = Console.ReadLine();
                            Console.WriteLine("Adja meg az új telefonszámot:");
                            telefonszama = Console.ReadLine();
                            Utas editeltutas = new Utas(utasneve, lakcime, telefonszama);
                            if (!utasok.Contains(editeltutas))
                            {
                                utasok.Add(editeltutas);
                                Console.WriteLine("Sikeres módosítás!");
                            }
                            else
                            {
                                Console.WriteLine("Ez az utas már létezik.");
                            }
                            break;
                        }
                    }
                    break;
                case '3':
                    Console.WriteLine("Adja meg az út nevét:");
                    string uticel = Console.ReadLine();
                    Console.WriteLine("Adja meg az út árát:");
                    int ar = int.Parse(Console.ReadLine());
                    Console.WriteLine("Adja meg az út maximális létszámát:");
                    int maxletszam = int.Parse(Console.ReadLine());
                    Utazas ujutazas = new Utazas(uticel, ar, maxletszam);
                    if (!utazasok.Contains(ujutazas))
                    {
                        utazasok.Add(ujutazas);
                        Console.WriteLine("Sikeres rögzítés!");
                    }
                    else
                    {
                        Console.WriteLine("Ez az út már rögzítve van.");
                    }
                    break;
                case '4':
                    break;
                default:
                    break;
            }


            Console.ReadKey();
        }
    }

    class Utas
    {
        string nev;
        string cim;
        string telefonszam;

        public Utas(string neve, string cime, string telefonszama)
        {
            nev = neve;
            cim = cime;
            telefonszam = telefonszama;
        }

        public string GetNev()
        {
            return nev;
        }
    }

    class Utazas
    {
        string uticel;
        int ar;
        int maxletszam;

        public Utazas(string utcel, int ara, int max)
        {
            uticel = utcel;
            ar = ara;
            maxletszam = max;
        }
    }
}
