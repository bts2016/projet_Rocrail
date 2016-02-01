using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

//Added
using RocrailLib_v4;
using RocrailLib_v4.Elements;
using System.Threading;
using System.Collections;
using System.Net.Sockets;

namespace ProjetRocrail
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static CRocrail Rocrail;
        public MainWindow()
        {
            InitializeComponent();

            // On se connecte au serveur rocrail
            Rocrail = new CRocrail("172.17.50.136", 8051);
            //Rocrail = new CRocrail("127.0.0.1", 8051);

            // On spécifie qu'on utilise le plan
            Rocrail.UsePlan = true;

            // On s'abonne aux évenements 
            Rocrail.PlanLoaded += OnPlanLoaded;
            Rocrail.ElementUpdated += OnElementUpdated;
            Rocrail.ElementAdded += OnElementAdded;

            // On lance le thread de reception des évenements
            Rocrail.StartScrut();

            // On de mande au serveur rocrail le plan
            Rocrail.Client.Send("<model cmd=\"plan\" />");
        }

        private static void OnPlanLoaded(object sender, EventArgs e)
        {
            // Cet évenement est appelé lorsque le plan est chargé par l'élément CRocrail
            // Les éléments du plan sont contenus dans 
            // Rocrail.Plan.<nomListe>
            // <nomListe> : nom de la liste d'éléments (ex: sglist (signaux), lclist (loco))

            // Pour atteindre l'élément k de la liste
            // Rocrail.Plan.<nomListe>.ElementAt(k)

            Console.WriteLine("\n##         Plan         ##");
            Console.WriteLine("\n## Nombre de signaux : " + Rocrail.Plan.sglist.Count);
            for (int k = 0; k < Rocrail.Plan.sglist.Count; k++)
            {
                Console.WriteLine(Rocrail.Plan.sglist.ElementAt(k).id +
                    " blockid:" + Rocrail.Plan.sglist.ElementAt(k).blockid +
                    " state:" + Rocrail.Plan.sglist.ElementAt(k).state +
                    " type:" + Rocrail.Plan.sglist.ElementAt(k).type +
                    " signal:" + Rocrail.Plan.sglist.ElementAt(k).signal);
                Console.WriteLine("routeids:" + Rocrail.Plan.sglist.ElementAt(k).routeids + "\n");
            }

            Console.WriteLine("\n## Nombre d'aiguillage : " + Rocrail.Plan.swlist.Count);
            for (int k = 0; k < Rocrail.Plan.swlist.Count; k++)
            {
                Console.WriteLine(Rocrail.Plan.swlist.ElementAt(k).id +
                   " blockid:" + Rocrail.Plan.swlist.ElementAt(k).blockid +
                   " state:" + Rocrail.Plan.swlist.ElementAt(k).state);
            }

            Console.WriteLine("\n## Nombre de routes : " + Rocrail.Plan.stlist.Count);
            for (int k = 0; k < Rocrail.Plan.stlist.Count; k++)
            {
                Console.WriteLine(Rocrail.Plan.stlist.ElementAt(k).id +
                   " bka:" + Rocrail.Plan.stlist.ElementAt(k).bka + //id du canton de départ
                   " bkb:" + Rocrail.Plan.stlist.ElementAt(k).bkb + //id du canton d'arrivé
                   " bkaside:" + Rocrail.Plan.stlist.ElementAt(k).bkaside +
                   " bkbside:" + Rocrail.Plan.stlist.ElementAt(k).bkbside
                   );
            }

            Console.WriteLine("\n## Nombre de locos : " + Rocrail.Plan.lclist.Count);
            for (int k = 0; k < Rocrail.Plan.lclist.Count; k++)
            {
                Console.WriteLine(Rocrail.Plan.lclist.ElementAt(k).id + " blockid:" + Rocrail.Plan.lclist.ElementAt(k).blockid +
                    " V:" + Rocrail.Plan.lclist.ElementAt(k).V +
                    " blockenterside:" + Rocrail.Plan.lclist.ElementAt(k).blockenterside +
                    " dir:" + Rocrail.Plan.lclist.ElementAt(k).dir);
            }

            Console.WriteLine("\n## Nombre de cantons : " + Rocrail.Plan.bklist.Count);
            for (int k = 0; k < Rocrail.Plan.sglist.Count; k++)
            {
                Console.WriteLine(Rocrail.Plan.bklist.ElementAt(k).id + " speed:" + Rocrail.Plan.bklist.ElementAt(k).speed);
            }

            Console.WriteLine("\n## Nombre de capteurs : " + Rocrail.Plan.fblist.Count);
            for (int k = 0; k < Rocrail.Plan.swlist.Count; k++)
            {
                Console.WriteLine(Rocrail.Plan.fblist.ElementAt(k).id +
                   " state:" + Rocrail.Plan.fblist.ElementAt(k).state);
            }

            Console.Write("\n\n##########################\n");
        }

        private static void OnElement(ElementEventArgs e)
        {
            // Cet évenement est appelé lorsqu'un élément est ajouté ou mis à jour sur le plan
            string _elementID = e.ID;
            string _nomElement = e.ElementName;
            object _element = e.Element;

            Console.Write("Nom:" + _nomElement + " ID:" + _elementID);

            switch (_nomElement)
            {
                case "auto":
                    auto aut = (auto)_element;
                    Console.Write(" cmd:" + aut.cmd);
                    break;
                case "clock":
                    clock horloge = (clock)_element;
                    Console.Write(" cmd:" + horloge.cmd);
                    break;
                case "state":
                    state etat = (state)_element;
                    break;
                case "exception":
                    exception exepti = (exception)_element;
                    Console.Write(" niveau:" + exepti.level + " " + exepti.text);
                    break;
                case "sw":
                    sw aiguillage = (sw)_element;
                    Console.Write(" blockid:" + aiguillage.blockid + " state:" + aiguillage.state);
                    break;
                case "st":
                    st route = (st)_element;
                    Console.Write(" bka:" + route.bka + " bkb:" + route.bkb + " bkaside:" + route.bkaside + " bkbside:" + route.bkbside);
                    break;
                case "lc":
                    lc loco = (lc)_element;
                    Console.Write(" blockid:" + loco.blockid + " vitesse:" + loco.V + " blockenterside:" + loco.blockenterside + " dir:" + loco.dir);
                    break;
                case "bk":
                    bk canton = (bk)_element;
                    Console.Write(" speed:" + canton.speed);
                    break;
                case "fb":
                    fb capteur = (fb)_element;
                    Console.Write(" state:" + capteur.state);
                    break;
            }

            Console.WriteLine("");
        }

        private static void OnElementAdded(object sender, ElementEventArgs e)
        {
            // Cet évenement est appelé lorsqu'un élément est ajouté au plan
            Console.Write("Added: ");
            OnElement(e);
        }

        private static void OnElementUpdated(object sender, ElementEventArgs e)
        {
            // Cet évenement est appelé lorsqu'un élément est modifié sur le plan
            Console.Write("Updated: ");
            OnElement(e);
        }

    }
}
