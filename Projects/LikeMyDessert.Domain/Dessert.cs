using System;
using Global.Utilities;

namespace LikeMyDessert.Domain
{
    public class Dessert : PersistentObject
    {
        public Dessert()
        {
            ID = CombGuid.Generate();
            TimeOfCreation = DateTime.Now;
        }

        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual int Likes { get; set; }
        public virtual int Dislikes { get; set; }
        public virtual Picture Picture { get; set; }
    }
}
