import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  userdata: any = {};
  constructor(private authService: AuthService) { }

  ngOnInit() {
  }
  login() {
    this.authService.login(this.userdata).subscribe(next => {
      console.log('logged in successfully');
    }, err => {
      console.log(err);
    });

  }
  loggedIn() {
    return !!localStorage.getItem('token');
  }
  logOut() {
    console.log('local storage cleared');
    localStorage.clear();
  }

}
