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

// added
using RocrailLib_v4;
using RocrailLib_v4.Elements;
using System.Threading;
using System.Collections;
using System.Net.Sockets;
//

namespace appRegulateurComposeur
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        private static CRocrail objRocrail; 
        public MainWindow()
        {
            // connexion 
            objRocrail = new CRocrail("172.17.50.136", 8051);
            objRocrail.UsePlan = true;          
            //
            objRocrail.PlanLoaded += OnPlanLoaded; // lorsque le plan a été charger par la librairie
            //
            objRocrail.StartScrut();
            objRocrail.Client.Send("<model cmd=\"plan\" />");
            InitializeComponent();
            displayingConsole();  
        }
        private static void OnPlanLoaded(object sender, EventArgs e)
        // lorsque le plan est chargé 
        {
            // locomotives
            Console.WriteLine("\n---- " + objRocrail.Plan.lclist.Count + " locomotives ----\n");
            for (int k = 0; k < objRocrail.Plan.lclist.Count; k++)
            {
                Console.Write("loco ID : " + objRocrail.Plan.lclist.ElementAt(k).ID + " | ");
                Console.Write("loco block ID : " + objRocrail.Plan.lclist.ElementAt(k).blockid + " | ");
                Console.Write("vitesse : " + objRocrail.Plan.lclist.ElementAt(k).V + " | ");
                Console.Write("direction : " + objRocrail.Plan.lclist.ElementAt(k).dir + "\n");
            }         
                        
            // canton
            Console.WriteLine("\n----" + objRocrail.Plan.bklist.Count + " cantons ----\n");
            for (int k = 0; k < objRocrail.Plan.bklist.Count; k++)
            {
                Console.Write("canton ID : " + objRocrail.Plan.bklist.ElementAt(k).ID + " | ");
                Console.Write("vitesse canton : " + objRocrail.Plan.bklist.ElementAt(k).speed + "\n");
            }
        }
        public void displayingConsole()
        {
            textBoxConsole.Text = "connexion établie";
        }
    }
}
