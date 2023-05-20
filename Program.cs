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
            //Listák létrehozása
            List<Utas> utasok = new List<Utas>();
            List<Utazas> utazasok = new List<Utazas>();

            //Menüsor kiíratása
            string[] menupontok = { "Utas felvétele", "Utas adatainak módosítása", "Utazás felvétele", "Utazásra jelentkezés" };
            while (true)
            {


                for (int i = 0; i < menupontok.Length; i++)
                {
                    Console.WriteLine($"[{i + 1}] {menupontok[i]}");
                }
                char valasz = Console.ReadKey().KeyChar;
                Console.Clear();
                //Menüsor működése switchel
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


                        //Végigmegy a listán és megnézi van e már ilyen utas, ha van, módosítja
                        int index = 0;
                        while (index < utasok.Count && utasok[index].GetNev() != ujutas.GetNev())
                        {
                            index++;
                        }
                        if (index < utasok.Count)
                        {
                            Console.WriteLine("Ez az utas már rögzítve van.");
                        }
                        else
                        {
                            //utasok.RemoveAt(i);
                            utasok.Add(ujutas);
                            Console.WriteLine("Sikeres rögzítés!");
                        }


                        //Ha üres a lista egyből hozzáadja az utast
                        if (utasok.Count == 0)
                        {
                            utasok.Add(ujutas);
                            Console.WriteLine("Sikeres rögzítés!");
                        }
                        break;
                    //Utas módosítása
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

                                index = 0;
                                while (index < utasok.Count && utasok[index].GetNev() != editeltutas.GetNev())
                                {
                                    index++;
                                }
                                if (index < utasok.Count)
                                {
                                    Console.WriteLine("Ez az utas már rögzítve van.");
                                }
                                else
                                {
                                    //utasok.RemoveAt(i);
                                    utasok.Add(editeltutas);
                                    Console.WriteLine("Sikeres rögzítés!");
                                }
                                break;
                            }
                        }
                        break;
                    //Út felvétele
                    case '3':
                        Console.Clear();
                        Console.WriteLine("Adja meg az út nevét:");
                        string uticel = Console.ReadLine();
                        Console.WriteLine("Adja meg az út árát:");
                        int ar = int.Parse(Console.ReadLine());
                        Console.WriteLine("Adja meg az út maximális létszámát:");
                        int maxletszam = int.Parse(Console.ReadLine());
                        Utazas ujutazas = new Utazas(uticel, ar, maxletszam);

                        //Ha üres a lista egyből hozzáadja az utast
                        if (utazasok.Count == 0)
                        {
                            utazasok.Add(ujutazas);
                        }

                        //Végigmegy a listán és megnézi van e már ilyen utas, ha van, módosítja
                        for (int i = 0; i < utazasok.Count; i++)
                        {
                            if (utazasok[i] == ujutazas)
                            {
                                utazasok.RemoveAt(i);
                                utazasok.Add(ujutazas);
                                Console.WriteLine("Sikeres rögzítés!");
                            }
                            else
                            {
                                Console.WriteLine("Ez az utas már rögzítve van.");
                            }
                        }
                        break;
                    case '4':
                        Console.Clear();
                        Console.WriteLine("Add meg a neved:");
                        string nev = Console.ReadLine();
                        int jelentkezoindex = 0;
                        for (int i = 0; i < utasok.Count; i++)
                        {
                            if (utasok[i].GetNev() == nev)
                            {
                                jelentkezoindex = i;
                                break;
                            }
                        }
                        Console.Clear();
                        Console.WriteLine($"Jelentkező: {utasok[jelentkezoindex].GetNev()}");
                        foreach (Utazas ut in utazasok)
                        {
                            Console.WriteLine(ut.GetUtazas() + ut.EddigiLetszam());
                        }
                        utazasok[0].Jelentkezes(utasok[jelentkezoindex]);
                        utasok[jelentkezoindex].Jelentkezes(utazasok[0]);
                        break;
                    default:
                        break;
                }


                Console.ReadKey();
                Console.Clear();
            }
        }
    }

    //1db utas adatai
    class Utas
    {
        string nev;
        string cim;
        string telefonszam;
        List<Utazas> jelentkezettutazasok = new List<Utazas>();

        //Konstruktor alapértékek megadása
        public Utas()
        {

        }
        public Utas(string neve, string cime, string telefonszama)
        {
            nev = neve;
            cim = cime;
            telefonszam = telefonszama;
        }

        //Név megszerzése
        public string GetNev()
        {
            return nev;
        }
        public void Jelentkezes(Utazas ut)
        {
            if (!jelentkezettutazasok.Contains(ut))
            {
                jelentkezettutazasok.Add(ut);
            }
            else
            {
                Console.WriteLine("Erre az utazásra már jelentkeztél!");
            }
        }

    }

    //1db út adatai
    class Utazas
    {
        string uticel;
        int ar;
        int maxletszam;
        List<Utas> jelentkezettek = new List<Utas>();

        public Utazas(string utcel, int ara, int max)
        {
            uticel = utcel;
            ar = ara;
            maxletszam = max;
        }
        public string GetUtazas()
        {
            return $"{uticel} | {ar} Ft";
        }
        public void Jelentkezes(Utas utas)
        {
            if (maxletszam > jelentkezettek.Count)
            {
                jelentkezettek.Add(utas);
            }
            else
            {
                Console.WriteLine("Ez az utazás már megtelt!");
            }
        }
        public string EddigiLetszam()
        {
            return $"| {jelentkezettek.Count} / {maxletszam} max főből";
        }
    }
}
