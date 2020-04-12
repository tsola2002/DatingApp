import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Photo } from 'src/app/_models/photo';
import { FileUploader } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {

  // this is a child component of out memberedit component
  // the user must have a photos array as part of the object

  // to bring in photos from parent component we use an @input decorator
  @Input() photos: Photo[];
  @Output() getMemberPhotoChange = new EventEmitter<string>();
  uploader: FileUploader;
  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;
  // we add a reference to current main photo
  currentMain: Photo;

  constructor(private authService: AuthService, private userService: UserService,
              private alertify: AlertifyService) { }

  ngOnInit() {
    this.initializeUploader();
  }

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/' + this.authService.decodedToken.nameid + '/photos',
      authToken: 'Bearer ' + localStorage.getItem('token'),
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

    this.uploader.onAfterAddingFile = (file) => {file.withCredentials = false; };

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const res: Photo = JSON.parse(response);
        const photo = {
          id: res.id,
          url: res.url,
          dateAdded: res.dateAdded,
          description: res.description,
          isMain: res.isMain
        };
        this.photos.push(photo);
      }
    };
  }

  // we pass in the photo that we want to set to main as type photo
  setMainPhoto(photo: Photo) {
    // pass in the user id & the photo id
    // we need to subscribe to this
    this.userService.setMainPhoto(this.authService.decodedToken.nameid, photo.id).subscribe(() => {
      // console.log('successfully set to main');
      // we use array filter method to filter out every other method apart from the main photo
      this.currentMain = this.photos.filter(p => p.isMain === true)[0];
      this.currentMain.isMain = false;
      photo.isMain = true;
       // we want to emit our photourl out of our output property
       // change the top photo url
      this.authService.changeMemberPhoto(photo.url);
       //overiding the set photo in local storage with new values
      this.authService.currentUser.photoUrl = photo.url;
      localStorage.setItem('user', JSON.stringify(this.authService.currentUser));
      this.getMemberPhotoChange.emit(photo.url);
       // this.authService.changeMemberPhoto(photo.url);
       // this.authService.currentUser.photoUrl = photo.url;
       // localStorage.setItem('user', JSON.stringify(this.authService.currentUser));
    }, error => {
      this.alertify.error(error);
    });
  }

  // deletePhoto(id: number) {
  //   this.alertify.confirm('Are you sure you want to delete this photo?', () => {
  //     this.userService.deletePhoto(this.authService.decodedToken.nameid, id).subscribe(() => {
  //       this.photos.splice(this.photos.findIndex(p => p.id === id), 1);
  //       this.alertify.success('Photo has been deleted');
  //     }, error => {
  //       this.alertify.error('Failed to delete the photo');
  //     });
  //   });
  // }

}
