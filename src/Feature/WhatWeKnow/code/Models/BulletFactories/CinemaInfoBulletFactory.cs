﻿using LearnEXM.Foundation.CollectionModel.Builder.Models.Facets;
using LearnEXM.Foundation.WhatWeKnowBullets.Concretions;
using LearnEXM.Foundation.WhatWeKnowBullets.Interfaces;
using Sitecore.XConnect;

namespace LearnEXM.Feature.WhatWeKnow.SitecoreCinema.Models.BulletFactories
{
  internal class CinemaInfoBulletFactory : IFacetBulletFactory
  {
    public string AssociatedDefaultFacetKey { get; set; } = CinemaInfo.DefaultFacetKey;
    
    public IBullet GetBullet(Facet facet)
    {
      CinemaInfo cinemaInfo = facet as CinemaInfo;
      var toReturn = new Bullet("Cinema Info");

      return toReturn;
    }
  }
}