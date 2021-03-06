﻿using LearnEXM.Feature.SitecoreCinema.Models.Proxies;
using LearnEXM.Foundation.LearnEXMRoot;
using LearnEXM.Foundation.LearnEXMRoot.Models;
using Sitecore.Data;
using Sitecore.Data.Items;
using System;

namespace LearnEXM.Feature.SitecoreCinema.Models
{
  public class MovieShowTimeProxy : _baseItemProxy
  {
    public MovieShowTimeProxy() : base()
    {
      // for generics
    }

    public MovieShowTimeProxy(Guid itemItem) : base(itemItem)
    {
    }

    protected override void CommonCTOR()
    {
      MovieDataRefFieldProxy = new MovieDataRefFieldProxy(Item, ProjectConst.Items.Templates.Feature.SitecoreCinema.MovieTicket.MovieNameField);
      MovieDateTimeFieldProxy = new DateTimeFieldProxy(Item, ProjectConst.Items.Templates.Feature.SitecoreCinema.MovieTicket.MovieTimeField);
      HarvestShowTime();
    }

    public MovieDataRefFieldProxy MovieDataRefFieldProxy { get; set; }
    public DateTimeFieldProxy MovieDateTimeFieldProxy { get; set; }
    public string MovieName { get; set; }

    public string MoviePoster { get; set; }
    public ID MovieShowTimeId { get; private set; }
    public DateTime? MovieTime { get; set; }
    protected override ID AssociatedTemplateID { get; } = ProjectConst.Items.Templates.Feature.SitecoreCinema.MovieShowTime.Template;

    private MovieItemProxy GetMovieDataItem(Item movieShowTime)
    {
      MovieItemProxy toReturn = null;
      if (movieShowTime != null)
      {
        var movieItem = MovieDataRefFieldProxy.TargetItem.GetItem();
        toReturn = new MovieItemProxy(movieItem);
      }
      return toReturn;
    }

    private void HarvestShowTime()
    {
      if (Item != null)
      {
        var movieDataItem = GetMovieDataItem(Item);

        if (Item != null)
        {
          MovieName = movieDataItem.MovieNameProxy.Value;
          MoviePoster = movieDataItem.PosterImageFieldProxy.ImageURL;
          MovieShowTimeId = Item.ID;
          MovieTime = MovieDateTimeFieldProxy.DateTime;
        };
      }
    }
  }
}