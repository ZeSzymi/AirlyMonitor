import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { LoginService } from '../login.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

  form: FormGroup = new FormGroup({
    username: new FormControl(null, Validators.email),
    password: new FormControl()
  });

  constructor(private _route: ActivatedRoute, private _loginService: LoginService, private _router: Router) { }

  save() {
    this._loginService.register(this.form.value)
      .subscribe(() =>
        this._router.navigate(['/Account/Login'], { queryParams: this._route.snapshot.queryParams })
      );
  }

  goToLogin() {
    this._router.navigate(['/Account/Login'], { queryParams: this._route.snapshot.queryParams })
  }
}
