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

        public Form1()
        {
            InitializeComponent();
        }

        private void btnRulla_Click(object sender, EventArgs e)
        {
            KastaTärningarna(tärningarGrafik, tärningarVärde, historikLista);

            UppdateraGrafik(tärningarGrafik, tärningarVärde);
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

            // TEMP: Sätt in i lbxHisto
            lbxHisto.Items.Add(string.Join(" ", tärningarVärde));

            // TEMP: Sätt in specialfall i lbxTest
            lbxTest.Items.Add(string.Join(", ", MöjligaSpecialfall(tärningarVärde)));

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

            // Skapa en double som mäter hur många gånger den
            // vanligaste siffran finns i tempTärningarVärde
            double vanligastRäkna = 0;

            // Och en double som sparar värdet på siffran
            double vanligastSiffra = 0;

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

            // Samma sak som vanligast fast med den andra vanligaste siffran
            double andraVanligastRäkna = 0;
            double andraVanligastSiffra;

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

            // Skapa en bool som är sann i slutet om alla if-satser nedan är falska
            bool ingaSpecial = true;

            // Om det har blivit Yatzy
            if (vanligastRäkna == 5)
            {
                // Lägg in att det blivit Yatzy i specialfall
                specialfall.Add("yatzy");

                // Och spara att det har skett ett specialfall
                ingaSpecial = false;
            }

            // Om det blivit fyrtal 
            if (vanligastRäkna == 4)
            {
                specialfall.Add("fyrtal");
                ingaSpecial = false;
            }

            // Om det blivit tretal
            if (vanligastRäkna == 3)
            {
                specialfall.Add("tretal");
                ingaSpecial = false;
            }

            // Om det blivit kåk (3 + 2 lika)
            if (vanligastRäkna == 3 && andraVanligastRäkna == 2)
            {
                specialfall.Add("kåk");
                ingaSpecial = false;
            }

            // Om det blivit ett par (2 lika)
            if (vanligastRäkna == 2)
            {
                specialfall.Add("ett par");
                ingaSpecial = false;
            }

            // Om det blivit två par (2 + 2 lika)
            if (vanligastRäkna == 2 && andraVanligastRäkna == 2)
            {
                specialfall.Add("två par");
                ingaSpecial = false;
            }

            // Om inga av de tidigare specialfallen har satts,
            // kolla de specialfall som inte kan ha några par
            if (ingaSpecial)
            {
                // Om det blivit liten stege
                if (!tempTärningarVärde.Contains(6))
                {
                    specialfall.Add("liten stege");
                    ingaSpecial = false;
                }

                // Om det blivit stor stege
                // (Om det inte finns en etta inuti)
                if (!tempTärningarVärde.Contains(1))
                {
                    specialfall.Add("stor stege");
                    ingaSpecial = false;
                }
            }

            // Om inga andra specialfall har satts, ge en sista chans enligt spelreglerna
            if (ingaSpecial)
            {
                specialfall.Add("chans");
            }

            // return poäng, specialfall
            return specialfall;
        }

        private void VäljTärning(int tärning, PictureBox pictureBox)
        {
            // Metoden startas när en av tärningarna klickas på

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

        private int RundPoäng()
        {
            // Metoden räknar och ger poäng till spelaren beroende på
            // vilket specialfall som är vald.

            // Spara spelarens poäng denna runda i en variabel
            int poängRunda = 0;

            if (chlbxSpecialfall.SelectedItem.ToString() == "yatzy")
            {
                poängRunda = 50;
            }

            return poängRunda;
        }

        private void RundanKlar()
        {
            
        }

        private void pctTärning1_Click(object sender, EventArgs e)
        {
            VäljTärning(0, pctTärning1);
        }

        private void pctTärning2_Click(object sender, EventArgs e)
        {
            VäljTärning(1, pctTärning2);
        }

        private void pctTärning3_Click(object sender, EventArgs e)
        {
            VäljTärning(2, pctTärning3);
        }

        private void pctTärning4_Click(object sender, EventArgs e)
        {
            VäljTärning(3, pctTärning4);
        }

        private void pctTärning5_Click(object sender, EventArgs e)
        {
            VäljTärning(4, pctTärning5);
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
            if (counter == 1)
            {
                // Lägger till poängen som tjänades under rundan
                spelarPoäng += RundPoäng();

                // Uppdaterar lblPoäng
                lblPoäng.Text = "Spelarpoäng: " + spelarPoäng;

                // Återställer spelet för nästa runda
                RundanKlar();
            }
        }
    }
}

