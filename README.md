# Rebus.HeaderConverterExtensions

[![NuGet version](https://badge.fury.io/nu/Rebus.HeaderConverterExtensions.svg)](https://badge.fury.io/nu/Rebus.HeaderConverterExtensions)
[![Build Status](https://travis-ci.com/hansehe/Rebus.HeaderConverterExtensions.svg?branch=master)](https://travis-ci.com/hansehe/Rebus.HeaderConverterExtensions)
[![MIT license](http://img.shields.io/badge/license-MIT-brightgreen.svg)](http://opensource.org/licenses/MIT)

An extension library for [Rebus](https://github.com/rebus-org/Rebus) to convert Rebus headers to/from a set of standardized headers. It will allow non-Rebus implementations, and other non-dotnet code, interact with Rebus.

## Message Headers
Messages headers give basic information about the message. This library offers an extension tool to provide your own default set of standard headers with Rebus. Using a set of standardized headers will allow for different service implementations to communicate. 

### Default Header Standard

This extension library comes with a default header standard, which is possible to extend or override.

| Header Key                     | Header Value Type                 | Description                                                                                                           |
|--------------------------------|-----------------------------------|-----------------------------------------------------------------------------------------------------------------------|
| STANDARD.MessageId                | Guid                              | Unique message id.                                                                                                    |
| STANDARD.CorrelationId            | Guid                              | Unique message correlation id.                                                                                        |
| STANDARD.ReplyToAddress           | String                            | Which address/queue to send replies.                                                                                  |
| STANDARD.OriginatingAddress       | String                            | Originating address/queue.                                                                                            |
| STANDARD.ContentType              | String                            | Content type, commonly `application/json;charset=utf8`.                                                               |
| STANDARD.MessageType              | String                            | Optional contract namespace of message type, commonly `<CONTRACT_NAMESPACE>.<CONTRACT_TYPE>`.                         |
| STANDARD.Intent                   | String                            | Message intent - `command` or `event`.                                                                                |
| STANDARD.TimeSent                 | String                            | UTC timestamp at what time the message was sent, commonly `07/22/2019 11:40:19` - (`month/day/year hour:min:sec`).    |
| STANDARD.ErrorDetails             | String                            | Error details attached in case of an exception.                                                                       |
| STANDARD.SourceQueue              | String                            | Source queue of a message that has been forwarded to an error queue after it has failed.                              |

## Example

In short, activate the header converter extension with the `AddStandardHeaderConverter()` method.

```csharp
using Rebus.HeaderConverterExtensions;

var handler = new BuiltinHandlerActivator();
var bus = Configure.With(handler)
    .Options(optionsConfigure =>
    {
        // Add standard header converter extension.
        // The extension will convert incoming messages with standard headers to rebus headers,
        // and convert rebus headers to standard headers on outgoing messages.
        optionsConfigure.AddStandardHeaderConverter();
    })
    .Transport(t => ...
    .Start();
```

Have a look at a more detailed example here: 
- [./src/Rebus.HeaderConverterExtensions/Example/](./src/Rebus.HeaderConverterExtensions/Example/)

The example implements a dotnet service with Rebus and a python service with [Pika](https://github.com/pika/pika). The services communicates over AMQP using a [RabbitMq](https://www.rabbitmq.com/) message broker.

Test the example with [Docker](https://www.docker.com/):
- Install [python](https://www.python.org/) with pip, and install [DockerBuildManagement](https://github.com/DIPSAS/DockerBuildManagement) build system tool:
```
pip install DockerBuildManagement
```
- Build and run example:
```
dbm -swarm -start
dbm -build -run example
dbm -swarm -stop
```


## Development
Install [Docker](https://www.docker.com/) and [DockerBuildManagement](https://github.com/DIPSAS/DockerBuildManagement) to build, test and publish the solution with Docker.
```
pip install DockerBuildManagement
```

Deploy RabbitMq With [Docker Swarm](https://docs.docker.com/engine/swarm/):
```
dbm -swarm -start
```

Build & Test Solution:
```
dbm -build -test
```

Stop RabbitMq Running On Docker Swarm:
```
dbm -swarm -stop
```

Build & Export Nuget:
```
dbm -build -run dotnet
```

Publish Nuget:
```
dbm -publish dotnet
```

Otherwise, install [Dotnet Core](https://dotnet.microsoft.com/download) to continue development.
