using LGRM.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LGRM.XamF.Pages
{
    public class GroceriesPageG : GroceriesPage
    {

        public GroceriesPageG() : base()
        {
            this.kind = Kind.Green;
            base.SetPerKind(this.kind);
            skiaPainter = new SkiaPainter(kind); 
        }
    }
}
