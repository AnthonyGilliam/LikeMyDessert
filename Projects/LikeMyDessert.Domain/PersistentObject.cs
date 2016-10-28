using System;

namespace LikeMyDessert.Domain
{
	public abstract class PersistentObject
	{
		public virtual Guid ID { get; set; }
		protected virtual DateTime TimeOfCreation { get; set; }
	}
}
