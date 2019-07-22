import { AuthService } from './../_services/auth.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  constructor(private autService: AuthService) { }

  ngOnInit() {
  }
  login() {
  this.autService.login(this.model).subscribe(
    next =>{
      console.log('Logged in succesfully');
    }, error => {
      console.log('Faild to login');
    }
  );
  }
    //  koristim je za NgIf
  loggedIn() {
    const token = localStorage.getItem('token');
    return !!token; // ako je token varijabla prazna, vratice false
                    // ako je puna, vratice true
  }
  logout() {
    localStorage.removeItem('token');
    console.log('logged out!!!');
  }
}
