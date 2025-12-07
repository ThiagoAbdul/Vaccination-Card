import { Routes } from '@angular/router';
import { VaccineListComponent } from './features/vaccines/vaccine-list/vaccine-list.component';
import { LoginComponent } from './auth/components/login/login.component';
import { HomeComponent } from './features/home/home/home.component';
import { LayoutComponent } from './layout/layout.component';
import { homeRoutes } from './features/home/home.routes';
import { vaccineRoutes } from './features/vaccines/vaccine.routes';
import { authGuard } from './auth/guards/auth.guard';
import { personRoutes } from './features/people/person.routes';

export const routes: Routes = [
  {
    path: "login", component: LoginComponent
  },
  {
    path: "", redirectTo: "home", pathMatch: "full"
  },
  {
    path: "",
    component: LayoutComponent,
    children: [
      { path: "home", children: homeRoutes },
      { path: "vacinas", children: vaccineRoutes },
      { path: "pessoas", children: personRoutes },
    ],
    canActivate: [ authGuard ]
  },
];
