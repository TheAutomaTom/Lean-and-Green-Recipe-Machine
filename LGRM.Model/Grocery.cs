using SQLite;
using Newtonsoft.Json;
using LGRM.Model.Framework;

namespace LGRM.Model
{
    [Table("Groceries")]
    public class Grocery : BindableBase
    {

        [PrimaryKey, AutoIncrement, Column("Id")]
        public int Id { get; set; }                 // This number could be different on any new database... I think it sppeds up async inserts
        public int CatalogNumber { get; set; }      // Unique Id assigned by human to control exact catalog contents
        public Kind Kind { get; set; }              

        public string Category { get; set; }
        public int Info1 { get; set; }                
        public string IconName { get; set; }
        public string IconColor1 { get; set; }

        //Todo: I don't believe I need private feilds for all these "descriptive" members that can't be changed by typical users...
        public string Name1 { get; set; }

        string _name2 { get; set; }
        public string Name2
        {
            get => _name2;
            set => _name2 = value != null ? value : "";                    
        }

        string _desc1 { get; set; }
        public string Desc1
        {
            get => _desc1;
            set => _desc1 = value != null ? value : "";            
        }

        public float BaseWeight { get; set; }
        public string UomWeight { get; set; }
        public float BaseVolume { get; set; }
        public string UomVolume { get; set; }
        public float BaseCount { get; set; }
        public string UomCount { get; set; }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Junk to concatentate for bindings...
        [Ignore] [JsonIgnore]
        public UOMs UOMs  //Controls heights of items displayed ...
        {
            get
            {
                var asInt = 0;
                if(BaseWeight > 0)
                {
                    asInt += 1;
                }
                if (BaseVolume > 0)
                {
                    asInt += 2;
                }
                if (BaseCount > 0)
                {
                    asInt += 4;
                }
                return (UOMs)asInt;
            }
            
        }

        [Ignore] [JsonIgnore]
        public string WeightServing 
        { 
            get =>  BaseWeight > 0 ? 
                    BaseWeight.ToString() + " " + UomWeight 
                    : null;
            
        }

        [Ignore] [JsonIgnore]
        public string VolumeServing
        {
            get =>  BaseVolume > 0 ?
                    BaseVolume.ToString() + " " + UomVolume
                    : null;            
        }

        [Ignore] [JsonIgnore]
        public string CountServing
        {
            get => BaseCount > 0 ?
                    BaseCount.ToString() + " " + UomCount
                : null;
        }

        [Ignore] [JsonIgnore]
        public string Info1String   // Maybe make a converter ?
        {
            get
            {
                if (Kind == Kind.Lean)
                {
                    return Info1 switch
                    {
                        1 => "Lean ",
                        2 => "Leaner ",
                        3 => "Leanest ",
                        _ => null
                    };
                }
                else if (Kind == Kind.Green)
                {
                    return Info1 switch
                    {
                        1 => "Low Carb ",
                        2 => "Mod. Carb ",
                        3 => "High Carb ",
                        _ => null,
                    };
                }
                else return null;
            }
        } 

        [Ignore] [JsonIgnore]
        public string EtcString
        {
            
            get
            {   // iOS needs all these null checks!
                string myString = "";
                if (!string.IsNullOrWhiteSpace(Info1String))                    
                {
                    myString += Info1String.Trim() + " ";
                }

                if (!string.IsNullOrWhiteSpace(Category))                    
                {
                    myString += Category.Trim() + " ";
                }
                
                if ( !string.IsNullOrWhiteSpace( Desc1 ) )
                {
                    myString += (Desc1.Trim() + " ");
                }
                return myString;
            }

        }



        
        private bool _isSelected { get; set; }
        [Ignore] [JsonIgnore]
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                RaisePropertyChanged(nameof(IsSelected));
            }
        }
















    }
}


