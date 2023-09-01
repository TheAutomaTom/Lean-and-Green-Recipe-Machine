using System;

namespace LGRM.Model
{
    public class Ingredient : Grocery
    {
        private int recipeId { get; set; }      // to join with RecipeDTO table
        public int RecipeId
        {
            get => recipeId;
            set
            {
                recipeId = value;
                RaisePropertyChanged(nameof(RecipeId));
            }
        }

        private int ingredientId { get; set; }  // to assign display order (unique per Recipe.Ingredients)
        public int IngredientId
        {
            get => ingredientId;
            set
            {
                ingredientId = value;
                RaisePropertyChanged(nameof(IngredientId));
            }
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ PORTION
        private float qtyPortion { get; set; }  
        private bool qtyPortionCalled;
        public float QtyPortion
        {
            get => qtyPortion; 
            set
            {
                if ((qtyPortion != value) && (!qtyPortionCalled))
                {
                    qtyPortionCalled = true;

                    //if (value < 99) { qtyPortion = value; } else { qtyPortion = 99; }
                    _ = value < 99 ? qtyPortion = (float)Math.Round(value, 3) : qtyPortion = 99;

                    QtyWeight = CalculateWeight();
                    QtyVolume = CalculateVolume();
                    QtyCount = CalculateCount();
                    qtyPortionCalled = false;
                    RaisePropertyChanged(nameof(QtyPortion));  // Raise PropertyChanged event as normal here, using base class or Invoke
                }
            }
        }

        private string uomPortion { get; set; }
        public string UomPortion
        {
            get => uomPortion;
            set
            {
                uomPortion = value;
                RaisePropertyChanged("UomPortion");
            }
        }


        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ WEIGHT
        private float qtyWeight { get; set; }
        public bool qtyWeightCalled;
        public float QtyWeight
        {
            get => qtyWeight;
            set
            {
                if ((qtyWeight != value) && (!qtyWeightCalled))
                {
                    qtyWeightCalled = true;
                    qtyWeight = value;

                    QtyPortion = CalculatePortion();
                    QtyVolume = CalculateVolume();
                    QtyCount = CalculateCount();
                    qtyWeightCalled = false;
                    RaisePropertyChanged(nameof(QtyWeight)); // Raise PropertyChanged event as normal here, using base class or Invoke
                }
            }
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ VOLUME
        private float qtyVolume { get; set; }
        public bool qtyVolumeCalled;
        public float QtyVolume
        {
            get => qtyVolume;
            set
            {
                if ((qtyVolume != value) && (!qtyVolumeCalled))
                {
                    qtyVolumeCalled = true;
                    qtyVolume = value;

                    QtyPortion = CalculatePortion();
                    QtyWeight = CalculateWeight();
                    QtyCount = CalculateCount();
                    qtyVolumeCalled = false;
                    RaisePropertyChanged(nameof(QtyVolume)); // Raise PropertyChanged event as normal here, using base class or Invoke
                }
            }
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ COUNT
        private float qtyCount { get; set; }
        public bool qtyCountCalled;
        public float QtyCount
        {
            get => qtyCount;
            set
            {
                if ((qtyCount != value) && (!qtyCountCalled))
                {
                    qtyCountCalled = true;
                    qtyCount = value;

                    QtyPortion = CalculatePortion();
                    QtyWeight = CalculateWeight();
                    QtyVolume = CalculateVolume();
                    qtyCountCalled = false;
                    RaisePropertyChanged(nameof(QtyCount)); // Raise PropertyChanged event as normal here, using base class or Invoke*
                }
            }
        }


        //    CTORs     \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        //SQLite needs a parameterless CTOR... this is not really a useful constructor, since every Grub will be based on an existing Grocery instance
        public Ingredient() : base()
        {
            //PlateId = App.MyPlate.Id;
        }

        //Used to artificially "Cast" from base to derived class...
        public Ingredient(Grocery g)
        {
            // IngredientId is set by RecipeVM
            QtyPortion = 1.0f;
            UomPortion = "Srv";

            CatalogNumber = g.CatalogNumber;

            Category = g.Category;
            Kind = g.Kind;

            IconName = g.IconName;
            IconColor1 = g.IconColor1;

            Name1 = g.Name1;
            Name2 = g.Name2;
            Desc1 = g.Desc1;
            Info1 = g.Info1;

            BaseWeight = g.BaseWeight;
            BaseVolume = g.BaseVolume;
            BaseCount = g.BaseCount;

            QtyWeight = g.BaseWeight;
            UomWeight = g.UomWeight;
            QtyVolume = g.BaseVolume;
            UomVolume = g.UomVolume;
            QtyCount = g.BaseCount;
            UomCount = g.UomCount;
        }


        public Ingredient(IngredientDTO ingDTO, Grocery grocery) // for deserializing SQLite DB
        {
            // Grub Data...
            IngredientId = ingDTO.Id; // .. maybe I can use this to maintain presentation order?
            
            QtyPortion = ingDTO.QtyPortion;

            // Grocery Base Class Data...
            CatalogNumber = ingDTO.CatalogNumber;
            Category = grocery.Category;
            Kind = grocery.Kind;

            IconName = grocery.IconName;
            IconColor1 = grocery.IconColor1;

            Name1 = grocery.Name1;
            Name2 = grocery.Name2;
            Desc1 = grocery.Desc1;
            Info1 = grocery.Info1;

            BaseWeight = grocery.BaseWeight;
            BaseVolume = grocery.BaseVolume;
            BaseCount = grocery.BaseCount;

            UomWeight = grocery.UomWeight;
            UomVolume = grocery.UomVolume;
            UomCount = grocery.UomCount;

            // Calcs...
            QtyWeight = BaseWeight * QtyPortion;
            QtyVolume = BaseVolume * QtyPortion;
            QtyCount = BaseCount * QtyPortion;
        }




        //   METHODS   \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        private float CalculatePortion()
        {
            if (qtyWeightCalled)
            {
                return (float)Math.Round((QtyWeight / BaseWeight), 3);
            }

            else if (qtyVolumeCalled)
            {
                var x = (float)Math.Round((QtyVolume / BaseVolume), 3);
                return x < 99 ? x : 99;
            }
            else if (qtyCountCalled)
            {
                var x = (float)Math.Round((QtyCount / BaseCount), 3);
                return x < 99 ? x : 99;
            }
            else
            {                
                throw new Exception(message: "~~~ Ingredient.CalculatePortion() hit a rounding error! Find ''return 22''");
                //return 22;
            }
        }
        private float CalculateWeight()
        {
            if (qtyPortionCalled || qtyVolumeCalled || qtyCountCalled)
            {
                var x = (float)Math.Round((BaseWeight * QtyPortion), 3);
                return x < 99 ? x : 99;
            }
            else
            {
                throw new Exception(message: "~~~ Ingredient.CalculateWeight() hit a weird condition! Find ''return 33''");
                //return 33;
            }
        }
        private float CalculateVolume()
        {
            if (qtyPortionCalled || qtyWeightCalled || qtyCountCalled)
            {
                var x = (float)Math.Round((BaseVolume * QtyPortion), 3);
                return x < 99 ? x : 99;
            }
            else
            {
                throw new Exception(message: "~~~ Ingredient.CalculateVolume() hit a weird condition! Find ''return 44''");
                //return 44;
            }
        }
        private float CalculateCount()
        {
            if (qtyPortionCalled || qtyWeightCalled || qtyVolumeCalled)
            {
                var x = (float)Math.Round((BaseCount * QtyPortion), 3);
                return x < 99 ? x : 99;
            }
            else
            {
                throw new Exception(message: "~~~ Ingredient.CalculateCount() hit a weird condition! Find ''return 55''");
                //return 55;
            }
        }










    }
}
