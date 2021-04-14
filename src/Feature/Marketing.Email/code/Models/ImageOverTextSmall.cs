﻿using LearnEXM.Feature.Marketing.Email.Controllers;
using Sitecore.Data.Items;

namespace LearnEXM.Feature.Marketing.Email.Models
{
  public class ImageOverTextSmall : _baseMarketingEmailViewModel
  {
    private readonly string Suffix;

    public ImageOverTextSmall(Item dataSource, string suffix, EmailMarketingControllerHelper controllerHelper)
    {
      Suffix = suffix;
      LinkData = controllerHelper.GetLinkData(DataSource, Link, Const.Fields.ImageOverTextThreeColumn.LinkTextFallBack);
    }

    public string Image { get { return Const.Fields.ImageOverTextThreeColumn.ImagePrefix + Suffix; } }
    public string Link { get { return Const.Fields.ImageOverTextThreeColumn.LinkPrefix + Suffix; } }
    public LinkData LinkData { get; set; }
    public string Text { get { return Const.Fields.ImageOverTextThreeColumn.TextPrefix + Suffix; } }
    public string Title { get { return Const.Fields.ImageOverTextThreeColumn.TitlePrefix + Suffix; } }
  }
}
