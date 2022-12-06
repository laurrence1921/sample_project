import { AfterViewInit, Component, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { AccountsViewerClient, UserViewModel } from '../web-api-client';
import { catchError, debounceTime, startWith, switchMap, tap, finalize } from "rxjs/operators";
import { AuthorizeService } from '../../api-authorization/authorize.service';
import { LoginModel } from '../models/login.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss']
})
export class LoginPageComponent implements OnInit, AfterViewInit {

  public searchObs$: Subject<string> = new Subject();
  accountList: any;

  constructor(private service: AccountsViewerClient,
    private authService: AuthorizeService,
    private fb: FormBuilder,
    private router: Router  ) {
    this.createForm();
  }

  login: LoginModel = new LoginModel();
  user: UserViewModel = null;

  loginForm: FormGroup;

  createForm() {
    this.loginForm = this.fb.group({
      email: ['', Validators.required],
      password: ['', Validators.required],
      rememberMe: [false],
    });
  }

  ngOnInit(): void {
    
  }

  ngAfterViewInit() {

  }

  performLogin(): void {
    this.login.email = this.loginForm.controls.email.value;
    this.login.password = this.loginForm.controls.password.value;
    this.login.rememberMe = true

    this.authService.login(this.login).subscribe((res: UserViewModel) => {
      this.user = res
      this.authService.saveUserInfo(res, true);
      this.router.navigate(['/']);
    },
      (err) => {
        console.log('Error occured!', err)
      }
    )
  }
}
