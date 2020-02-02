# Rebus.HeaderConverterExtensions

An extension library to [Rebus](https://github.com/rebus-org/Rebus) to convert Rebus headers to/from a set of standardized headers. It will allow non-Rebus implementations, and other non-dotnet code, interact with Rebus.

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