import pika
import uuid
import datetime
import os
import json
import time

# pip install pika

def GetMessagePayload(message='\n\t - Hello Dotnet From Python - Standard Headers Works!\n', sendExitMessage=False):
    message = {
        'MessagePayload': message,
        'TellMeToExit': sendExitMessage
    }
    return json.dumps(message)


def GetProperties(originatingQueue: str, intent: str = 'command', exception: str = None):
    headers = {}
    headers['STANDARD.MessageId'] = str(uuid.uuid1())
    headers['STANDARD.CorrelationId'] = str(uuid.uuid1())
    headers['STANDARD.ReplyToAddress'] = originatingQueue
    headers['STANDARD.OriginatingAddress'] = originatingQueue
    headers['STANDARD.ContentType'] = 'application/json;charset=utf-8'
    headers['STANDARD.MessageType'] = 'Example.Rebus.RabbitMqWithStandardHeaders.Message, Example.Rebus.RabbitMqWithStandardHeaders'
    headers['STANDARD.Intent'] = intent
    headers['STANDARD.TimeSent'] = '{0:%m/%d/%Y %H:%M:%S}'.format(datetime.datetime.now())
    if exception is not None:
        headers['STANDARD.ErrorDetails'] = exception
        headers['STANDARD.SourceQueue'] = originatingQueue
    properties = pika.spec.BasicProperties(headers=headers)
    return properties


def VerifyQueueExists(conn: pika.BlockingConnection, queue: str, retries = 10):
    count = 0
    while count < retries:
        ch: pika.adapters.blocking_connection.BlockingChannel = conn.channel()
        try:
            ch.queue_declare(queue, durable=True, passive=True)
            ch.close()
            return
        except Exception as e:
            print(f"Queue {queue} does not exist - {e}")
            time.sleep(1)
            count += 1
    raise Exception(f"Queue {queue} does not exist!")


def BasicGetBlockingMessage(ch: pika.adapters.blocking_connection.BlockingChannel, queue: str, timeoutSec = 10):
    method_frame = None
    sleepPause = 0.5
    retry = 0
    maxRetries = int(timeoutSec / sleepPause)
    while method_frame is None and retry < maxRetries:
        result = ch.basic_get(queue)
        method_frame = result[0]
        if method_frame is not None:
            return result
        time.sleep(sleepPause)
        retry += 1
    raise Exception("Could not receive message!")


def ReceiveMessage(ch: pika.adapters.blocking_connection.BlockingChannel, queue: str):
    method_frame, header_frame, body = BasicGetBlockingMessage(ch, queue)
    headers: dict = header_frame.headers
    if 'STANDARD.MessageId' not in headers:
        raise Exception("Standard headers not in message frame!")
    message = json.loads(body)
    print(f"Received message: {message['MessagePayload']}")
    tellMeToExit = message['TellMeToExit']
    ch.basic_ack(method_frame.delivery_tag)
    if tellMeToExit:
        print("I'm told to exit - Bye!")
        exit(0)


def SendMessage(port=5672, host='localhost', username='amqp', rabbitpassword='amqp', sendExitMessage=False):
    credentials = pika.PlainCredentials(username, rabbitpassword)
    conn_params = pika.ConnectionParameters(
        host=host,
        port=port,
        virtual_host='/',
        credentials=credentials)

    queue = "pika_test_queue"
    exchange = "RebusDirect"
    destinationQueue = "dotnetQueue"
    payload = GetMessagePayload(sendExitMessage=sendExitMessage)
    properties = GetProperties(queue)

    with pika.BlockingConnection(conn_params) as conn:
        ch: pika.adapters.blocking_connection.BlockingChannel = conn.channel()
        ch.queue_declare(queue, durable=True)
        VerifyQueueExists(conn, destinationQueue)
        ch.queue_purge(queue)
        ch.queue_purge(destinationQueue)
        ch.basic_publish(exchange, destinationQueue, payload, properties=properties)
        ReceiveMessage(ch, queue)


def SendMessages(nMessages = 10, port=5672, host='localhost', username='amqp', rabbitpassword='amqp', sendExitMessage=False):
    for i in range(nMessages-1):
        SendMessage(port, host, username, rabbitpassword)
    SendMessage(port, host, username, rabbitpassword, sendExitMessage=sendExitMessage)


if __name__ == "__main__":
    host = 'localhost'
    if os.getenv('RUNNING_IN_CONTAINER', 'false') == 'true':
        host = 'rabbitmq'
    sendExitMessage = os.getenv('SEND_EXIT_MESSAGE', 'false') == 'true'
    SendMessages(host=host, sendExitMessage=sendExitMessage)
