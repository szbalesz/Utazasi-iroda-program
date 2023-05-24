using System;
using System.Collections.Generic;
using System.IO;

namespace utazas
{
    class Program
    {
        static void Main(string[] args)
        {
            //Listák létrehozása
            List<Utas> utasok = new List<Utas>();
            List<Utazas> utazasok = new List<Utazas>();
            Console.Title = "Utazási iroda program";
            if (File.Exists("utasok.txt"))
            {
                string[] utasokfajl = File.ReadAllLines("utasok.txt");
                for (int i = 0; i < utasokfajl.Length; i++)
                {
                    string[] sor = utasokfajl[i].Split('\t');
                    Utas ujutas = new Utas(sor[0], sor[1], sor[2]);

                    utasok.Add(ujutas);
                }
            }
            if (File.Exists("utazasok.txt"))
            {
                string[] utazasokfajl = File.ReadAllLines("utazasok.txt");
                for (int i = 0; i < utazasokfajl.Length; i++)
                {
                    Utazas ujutazas = new Utazas(utazasokfajl[i].Split('\t')[0], int.Parse(utazasokfajl[i].Split('\t')[1]), int.Parse(utazasokfajl[i].Split('\t')[2]));
                    utazasok.Add(ujutazas);
                }
            }
            if (File.Exists("utasok.txt"))
            {
                string[] utasokfajl = File.ReadAllLines("utasok.txt");
                for (int i = 0; i < utasokfajl.Length; i++)
                {
                    string[] sor = utasokfajl[i].Split('\t');
                    if (sor.Length >= 3)
                    {
                        for (int f = 3; f < sor.Length; f += 2)
                        {
                            string jelentkezett = sor[f];
                            string eloleg = sor[f + 1];
                            for (int g = 0; g < utazasok.Count; g++)
                            {
                                if (utazasok[g].GetUticel() == jelentkezett)
                                {
                                    utasok[i].Jelentkezes(utazasok[g]);
                                    utazasok[g].Jelentkezes(utasok[i]);
                                    utasok[i].ElolegBetoltes(int.Parse(eloleg));
                                }
                            }
                        }
                    }
                }
            }
            //Menüsor kiíratása
            string[] menupontok = { "Utas felvétele", "Utas adatainak módosítása", "Utas adatainak, és utazásainak lekérése", "Utazáshoz tartozó utasok kiíratása állományba", "Utazás felvétele", "Utazásra jelentkezés", "Kilépés" };
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
                            StreamWriter ujUtasokfajl = new StreamWriter("utasok.txt");
                            foreach (Utas u in utasok)
                            {
                                ujUtasokfajl.WriteLine(u.GetAdatok());
                            }
                            ujUtasokfajl.Close();
                        }


                        //Ha üres a lista egyből hozzáadja az utast
                        if (utasok.Count == 0)
                        {
                            utasok.Add(ujutas);
                            Console.WriteLine("Sikeres rögzítés!");
                        }
                        Vissza();
                        break;
                    //Utas módosítása
                    case '2':
                        Console.WriteLine("Adja meg a módosítandó utas nevét:");
                        string modositandonev = Console.ReadLine();
                        bool talalt = false;
                        foreach (Utas utas in utasok)
                        {
                            if (utas.GetNev() == modositandonev)
                            {
                                talalt = true;
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
                                    StreamWriter ujUtasokfajl = new StreamWriter("utasok.txt");
                                    foreach (Utas u in utasok)
                                    {
                                        ujUtasokfajl.WriteLine(u.GetAdatok());
                                    }
                                    ujUtasokfajl.Close();
                                }
                                break;
                            }
                        }
                        if (!talalt)
                        {
                            NincsUtas();
                        }
                        Vissza();
                        break;
                    //Utas adatainak megtekintése
                    case '3':
                        Console.WriteLine("Adja meg az utas nevét:");
                        string kereses = Console.ReadLine();
                        talalt = false;
                        //utas megkeresése
                        foreach (Utas u in utasok)
                        {
                            //ha van utas
                            if (u.GetNev() == kereses)
                            {
                                talalt = true;
                                ConsoleKey jelenlegivalasz = ConsoleKey.D0;
                                while (jelenlegivalasz != ConsoleKey.D2)
                                {
                                    Console.Clear();
                                    //kiíratjuk a teljes adatlapot
                                    u.TeljesAdatlap();
                                    Console.WriteLine();
                                    Console.WriteLine(Disz());
                                    //2 választási lehetősége van a felhasználónak
                                    Console.WriteLine("\t\t\t\t[1] Előleg módosítása\t\t [2] Vissza a menübe");
                                    jelenlegivalasz = Console.ReadKey().Key;
                                    switch (jelenlegivalasz)
                                    {
                                        case ConsoleKey.D1:
                                            //előleg módosítása
                                            Console.WriteLine("\nAdja meg, hogy melyik utazáshoz tartozó előleget szeretné megváltoztatni!");
                                            string mely = Console.ReadLine();
                                            int melyikutazasindex = 0;
                                            for (int i = 0; i < utazasok.Count; i++)
                                            {
                                                if (mely == utazasok[i].GetUticel())
                                                {
                                                    melyikutazasindex = i;
                                                    break;
                                                }
                                                melyikutazasindex++;
                                            }
                                            //ha az index eléri a lista végét akkor nincs ilyen utazás
                                            if (melyikutazasindex == utazasok.Count)
                                            {
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.Write("Nem jelentkezett ilyen utazásra! ");
                                                Console.ForegroundColor = ConsoleColor.Yellow;
                                                Console.Write("(Nyomjon meg egy gombot a visszalépéshez)");
                                                Console.ForegroundColor = ConsoleColor.White;
                                                Console.ReadKey();
                                            }
                                            else
                                            {
                                                //ha van ilyen néven utazás
                                                Console.WriteLine($"Adja meg, hogy mennyire szeretné módosítani az előleget! Utazás teljes ára: {utazasok[melyikutazasindex].GetAr()} Ft");
                                                int mennyire = int.Parse(Console.ReadLine());
                                                //megvizsgáljuk, hogy a megadott szám nem e több mint az utazás ára
                                                if (mennyire > int.Parse(utazasok[melyikutazasindex].GetAr()))
                                                {
                                                    //ha több akkor a teljes árat kifizeti a felhasználó
                                                    mennyire = int.Parse(utazasok[melyikutazasindex].GetAr());
                                                    u.SetEloleg(mennyire, utazasok[melyikutazasindex].GetUticel());
                                                }
                                                else
                                                {
                                                    u.SetEloleg(mennyire, utazasok[melyikutazasindex].GetUticel());
                                                }
                                                Console.WriteLine(Disz());
                                                Console.ForegroundColor = ConsoleColor.Green;
                                                Console.WriteLine($"Az előleg sikeresen megváltoztatva a(z) {utazasok[melyikutazasindex].GetUticel()} utazásnál {mennyire} Ft-ra!");
                                                Console.ForegroundColor = ConsoleColor.White;
                                                Console.WriteLine(Disz());
                                                Console.ForegroundColor = ConsoleColor.Yellow;
                                                Console.Write("(Nyomjon meg egy gombot a visszalépéshez)");
                                                Console.ReadKey();
                                                Console.ForegroundColor = ConsoleColor.White;
                                                //utasok fájl frissítése az új adatokkal
                                                StreamWriter ujUtasokfajl = new StreamWriter("utasok.txt");
                                                foreach (Utas uts in utasok)
                                                {
                                                    ujUtasokfajl.WriteLine(uts.FajlSor());
                                                }
                                                ujUtasokfajl.Close();
                                            }
                                            break;
                                        case ConsoleKey.D2:
                                            //ezzel lép vissza a menübe
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            }
                        }
                        //ha nincs utas
                        if (!talalt)
                        {
                            NincsUtas();
                        }
                        Vissza();
                        break;
                    //Adott utazáshoz tartozó utasok kiratása állományba
                    case '4':
                        //utazások kiíratása ha van
                        if (utazasok.Count == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.SetCursorPosition(Console.WindowWidth / 2 - 15, 4);
                            Console.WriteLine("Nincs egy utazás sem!");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine(Disz());
                            Console.SetCursorPosition(Console.WindowWidth / 2 - 35, 1);
                            Console.WriteLine("Uticél");
                            Console.SetCursorPosition(Console.WindowWidth / 2 - 8, 1);
                            Console.WriteLine("Ár");
                            Console.SetCursorPosition(Console.WindowWidth / 2 + 18, 1);
                            Console.WriteLine("Létszám");

                            for (int i = 0; i < utazasok.Count; i++)
                            {
                                Console.SetCursorPosition(Console.WindowWidth / 2 - 36, i + 3);
                                Console.WriteLine(utazasok[i].GetUticel());
                                Console.SetCursorPosition(Console.WindowWidth / 2 - 10, i + 3);
                                Console.WriteLine($"{utazasok[i].GetAr()} Ft");
                                Console.SetCursorPosition(Console.WindowWidth / 2 + 15, i + 3);
                                if (utazasok[i].JelentkezettLetszam() == utazasok[i].MaxLetszam())
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                }
                                else if (utazasok[i].JelentkezettLetszam() >= utazasok[i].MaxLetszam() / 2)
                                {
                                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Green;
                                }
                                Console.WriteLine($"{utazasok[i].JelentkezettLetszam()} / {utazasok[i].MaxLetszam()} max főből");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            Console.WriteLine("Melyik utazás utasait szeretné állományba kiíratni?");
                            string melyik = Console.ReadLine();
                            for (int i = 0; i < utazasok.Count; i++)
                            {
                                if (utazasok[i].GetUticel() == melyik)
                                {
                                    StreamWriter utasLista = new StreamWriter($"{utazasok[i].GetUticel()}_utaslista.txt");
                                    List<Utas> jelentkezettek = utazasok[i].Jelentkezettek();
                                    for (int f = 0; f < jelentkezettek.Count; f++)
                                    {
                                        List<Utazas> jelentkezettutazasok = jelentkezettek[f].GetJelentkezesekLista();
                                        int index2 = 0;
                                        for (int g = 0; g < jelentkezettutazasok.Count; g++)
                                        {
                                            //megkeressük, hogy az adott utasnál ez hanyadik a jelentkezett utazások között
                                            if(jelentkezettutazasok[g].GetUticel() == melyik)
                                            {
                                                index2 = g;
                                                break;
                                            }
                                        }
                                        utasLista.WriteLine($"{jelentkezettek[f].GetAdatok()}\t{jelentkezettek[f].GetEloleg(index2)}");

                                    }
                                    utasLista.Close();
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine($"Az utaslista elkészült {utazasok[i].GetUticel()}_utaslista.txt néven.");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    break;
                                }
                            }
                        }
                        Vissza();
                        break;
                    //Út felvétele
                    case '5':
                        Console.Clear();
                        Console.WriteLine("Adja meg az uticélt:");
                        string uticel = Console.ReadLine();
                        Console.WriteLine("Adja meg az út árát:");
                        int ar = int.Parse(Console.ReadLine());
                        Console.WriteLine("Adja meg az út maximális létszámát:");
                        int maxletszam = int.Parse(Console.ReadLine());
                        Utazas ujutazas = new Utazas(uticel, ar, maxletszam);
                        index = 0;
                        while (index < utazasok.Count && utazasok[index].GetUticel() != uticel)
                        {
                            index++;
                        }
                        if (index < utazasok.Count)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Ez az utazás már rögzítve van.");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else
                        {
                            //utazasok.RemoveAt(i);
                            utazasok.Add(ujutazas);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Sikeres rögzítés!");
                            Console.ForegroundColor = ConsoleColor.White;
                            StreamWriter ujUtazasokfajl = new StreamWriter("utazasok.txt");
                            foreach (Utazas ut in utazasok)
                            {
                                ujUtazasokfajl.WriteLine(ut.GetAdatok());
                            }
                            ujUtazasokfajl.Close();
                        }
                        //Ha üres a lista egyből hozzáadja az utazast
                        if (utazasok.Count == 0)
                        {
                            utazasok.Add(ujutazas);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Sikeres rögzítés!");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        Vissza();
                        break;
                    case '6':
                        Console.Clear();
                        //utas nevének bekérése
                        Console.WriteLine("Add meg az utas nevét:");
                        string nev = Console.ReadLine();
                        int jelentkezoindex = int.MaxValue;
                        int utazasindex = 0;
                        //megkeressük az utast
                        for (int i = 0; i < utasok.Count; i++)
                        {
                            if (utasok[i].GetNev() == nev)
                            {
                                jelentkezoindex = i;
                                break;
                            }
                        }
                        Console.Clear();
                        //ha nem üres az utasok lista
                        if (jelentkezoindex < int.MaxValue)
                        {
                            //a felhasználónak kiíratjuk az utazásokat
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"Jelentkező: {utasok[jelentkezoindex].GetNev()}");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine(Disz());
                            Console.SetCursorPosition(Console.WindowWidth / 2 - 35, 2);
                            Console.WriteLine("Uticél");
                            Console.SetCursorPosition(Console.WindowWidth / 2 - 8, 2);
                            Console.WriteLine("Ár");
                            Console.SetCursorPosition(Console.WindowWidth / 2 + 18, 2);
                            Console.WriteLine("Létszám");
                            Console.WriteLine(Disz());
                            for (int i = 0; i < utazasok.Count; i++)
                            {
                                Console.SetCursorPosition(Console.WindowWidth / 2 - 36, 4 + i);
                                Console.WriteLine(utazasok[i].GetUticel());
                                Console.SetCursorPosition(Console.WindowWidth / 2 - 10, 4 + i);
                                Console.WriteLine($"{utazasok[i].GetAr()} Ft");
                                Console.SetCursorPosition(Console.WindowWidth / 2 + 15, 4 + i);
                                if (utazasok[i].JelentkezettLetszam() == utazasok[i].MaxLetszam())
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                }
                                else if (utazasok[i].JelentkezettLetszam() >= utazasok[i].MaxLetszam() / 2)
                                {
                                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Green;
                                }
                                Console.WriteLine($"{utazasok[i].JelentkezettLetszam()} / {utazasok[i].MaxLetszam()} max főből");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            if (utazasok.Count == 0)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.SetCursorPosition(Console.WindowWidth / 2 - 15, 4);
                                Console.WriteLine("Nincs egy utazás sem!");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            Console.WriteLine(Disz());
                            //bekérjük az uticélt, hogy melyik utazásra szeretne jelentkezni
                            Console.WriteLine("Adja meg az uticélt:");
                            string uticelvalasz = Console.ReadLine();
                            //ha van ilyen utazás akkkor annak az indexét megkeressük
                            for (int i = 0; i < utazasok.Count; i++)
                            {
                                if (uticelvalasz == utazasok[i].GetUticel())
                                {
                                    utazasindex = i;
                                    break;
                                }
                                utazasindex++;
                            }
                            //ha az index eléri a lista végét akkor nincs ilyen utazás
                            if (utazasindex == utazasok.Count)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                Console.WriteLine("Nincs ilyen utazás próbálja újra!");
                                Console.ForegroundColor = ConsoleColor.White;
                            }//ha még nem jelentkezett erre az utazásra
                            else if (!utasok[jelentkezoindex].JelentkezettE(utazasok[utazasindex]))
                            {
                                string szeretne = "";
                                Console.WriteLine(Disz());
                                //amíg nem ír i-t vagy n-t
                                while (szeretne != "n" && szeretne != "i")
                                {
                                    //addíg megkérdezzük hogy szeretne-e előleget fizetni
                                    Console.WriteLine("Szeretne előleget fizetni? (i/n)");
                                    szeretne = Console.ReadLine();
                                    Console.WriteLine(Disz());
                                    //ha szeretne
                                    if (szeretne == "i")
                                    {
                                        //megkérdezzük mennyit
                                        Console.WriteLine($"Mennyi előleget szeretne fizetni? Teljes ár: {utazasok[utazasindex].GetAr()} Ft");
                                        int mennyit = int.Parse(Console.ReadLine());
                                        //ha a mennyiség több mint amennyi az ár akkor kifizeti az egész árat
                                        if (mennyit > int.Parse(utazasok[utazasindex].GetAr()))
                                        {
                                            utasok[jelentkezoindex].SetEloleg(int.Parse(utazasok[utazasindex].GetAr()), utazasok[utazasindex].GetUticel());
                                        }
                                        else//ha kevesebb, akkor az előleg ként tárolódik
                                        {
                                            utasok[jelentkezoindex].SetEloleg(mennyit, utazasok[utazasindex].GetUticel());
                                        }
                                        Console.WriteLine(Disz() + "\n");

                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine($"Sikeres jelentkezés! Előleg fizetve: {mennyit} Ft");
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                    //ha nem szeretne
                                    if (szeretne == "n")
                                    {
                                        //akkor az előleg 0
                                        utasok[jelentkezoindex].SetEloleg(0, utazasok[utazasindex].GetUticel());
                                    }
                                }
                                //jelentkeztetjük a fejlhasználót az utazásra
                                utasok[jelentkezoindex].Jelentkezes(utazasok[utazasindex]);
                                utazasok[utazasindex].Jelentkezes(utasok[jelentkezoindex]);
                                //az utasok fájlba kiíratjuk az utasokat, és az utazásokat amikre jelentkeztek
                                StreamWriter ujUtasokfajl = new StreamWriter("utasok.txt");
                                foreach (Utas u in utasok)
                                {
                                    ujUtasokfajl.WriteLine(u.FajlSor());
                                }
                                ujUtasokfajl.Close();
                            }//ha már jelentkezett erre az utazásra, akkor nem engedi őt újra jelentkezni a program
                            else if (utasok[jelentkezoindex].JelentkezettE(utazasok[utazasindex]))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Erre az utazásra már korábban jelentkeztél!");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }
                        else //ha nincs utas az utasok listában
                        {
                            NincsUtas();
                        }
                        Vissza();
                        break;
                    case '7':
                        System.Environment.Exit(0);
                        break;
                    default:
                        Vissza();
                        break;
                }


                Console.ReadKey();
                Console.Clear();
            }
        }
        static string Disz() //dísz
        {
            string disz = "";
            for (int i = 0; i < Console.WindowWidth - 1; i++)
            {
                disz += "─";
            }
            return disz;
        }
        static void Vissza() //vissza a menübe felirat
        {
            Console.WriteLine();
            Console.WriteLine(Disz());
            Console.WriteLine();
            Console.WriteLine($"\t\t\t\tNyomj meg egy gombot, hogy visszatérj a menübe!");
            Console.WriteLine();
            Console.WriteLine(Disz());
        }
        static void NincsUtas() //ha nincs ilyen utas felirat
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(Console.WindowWidth / 2 - 12, 1);
            Console.WriteLine("Nincs ilyen utas!");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    //1db utas adatai
    class Utas
    {
        string nev;
        string cim;
        string telefonszam;
        List<Utazas> jelentkezettutazasok = new List<Utazas>();
        List<int> elolegek = new List<int>();
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
        //Adatok megszerzése
        public string GetAdatok()
        {
            return $"{nev}\t{cim}\t{telefonszam}";
        }
        //Teljes adatlap kííratása az utasról
        public void TeljesAdatlap()
        {
            Console.Clear();
            string disz = "";
            for (int i = 0; i < Console.WindowWidth - 1; i++)
            {
                disz += "─";
            }
            Console.WriteLine(disz);
            Console.WriteLine();
            Console.WriteLine($"\t\tNév:\t{nev}\t\t\tCím:\t{cim}\t\t\tTel.:\t{telefonszam}");
            Console.WriteLine();
            Console.WriteLine(disz);
            //ha jelentkezett utazásokra akkor kiírjuk őket
            if (jelentkezettutazasok.Count > 0)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 - 10, 6);
                Console.WriteLine("Jelentkezett utak");
                Console.SetCursorPosition(32, 7);
                Console.WriteLine("Uticél");
                Console.SetCursorPosition(73, 7);
                Console.WriteLine("Előleg / Ár");
                for (int i = 0; i < jelentkezettutazasok.Count; i++)
                {
                    Console.SetCursorPosition(30, 9 + i);
                    Console.WriteLine($"{jelentkezettutazasok[i].GetUticel()}");
                    Console.SetCursorPosition(70, 9 + i);
                    if (jelentkezettutazasok[i].GetFizetett(this, i) == int.Parse(jelentkezettutazasok[i].GetAr()))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else if (jelentkezettutazasok[i].GetFizetett(this, i) == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                    }
                    Console.WriteLine($"{jelentkezettutazasok[i].GetFizetett(this, i)} Ft / {jelentkezettutazasok[i].GetAr()} Ft");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            else//ha nem jelentkezett akkor ezt írjuk ki
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 - 27, 6);
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("Ez az utas még egy utazásra sem jelentkezett!");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        //megvizsgáljuk, hogy az utas már jelentkezett-e erre az utazásra
        public bool JelentkezettE(Utazas ut)
        {
            if (jelentkezettutazasok.Contains(ut))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //jelentkezés egy utazásra
        public void Jelentkezes(Utazas ut)
        {
            jelentkezettutazasok.Add(ut);
        }
        //előleg lekérése
        public int GetEloleg(int hanyadik)
        {
            return elolegek[hanyadik];
        }
        public void ElolegBetoltes(int mennyit)
        {
            elolegek.Add(mennyit);
        }
        //előleg beállítása
        public void SetEloleg(int mennyit, string uticel)
        {
            int hanyadik = jelentkezettutazasok.Count;
            for (int i = 0; i < jelentkezettutazasok.Count; i++)
            {
                if (jelentkezettutazasok[i].GetUticel() == uticel)
                {
                    hanyadik = i;
                    break;
                }
            }
            if (elolegek.Count > 0 && hanyadik != jelentkezettutazasok.Count)
            {
                elolegek[hanyadik] = mennyit;
            }
            else
            {
                elolegek.Add(mennyit);
            }
        }
        //jelentkezett utazások lekérése
        public string GetJelentkezesek()
        {
            string jelentkezett = "";
            for (int i = 0; i < jelentkezettutazasok.Count; i++)
            {
                jelentkezett += $"\t{jelentkezettutazasok[i].GetUticel()}";
            }
            return jelentkezett;
        }
        //jelentkezett utazások lekérése listában
        public List<Utazas> GetJelentkezesekLista()
        {
            return jelentkezettutazasok;
        }
        //sor létrehozása a fájlba íráshoz
        public string FajlSor()
        {
            string kimenet = $"{nev}\t{cim}\t{telefonszam}";
            for (int i = 0; i < jelentkezettutazasok.Count; i++)
            {
                kimenet += $"\t{jelentkezettutazasok[i].GetUticel()}\t{elolegek[i]}";
            }
            return kimenet;
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
        //utazás adatainak lekérése
        public string GetAdatok()
        {
            return $"{uticel}\t{ar}\t{maxletszam}";
        }
        //uticél lekérése
        public string GetUticel()
        {
            return $"{uticel}";
        }
        //ár lekérése
        public string GetAr()
        {
            return $"{ar}";
        }
        //fizetett rész lekérése
        public int GetFizetett(Utas utas, int hanyadik)
        {
            return utas.GetEloleg(hanyadik);
        }
        //létszámok lekérése
        public int JelentkezettLetszam()
        {
            return jelentkezettek.Count;
        }
        public void Jelentkezes(Utas u)
        {
            if (!jelentkezettek.Contains(u))
            {
                jelentkezettek.Add(u);
            }
        }
        public int MaxLetszam()
        {
            return maxletszam;
        }
        //Jelentkezett utasok listájának lekérése
        public List<Utas> Jelentkezettek()
        {

            return jelentkezettek;
        }
    }
}
