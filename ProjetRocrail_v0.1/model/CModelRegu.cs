using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//INotifyPropertyChanged
using System.ComponentModel;

using RocrailLib_v4;
using RocrailLib_v4.Elements;
using System.Threading;
using System.Collections;
using System.Net.Sockets;
using System.Collections.ObjectModel;

namespace ProjetRocrail_v0._1.model
{
    class CModelRegu// : INotifyPropertyChanged
    {
        private List<bk> _bloc;
        private List<fb> _capteur;
        private List<lc> _loco;
        private List<sg> _signal;
        private List<st> _route;
        private List<sw> _switch;
        private ObservableCollection<lc> _loc;
        public ObservableCollection<lc> loc { get; set; }

        public List<bk> bloc { get { return _bloc; } set { return; } }
        public List<fb> capteur { get { return _capteur; } set { return; } }
        public List<lc> loco { get { return _loco; } set { return; } }
        public List<sg> signal { get { return _signal; } set { return; } }
        public List<st> route { get { return _route; } set { return; } }
        public List<sw> @switch { get { return _switch; } set { return; } }

        public void LoadPlan(plan pl)
        {
            _bloc = pl.bklist;
            _capteur = pl.fblist;
            _loco = pl.lclist;
            _signal = pl.sglist;
            _route = pl.stlist;
            _switch = pl.swlist;
            _loc = new ObservableCollection<lc>(pl.lclist);

        }

        public void test()
        {
            return;
        }
    }
}

/*
 CRocrail.Element
 * Bk : Bloc
 * Fb : Capteur (Feedback)
 * Lc : Loco
 * Sg : Signal
 * St : Route (Street)
 * Sw : Aiguillage (Switch)
 * Tk : Rail (Track)
 */