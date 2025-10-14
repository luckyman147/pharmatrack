import { Component , OnInit } from '@angular/core';
import { SidebarService } from '../sidebar/sidebar.service';
import { CommonModule, Location } from '@angular/common';
import { Router } from '@angular/router';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';



@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
})
export class NavbarComponent {
  route: string | undefined;
  default: any;

  constructor(
    public sidebarservice: SidebarService,
    location: Location,
    router: Router
  ) {
    router.events.subscribe((val) => {
      this.route = location.path();
    }
    );
    
  }
}
