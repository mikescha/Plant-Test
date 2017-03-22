using SQLite.Net.Attributes;

namespace Plant_Test
{
    [Table("PlantDB")]
    public class Plant
    {
        [Column("Plant")]
        public string PlantName { get; set; }

        [Column("ScientificName")]
        public string ScientificName { get; set; }

        [Column("Type")]
        public string Type { get; set; }

        [Column("MinHeight")]
        public float MinHeight { get; set; }

        [Column("MaxHeight")]
        public float MaxHeight { get; set; }

        [Column("FloweringMonths")]
        public string FloweringMonths { get; set; }

        [Column("Sun")]
        public string Sun { get; set; }

        /*  //For if we ever want to make this into a DB that can be updated
            public event PropertyChangedEventHandler PropertyChanged;

            private void OnPropertyChanged(string propertyName)
            {
                this.PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs(propertyName));
            }
            */

        public override string ToString()
        {
            return string.Format("[Plant: {0}, {1}, {2}, {3}]", PlantName, ScientificName, Sun, MaxHeight.ToString());
        }

    }

}
