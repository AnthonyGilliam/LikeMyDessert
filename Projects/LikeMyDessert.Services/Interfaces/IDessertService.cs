using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LikeMyDessert.Domain;

namespace LikeMyDessert.Services.Interfaces
{
    public interface IDessertService
    {
        IList<Dessert> GetRatedDesserts(bool ascending);
        Dessert LikeDessert(Guid ID);
        Dessert DislikeDessert(Guid ID);
        void Save(Dessert dessert);
    }
}
