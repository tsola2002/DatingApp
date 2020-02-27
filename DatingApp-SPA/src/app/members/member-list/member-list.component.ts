import { Component, OnInit } from '@angular/core';
import { UserService } from '../../_services/user.service';
import { AlertifyService } from '../../_services/alertify.service';
import { User } from '../../_models/user';
import { error } from 'util';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})

export class MemberListComponent implements OnInit {

  // varables
  users: User[];

  // we use dependency injection to inject our user service & alertify service
  constructor(private userService: UserService , private alertify: AlertifyService, private route: ActivatedRoute) { }

  // we want to pass our data to the route, then retrieve data from the route resolver
  // there will be no way the component will be loaded without having the data available
  ngOnInit() {
    // we need to subscribe to the route
    this.route.data.subscribe(data => {
      this.users = data['users'];
    });
  }

  // this method will go out to our user service & load users
  // it will subscribe to the method since it returns an observable(we return the array users)
  // we use alertify to handle any errors
  // loadUsers() {
  //   this.userService.getUsers().subscribe((users: User[]) => {
  //     this.users = users;
  //   }, error => {
  //     this.alertify.error(error);
  //   });
  // }

}
