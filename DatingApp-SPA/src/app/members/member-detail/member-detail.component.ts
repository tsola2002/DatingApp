import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/_models/user';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { error } from 'util';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {

  // user variables
  user: User;

  constructor(private userService: UserService, private alertify: AlertifyService, private route: ActivatedRoute) { }

  // we want to pass our data to the route, then retrieve data from the route resolver
  // there will be no way the component will be loaded without having the data available
  ngOnInit() {
    // we need to subscribe to the route
    this.route.data.subscribe(data => {
      this.user = data['user'];
    });
  }

  // loadUser() {
  //   this.userService.getUser(+this.route.snapshot.params['id'])
  //                   .subscribe((user: User) => {
  //                     this.user = user;
  //                   }, error => {
  //                     this.alertify.error(error);
  //                   });
  // }
}
