import { Component, inject } from '@angular/core';
import { SharedModule } from '../../../shared/shared.module';
import { AuthService } from '../../../auth/services/auth.service';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [SharedModule, RouterModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {


  private router = inject(Router)

  goTo(module: string){
    this.router.navigate([module])
  }
}
