import { Component, OnInit, Input } from '@angular/core';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})

// membercard will be a child component of memberlist we will pass our user down to the child component
export class MemberCardComponent implements OnInit {

  // @input means its coming in from our parent component(it needs to be imported from angular core)
  // User class will be imported from models user
  @Input() user: User;
  constructor() { }

  ngOnInit() {
  }

}
