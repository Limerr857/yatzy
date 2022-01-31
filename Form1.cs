using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yatzy_1
{
    public partial class Form1 : Form
    {
        // Viktiga variabler som kommer att användas genom hela programmet
        // Skapa en array som innehåller tärningsgrafiken
        Image[] tärningarGrafik = new Image[] { tärningsgrafik.d0, tärningsgrafik.d1,
                                                tärningsgrafik.d2, tärningsgrafik.d3,
                                                tärningsgrafik.d4, tärningsgrafik.d5,
                                                tärningsgrafik.d6 };

        // Skapa en array som innehåller tärningarnas värde
        // Siffran som visas på tärningen 0 = ingen siffra
        int[] tärningarVärde = new int[5];

        // Skapa en array som innehåller tärningarnas status
        // 0 = ej vald, 1 = vald, 2 = rullar
        int[] tärningarStatus = new int[5];

        // Skapa en slumpgenerator
        Random rand = new Random();

        // Skapa en array med alla färger i KnownColor
        KnownColor[] färgnamn = (KnownColor[])Enum.GetValues(typeof(KnownColor));

        // Skapa en lista med array:er av alla tidigare tärningsslag
        // TODO: Ta bort??
        List<Array> historikLista = new List<Array>();

        // Skapa en int som sparar spelarens total poäng
        int spelarPoäng = 0;

        // Skapa en int som räknar hur många kast spelaren har kvar
        int kastKvar = 3;

        // Skapa en bool som sparar om spelaren har valt ett specialfall och fått poäng för det.
        bool fåttPoäng = false;

        // Skapa en dict som sparar de specialfall som har använts sedan spelet startades
        Dictionary<string, bool> användaSpecialfall = new Dictionary<string, bool>();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnRulla_Click(object sender, EventArgs e)
        {
            // Rulla bara tärningarna om spelaren har kast kvar
            if (kastKvar > 0)
            {
                KastaTärningarna(tärningarGrafik, tärningarVärde, historikLista);

                // TODO: ta bort???
                UppdateraGrafik(tärningarGrafik, tärningarVärde);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LaddaSpelet(tärningarVärde, tärningarStatus, tärningarGrafik, historikLista);
        }

        private void KastaTärningarna(Image[] tärningarGrafik, int[] tärningarVärde, List<Array> historikLista)
        {
            // Sätt alla tärningars status till "Rullar" (2)
            // om deras status är "inte vald" (0)
            for (int i = 0; i < 5; i++)
            {
                if (tärningarStatus[i] == 0)
                {
                    tärningarStatus[i] = 2;
                }
            }

            // Återställ tärningarnas markeringar, bara visuellt
            pctTärning1.BorderStyle = BorderStyle.None;
            pctTärning2.BorderStyle = BorderStyle.None;
            pctTärning3.BorderStyle = BorderStyle.None;
            pctTärning4.BorderStyle = BorderStyle.None;
            pctTärning5.BorderStyle = BorderStyle.None;

            // Stäng av "Rulla!" knappen
            btnRulla.Enabled = false;

            // Ta bort ett kast från kasträknaren
            kastKvar--;

            // Uppdatera lblSlagKvar med det nya värdet på kastKvar
            lblSlagKvar.Text = "Slag kvar: " + kastKvar;

            // Återställ huruvida spelaren har fått poäng
            fåttPoäng = false;

            // Loopen kan starta
            bool rullar = true;
            // Starta en loop med tärningsgrafik
            while (rullar)
            {

                // Om alla tärningar har slutat rulla, stoppa loopen
                if (!tärningarStatus.Contains(2))
                {
                    rullar = false;
                }

                // Byt nummer på alla tärningar slumpmässigt
                for (int i = 0; i < 5; i++)
                {
                    // Men bara om tärningen har statusen "Rullar"
                    if (tärningarStatus[i] == 2)
                    {
                        tärningarVärde[i] = rand.Next(1, 7);
                        SlumpaTärningsfärg(false, tärningarStatus);
                    }
                }

                // Uppdatera tärningsgrafiken
                UppdateraGrafik(tärningarGrafik, tärningarVärde);

                // Vänta en liten stund
                Thread.Sleep(50);

                // Sluta rulla den första rullande tärningen med 1/10 chans varje "frame"
                if (rand.Next(10) == 9)
                {
                    // Iterera över alla tärningars status
                    for (int i = 0; i < 5; i++)
                    {
                        // Om tärningen har statusen rullar
                        if (tärningarStatus[i] == 2)
                        {
                            // Stoppa tärningen från att rulla
                            tärningarStatus[i] = 0;

                            // Bryt for-loopen
                            break;
                        }
                    }
                }
            }

            // Sätt på "Rulla!" knappen efter tärningarna har rullats
            btnRulla.Enabled = true;

            // Och återställ tärningarnas bakgrundsfärg
            SlumpaTärningsfärg(true, tärningarStatus);

            // Lägg till resultatet i historiken
            historikLista.Add(tärningarVärde);

            // Efter tärningarna har rullats kan alla tärningarnas status
            // återställas till "inte vald" (0)
            for (int i = 0; i < 5; i++)
            {
                tärningarStatus[i] = 0;
            }

            // Rensa specialfallen i chlbx för att kunna lägga in nya
            chlbxSpecialfall.Items.Clear();

            // Lägg in de möjliga specialfallen i chlbx genom metoden MöjligaSpecialfall
            chlbxSpecialfall.Items.AddRange(MöjligaSpecialfall(tärningarVärde).ToArray());

        }

        private void UppdateraGrafik(Image[] tärningarGrafik, int[] tärningarVärde)
        {
            // Sätt varje tärnings bild till dess värde
            pctTärning1.Image = tärningarGrafik[tärningarVärde[0]];
            pctTärning2.Image = tärningarGrafik[tärningarVärde[1]];
            pctTärning3.Image = tärningarGrafik[tärningarVärde[2]];
            pctTärning4.Image = tärningarGrafik[tärningarVärde[3]];
            pctTärning5.Image = tärningarGrafik[tärningarVärde[4]];

            // Tvinga alla tärningar att uppdatera sina bilder
            pctTärning1.Refresh();
            pctTärning2.Refresh();
            pctTärning3.Refresh();
            pctTärning4.Refresh();
            pctTärning5.Refresh();

            // Återställ tärningarnas borderstyle,
            // avmarkerar alla tärningar
            pctTärning1.BorderStyle = BorderStyle.None;
            pctTärning2.BorderStyle = BorderStyle.None;
            pctTärning3.BorderStyle = BorderStyle.None;
            pctTärning4.BorderStyle = BorderStyle.None;
            pctTärning5.BorderStyle = BorderStyle.None;
        }

        private void LaddaSpelet(int[] tärningarVärde, int[] tärningarStatus, Image[] tärningarGrafik, List<Array> historikLista)
        {
            // Återställ array:en som innehåller tärningarnas värde
            // Siffran som visas på tärningen 0 = ingen siffra
            for (int i = 0; i < tärningarVärde.Length; i++)
            {
                tärningarVärde[i] = 0;
            }

            // Återställ array:en som innehåller tärningarnas status
            // 0 = ej vald, 1 = vald, 2 = rulla
            for (int i = 0; i < tärningarStatus.Length; i++)
            {

                tärningarStatus[i] = 0;
            }

            // Återställ tärningsgrafiken
            UppdateraGrafik(tärningarGrafik, tärningarVärde);

            // Återställ tärningshistoriken
            historikLista.Clear();

            // Återställ tärningarnas borderstyle,
            // avmarkerar alla tärningar
            pctTärning1.BorderStyle = BorderStyle.None;
            pctTärning2.BorderStyle = BorderStyle.None;
            pctTärning3.BorderStyle = BorderStyle.None;
            pctTärning4.BorderStyle = BorderStyle.None;
            pctTärning5.BorderStyle = BorderStyle.None;

            // Återställ spelarpoängen
            spelarPoäng = 0;

            // Återställ antal kast kvar
            kastKvar = 3;

            // Återställ huruvida spelaren har fått poäng
            fåttPoäng = false;

            ÅterställAnvändaSpecialfall();
        }

        private void SlumpaTärningsfärg(bool återställ, int[] tärningarStatus)
        {
            // Om färgerna ska återställas, gör det och avsluta
            if (återställ)
            {
                pctTärning1.BackColor = Control.DefaultBackColor;
                pctTärning2.BackColor = Control.DefaultBackColor;
                pctTärning3.BackColor = Control.DefaultBackColor;
                pctTärning4.BackColor = Control.DefaultBackColor;
                pctTärning5.BackColor = Control.DefaultBackColor;
                return;
            }

            // Ger alla tärningarna en bakgrundsfärg slumpmässigt om de har statusen
            // "rullar" (2)
            if (tärningarStatus[0] == 2)
            {
                pctTärning1.BackColor = Color.FromKnownColor(färgnamn[rand.Next(färgnamn.Length)]);
            }
            if (tärningarStatus[1] == 2)
            {
                pctTärning2.BackColor = Color.FromKnownColor(färgnamn[rand.Next(färgnamn.Length)]);
            }
            if (tärningarStatus[2] == 2)
            {
                pctTärning3.BackColor = Color.FromKnownColor(färgnamn[rand.Next(färgnamn.Length)]);
            }
            if (tärningarStatus[3] == 2)
            {
                pctTärning4.BackColor = Color.FromKnownColor(färgnamn[rand.Next(färgnamn.Length)]);
            }
            if (tärningarStatus[4] == 2)
            {
                pctTärning5.BackColor = Color.FromKnownColor(färgnamn[rand.Next(färgnamn.Length)]);
            }
        }

        private List<string> MöjligaSpecialfall(int[] TärningarVärde)
        {
            // Metoden ger en lista med alla möjliga specialfall
            // utifrån en viss samling tärningsvärden

            // Kopiera TärningarVärde för att inte ändra på den 
            int[] tempTärningarVärde = (int[])TärningarVärde.Clone();

            // Skapa en lista som sparar de möjliga specialfallen
            List<string> specialfall = new List<string>();

            // Skapa en int som mäter hur många gånger den
            // vanligaste siffran finns i tempTärningarVärde
            int vanligastRäkna = 0;

            // Och en int som sparar värdet på siffran
            int vanligastSiffra = 0;

            // Räkna det vanligaste värdet i tempTärningarVärde
            RäknaVanligasteVärdet(ref vanligastRäkna, ref vanligastSiffra, tempTärningarVärde);

            // Samma sak som vanligast fast med den andra vanligaste siffran
            int andraVanligastRäkna = 0;
            int andraVanligastSiffra = 0;

            // Räkna den andra vanligaste värdet i tempTärningarVärde
            RäknaAndraVärdet(ref andraVanligastRäkna, ref andraVanligastSiffra, tempTärningarVärde, vanligastSiffra);

            // Skapa en bool som är sann i slutet om alla if-satser nedan är falska
            bool ingaSpecial = true;

            // Om det har blivit Yatzy och det inte tidigare blivit valt
            if (vanligastRäkna == 5 && !användaSpecialfall["yatzy"])
            {
                // Lägg in att det blivit Yatzy i specialfall
                specialfall.Add("yatzy");

                // Och spara att det har skett ett specialfall
                ingaSpecial = false;
            }

            // Om det blivit fyrtal och det inte tidigare blivit valt
            if (vanligastRäkna >= 4 && !användaSpecialfall["fyrtal"])
            {
                specialfall.Add("fyrtal");
                ingaSpecial = false;
            }

            // Om det blivit tretal och det inte tidigare blivit valt
            if (vanligastRäkna >= 3 && !användaSpecialfall["tretal"])
            {
                specialfall.Add("tretal");
                ingaSpecial = false;
            }

            // Om det blivit ett par (2 lika) och det inte tidigare blivit valt
            if (vanligastRäkna >= 2 && !användaSpecialfall["ett par"])
            {
                specialfall.Add("ett par");
                ingaSpecial = false;
            }

            // Om det blivit kåk (3 + 2 lika) och det inte tidigare blivit valt
            if (vanligastRäkna == 3 && andraVanligastRäkna == 2 && !användaSpecialfall["kåk"])
            {
                specialfall.Add("kåk");
                ingaSpecial = false;

                // Lägg dessutom in två varianter av "ett par" om det inte tidigare blivit valt
                // så att båda paren kan väljas individuellt.
                if (!användaSpecialfall["ett par"])
                {
                    // Ta allra först bort "ett par"
                    specialfall.Remove("ett par");

                    specialfall.Add("ett par stor");
                    specialfall.Add("ett par liten");
                }
            }

            // Om det blivit två par (2 + 2 lika) och det inte tidigare blivit valt
            if (vanligastRäkna >= 2 && andraVanligastRäkna == 2 && !användaSpecialfall["två par"])
            {
                specialfall.Add("två par");
                ingaSpecial = false;

                // Lägg dessutom in två varianter av "ett par" om det inte tidigare blivit valt
                // så att båda paren kan väljas individuellt.
                if (!användaSpecialfall["ett par"])
                {
                    // Ta allra först bort "ett par"
                    specialfall.Remove("ett par");

                    specialfall.Add("ett par stor");
                    specialfall.Add("ett par liten");
                }
            }

            // Om inga av de tidigare specialfallen har satts,
            // kolla de specialfall som inte kan ha några par
            if (ingaSpecial)
            {
                // Om det blivit liten stege och det inte tidigare blivit valt
                if (!tempTärningarVärde.Contains(6) && !användaSpecialfall["liten stege"])
                {
                    specialfall.Add("liten stege");
                }

                // Om det blivit stor stege och det inte tidigare blivit valt
                // (Om det inte finns en etta inuti)
                if (!tempTärningarVärde.Contains(1) && !användaSpecialfall["stor stege"])
                {
                    specialfall.Add("stor stege");
                }
            }

            // Om minst en etta förekommer och det inte tidigare blivit valt
            if (tempTärningarVärde.Contains(1) && !användaSpecialfall["ettor"])
            {
                specialfall.Add("ettor");
            }

            // Om minst en tvåa förekommer och det inte tidigare blivit valt
            if (tempTärningarVärde.Contains(2) && !användaSpecialfall["tvåor"])
            {
                specialfall.Add("tvåor");
            }

            // Om minst en trea förekommer och det inte tidigare blivit valt
            if (tempTärningarVärde.Contains(3) && !användaSpecialfall["treor"])
            {
                specialfall.Add("treor");
            }

            // Om minst en fyra förekommer och det inte tidigare blivit valt
            if (tempTärningarVärde.Contains(4) && !användaSpecialfall["fyror"])
            {
                specialfall.Add("fyror");
            }

            // Om minst en femma förekommer och det inte tidigare blivit valt
            if (tempTärningarVärde.Contains(5) && !användaSpecialfall["femmor"])
            {
                specialfall.Add("femmor");
            }

            // Om minst en sexa förekommer och det inte tidigare blivit valt
            if (tempTärningarVärde.Contains(6) && !användaSpecialfall["sexor"])
            {
                specialfall.Add("sexor");
            }

            // Ge alltid en sista chans enligt spelreglerna om det inte tidigare blivit valt
            if (!användaSpecialfall["chans"])
            {
                specialfall.Add("chans");
            }

            // return poäng, specialfall
            return specialfall;
        }

        private void VäljTärning(int tärning, PictureBox pictureBox)
        {
            // Metoden startas när en av tärningarna klickas på
            // och ser till så att tärningen väljs om det är möjligt


            // Förhindra spelaren från att hålla en tärning om denne just har fått poäng
            if (!fåttPoäng)
            {
                switch (tärningarStatus[tärning])
                {
                    // Tärningen är inte vald
                    case 0:
                        // Sätt på en "3D effekt" som markerar knappen som vald
                        pictureBox.BorderStyle = BorderStyle.Fixed3D;

                        // Sätt tärningens status till "vald" (1)
                        tärningarStatus[tärning] = 1;

                        // Avsluta programmet
                        return;

                    // Tärningen är vald
                    case 1:
                        // Stäng av "3D effekten"
                        pictureBox.BorderStyle = BorderStyle.None;

                        // Sätt tärningens status till "ej vald" (0)
                        tärningarStatus[tärning] = 0;

                        // Avsluta programmet
                        return;

                    // Tärningen rullar
                    case 2:
                        // Gör ingenting
                        return;

                    default:
                        break;
                }
            }
        }

        private int RundPoäng(int[] tempTärningarVärde)
        {
            // Metoden räknar och ger poäng till spelaren beroende på
            // vilket specialfall som är vald.

            // Spara spelarens poäng denna runda i en variabel
            int poängRunda = 0;

            // Hitta och räkna den vanligaste siffran TODO: FLytta upp här och vid ett annat ställe till toppen av dokumentet
            int vanligastRäkna = 0;
            int vanligastSiffra = 0;
            RäknaVanligasteVärdet(ref vanligastRäkna, ref vanligastSiffra, tempTärningarVärde);

            // Hitta och räkna den andra vanligaste siffran TODO: FLytta upp här och vid ett annat ställe till toppen av dokumentet
            int andraVanligastRäkna = 0;
            int andraVanligastSiffra = 0;
            RäknaAndraVärdet(ref andraVanligastRäkna, ref andraVanligastSiffra, tempTärningarVärde, vanligastSiffra);

            // Beräkna spelarens poäng och sätt vilket specialfall som faktiskt valts
            switch (chlbxSpecialfall.CheckedItems[0].ToString())
            {
                case "yatzy":
                    poängRunda = 50;
                    användaSpecialfall["yatzy"] = true;
                    break;
                case "chans":
                    poängRunda = tempTärningarVärde.Sum();
                    användaSpecialfall["chans"] = true;
                    break;
                case "stor stege":
                    poängRunda = tempTärningarVärde.Sum();
                    användaSpecialfall["stor stege"] = true;
                    break;
                case "liten stege":
                    poängRunda = tempTärningarVärde.Sum();
                    användaSpecialfall["liten stege"] = true;
                    break;
                case "kåk":
                    poängRunda = tempTärningarVärde.Sum();
                    användaSpecialfall["kåk"] = true;
                    break;
                case "fyrtal":
                    poängRunda = 4 * vanligastSiffra;
                    användaSpecialfall["fyrtal"] = true;
                    break;
                case "tretal":
                    poängRunda = 3 * vanligastSiffra;
                    användaSpecialfall["tretal"] = true;
                    break;
                case "två par":
                    poängRunda = 2 * vanligastSiffra + 2 * andraVanligastSiffra;
                    användaSpecialfall["två par"] = true;
                    break;
                case "ett par":
                    poängRunda = 2 * vanligastSiffra;
                    användaSpecialfall["ett par"] = true;
                    break;
                case "ett par stor":
                    poängRunda = 2 * vanligastSiffra;
                    användaSpecialfall["ett par"] = true;
                    break;
                case "ett par liten":
                    poängRunda = 2 * andraVanligastSiffra;
                    användaSpecialfall["ett par"] = true;
                    break;
                case "sexor":
                    poängRunda = 6 * RäknaSiffror(6, tempTärningarVärde);
                    användaSpecialfall["sexor"] = true;
                    break;
                case "femmor":
                    poängRunda = 5 * RäknaSiffror(5, tempTärningarVärde);
                    användaSpecialfall["femmor"] = true;
                    break;
                case "fyror":
                    poängRunda = 4 * RäknaSiffror(4, tempTärningarVärde);
                    användaSpecialfall["fyror"] = true;
                    break;
                case "treor":
                    poängRunda = 3 * RäknaSiffror(3, tempTärningarVärde);
                    användaSpecialfall["treor"] = true;
                    break;
                case "tvåor":
                    poängRunda = 2 * RäknaSiffror(2, tempTärningarVärde);
                    användaSpecialfall["tvåor"] = true;
                    break;
                case "ettor":
                    poängRunda = 1 * RäknaSiffror(1, tempTärningarVärde);
                    användaSpecialfall["ettor"] = true;
                    break;

                default:
                    break;
            }

            return poängRunda;
        }

        private void RundanKlar()
        {
            // Återställ kasten
            kastKvar = 3;

            // Uppdatera kasträknaren
            lblSlagKvar.Text = "Slag kvar: " + kastKvar;

            // Återställ tärningarnas status
            for (int i = 0; i < 5; i++)
            {
                tärningarStatus[i] = 0;
            }

            // Uppdatera tärningsgrafiken
            UppdateraGrafik(tärningarGrafik, tärningarVärde);
        }

        private int RäknaSiffror(int siffra, int[] tempTärningarVärde)
        {
            // Räknar antal instanser av en viss siffra i listan tempTärningarVärde
            int antal = 0;

            for (int i = 0; i < tempTärningarVärde.Length; i++)
            {
                if (tempTärningarVärde[i] == siffra) { antal++; }
            }

            return antal;
        }

        private void RäknaVanligasteVärdet(ref int vanligastRäkna, ref int vanligastSiffra, int[] tempTärningarVärde) 
        {
            // Och beräkna båda variablarnas värden
            for (int i = 0; i < tempTärningarVärde.Count(); i++)
            {
                // Om det finns fler av *i* inuti tempTärningarVärde än tidigare *i*
                if (tempTärningarVärde.Count(c => c == tempTärningarVärde[i]) > vanligastRäkna)
                {
                    // Spara hur många gånger *i* finns inuti tempTärningarVärde
                    vanligastRäkna = tempTärningarVärde.Count(c => c == tempTärningarVärde[i]);

                    // Spara värdet på *i*
                    vanligastSiffra = tempTärningarVärde[i];
                }
            }
            return;
        }

        private void RäknaAndraVärdet(ref int andraVanligastRäkna, ref int andraVanligastSiffra, int[] tempTärningarVärde, int vanligastSiffra)
        {
            // Skapa en ny array från tempTärningarVärde
            int[] tempTärningarVärdeAndra = (int[])tempTärningarVärde.Clone();

            // Ta bort den vanligaste siffran från tempTärningarVärdeAndra
            tempTärningarVärdeAndra = tempTärningarVärdeAndra.Where(val => val != vanligastSiffra).ToArray();

            // Beräkna båda variablarnas värden
            for (int i = 0; i < tempTärningarVärdeAndra.Count(); i++)
            {
                // Om det finns fler av *i* inuti tempTärningarVärdeAndra än tidigare *i*
                if (tempTärningarVärdeAndra.Count(c => c == tempTärningarVärdeAndra[i]) > andraVanligastRäkna)
                {
                    // Spara hur många gånger *i* finns inuti tempTärningarVärdeAndra
                    andraVanligastRäkna = tempTärningarVärdeAndra.Count(c => c == tempTärningarVärdeAndra[i]);

                    // Spara värdet på *i*
                    andraVanligastSiffra = tempTärningarVärdeAndra[i];
                }
            }
            return;
        }

        public void ÅterställAnvändaSpecialfall()
        {
            användaSpecialfall.Clear();
            användaSpecialfall.Add("yatzy", false);
            användaSpecialfall.Add("chans", false);
            användaSpecialfall.Add("stor stege", false);
            användaSpecialfall.Add("liten stege", false);
            användaSpecialfall.Add("kåk", false);
            användaSpecialfall.Add("fyrtal", false);
            användaSpecialfall.Add("tretal", false);
            användaSpecialfall.Add("två par", false);
            användaSpecialfall.Add("ett par", false);
            användaSpecialfall.Add("sexor", false);
            användaSpecialfall.Add("femmor", false);
            användaSpecialfall.Add("fyror", false);
            användaSpecialfall.Add("treor", false);
            användaSpecialfall.Add("tvåor", false);
            användaSpecialfall.Add("ettor", false);
        }

        private void pctTärning1_Click(object sender, EventArgs e)
        {
            // Förhindra att spelaren håller en tom tärning
            if (tärningarVärde[0] != 0)
            {
                VäljTärning(0, pctTärning1);
            }
        }

        private void pctTärning2_Click(object sender, EventArgs e)
        {
            // Förhindra att spelaren håller en tom tärning
            if (tärningarVärde[1] != 0)
            {
                VäljTärning(1, pctTärning2);
            }
        }

        private void pctTärning3_Click(object sender, EventArgs e)
        {
            // Förhindra att spelaren håller en tom tärning
            if (tärningarVärde[2] != 0)
            {
                VäljTärning(2, pctTärning3);
            }
        }

        private void pctTärning4_Click(object sender, EventArgs e)
        {
            // Förhindra att spelaren håller en tom tärning
            if (tärningarVärde[3] != 0)
            {
                VäljTärning(3, pctTärning4);
            }
        }

        private void pctTärning5_Click(object sender, EventArgs e)
        {
            // Förhindra att spelaren håller en tom tärning
            if (tärningarVärde[4] != 0)
            {
                VäljTärning(4, pctTärning5);
            }
        }

        private void btnKlar_Click(object sender, EventArgs e)
        {
            int counter = 0;
            // Räknar antalet valda checkboxes
            foreach (int check in chlbxSpecialfall.CheckedIndices)
            {
                counter++;
            }

            // Slutför spelet om endast en checkbox är vald i chlbxSpecialfall
            // OCH om spelaren inte har fått poäng tidigare
            if (counter == 1 && !fåttPoäng)
            {
                // Lägger till poängen som tjänades under rundan
                spelarPoäng += RundPoäng(tärningarVärde);

                // Uppdaterar lblPoäng
                lblPoäng.Text = "Spelarpoäng: " + spelarPoäng;

                // Spelaren har nu fått poäng för denna "runda", spara detta
                fåttPoäng = true;

                // Återställer spelet för nästa runda
                RundanKlar();
            }
        }
    }
}

