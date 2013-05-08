namespace LikeMyDessert.Domain
{
    public class Dessert : PersistentObject
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual int Likes { get; set; }
        public virtual int Dislikes { get; set; }
        public virtual Picture Picture { get; set; }
    }
}
