FROM python:3.6-slim as dev

COPY requirements.txt requirements.txt
RUN pip3 install -r requirements.txt

FROM dev as final

ENV RUNNING_IN_CONTAINER=true
COPY . .

ENTRYPOINT python amqp.py