import { UserService } from './../../_services/user.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute } from '@angular/router';
import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { User } from 'src/app/_models/user';
import { NgForm } from '@angular/forms';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm', {static: false}) editForm: NgForm;
  user: User;
  photoUrl: string;
  @HostListener('window:beroreunload', ['$event'])
  unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.retunValue = true;
    }
  }
  constructor(private route: ActivatedRoute,
              private toast: ToastrService,
              private userService: UserService,
              private autService: AuthService) { }

  ngOnInit() {
    this.route.data.subscribe( data => {
       this.user = data['user'];
    });
    this.autService.currentPotoUrl.subscribe(photoUrl => this.photoUrl = photoUrl);
  }
  updateUser() {
    //  this.autService.decodedToken.nameid
    // sa ovim mi izvlacimo Korisnikov ID !!!
    this.userService.updateUser( this.autService.decodedToken.nameid, this.user).subscribe(next => {
      this.toast.success(' Profile updeted successflu');
      this.editForm.reset(this.user);
    }, error => {
      this.toast.error(error);
    }
    );
  }
  updateMainPhoto(photoUrl: string) {
    this.user.photoUrl = photoUrl;
  }
}
