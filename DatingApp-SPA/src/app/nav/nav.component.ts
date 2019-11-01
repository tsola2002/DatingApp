import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  //create empty object to store our inputs values
  model: any = {}; 

  constructor( private authService: AuthService) { }

  ngOnInit() {
  }

  login(){
    //checking for typed in username
    //console.log(this.model);
    this.authService.login(this.model).subscribe(next => {
      console.log('Logged in successfully');
    }, error => {
      console.log('Failed to  login');
    });
  }

}
