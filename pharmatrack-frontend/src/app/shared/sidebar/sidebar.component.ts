import {
  Component,
  OnInit,
  signal,
  WritableSignal,
  ChangeDetectionStrategy,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLinkActive, RouterModule } from '@angular/router';
import { SidebarRoute } from '../models/sidebar-route.model';
import { MAIN_ROUTES, PROFILE_ROUTES } from './config';
import { MenuSection } from '../models/menu-section.model';

@Component({
  selector: 'app-sidebar',
  standalone: true,

  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss'],
  imports: [CommonModule, RouterModule, RouterLinkActive],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SidebarComponent implements OnInit {
  menuItems!: WritableSignal<SidebarRoute[]>;
  profileItems!: WritableSignal<SidebarRoute[]>;
  menuSections: WritableSignal<MenuSection[]> = signal([]);
  ngOnInit(): void {
    // No need for initializeMenu — already class instances
    this.menuItems = signal(MAIN_ROUTES);
    this.profileItems = signal(PROFILE_ROUTES);

    this.menuSections.set([
      {
        header: 'LES RUBRIQUES',
        separator: true, // Line after this section
        items: MAIN_ROUTES,
      },
      {
        header: 'PROFILE',
        separator: false, // No line after the profile section
        items: PROFILE_ROUTES,
      },
    ]);
  }

  toggleSubmenu(menuItem: SidebarRoute): void {
    if (menuItem.submenu!.length > 0) {
      menuItem.expanded!.update((value: boolean) => !value);
    }
  }
}
