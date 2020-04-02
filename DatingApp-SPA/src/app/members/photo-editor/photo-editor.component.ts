import { Component, OnInit, Input } from '@angular/core';
import { Photo } from 'src/app/_models/photo';

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
  
  constructor() { }

  ngOnInit() {
  }

}
