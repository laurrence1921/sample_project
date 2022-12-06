import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthorizeService } from '../../api-authorization/authorize.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent implements OnInit {

  constructor(private authService: AuthorizeService,
    private router: Router) { }

  isExpanded = false;
  isLoggedIn = false

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  ngOnInit(): void {
    this.toggle();
    this.collapse();

    if (this.authService.isLoggedIn) {
      let tmp = this.authService.getUserProfile()
      if (tmp)
        this.isLoggedIn = !this.isLoggedIn
      //this.userRole = tmp.roleName
    }
  }

  logout() {
    //try {
    //  this.accountService.logout().subscribe();
    //} catch (err) {
    //}

    localStorage.clear();
    this.router.navigateByUrl('/login');
  }
}
