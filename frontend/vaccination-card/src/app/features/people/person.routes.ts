import { Routes } from "@angular/router";
import { PeopleListComponent } from "./people-list/people-list.component";


export const personRoutes: Routes = [
      { path: "", redirectTo: "listar", pathMatch: "full" },
      { path: "listar", component: PeopleListComponent }
]

