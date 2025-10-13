import { Component, Input, OnInit } from '@angular/core';
import { GroupedResult } from '../models/GroupedResult';
import { VisiteService } from '../visite.service';
import { Visite } from '../models/Visite';

@Component({
  selector: 'app-components',
  templateUrl: './components.component.html',
  styleUrls: ['./components.component.scss'],
})
export class ComponentsComponent implements OnInit {
  @Input() visites: GroupedResult[];
  constructor(private visiteService: VisiteService) {}
  deleteVisite(visite: Visite): void {
    console.log('Deleting visite:', visite);
    this.visiteService.deleteVisite(visite.id).subscribe(() => {
      console.log('Visite deleted successfully');
      // Optionally, you can emit an event or call a method to refresh the list after deletion
      // remove the deleted visite from the local list
      this.visites = this.visites.filter(v => v.items.some(item => item.id !== visite.id));
    });
  }

  ngOnInit(): void {}
}
