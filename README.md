
# Attend - Event management for Episerver

Attend is an event module for EPiServer 7 CMS. It enables you to create events, manage participants, send e-mail confirmation, set up tracks, and more. 
![Start Page](https://github.com/BVNetwork/Attend/blob/master/doc/img/EventEditor.png)


# Installation
`Install-Package BVNetwork.Attend`

The package can be found in the [EPiServer Nuget Feed](http://nuget.episerver.com/).

# Configuration
No configuration needed to get started. 

Included in the Nuget Feed are the Event Page type, allowing you to create new events. 


# Basic concepts
An **event** is a page in Episerver CMS of the page type Event. Every event consists of event details, defining event start, event end, location, etc. Every event can have an unlimited number of **participants**, and an unlimited number of **sessions**. Each participant can attend an unlimited number of sessions. A participant can only be assigned to one event, but can be copied if the same attendee would like to attend multiple events. Each event can have multiple **scheduled messages** associated for communication with participants.

See user guide for more information about creating events. 

# Getting started with basic templates
The easiest way to get started with Attend is creating event pages, and display them to end users through the content areas in the project templates. 

This method will require no custom development after module installation.


# More advanced setup, with custom templates

For a more automated way of creating and displaying events, there are basically three options for creating custom templates.

1.	Create a template rendering the event page type. There are no such end user view included in the module, except the generic partial view for adding the event page to content areas. 
2.	Create a template for form submission. If the event pages are not supposed to be viewed or navigated to separately (just shown in listings etc), a common template for all events could be created. Accepting the event page id as a querystring for instance, could let the common template display information about the event, and fetch the associated form of the event.
3.	Create a custom event page type. The event page type inherits EventPageBase, having all the properties necessary for the API to handle the page as an event. This makes it possible to create a custom event page with extended properties and functionality. Different kinds of events could have their own page type if needed.

# License
The module has a licence fee of NOK 25.000 pr site. There are no software subscription, no fee for test and demo sites, and no usage limits. The software comes with full source code, and is licensed "as-is". You bear the risk of using it, and for supporting any distribution to third party. It comes with no warranties or guarantees of any kind.

# Documentation
* [Read full documentation for both editors and developers in the wiki](https://github.com/BVNetwork/Attend/wiki)

#Feature summary
![Features](https://raw.githubusercontent.com/BVNetwork/Attend/master/doc/guides/Attend%20Sales%20Sheet.png)
