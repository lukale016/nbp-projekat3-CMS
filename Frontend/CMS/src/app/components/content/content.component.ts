import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { downloadFile } from 'src/app/helpers/dowloadFile';
import { swapSlashes } from 'src/app/helpers/swapSlashes';
import { FolderItem } from 'src/app/models/folderItem';
import { UserService } from 'src/app/services/user/user.service';
import { FormBuilder, FormGroup } from '@angular/forms';
@Component({
  selector: 'app-content',
  templateUrl: './content.component.html',
  styleUrls: ['./content.component.css']
})
export class ContentComponent implements OnInit {

   
  folders :Array<string> = []//[ "folder", "folder2", "folder3", "folder4", "folder5", "folder6", "folder7", "folder8", "folder9", ];
  files :Array<string> = []//[ 'username_root/fajl', 'username_root/fajl2', 'username_root/fajl3', 'username_root/fajl4', 'username_root/fajl5', ];
  
  user :string = 'bbbbbb';
  baseDir :string = `${this.user}_root/`;
  
  selectedFolder :number | null = null;
  currentPath :string = this.baseDir;

  showAddFolder :boolean = false;
  newFolderName :string = '';
  uploadForm: FormGroup; 
  fileSel:any = '';
  
  constructor(public router :Router, private userService :UserService, private formBuilder: FormBuilder){
    this.uploadForm = this.formBuilder.group({
      file: ['']
    });
    this.loadDirectoryData(); 
  }

  onFileSelect(event :any) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      this.uploadForm.get('file')?.setValue(file);
      console.log("file selected", file);
      this.fileSel = file;
    }
  }
  onSubmit() {
    const formData = new FormData();
    formData.append('file', this.fileSel, this.fileSel.name);
    console.log("file sent", formData, this.fileSel);
    this.userService.createNewFile(formData,this.currentPath).subscribe(response => {
      console.log("file upload response", response);
      // this.files.push(response.name);
    });
  }

  ngOnInit(): void {
    this.loadDirectoryData();
  }

  loadDirectoryData(){
    this.folders = this.userService.currentFolder.children.filter( (child :FolderItem) => child.type == "Folder").map(child => child.name);
    this.files = this.userService.currentFolder.children.filter( (child :FolderItem) => child.type !== "Folder").map(child => child.name);    
    console.log("loaded current dir", this.folders, this.files);
  }

  routeToDirectory(selectedDir :string){
    console.log("routing", selectedDir, this.currentPath);
    let routeTo :string = this.currentPath;
      if(this.currentPath.lastIndexOf('/') === this.currentPath.length-1)
        routeTo = this.currentPath.substring(0, this.currentPath.length-1);
    this.userService.getFolderContents(routeTo).subscribe( (response :any)=> {
      console.log("got response folder Contents", response)
      response.path = swapSlashes(response.path);
      this.userService.setCurrentFolder(response);
      this.loadDirectoryData(); 
      // this.router.navigateByUrl(`folder/${selectedDir}`);
    });
  }

  returnOneLevel(){
    const lastSlashPosition :number = this.currentPath.lastIndexOf('/');
    if(lastSlashPosition != -1){
      this.currentPath = this.currentPath.substring(0, lastSlashPosition + 1);
      const deepestFolderSelected = this.folders.indexOf(this.currentPath.substring(this.currentPath.lastIndexOf('/'), this.currentPath.length - 1));
      this.selectedFolder = deepestFolderSelected !== -1 ? deepestFolderSelected : null;
      // poziv da se ucita sadrzaj foldera sa trenutnim pathom
      if(this.currentPath.lastIndexOf('/') === this.currentPath.length-1)
        this.currentPath = this.currentPath.substring(0, this.currentPath.length-1);
      console.log("return one level,",this.selectedFolder, this.currentPath, this.selectedFolder ? this.folders[this.selectedFolder]: null);
      
      this.routeToDirectory(this.selectedFolder ? this.folders[this.selectedFolder] : this.currentPath);
    }
  }

  folderSelected(name :string){
    const selected = this.folders.find( (folderName) => folderName === name);
    if(selected){
      this.selectedFolder = this.folders.indexOf(selected);
      if(this.currentPath[this.currentPath.length-1] != '/')
        this.currentPath += '/';
      this.currentPath = this.currentPath + selected;
      localStorage.setItem('currentPath',this.currentPath);
      // poziv da se ucita sadrzaj foldera sa trenutnim pathom
      //routing na /folderpath
      console.log("selectedFolder,",this.selectedFolder, this.currentPath);
      this.routeToDirectory(selected);
    }
  }

  fileSelected(filePath :string){
    //to do logic 
    //API call where service grabs the file with selected path and returns its readableStream data(text); 
    const fileName :string = filePath.substring(filePath.lastIndexOf('/')+1, filePath.length);
    const path = this.currentPath//filePath.substring(0,filePath.lastIndexOf('/'));
    this.userService.readFile(fileName, path).subscribe( (response)=> {
      //call helper function to download file;c
      console.log("file response", response);
      downloadFile(response, fileName);
    });
  }

  addFolderClicked(){
    this.showAddFolder = true;
  }

  addNewFolder(){
    this.showAddFolder = false;
    console.log("path",this.currentPath, this.newFolderName);
    this.userService.createNewFolder(this.newFolderName, this.currentPath).subscribe( (response :any)=>{
      console.log("dobio sam resp", response);
      if((response as string).includes("Folder created")){
          this.folders.push(this.newFolderName);
      }
    });
  }

}
