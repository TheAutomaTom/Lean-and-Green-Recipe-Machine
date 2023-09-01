using LGRM.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LGRM.XamF.Pages
{
    public class GroceriesPageH : GroceriesPage
    {

        public GroceriesPageH() : base()
        {
            this.kind = Kind.HealthyFat;
            base.SetPerKind(this.kind);
            skiaPainter = new SkiaPainter(kind); 
        }
    }
}
