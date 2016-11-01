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
    public class DessertService : IDessertService
    {
        private readonly IDessertRepository _dessertRepository;

        public DessertService(IDessertRepository dessertRepository)
        {
            _dessertRepository = dessertRepository;
        }

        public IList<Dessert> GetRatedDesserts(bool ascending)
        {
            return _dessertRepository.GetInOrder(dessert => dessert.Likes >= 0
                , dessert => dessert.Likes
                , ascending);
        }

        public Dessert LikeDessert(Guid ID)
        {
            var dessert = _dessertRepository.GetByID(ID);

            dessert.Likes++;

            _dessertRepository.Update(dessert);

            return dessert;
        }

        public Dessert DislikeDessert(Guid ID)
        {
            var dessert = _dessertRepository.GetByID(ID);

            dessert.Dislikes++;

            _dessertRepository.Update(dessert);

            return dessert;
        }

        public void Save(Dessert dessert)
        {
            _dessertRepository.Save(dessert);
        }
    }
}
