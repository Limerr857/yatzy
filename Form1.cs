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

        // Skapa en int som sparar spelarens total poäng
        // Återställs till 0 när spelet startas
        int spelarPoäng;

        // Skapa en int som räknar hur många kast spelaren har kvar
        // Återställs till 3 när spelet startas
        int kastKvar;

        // Skapa en bool som sparar om spelaren har valt ett specialfall och fått poäng för det.
        // Återställs till false när spelet startas
        bool fåttPoäng;

        // En bool som sparar om spelaren har fått bonuspoäng då de fick >63p sammanlagt från Ettor, Tvåor osv.
        // Återställs till false när spelet startas
        bool fåttBonuspoäng;

        // Sparar hur många poäng som spelaren fått från Ettor, Tvåor osv. för att beräkna om spelaren borde få bonus.
        // Återställs till 0 när spelet startas
        int enklaPoäng;

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
                KastaTärningarna(tärningarGrafik, tärningarVärde);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LaddaSpelet();
        }

        private void KastaTärningarna(Image[] tärningarGrafik, int[] tärningarVärde)
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

                // Om alla tärningar har slutat rulla, stoppa loopen
                if (!tärningarStatus.Contains(2))
                {
                    rullar = false;
                }
            }

            // Sätt på "Rulla!" knappen efter tärningarna har rullats
            btnRulla.Enabled = true;

            // Och återställ tärningarnas bakgrundsfärg
            SlumpaTärningsfärg(true, tärningarStatus);

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

            // Avsluta spelet om spelaren inte har några fler valmöjligheter.
            // (Inga kast kvar och inga valbara specialfall)
            if (kastKvar == 0 && MöjligaSpecialfall(tärningarVärde).Count == 0)
            {
                AvslutaSpelet(false, spelarPoäng, fåttBonuspoäng);
            }

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

        private void LaddaSpelet()
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

            // Återställ om spelaren fått bonuspoäng
            fåttBonuspoäng = false;

            // Återställ spelarens poäng från Ettor, Tvåor osv. som används för att beräkna om spelaren borde få bonuspoäng
            enklaPoäng = 0;

            ÅterställAnvändaSpecialfall();

            // Återställ de använda specialfallen i chlbxMöjligheter
            for (int i = 0; i < chlbxMöjligheter.Items.Count; i++)
            {
                chlbxMöjligheter.SetItemChecked(i, false);
            }

            // Återställ lblPoäng
            lblPoäng.Text = "Spelarpoäng: 0";
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

            // Räkna det vanligaste värdet i tempTärningarVärde
            RäknaVanligasteVärdet(out int vanligastRäkna, out int vanligastSiffra, tempTärningarVärde);

            // Räkna den andra vanligaste värdet i tempTärningarVärde
            RäknaAndraVärdet(out int andraVanligastRäkna, out int andraVanligastSiffra, tempTärningarVärde, vanligastSiffra);

            // Skapa en bool som är sann i slutet om alla if-satser nedan är falska
            bool ingaSpecial = true;

            // Om det har blivit Yatzy
            if (vanligastRäkna == 5)
            {
                // Lägg in att det blivit Yatzy i specialfall om det inte tidigare blivit valt
                if (!användaSpecialfall["Yatzy"])
                {
                    specialfall.Add("Yatzy");
                }

                // Och spara att det har skett ett specialfall
                ingaSpecial = false;
            }

            // Om det blivit Fyrtal
            if (vanligastRäkna >= 4)
            {
                if (!användaSpecialfall["Fyrtal"])
                {
                    specialfall.Add("Fyrtal");
                }

                ingaSpecial = false;
            }

            // Om det blivit Tretal 
            if (vanligastRäkna >= 3)
            {
                if (!användaSpecialfall["Tretal"])
                {
                    specialfall.Add("Tretal");
                }

                ingaSpecial = false;
            }

            // Om det blivit Par (2 lika) 
            if (vanligastRäkna >= 2)
            {
                if (!användaSpecialfall["Par"])
                {
                    specialfall.Add("Par");
                }

                ingaSpecial = false;
            }

            // Om det blivit kåk (3 + 2 lika)
            if (vanligastRäkna == 3 && andraVanligastRäkna == 2)
            {
                if (!användaSpecialfall["kåk"])
                {
                    specialfall.Add("kåk");
                }

                ingaSpecial = false;

                // Lägg dessutom in två varianter av "Par" om det inte tidigare blivit valt
                // så att båda paren kan väljas individuellt.
                if (!användaSpecialfall["Par"])
                {
                    // Ta allra först bort "Par"
                    specialfall.Remove("Par");

                    specialfall.Add("Par Stor");
                    specialfall.Add("Par Liten");
                }
            }

            // Om det blivit Två Par (2 + 2 lika)
            if (vanligastRäkna >= 2 && andraVanligastRäkna == 2)
            {
                if (!användaSpecialfall["Två Par"])
                {
                    specialfall.Add("Två Par");
                }

                ingaSpecial = false;

                // Lägg dessutom in två varianter av "Par" om det inte tidigare blivit valt
                // så att båda paren kan väljas individuellt.
                if (!användaSpecialfall["Par"])
                {
                    // Ta allra först bort "Par"
                    specialfall.Remove("Par");

                    specialfall.Add("Par Stor");
                    specialfall.Add("Par Liten");
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
            if (tempTärningarVärde.Contains(1) && !användaSpecialfall["Ettor"])
            {
                specialfall.Add("Ettor");
            }

            // Om minst en tvåa förekommer och det inte tidigare blivit valt
            if (tempTärningarVärde.Contains(2) && !användaSpecialfall["Tvåor"])
            {
                specialfall.Add("Tvåor");
            }

            // Om minst en trea förekommer och det inte tidigare blivit valt
            if (tempTärningarVärde.Contains(3) && !användaSpecialfall["Treor"])
            {
                specialfall.Add("Treor");
            }

            // Om minst en fyra förekommer och det inte tidigare blivit valt
            if (tempTärningarVärde.Contains(4) && !användaSpecialfall["Fyror"])
            {
                specialfall.Add("Fyror");
            }

            // Om minst en femma förekommer och det inte tidigare blivit valt
            if (tempTärningarVärde.Contains(5) && !användaSpecialfall["Femmor"])
            {
                specialfall.Add("Femmor");
            }

            // Om minst en sexa förekommer och det inte tidigare blivit valt
            if (tempTärningarVärde.Contains(6) && !användaSpecialfall["Sexor"])
            {
                specialfall.Add("Sexor");
            }

            // Ge alltid en sista chans enligt spelreglerna om det inte tidigare blivit valt
            if (!användaSpecialfall["Chans"])
            {
                specialfall.Add("Chans");
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

            // Hitta och räkna den vanligaste siffran
            RäknaVanligasteVärdet(out int vanligastRäkna, out int vanligastSiffra, tempTärningarVärde);

            // Hitta och räkna den andra vanligaste siffran
            RäknaAndraVärdet(out int andraVanligastRäkna, out int andraVanligastSiffra, tempTärningarVärde, vanligastSiffra);

            // Beräkna spelarens poäng och sätt vilket specialfall som faktiskt valts
            switch (chlbxSpecialfall.CheckedItems[0].ToString())
            {
                case "Yatzy":
                    poängRunda = 50;
                    användaSpecialfall["Yatzy"] = true;
                    break;
                case "Chans":
                    poängRunda = tempTärningarVärde.Sum();
                    användaSpecialfall["Chans"] = true;
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
                case "Fyrtal":
                    poängRunda = 4 * vanligastSiffra;
                    användaSpecialfall["Fyrtal"] = true;
                    break;
                case "Tretal":
                    poängRunda = 3 * vanligastSiffra;
                    användaSpecialfall["Tretal"] = true;
                    break;
                case "Två Par":
                    poängRunda = 2 * vanligastSiffra + 2 * andraVanligastSiffra;
                    användaSpecialfall["Två Par"] = true;
                    break;
                case "Par":
                    poängRunda = 2 * vanligastSiffra;
                    användaSpecialfall["Par"] = true;
                    break;
                case "Par Stor":
                    poängRunda = 2 * vanligastSiffra;
                    användaSpecialfall["Par"] = true;
                    break;
                case "Par Liten":
                    poängRunda = 2 * andraVanligastSiffra;
                    användaSpecialfall["Par"] = true;
                    break;
                case "Sexor":
                    poängRunda = 6 * RäknaSiffror(6, tempTärningarVärde);
                    användaSpecialfall["Sexor"] = true;
                    // Lägg till poäng som används för att beräkna om spelaren
                    // borde få en bonus eller inte
                    enklaPoäng += poängRunda;
                    break;
                case "Femmor":
                    poängRunda = 5 * RäknaSiffror(5, tempTärningarVärde);
                    användaSpecialfall["Femmor"] = true;
                    enklaPoäng += poängRunda;
                    break;
                case "Fyror":
                    poängRunda = 4 * RäknaSiffror(4, tempTärningarVärde);
                    användaSpecialfall["Fyror"] = true;
                    enklaPoäng += poängRunda;
                    break;
                case "Treor":
                    poängRunda = 3 * RäknaSiffror(3, tempTärningarVärde);
                    användaSpecialfall["Treor"] = true;
                    enklaPoäng += poängRunda;
                    break;
                case "Tvåor":
                    poängRunda = 2 * RäknaSiffror(2, tempTärningarVärde);
                    användaSpecialfall["Tvåor"] = true;
                    enklaPoäng += poängRunda;
                    break;
                case "Ettor":
                    poängRunda = 1 * RäknaSiffror(1, tempTärningarVärde);
                    användaSpecialfall["Ettor"] = true;
                    enklaPoäng += poängRunda;
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

        private void RäknaVanligasteVärdet(out int vanligastRäkna, out int vanligastSiffra, int[] tempTärningarVärde) 
        {
            // Skapa två variabler 
            // Som sparar hur ofta den vanligaste siffran förekommer
            // och vilken den vanligaste siffran är
            vanligastRäkna = 0;
            vanligastSiffra = 0;

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

        private void RäknaAndraVärdet(out int andraVanligastRäkna, out int andraVanligastSiffra, int[] tempTärningarVärde, int vanligastSiffra)
        {
            // Skapa två variabler 
            // Som sparar hur ofta den andra vanligaste siffran förekommer
            // och vilken den andra vanligaste siffran är
            andraVanligastRäkna = 0;
            andraVanligastSiffra = 0;

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
            användaSpecialfall.Add("Yatzy", false);
            användaSpecialfall.Add("Chans", false);
            användaSpecialfall.Add("stor stege", false);
            användaSpecialfall.Add("liten stege", false);
            användaSpecialfall.Add("kåk", false);
            användaSpecialfall.Add("Fyrtal", false);
            användaSpecialfall.Add("Tretal", false);
            användaSpecialfall.Add("Två Par", false);
            användaSpecialfall.Add("Par", false);
            användaSpecialfall.Add("Sexor", false);
            användaSpecialfall.Add("Femmor", false);
            användaSpecialfall.Add("Fyror", false);
            användaSpecialfall.Add("Treor", false);
            användaSpecialfall.Add("Tvåor", false);
            användaSpecialfall.Add("Ettor", false);
        }

        private void UppdateraAnvändaSpecialfall(Dictionary<string, bool> specialfall, bool fåttBonuspoäng)
        {
            // En metod som uppdaterar de använda specialfallen i chlbxMöjligheter

            if (specialfall["Yatzy"] == true)
            {
                // Bocka av Yatzy från möjliga specialfall om det används
                chlbxMöjligheter.SetItemChecked(15, true);
            }
            if (specialfall["Chans"])       { chlbxMöjligheter.SetItemChecked(14, true); }
            if (specialfall["stor stege"])  { chlbxMöjligheter.SetItemChecked(13, true); }
            if (specialfall["liten stege"]) { chlbxMöjligheter.SetItemChecked(12, true); }
            if (specialfall["kåk"])         { chlbxMöjligheter.SetItemChecked(11, true); }
            if (specialfall["Fyrtal"])      { chlbxMöjligheter.SetItemChecked(10, true); }
            if (specialfall["Tretal"])      { chlbxMöjligheter.SetItemChecked(9, true); }
            if (specialfall["Två Par"])     { chlbxMöjligheter.SetItemChecked(8, true); }
            if (specialfall["Par"])     { chlbxMöjligheter.SetItemChecked(7, true); }
            // Sätt bonus som "valt" om spelaren fått bonusen
            if (fåttBonuspoäng)             { chlbxMöjligheter.SetItemChecked(6, true); }
            if (specialfall["Sexor"])       { chlbxMöjligheter.SetItemChecked(5, true); }
            if (specialfall["Femmor"])      { chlbxMöjligheter.SetItemChecked(4, true); }
            if (specialfall["Fyror"])       { chlbxMöjligheter.SetItemChecked(3, true); }
            if (specialfall["Treor"])       { chlbxMöjligheter.SetItemChecked(2, true); }
            if (specialfall["Tvåor"])       { chlbxMöjligheter.SetItemChecked(1, true); }
            if (specialfall["Ettor"])       { chlbxMöjligheter.SetItemChecked(0, true); }            

        }

        private void AvslutaSpelet(bool vinst, int spelarpoäng, bool fåttBonuspoäng)
        {
            // Metoden körs när spelaren antingen vunnit eller förlorat,
            // och återställer allt, beräknar slutpoängen och visar vinst/förlorarmeddelande


            // Beräkna spelarens slutpoäng
            int slutpoäng = spelarpoäng;
            if (fåttBonuspoäng)
            {
                slutpoäng += 50;
            }

            // Vinstmeddelande
            // Vänta först en liten stund för att låta spelaren reflektera över vad de har gjort
            Thread.Sleep(1000);
            if (vinst)
            {
                MessageBox.Show("Du vann!, Slutpoäng: " + slutpoäng);
            }
            else
            {
                // Vänta ännu lite längre för att lära spelaren en läxa
                Thread.Sleep(1000);
                MessageBox.Show("Du fick slut på valmöjligheter!, Slutpoäng: " + slutpoäng);
            }

            // Återställer spelet
            LaddaSpelet();
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
            // OCH om spelaren inte har fått poäng tidigare under rundan
            if (counter == 1 && !fåttPoäng)
            {
                // Lägger till poängen som tjänades under rundan
                spelarPoäng += RundPoäng(tärningarVärde);

                // Ger spelaren bonuspoäng om spelaren gjort sig förtjänt till det
                if (enklaPoäng > 63)
                {
                    fåttBonuspoäng = true;
                }

                // Uppdaterar lblPoäng
                if (fåttBonuspoäng)
                {
                    lblPoäng.Text = "Spelarpoäng: " + spelarPoäng + "+50";
                }
                else
                {
                    lblPoäng.Text = "Spelarpoäng: " + spelarPoäng;
                }
                

                // Spelaren har nu fått poäng för denna "runda", spara detta
                fåttPoäng = true;

                // Uppdatera specialfallen som har blivit använda
                UppdateraAnvändaSpecialfall(användaSpecialfall, fåttBonuspoäng);

                // Återställer spelet för nästa runda
                RundanKlar();

                // Avsluta spelet om spelaren har gjort klart alla specialfall.
                // (Om värdet på alla specialfall i användaSpecialfall är true)
                if (användaSpecialfall.Values.All(value => value))
                {
                    AvslutaSpelet(true, spelarPoäng, fåttBonuspoäng);
                }
            }
        }
    }
}

