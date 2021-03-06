﻿using LearnEXM.Foundation.WhatWeKnowTree.Concretions;
using LearnEXM.Foundation.WhatWeKnowTree.Helpers.NodeBuilders;
using LearnEXM.Foundation.WhatWeKnowTree.Interfaces;
using Newtonsoft.Json;
using Sitecore.Analytics.XConnect.Facets;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Client.Serialization;
using Sitecore.XConnect.Collection.Model;
using System.Collections.Generic;
using System.Linq;

namespace LearnEXM.Foundation.WhatWeKnowTree.Helpers
{
  public class Tier1Nodes
  {
    public Tier1Nodes(Sitecore.XConnect.Contact XConnectContact ,  WeKnowTreeOptions options)
    {
      var facets = XConnectContact.Facets;
      this.XConnectFacets = null;
      TreeOptions = options;

    }
    public Tier1Nodes(Sitecore.Analytics.Tracking.Contact trackingContact,  WeKnowTreeOptions options)
    {
      this.XConnectFacets = trackingContact.GetFacet<IXConnectFacets>("XConnectFacets"); ;
      TreeOptions = options;
    }

    private WeKnowTreeOptions TreeOptions { get; }
    private IXConnectFacets XConnectFacets { get; set; }

    public IWeKnowTreeNode EventsNode(List<xConnectHelper.Proxies.EventRecordProxy> events)
    {
      WeKnowTreeNode toReturn = null;

      if (events != null && events.Any() && TreeOptions.Interactions.IncludeInteractionEvents)
      {
        toReturn = new WeKnowTreeNode("Events", TreeOptions);
        foreach (var eventProxy in events)
        {
          var eventNode = new WeKnowTreeNode(eventProxy.ItemDisplayName, TreeOptions);
          eventNode.AddNode(new WeKnowTreeNode(eventProxy.TimeStamp.ToString(), TreeOptions));
          eventNode.AddNode(new WeKnowTreeNode("Duration", eventProxy.Duration.ToString(), TreeOptions));
        }
      }

      return toReturn;
    }

    public IWeKnowTreeNode FacetsNode(XConnectClient xConnectClient)
    {
      Sitecore.Diagnostics.Log.Debug(ProjConstants.Logger.Prefix + "s) FacetsNode");
      WeKnowTreeNode toReturn = null;

      if (TreeOptions.IncludeFacets)
      {
        toReturn = new WeKnowTreeNode("Facets", TreeOptions);

        var FacetTreeHelper = new FacetsNodeBuilder(xConnectClient, XConnectFacets, TreeOptions);

        if (XConnectFacets != null)
        {

          //          Emails
          //Personal
          //Addresses
          //ListSubscriptions
          //AutomationPlanExit
          //AutomationPlanEnrollmentCache
          //KeyBehaviorCache
          //EngagementMeasures
          //CinemaVisitorInfo

      TreeOptions.    TargetedFacetKeys.Add("ListSubscriptions");
          TreeOptions.TargetedFacetKeys.Add("AutomationPlanExit");
          TreeOptions.TargetedFacetKeys.Add("AutomationPlanEnrollmentCache");
          TreeOptions.TargetedFacetKeys.Add("KeyBehaviorCache");
          TreeOptions.TargetedFacetKeys.Add("EngagementMeasures");


          foreach (var targetFacetKey in TreeOptions.TargetedFacetKeys)
          {
            toReturn.AddNode(FacetTreeHelper.BuildFacetsNode(targetFacetKey));
          }
        }

        toReturn.AddNode(FacetTreeHelper.FoundFacetKeys());

        Sitecore.Diagnostics.Log.Debug(ProjConstants.Logger.Prefix + "e) FacetsNode");
      }
      return toReturn;
    }

    public IWeKnowTreeNode IdentifiersNode(List<Sitecore.Analytics.Model.Entities.ContactIdentifier> contactIdentifiers)
    {
      WeKnowTreeNode toReturn = null;

      if (contactIdentifiers != null && contactIdentifiers.Any() && TreeOptions.IncludeIdentifiers)
      {
        toReturn = new WeKnowTreeNode("Identifiers", TreeOptions);

        foreach (var contactIdentifier in contactIdentifiers)
        {
          toReturn.AddNode(new WeKnowTreeNode(contactIdentifier.Source, contactIdentifier.Identifier, TreeOptions));
        }
      }

      return toReturn;
    }

    public IWeKnowTreeNode InteractionsNode(Sitecore.XConnect.Contact xConnectContact, XConnectClient xConnectClient)
    {
      WeKnowTreeNode toReturn = null;

      if (TreeOptions.Interactions.IncludeInteractions)
      {
        toReturn = new WeKnowTreeNode("Interactions", TreeOptions);
        var interactionHelper = new InteractionsNodeBuilder(TreeOptions);

        toReturn.AddNode(interactionHelper.Something(xConnectContact, xConnectClient));

        //var knownInteractions = interactionHelper.GetKnownInteractions(xConnectContact, xConnectClient);

        //if (knownInteractions != null && knownInteractions.Any())
        //{
        //  foreach (var knownInteraction in knownInteractions)
        //  {
        //    var treeNode = new WeKnowTreeNode(knownInteraction.ChannelName, TreeOptions);

        //    treeNode.AddNode(EventsNode(knownInteraction.EventsB));

        //    toReturn.AddNode(treeNode);
        //  }
        //}
      }
      return toReturn;
    }

    public List<IWeKnowTreeNode> Tier1NodeBuilder(Sitecore.Analytics.Tracking.Contact trackingContact, XConnectClient xConnectClient, Sitecore.XConnect.Contact xConnectContact)
    {
      Sitecore.Diagnostics.Log.Debug(ProjConstants.Logger.Prefix + "s) Tier1NodeBuilder");

      var identifies = trackingContact?.Identifiers != null ? trackingContact.Identifiers.ToList() : null;


      var toReturn = new List<IWeKnowTreeNode>
      {
        TrackingContactNode(trackingContact, xConnectClient),
        IdentifiersNode(identifies),
        FacetsNode(xConnectClient),
        InteractionsNode(xConnectContact, xConnectClient)
      };

      Sitecore.Diagnostics.Log.Debug(ProjConstants.Logger.Prefix + "s) Tier1NodeBuilder");
      return toReturn;
    }

    public IWeKnowTreeNode TrackingContactNode(Sitecore.Analytics.Tracking.Contact trackingContact, XConnectClient xConnectClient)
    {
      WeKnowTreeNode toReturn = null;
      if (trackingContact != null && TreeOptions.IncludeTrackingContact)
      {
        toReturn = new WeKnowTreeNode("Tracking Contact", TreeOptions);

        toReturn.AddNode(new WeKnowTreeNode("Is New", trackingContact.IsNew.ToString(), TreeOptions));
        toReturn.AddNode(new WeKnowTreeNode("Contact Id", trackingContact.ContactId.ToString(), TreeOptions));
        toReturn.AddNode(new WeKnowTreeNode("Identification Level", trackingContact.IdentificationLevel.ToString(), TreeOptions));

        var ContractResolver = new XdbJsonContractResolver(xConnectClient.Model, true, true);

        var serializerSettings = new JsonSerializerSettings
        {
          ContractResolver = ContractResolver,
          DateTimeZoneHandling = DateTimeZoneHandling.Utc,
          DefaultValueHandling = DefaultValueHandling.Ignore,
          Formatting = Formatting.Indented
        };

        var serialized = JsonConvert.SerializeObject(trackingContact, serializerSettings);

        toReturn.AddRawNode(serialized);
      }

      return toReturn;
    }
  }
}