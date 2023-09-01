using LGRM.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LGRM.XamF.Pages
{
    public class GroceriesPageC : GroceriesPage
    {

        public GroceriesPageC() : base()
        {
            this.kind = Kind.Condiment;
            base.SetPerKind(this.kind);
            skiaPainter = new SkiaPainter(kind); 
        }
    }
}
