import { Component , OnInit } from '@angular/core';
import { SidebarService } from '../sidebar/sidebar.service';
import { Location } from '@angular/common';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs/operators';
import { User } from '../Models/User';



@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
})
export class NavbarComponent implements OnInit {
  route: string;
  default: any;
  user?:User;
  breadcrumbTitle: string = 'Dashboard';
  constructor(
    public sidebarservice: SidebarService,
    location: Location,
  private  router: Router,
    private activatedRouter: ActivatedRoute
  ) {

  }

  toggleSidebar() {
    this.sidebarservice.setSidebarState(!this.sidebarservice.getSidebarState());
  }

  getSideBarState() {
    return this.sidebarservice.getSidebarState();
  }

  hideSidebar() {
    this.sidebarservice.setSidebarState(true);
  }

  ngOnInit() {
    this.user=JSON.parse(localStorage.getItem('user')||null);
    
    /* Search Bar */
this.router.events
    .pipe(filter((event) => event instanceof NavigationEnd))
    .subscribe((event: NavigationEnd) => {
      this.breadcrumbTitle = event.urlAfterRedirects.split('/')[1].normalize();
      console.log('Active route URL:', this.breadcrumbTitle);
    });  console.log('Active route URL:', this.breadcrumbTitle);
 
      // header color change on scrol
     
  }
}
