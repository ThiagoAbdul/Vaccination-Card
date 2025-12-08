import { Routes } from "@angular/router";
import { VaccinationCardComponent } from "./components/vaccination-card/vaccination-card.component";


export const vaccinationRoutes: Routes = [
      { path: "carteirinha/:personId", component: VaccinationCardComponent }
]

