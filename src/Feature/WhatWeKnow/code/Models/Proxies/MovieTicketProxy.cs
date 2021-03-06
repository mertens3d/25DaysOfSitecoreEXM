﻿using LearnEXM.Foundation.LearnEXMRoot;
using Sitecore.Data;

namespace LearnEXM.Feature.SitecoreCinema.Models.Proxies
{
  public class MovieTicketProxy : _baseItemProxy
  {
    protected override ID AssociatedTemplateID { get; } = ProjectConst.Items.Templates.Feature.SitecoreCinema.MovieTicket.Root;
  }
}
