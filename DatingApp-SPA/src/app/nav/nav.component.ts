import { ToastrService } from 'ngx-toastr';
import { AuthService } from './../_services/auth.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  constructor(public autService: AuthService, private toastr: ToastrService,
              private router: Router) { }

  ngOnInit() {
  }
  login() {
  this.autService.login(this.model).subscribe(
    next => {
      this.toastr.success('Logged in syccessfullu');
      this.router.navigate(['/members']);
      // moze ovde, jer je ovo deo ako se USPESNO zavrsi
      // a imamo i 3 deo  koji je Po zavrsetku
    }, error => {
      this.toastr.error('Something went wrong');
    }, () => {
     // this.router.navigate(['/members']);
    }
  );
  }
    //  koristim je za NgIf
  loggedIn() {
    // const token = localStorage.getItem('token');
    // return !!token; // ako je token varijabla prazna, vratice false
    return this.autService.loggedId();
  }
  logout() {
    localStorage.removeItem('token');
    this.toastr.show('logged out');
    this.router.navigate(['/home']);
  }
}
