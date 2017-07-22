Welcome to **StoryLine.Rest** examples. This folder contains the following solutions:
* **Microservice.Membership** contains implementation of simple REST microservice which provides CRUD operations for user list. 
* **Microservice.Membership.Subsys** contains subsystem tests written with help of **StoryLine.Rest** library. 

In order to run tests successfully the follection actions must be performed:
* Build **Microservice.Membership** solution.
* Run **Microservice.Membership** microservice.
* Build **Microservice.Membership.Subsys** solution.
* Run tests included in **Microservice.Membership.Subsys**.

Subsystem tests performs validation of CRUD functionality implemented by service. Tests attemp to create, update, delete and search for users created using API.