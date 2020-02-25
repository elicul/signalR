import { Component, OnInit } from '@angular/core';

import * as signalR from '@aspnet/signalr';
import { MessageService } from 'primeng/components/common/messageservice';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  providers: [MessageService]
})
export class AppComponent implements OnInit {
  constructor(private messageService: MessageService) {}

  ngOnInit(): void {
    const connection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withUrl('https://localhost:5628/signalr')
      .build();

    connection
      .start()
      .then(() => {
        console.log('Connected!');
        connection.invoke('GetConnectionId').then(connectionId => {
          console.log('ConnectionId:', connectionId);
          const user = {
            Email: 'test@test.com',
            TenantGuid: '586229de-1f19-4bb0-8cc5-7c11f6b2f739',
            TenantType: 'Carrier',
            ConnectionId: connectionId
          };
          connection
            .invoke('SaveUserConnection', user)
            .catch(err => console.error(err.toString()));
        });
      })
      .catch(err => {
        return console.error(err.toString());
      });

    connection.on('BroadcastMessage', (type: string, payload: string) => {
      this.messageService.add({
        severity: type,
        summary: payload,
        detail: 'Via SignalR'
      });
    });
  }
}
