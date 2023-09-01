using LGRM.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LGRM.XamF.Pages
{
    public class GroceriesPageL : GroceriesPage
    {

        public GroceriesPageL() : base()
        {
            base.kind = Kind.Lean;
            base.SetPerKind(this.kind);
            base.skiaPainter = new SkiaPainter(kind); 
            
            
        }
    }
}
