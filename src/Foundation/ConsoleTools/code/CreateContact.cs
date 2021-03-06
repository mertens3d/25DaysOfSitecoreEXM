﻿using LearnEXM.Foundation.CollectionModel.Builder;
using LearnEXM.Foundation.ConsoleTools.Helpers;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;

namespace LearnEXM.Foundation.ConsoleTools
{
  public class CreateContact
  {
    public CreateContact()
    {
    }

    public async System.Threading.Tasks.Task CreateAsync(string source, string identifier)
    {
      ContactIdentifier contactIdentifier = new ContactIdentifier(source, identifier, ContactIdentifierType.Known);
      var Identifier = contactIdentifier.Identifier;
      var XConnectContact = new Contact(new ContactIdentifier[] { contactIdentifier });

      XConnectConfigHelper configHelper = new XConnectConfigHelper();
      XConnectClientConfiguration cfg = await configHelper.ConfigureClient();
      using (var Client = new XConnectClient(cfg))
      {
        try
        {
          var clientHelper = new XConnectClientHelper(Client);

          if (!string.IsNullOrEmpty(Identifier))
          {
            XConnectContact = await clientHelper.GetXConnectContactByIdentifierAsync(CollectionConst.XConnect.ContactIdentifiers.Sources.SitecoreCinema, Identifier);
          }

          Client.AddContact(XConnectContact);
        }
        catch (XdbExecutionException ex)
        {
          Sitecore.Diagnostics.Log.Error(ex.Message, this);
        }
      }
    }
  }
}