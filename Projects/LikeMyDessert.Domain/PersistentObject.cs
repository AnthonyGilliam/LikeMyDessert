using System;

namespace LikeMyDessert.Domain
{
	public abstract class PersistentObject
	{
		public virtual Guid ID { get; protected set; }
		public virtual DateTime TimeOfCreation { get; protected set; }
	}
}
