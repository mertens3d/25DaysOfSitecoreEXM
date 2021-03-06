﻿using LearnEXM.Foundation.CollectionModel.Builder.Models.Facets;
using LearnEXM.Foundation.xConnectHelper.Helpers;
using Sitecore.Analytics.XConnect.Facets;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Collection.Model;
using System.Collections.Generic;

namespace LearnEXM.Foundation.CollectionModel.Builder.Interactions
{
  public abstract class _interactionBase
  {
    private XConnectHelper _xConnectHelper;

    protected _interactionBase(Sitecore.Analytics.Tracking.Contact trackingContact)
    {
      IdentifiedContactReference = XConnectHelper.GetIdentifierFromTrackingContact(trackingContact);
      XConnectContact = XConnectHelper.IdentifyKnownContact(IdentifiedContactReference);
      if(XConnectContact != null)
      {
        XConnectFacets = trackingContact.GetFacet<IXConnectFacets>("XConnectFacets");
      }
    }

    public List<string> Errors { get; set; } = new List<string>();
    public IdentifiedContactReference IdentifiedContactReference { get; set; }
    public string Identifier { get; set; }

    //public Sitecore.Analytics.Tracking.Contact TrackingContact { get; private set; }
    public Contact XConnectContact { get; set; } = null;

    protected XConnectClient Client { get; set; }
    protected IXConnectFacets XConnectFacets { get; set; }

    private List<string> AllFacetKeys { get; set; } = new List<string> {
              CinemaInfo.DefaultFacetKey,
              CinemaVisitorInfo.DefaultFacetKey,
              EmailAddressList.DefaultFacetKey,
              PersonalInformation.DefaultFacetKey,
              CinemaDetails.DefaultFacetKey,
              AddressList.DefaultFacetKey,
    };

    private Sitecore.Analytics.Tracking.ContactManager Manager { get; set; }
    private XConnectHelper XConnectHelper { get { return _xConnectHelper ?? (_xConnectHelper = new XConnectHelper(AllFacetKeys)); } }

    public void ExecuteInteraction()
    {
      using (Client = Sitecore.XConnect.Client.Configuration.SitecoreXConnectClientConfiguration.GetClient())
      {
        try
        {
          if (XConnectContact != null && XConnectContact.IsKnown)
          {
            InteractionBody();
          }
          else
          {
            Sitecore.Diagnostics.Log.Error(CollectionConst.Logger.Prefix + "Contact is new. Ensure contact exists before executing interaction", this);
          }

          Client.Submit();
          ResetSession();
        }
        catch (XdbExecutionException ex)
        {
          Errors.Add(ex.Message);
        }
      }
    }

    public abstract void InteractionBody();

    private void ResetSession()
    {
      Manager = Sitecore.Configuration.Factory.CreateObject("tracking/contactManager", true) as Sitecore.Analytics.Tracking.ContactManager;
      Manager.RemoveFromSession(Sitecore.Analytics.Tracker.Current.Contact.ContactId);
      Sitecore.Analytics.Tracker.Current.Session.Contact = Manager.LoadContact(Sitecore.Analytics.Tracker.Current.Contact.ContactId);
    }

    //protected IdentifiedContactReference IdentifiedContactReference { get; set; }
    //public KnownData KnownData { get; private set; }
  }
}