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
    public class DessertService
    {
        private readonly IDessertRepository _repository;

        public DessertService(IDessertRepository repository)
        {
            _repository = repository;
        }

        public IList<Dessert> GetRatedDesserts(bool ascending)
        {
            return _repository.GetInOrder(dessert => dessert.Likes >= 0
                , dessert => dessert.Likes
                , ascending);
        }

        public Dessert LikeDessert(Guid ID)
        {
            var dessert = _repository.GetByID(ID);

            dessert.Likes++;

            _repository.Update(dessert);

            return dessert;
        }

        public Dessert DislikeDessert(Guid ID)
        {
            var dessert = _repository.GetByID(ID);

            dessert.Dislikes++;

            _repository.Update(dessert);

            return dessert;
        }
    }
}
