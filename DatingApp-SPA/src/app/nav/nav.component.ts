import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  userdata: any = {};
  constructor(private authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
  }
  login() {
    this.authService.login(this.userdata)
      .subscribe(next => {
        this.alertify.success('Logged in');
      }, err => {
        this.alertify.error(err.message);
      });

  }
  loggedIn() {
    return !!localStorage.getItem('token');
  }
  logOut() {
    localStorage.clear();
    this.alertify.success('Logged out');
  }

}
