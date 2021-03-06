import { PaginatedResult } from './../_models/pagination';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from './../_services/auth.service';
import { Component, OnInit } from '@angular/core';
import { Message } from '../_models/message';
import { Pagination } from '../_models/pagination';
import { UserService } from '../_services/user.service';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
  messages: Message[];
  pagination: Pagination;
  messageContainer = 'Unread';
  constructor(private userService: UserService, private authService: AuthService,
              private route: ActivatedRoute, private toast: ToastrService) { }

  ngOnInit() {
    this.route.data.subscribe( data => {
      this.messages = data['messages'].result;
      this.pagination = data['messages'].pagination;
    });
  }
loadMessages() {
  this.userService.getMessages(this.authService.decodedToken.nameid, this.pagination.currentPage,
  this.pagination.itemsPerPage, this.messageContainer)
  .subscribe(( res: PaginatedResult<Message[]>) => {
    this.messages = res.result;
    this.pagination = res.pagination;
  }, error => {
    this.toast.error('error');
  });

}
 pagedChanged(event: any): void {
   this.pagination.currentPage = event.page;
   this.loadMessages();
 }
 deleteMessage(id: number) {
   window.confirm('Are you shore you want to delete this message?');
   this.userService.deleteMessage(id, this.authService.decodedToken.nameid).subscribe( () => {
      this.messages.splice( this.messages.findIndex( m => m.id === id), 1);
      this.toast.success('mesage has been deleted');
   }, error => {
     this.toast.error('error');
   });
 }
}
