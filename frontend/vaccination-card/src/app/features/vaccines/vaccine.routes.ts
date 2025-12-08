import { Routes } from "@angular/router";
import { VaccineListComponent } from "./components/vaccine-list/vaccine-list.component";

export const vaccineRoutes: Routes = [
      { path: "", redirectTo: "listar", pathMatch: "full" },
      { path: "listar", component: VaccineListComponent }
]

