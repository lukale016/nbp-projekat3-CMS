import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { downloadFile } from './helpers/dowloadFile';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'CMS';
  
  folders :Array<string> = [ "folder", "folder2", "folder3", "folder4", "folder5", "folder6", "folder7", "folder8", "folder9", ]
  files :Array<string> = [ 'username_root/fajl', 'username_root/fajl2', 'username_root/fajl3', 'username_root/fajl4', 'username_root/fajl5', ];
  
  user :string = 'Nemanja';
  baseDir :string = `${this.user}_root/`;
  
  selectedFolder :number | null = null;
  currentPath :string = this.baseDir;
  
  constructor(public router :Router,){
  }

  returnOneLevel(){
    const lastSlashPosition :number = this.currentPath.lastIndexOf('/');
    if(lastSlashPosition != -1){
      this.currentPath = this.currentPath.substring(0, lastSlashPosition + 1);
      const deepestFolderSelected = this.folders.indexOf(this.currentPath.substring(this.currentPath.lastIndexOf('/'), this.currentPath.length - 1));
      this.selectedFolder = deepestFolderSelected !== -1 ? deepestFolderSelected : null;
    }
  }

  folderSelected(name :string){
    const selected = this.folders.find( (folderName) => folderName === name);
    if(selected){
      this.selectedFolder = this.folders.indexOf(selected);
      this.currentPath = this.baseDir + selected;
    }
  }

  fileSelected(filePath :string){
    //to do logic 
    //API call where service grabs the file with selected path and returns its readableStream data(text); 
    const response = { data: "NEKI TEKST SA BACKENDA"};
    const fileName :string = filePath.substring(filePath.lastIndexOf('/')+1, filePath.length);
    //call helper function to download file;
    downloadFile(response.data, fileName);
  }
}
