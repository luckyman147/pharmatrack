import { Component, Input, OnInit } from '@angular/core';


@Component({
  selector: 'app-auth-form',
  templateUrl: './auth-form.component.html',
  styleUrls: ['./auth-form.component.scss'],
})
export class AuthFormComponent 
 {
  @Input() title!: string;
  @Input() mode: 'login' | 'register' = 'login';
}
