import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-folder',
  templateUrl: './folder.component.html',
  styleUrls: ['./folder.component.css']
})
export class FolderComponent implements OnInit {

  @Input()
  public files : Array<string>;
  @Input()
  public folders :Array<string>;
  
  @Output()
  public folderSelected :EventEmitter<string> = new EventEmitter(); 
  @Output()
  public fileSelected :EventEmitter<string> = new EventEmitter(); 

  public name : string;
  private sub: any;

  constructor(private userService :UserService, private route :ActivatedRoute) {
    this.folders = [];
    this.files = [ 'username_root/fajl', 'username_root/fajl2', 'username_root/fajl3', 'username_root/fajl4', 'username_root/fajl5', ];
    this.name = '';
   }

  ngOnInit(): void {
    this.sub = this.route.params.subscribe(params => {
      this.name = params['path'];
    });
  }

  folderClicked(folder :string){
    this.folderSelected.emit(folder);
  }

  fileClicked(file :string){
    this.fileSelected.emit(file);
  }

}
