import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';

import { first } from 'rxjs/operators';
import { UserService } from 'src/app/services/user/user.service';
import { User } from 'src/app/models/user';
import { Folder } from 'src/app/models/folder';
import { swapSlashes } from 'src/app/helpers/swapSlashes';

@Component({ templateUrl: './login.component.html',  styleUrls: ['./login.component.css'] })
export class LoginComponent implements OnInit {
    form: FormGroup;
    loading = false;
    submitted = false;
    returnUrl: string = '';
    error = '';

    constructor(
        private formBuilder: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private userService :UserService,
    ) {
      this.form = this.formBuilder.group({
        username: ['', Validators.required],
        password: ['', Validators.required]
    });
    }

    ngOnInit() {
      this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
    }

    get f() { return this.form.controls; }

    async onSubmit() {
        this.submitted = true;

        // this.alertService.clear();

        if (this.form.invalid) {
            return;
        }

         this.loading = true;
        // console.log("user:",this.f['username'].value,this.f.['password'].value);
        
        //  const response =  await this.userService.login(this.f['username'].value, this.f['password'].value);
        //  console.log("return value from login:",response);
        this.userService.login(this.f['username'].value, this.f['password'].value)
        .subscribe( (response :any) => {
          console.log("user", response);
          this.userService.setUser(response);
          this.userService.getFolderContents(response.rootDir).subscribe( (response :any) => {
            console.log("got folder", response);
            response.path = swapSlashes(response.path);
            this.userService.setCurrentFolder(response);
            this.router.navigate(['/home']);
          })
        })
         
      }
}