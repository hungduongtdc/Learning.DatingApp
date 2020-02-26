import { Component, OnInit } from '@angular/core';
import { ReturnStatement } from '@angular/compiler';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registering = false;
  constructor() { }

  ngOnInit() {
  }

  toggleRegister() {
    this.registering = !this.registering;
  }
  isRegistering() {
    return this.registering;
  }
  cancelRegister(isCancel: boolean) {
    this.registering = isCancel;
  }
}
