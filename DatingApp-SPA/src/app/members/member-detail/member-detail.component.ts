import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/_models/user';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { error } from 'util';
import { NgxGalleryOptions, NgxGalleryAnimation, NgxGalleryImage } from 'ngx-gallery';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {

  // user variables
  user: User;
  // classes need for gallery functionality
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];

  constructor(private userService: UserService, private alertify: AlertifyService, private route: ActivatedRoute) { }

  // we want to pass our data to the route, then retrieve data from the route resolver
  // there will be no way the component will be loaded without having the data available
  ngOnInit() {
    // we need to subscribe to the route
    this.route.data.subscribe(data => {
      this.user = data['user'];
    });

    // we define our gallery optins in the array below
    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false
      }
    ];

    // we define an empty array to store our gallery images
    this.galleryImages = this.getImages();
  }

  
  

  // we define the image properties needed for our library

  // return the finally composed image url
  getImages() {
    // we define a const of imageUrl & set to an empty array
    const imageUrls = [];
    // the forOf loop will allow use iterates our images in our user object
    // tslint:disable-next-line: prefer-for-of
    for (let i = 0; i < this.user.photos.length; i++) {
      imageUrls.push({
        small: this.user.photos[i].url,
        medium: this.user.photos[i].url,
        big: this.user.photos[i].url,
        description: this.user.photos[i].description
      });
    }
    return imageUrls;
  }

}
