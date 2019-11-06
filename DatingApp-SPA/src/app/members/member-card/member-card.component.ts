import { UserService } from './../../_services/user.service';
import { AuthService } from 'src/app/_services/auth.service';
import { Component, OnInit, Input } from '@angular/core';
import { User } from 'src/app/_models/user';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {

  @Input() user: User;
  constructor(private authService: AuthService,
              private userservice: UserService,
              private toast: ToastrService) { }

  ngOnInit() {
  }
  sendLike(id: number) {
    this.userservice.sendLike(this.authService.decodedToken.nameid, id).subscribe(data => {
      this.toast.success('You have liked: ' + this.user.knownAs);
    }, error => {
      this.toast.error('error');
    }
    );
  }
}
