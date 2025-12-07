import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { InputComponent } from '../../../ui/components/input/input.component';
import { passwordValidator } from '../../validators/password-validator';
import { passwordMatchValidator } from '../../validators/password-match.validator';
import { SharedModule } from '../../../shared/shared.module';
import { ButtonComponent } from "../../../ui/components/button/button.component";
import { LoaderService } from '../../../shared/services/loader.service';
import { AuthService } from '../../services/auth.service';
import { finalize } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, SharedModule, ButtonComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {

  private loader = inject(LoaderService)
  private authService = inject(AuthService)
  private router = inject(Router)

  protected form = new FormGroup({
    email: new FormControl<string>("", {nonNullable: true, validators: [Validators.required, Validators.email] }),
    password: new FormControl<string>("", { nonNullable: true, validators: [Validators.required, passwordValidator()] } ),
  },{ validators: passwordMatchValidator() })

  doLogin(){
    if(this.form.invalid){
      this.form.markAllAsTouched()
      return
    }

    const email = this.form.controls.email.value
    const password = this.form.controls.password.value

    this.loader.displayLoading()

    this.authService.login(email, password)
    .pipe(finalize(() => this.loader.hideLoading()))
    .subscribe({
      next: response => {
        this.authService.storeTokens(response.accessToken, response.refreshToken)
        this.router.navigate(["home"])
      }
    })

  }
}
