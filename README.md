# CEBSMSAlert
Azure Function App that scrapes CEB outage website and send SMS via Twilio in specific location. The app will always scrape CEB Outage Site (https://ceb.mu/customer-corner/power-outage-information). The array "AlertingTown" can be used to configure the list of locations to alert if location of outages matches.

A shorten version of the URL of the outage page will be included in the SMS.

The CRON Schedule is set to run four times per day

# Requirement
1. Create a Twilio Account and register a sender number
2. Replace the 3 values under the configuration (local.settings.json or configuration) with the values obtained from Twilio account
- TwilioaccountSid
- TwilioauthToken
- TwilioSender
3. Replace TwilioReceiver with the receive's phone number 



