import { Component, inject } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { RouterModule } from '@angular/router';
import { AuthService } from '../auth/services/auth.service';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [SharedModule, RouterModule],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss'
})
export class LayoutComponent {
  private authSevice = inject(AuthService)

  doLogout(){
    this.authSevice.logout()
  }
}
