import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppConfigService } from './config/app-config.service';



@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ],
  
  exports: [AppConfigService]
})
export class CoreModule { }
