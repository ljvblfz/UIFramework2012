using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ComponentArt.Silverlight.UI.Input;
using ComponentArt.Silverlight.UI.Utils;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.Globalization;
using System.Windows.Browser;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Text;

namespace ComponentArt.Silverlight.Demos {

    public partial class ComboBoxTemplateColumns : UserControl, IDisposable
    {
        ObservableCollection<Bird> items_source;

        public ComboBoxTemplateColumns()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(ComboBoxTemplateColumns_Loaded);

        }
        void ComboBoxTemplateColumns_Loaded(object sender, RoutedEventArgs e)
        {
            items_source = new ObservableCollection<Bird>();

            items_source.Add(new Bird("Angola","Peregrine Falcon","Falco peregrinus"));
            items_source.Add(new Bird("Antigua and Barbuda","Magnificent Frigatebird,","Fregata magnificens"));
            items_source.Add(new Bird("Argentina","Rufous Hornero","Furnarius rufus"));
            items_source.Add(new Bird("Australia","Emu","Dromaius novaehollandiae"));
            items_source.Add(new Bird("Bahamas","Caribbean Flamingo","Phoenicopterus ruber"));
            items_source.Add(new Bird("Bangladesh","Oriental Magpie Robin","Copsychus saularis"));
            items_source.Add(new Bird("Belarus","White Stork","Ciconia ciconia"));
            items_source.Add(new Bird("Belize","Keel-billed Toucan","Ramphastos sulfuratus"));
            items_source.Add(new Bird("Bhutan","Common Raven","Corvus corax "));
            items_source.Add(new Bird("Botswana","Cattle Egret","Bubulcus ibis"));
            items_source.Add(new Bird("Canada","Great Northern Diver","Gavia immer"));
            items_source.Add(new Bird("El Salvador","Turquoise-browed Motmot","Eumomota superciliosa"));
            items_source.Add(new Bird("France","Gallic Rooster","Gallus gallus"));
            items_source.Add(new Bird("India","Indian Peacock","Pavo cristatus"));
            items_source.Add(new Bird("Israel","Hoopoe","Upupa epops"));
            items_source.Add(new Bird("Estonia","Barn Swallow","Hirundo rustica"));
            items_source.Add(new Bird("Latvia","White Wagtail","Motacilla alba"));
            items_source.Add(new Bird("Luxembourg","Goldcrest","Regulus regulus"));
            items_source.Add(new Bird("Malawi","Bar-tailed Trogon","Apaloderma vittatum"));
            items_source.Add(new Bird("Mexico","Golden Eagle","Aquila chrysaetos "));
            items_source.Add(new Bird("Namibia","Crimson-breasted Shrike","Laniarius atrococcineus"));
            items_source.Add(new Bird("Norway","Cinclus cinclus","Cinclus cinclus"));
            items_source.Add(new Bird("Pakistan","Peregrine falcon","Falco peregrinus"));
            items_source.Add(new Bird("Panama","Harpy Eagle","Harpia harpyja"));
            items_source.Add(new Bird("Papua New Guinea","Raggiana Bird of Paradise,","Paradisaea raggiana"));
            items_source.Add(new Bird("Singapore","Crimson Sunbird","Aethopyga siparaja"));
            items_source.Add(new Bird("South Africa","Blue Crane","Anthropoides paradisea"));
            items_source.Add(new Bird("Spain","Short-toed Eagle","Circaetus gallicus"));
            items_source.Add(new Bird("Swaziland","Purple-crested Turaco","Tauraco porphyreolophus"));
            items_source.Add(new Bird("Sweden","Common blackbird","Turdus merula"));
            items_source.Add(new Bird("United States","Bald Eagle","Haliaeetus leucocephalus"));
            items_source.Add(new Bird("Venezuela","Troupial","Icterus icterus"));
            items_source.Add(new Bird("Zambia","African Fish Eagle","Haliaeetus vocifer"));
            items_source.Add(new Bird("Zimbabwe","African Fish Eagle","Haliaeetus vocifer"));

            MyComboBox.ItemsSource = items_source;
        }

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            MyComboBox.Dispose();
        }

        #endregion
    }

    public class Bird
    {
        private string _country { get; set; }
        public string Country { get { return _country; } set { _country = value; } }
        
        private string _birdName { get; set; }
        public string BirdName { get { return _birdName; } set { _birdName = value; } }

        private string _scientificName { get; set; }
        public string ScientificName { get { return _scientificName; } set { _scientificName = value; } }

        public Bird(string country, string birdName, string scientificName)
        {
            _country = country;
            _birdName = birdName;
            _scientificName = scientificName;
        }
    }
}
