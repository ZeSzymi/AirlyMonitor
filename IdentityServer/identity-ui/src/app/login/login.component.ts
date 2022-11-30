import { HttpParams } from '@angular/common/http';
import { Component, } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { LoginService } from '../login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {

  form: FormGroup = new FormGroup({
    username: new FormControl(null, Validators.email),
    password: new FormControl()
  });

  constructor(private _route: ActivatedRoute, private _loginService: LoginService, private _router: Router) { }

  save() {
    const returnUrl = this._route.snapshot.queryParams['ReturnUrl']
    this._loginService.login(this._route.snapshot.queryParams, { ...this.form.value, returnUrl }).subscribe(
      response => {
        window.location.href = response.returnUrl
      }
    );
  }

  goToRegister() {
    this._router.navigate(['/Account/Register'], { queryParams: this._route.snapshot.queryParams })
  }
}
