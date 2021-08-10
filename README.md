# CEBSMSAlert
Azure Function App that scrapes CEB outage website and send SMS via Twilio in specific location. The app will always scrape CEB Outage Site (https://ceb.mu/customer-corner/power-outage-information). This Azure Function App uses Azure Cosmos DB to store records of SMS that was already sent.

A shorten version of the URL of the outage page will be included in the SMS.

The CRON Schedule is set to run every hour

# Requirement
1. Create a Twilio Account and register a sender number
2. Replace the 3 values under the configuration (local.settings.json or configuration) with the values obtained from Twilio account
- TwilioaccountSid
- TwilioauthToken
- TwilioSender
- LocalityCSV (CSV of location that the app should monitor and send SMS)
- CosmosDBConnectionStringSetting (Azure Cosmos DB Connection String)
3. Replace TwilioReceiver with the receive's phone number 



