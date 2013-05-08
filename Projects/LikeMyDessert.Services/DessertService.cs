using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using LikeMyDessert.Domain;
using LikeMyDessert.Repositories.Interfaces;
using LikeMyDessert.Services.Interfaces;

namespace LikeMyDessert.Services
{
    public class DessertService : ServiceBase<Dessert, IDessertRepository>, IDessertService
    {
        public DessertService(IDessertRepository repository) : base(repository)
        {
        }

        public IList<Dessert> GetRatedDesserts(bool ascending)
        {
            return Repository.GetInOrder(dessert => dessert.Likes >= 0
                , dessert => dessert.Likes
                , ascending);
        }

        public Dessert LikeDessert(Guid ID)
        {
            var dessert = Repository.GetByID(ID);

            dessert.Likes++;

            Repository.Update(dessert);

            return dessert;
        }

        public Dessert DislikeDessert(Guid ID)
        {
            var dessert = Repository.GetByID(ID);

            dessert.Dislikes++;

            Repository.Update(dessert);

            return dessert;
        }
    }
}
