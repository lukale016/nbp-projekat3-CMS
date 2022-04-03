import { HttpClient, HttpEventType } from "@angular/common/http";
import { Component, Input } from "@angular/core";
import { finalize, Subscription } from "rxjs";
import { swapSlashes } from "src/app/helpers/swapSlashes";

@Component({
  selector: 'file-upload',
  templateUrl: "file-upload.component.html",
  styleUrls: ["file-upload.component.css"]
})
export class FileUploadComponent {

    @Input()
    requiredFileType:string = '';

    fileName = '';
    uploadProgress:number = 0;
    uploadSub: Subscription = new Subscription();

    constructor(private http: HttpClient) {}

    onFileSelected(event :any) {
        const file:File = event.target.files[0];
      
        if (file) {
            this.fileName = file.name;
            const formData = new FormData();
            formData.append("thumbnail", file);

            let path = localStorage.getItem('currentPath');
            path = swapSlashes(String(path)).replace('\\\\','\\');
            const upload$ = this.http.post("https://localhost:7068/api/File/StoreFile", formData, { headers:{
              'Access-Control-Allow-Origin': '*',
              'Content-Type': 'multipart/form-data',
              'Accept': '*',
              'X_ParentPath': path ? path : ''
              },
                reportProgress: true,
                observe: 'events'
            })
            .pipe(
                finalize(() => this.reset())
            );
          
            this.uploadSub = upload$.subscribe( (event :any) => {
              if (event.type == HttpEventType.UploadProgress) {
                this.uploadProgress = Math.round(100 * (event.loaded / event.total));
              }
            })
        }
    }

  cancelUpload() {
    this.uploadSub.unsubscribe();
    this.reset();
  }

  reset() {
    this.uploadProgress = 0;
    this.uploadSub = new Subscription();
  }
}