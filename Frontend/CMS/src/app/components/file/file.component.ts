import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-file',
  templateUrl: './file.component.html',
  styleUrls: ['./file.component.css']
})
export class FileComponent implements OnInit {

  @Input()
  public fileName : string;
  constructor() { 
    this.fileName = '';
  }

  ngOnInit(): void {
  }

}
