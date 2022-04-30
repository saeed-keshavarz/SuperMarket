using System.Collections.Generic;

namespace SuperMarket.Entities
{
    public class Category
    {
        public Category()
        {
            Stuffs = new HashSet<Stuff>();
        }

        public int Id { get; set; }
        public string Title { get; set; }

        public HashSet<Stuff> Stuffs { get; set; }
    }
}
