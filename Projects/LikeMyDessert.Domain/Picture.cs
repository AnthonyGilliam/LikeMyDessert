using System;
using Global.Utilities;

namespace LikeMyDessert.Domain
{
	public class Picture : PersistentObject
	{
	    public Picture()
	    {
	        ID = CombGuid.Generate();
            TimeOfCreation = DateTime.Now;
	    }

		public virtual string Alt { get; set; }
		public virtual string ImageType { get; set; }
        public virtual int OrdinalIndex { get; set; }
	}
}
