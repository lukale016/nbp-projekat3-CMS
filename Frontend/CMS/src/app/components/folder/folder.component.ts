import { Component, Input, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-folder',
  templateUrl: './folder.component.html',
  styleUrls: ['./folder.component.css']
})
export class FolderComponent implements OnInit {

  @Input()
  public files : Array<string>;
  public name : string;

  
  constructor(private userService :UserService) {
    this.files = [ 'username_root/fajl', 'username_root/fajl2', 'username_root/fajl3', 'username_root/fajl4', 'username_root/fajl5', ];
    this.name = '';
   }

  ngOnInit(): void {
  }

}
