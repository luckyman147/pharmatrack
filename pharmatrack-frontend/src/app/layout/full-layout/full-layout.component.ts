import { Component } from '@angular/core';
import { NavigationEnd, Router, RouterModule } from '@angular/router';
import { SidebarService } from '../../shared/sidebar/sidebar.service';
import { FooterComponent } from '../../shared/footer/footer.component';
import { CommandesModule } from '../../features/commandes/commandes.module';
import { NavbarComponent } from '../../shared/navbar/navbar.component';
import { SidebarComponent } from '../../shared/sidebar/sidebar.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-full-layout',
  imports: [FooterComponent,CommonModule,RouterModule,NavbarComponent,SidebarComponent],

  
  templateUrl: './full-layout.component.html',
  styleUrl: './full-layout.component.scss',
})
export class FullLayoutComponent {
  

}
