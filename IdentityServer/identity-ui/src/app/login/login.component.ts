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
    console.log(returnUrl);
    this._loginService.login(this._route.snapshot.queryParams, { ...this.form.value, returnUrl }).subscribe(
      response => {
        //this._loginService.authorize(this._route.snapshot.queryParams).subscribe();
        // console.log(this._route.snapshot.queryParams);
        // const queryParams = Object.keys(this._route.snapshot.queryParams)
        // .reduce((prev, curr, i) => prev + '&' + curr + '=' + encodeURIComponent(this._route.snapshot.queryParams[curr]), '').substring(1);
        // console.log(`https://${response.returnUrl}`, queryParams);
        console.log(`https://${response.returnUrl}`);
        window.location.href = `https://${response.returnUrl}`
      }
    );
  }

  goToRegister() {
    this._router.navigate(['/Account/Register'], { queryParams: this._route.snapshot.queryParams })
  }
}
