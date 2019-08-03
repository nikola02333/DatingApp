import { User } from './../_models/user';
import { AuthService } from './../_services/auth.service';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
@Output() cancelRegister = new EventEmitter();
registerForm: FormGroup;
bsConfig: Partial<BsDatepickerConfig>;
user: User;
  constructor(private authService: AuthService,
              private toastr: ToastrService,
              private fb: FormBuilder,
              private router: Router) { }

  ngOnInit() {
    this.bsConfig = {
      containerClass: 'theme-blue'
    }
    this.createRegisterForm();
  }
  createRegisterForm() {
    this.registerForm = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: [null, Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', Validators.required]
    }, {validator: this.passwordMatchValidator});
  }
  passwordMatchValidator(g: FormGroup) {
    return g.get('password').value === g.get('confirmPassword').value ? null : { 'mismatch': true};
  }
  register() {
    if (this.registerForm.valid){
      this.user = Object.assign({}, this.registerForm.value);
      // kopira vrednosti iz RegisterForm-a
      this.authService.register(this.user).subscribe( () => {
        this.toastr.success('Registration successful');
      }, error => {
        this.toastr.error(error);
      },
       () => this.authService.login(this.user).subscribe( () => {
        this.router.navigate(['/members']);
       })
       );
    }
  }
  cancel() {
    this.cancelRegister.emit(false);
    this.toastr.show('Canceled');
  }
}
