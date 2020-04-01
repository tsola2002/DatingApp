import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  // create empty object to store our inputs values
  model: any = {};

  // we inject our auth service into our constructor
  constructor(public authService: AuthService, private alertify: AlertifyService,
              private router: Router) { }

  ngOnInit() {
  }

  login() {
    // checking for typed in username & password
    // console.log(this.model);

    // our login method will take in the form fields
    // the method returns an observable we have to subscribe to observable
    // we use the nxt overload bcos we want to do something if theres an error
    this.authService.login(this.model).subscribe(next => {
    // console.log('Logged in successfully');
      this.alertify.success('Logged in successfully');
    }, error => {
    // console.log(error);
      this.alertify.error(error);
    }, () => {
      this.router.navigate(['/members']);
    });
  }

  loggedIn() {
     return this.authService.loggedIn();
  }

  logout() {
    localStorage.removeItem('token');
  // console.log('logged out');
    this.alertify.message('logged out');
    this.router.navigate(['/home']);
  }

}