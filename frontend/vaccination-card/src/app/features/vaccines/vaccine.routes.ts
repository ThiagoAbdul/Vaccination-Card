import { Routes } from "@angular/router";
import { VaccineListComponent } from "./vaccine-list/vaccine-list.component";

export const vaccineRoutes: Routes = [
      { path: "", redirectTo: "todas", pathMatch: "full" },
      { path: "todas", component: VaccineListComponent }
]

