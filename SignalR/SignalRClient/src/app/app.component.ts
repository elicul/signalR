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
      .withUrl('https://localhost:5628/notify')
      .build();

    connection
      .start()
      .then(() => {
        console.log('Connected!');
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
