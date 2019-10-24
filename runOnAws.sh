#!/bin/sh

mate-terminal -e 'dotnet /home/elicul/Documents/signalR/SignalR/SignalR/bin/Release/netcoreapp3.0/linux-x64/SignalR.dll'

export PATH=/usr/local/lib/nodejs/node-v10.16.3-linux-x64/bin:$PATH
cd /home/elicul/Documents/signalR/SignalR/SignalRClient
npm start 

#curl --header "Content-Type:ation/json"   --request POST   --data '{"Type":"success","Payload": "This is our success message"}'   http://localhost:5000/api/notification
