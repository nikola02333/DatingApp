import { AuthService } from './../_services/auth.service';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model: any = {};
@Output() cancelRegister = new EventEmitter();

  constructor(private authService: AuthService, private toastr: ToastrService) { }

  ngOnInit() {
  }
  register(){
    console.log(this.model);
    this.authService.register(this.model).subscribe( () => {
      this.toastr.success('registration successful' );
    }, error => {
     this.toastr.error('error!!!');
    });
  }
  cancel() {
    this.cancelRegister.emit(false);
    this.toastr.show('Canceled');
  }
}
