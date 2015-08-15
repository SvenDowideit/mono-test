FROM mono

WORKDIR /app
ADD . /app

RUN mcs hello.cs

CMD mono hello.exe
