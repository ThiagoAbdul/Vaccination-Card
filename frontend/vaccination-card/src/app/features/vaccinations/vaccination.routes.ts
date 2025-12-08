import { Routes } from "@angular/router";
import { VaccinationCardComponent } from "./components/vaccination-card/vaccination-card.component";


export const vaccinationRoutes: Routes = [
      { path: "cartao/:personId", component: VaccinationCardComponent }
]

