import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { NgModel } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  @Output() cancelRegister = new EventEmitter();
  registerModel: any = {};
  constructor(private authservice: AuthService) { }

  ngOnInit() {
  }
  register() {
    console.log(this.registerModel);
    this.authservice.register(this.registerModel).subscribe(response => {
      this.authservice.login(this.registerModel).subscribe();
    }, err => {
      console.log(err.error.message);
    });

  }
  cancel() {
    this.cancelRegister.emit(false);
  }

}
