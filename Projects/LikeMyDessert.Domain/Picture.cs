namespace LikeMyDessert.Domain
{
	public class Picture : PersistentObject
	{
		public virtual string Alt { get; set; }
		public virtual string ImageType { get; set; }
        public virtual int OrdinalIndex { get; set; }
	}
}
