﻿using LearnEXM.Foundation.WhatWeKnowTree.Interfaces;
using LearnEXM.Foundation.xConnectHelper.Helpers;
using Sitecore.Analytics;
using Sitecore.Analytics.XConnect.Facets;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LearnEXM.Foundation.WhatWeKnowTree.Helpers
{
  public class WhatWeKnowTreeBuilder
  {
    public WhatWeKnowTreeBuilder(List<string> targetedFacetKeys, List<IFacetNodeFactory> customFacetKeyBulletFactories)
    {
      TargetedFacetKeys = targetedFacetKeys;
      CustomFacetKeyBulletFactories = customFacetKeyBulletFactories;
      xConnectHelper = new XConnectHelper(TargetedFacetKeys);
    }

    public FacetHelper FacetHelper { get; private set; }
    public FacetBranchHelper FacetTreeHelper { get; private set; }
    private List<IFacetNodeFactory> CustomFacetKeyBulletFactories { get; }
    private List<string> TargetedFacetKeys { get; set; } = new List<string>();
    private IXConnectFacets XConnectFacets { get; set; }
    private XConnectHelper xConnectHelper { get; }

    public IWhatWeKnowTree GetWhatWeKnowTree(Sitecore.Analytics.Tracking.Contact trackingContact)
    {
      IWhatWeKnowTree toReturn = new Concretions.WhatWeKnowTree("What We Know");

      using (XConnectClient xConnectClient = Sitecore.XConnect.Client.Configuration.SitecoreXConnectClientConfiguration.GetClient())
      {
        try
        {
          IdentifiedContactReference IdentifiedContactReference = xConnectHelper.GetIdentifierFromTrackingContact(trackingContact);
          Contact XConnectContact = xConnectHelper.IdentifyKnownContact(IdentifiedContactReference);

          var tier1Nodes = new Tier1Nodes(trackingContact, TargetedFacetKeys, CustomFacetKeyBulletFactories);
          toReturn.Root.AddNodes(tier1Nodes. Tier1NodeBuilder(trackingContact, xConnectClient, XConnectContact));
         
        }
        catch (XdbExecutionException ex)
        {
          Sitecore.Diagnostics.Log.Error(ProjConstants.Logger.Prefix + ex.Message, this);
        }
      }

      return toReturn;
    }

   
  }
}