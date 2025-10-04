import { Component } from '@angular/core';
import { AuthFormComponent } from '../components/auth-form/auth-form.component';
import { IconModule } from '@coreui/icons-angular';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-login',
  imports: [AuthFormComponent,IconModule,HttpClientModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {}
