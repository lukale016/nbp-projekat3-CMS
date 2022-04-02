import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';

// import { AccountService } from '../../../services/account/account.service';
import { first } from 'rxjs/operators';
// import { User } from 'src/app/models/user';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  form: FormGroup;
  loading = false;
  submitted = false;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService,
  ) {
    this.form = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]],
      name: ['', [Validators.required, , Validators.minLength(6)]],
      surname: ['', [Validators.required, Validators.minLength(6)]]
    });
  }


  get f() { return this.form.controls; }

  onSubmit() {
    this.submitted = true;
    // this.alertService.clear();

    if (this.form.invalid) {
      return;
    }
    console.log("submit");

    this.loading = true;
    //     this.accountService.registerToStore(this.form.value);
    this.userService.register(this.form.value)
      .pipe(first())
      .subscribe(
        data => {
          // this.alertService.success('Registration successful', { keepAfterRouteChange: true });
          this.router.navigate(['../login'], { relativeTo: this.route });
        },
        error => {
          // this.alertService.error(error);
          this.loading = false;
        });
  }
}