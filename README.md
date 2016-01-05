Azure Queue Demo
================

## Intro

The following is a small C# console app demonstrating how the Azure Table storage
service can be leveraged to create a cloud-based queue. Note that depending on how
the queue object is initialized, it will store its items either in an Azure table or
locally.

Unit tests for the queue can be run using Visual Studio's test infrastructure.
Note that the cloud component of the test runs on a mock table service.

## Usage
1. Open up the project in Visual Studio
2. In the QueueDemo project, edit line 25 of AzureQueuesInTableService.cs, replacing
   ACCOUNTNAME with your Azure Table Service account name and ACCOUNTKEY with your
   account key
3. You can then build the console app and run it in Visual Studio
