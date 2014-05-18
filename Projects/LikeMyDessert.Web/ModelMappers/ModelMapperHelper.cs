using System;
using System.Collections.Generic;
using System.Linq;

namespace LikeMyDessert.Web.ModelMappers
{
    public static class ModelMapperHelper
    {
        public static bool DoesMapExist<TSource, TDestination>()
        {
            return AutoMapper.Mapper.FindTypeMapFor<TSource, TDestination>() != null;
        }
    }
}