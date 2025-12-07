import { Routes } from '@angular/router';
import { VaccineListComponent } from './features/vaccines/vaccine-list/vaccine-list.component';

export const routes: Routes = [
  {
    path: "vacinas", children: [
      { path: "", redirectTo: "todas", pathMatch: "full" },
      { path: "todas", component: VaccineListComponent }
    ]
  }
];
