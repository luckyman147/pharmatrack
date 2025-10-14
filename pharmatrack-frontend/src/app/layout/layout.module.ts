import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FullLayoutComponent } from './full-layout/full-layout.component';
import { NavbarComponent } from '../shared/navbar/navbar.component';
import { SidebarComponent } from '../shared/sidebar/sidebar.component';



@NgModule({
  declarations: [],
  exports: [
    FullLayoutComponent
  ],
  imports: [
    CommonModule, 
    NavbarComponent,
    SidebarComponent,
    FullLayoutComponent
  ]
})
export class LayoutModule { }
