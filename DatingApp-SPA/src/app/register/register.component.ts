import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { User } from '../_models/user';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  user: User;
  @Input() valuesFromHome: any;
  @Output() cancelRegister = new EventEmitter();
  registerForm: FormGroup;
  bsConfig: Partial<BsDatepickerConfig>;

  constructor(private authService: AuthService, private router: Router,
                private alertify: AlertifyService, private fb: FormBuilder) { }

  ngOnInit() {
    // NEW WAY OF CREATING REACTIVE FORMS USING FORM BUILDER
    this.createRegisterForm();
    // change datepicker config theme color to red
    this.bsConfig = {
      containerClass: 'theme-red'
    },
    this.createRegisterForm();

    // OLD WAY OF CREATING REACTIVE FORMS
    // this.registerForm = new FormGroup({
    //   username: new FormControl('', Validators.required),
    //   password: new FormControl('',
    //     [Validators.required, Validators.minLength(4), Validators.maxLength(8)]),
    //   confirmPassword: new FormControl('', Validators.required)
    // }, this.passwordMatchValidator);
  }

  createRegisterForm() {
    // this method here creates the form using the form builder service
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
    // if the values dont match we return an object with the key of mismatch and a value of true
    return g.get('password').value === g.get('confirmPassword').value ? null : {'mismatch': true}; 
  }

  register() {
    if(this.registerForm.valid) {
      // we use javascript method object.assign to map source object to target object
      // it clones form values to the empty object and assigns it to the user
      this.user = Object.assign({}, this.registerForm.value);
      this.authService.register(this.user).subscribe(() => {
        this.alertify.success('Registration Successful');
      }, error => {
        this.alertify.error(error);
      }, () =>{
        this.authService.login(this.user).subscribe(() => {
          this.router.navigate(['/members']);
        })
      })
    }
    // console.log(this.registerForm.value);
  }

  cancel() {
    this.cancelRegister.emit(false);
    console.log('cancelled');
  }

}
